using EvolApp.API.Data;
using EvolApp.API.Repositories;
using EvolApp.API.Repositories.Interfaces;
using EvolApp.API.Swagger;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Quitar header de servidor (opcional)
builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

// DbContext (si lo usás para otras cosas)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowAll", p => p
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
        // Para que el front pueda leer el filename si devolvés Content-Disposition
        .WithExposedHeaders("Content-Disposition"));
});

// Dapper connection
builder.Services.AddScoped<IDbConnection>(sp =>
{
    var cs = sp.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection");
    return new SqlConnection(cs);
});

// Repos
builder.Services.AddScoped<IAfiliadoRepository, AfiliadoRepository>();
builder.Services.AddScoped<IAhorroRepository, AhorroRepository>();
builder.Services.AddScoped<IEleccionRepository, EleccionRepository>();
builder.Services.AddScoped<IGeneralesRepository, GeneralesRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IPrestamoRepository, PrestamoRepository>();
builder.Services.AddScoped<IVotacionRepository, VotacionRepository>();
builder.Services.AddScoped<ICargosRepository, CargosRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 🔐 SWAGGER + API-KEY
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EvolApp API v2",
        Version = "v2"
    });

    // Definición del esquema de seguridad por API-KEY
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Name = "X-API-KEY",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Description = "Ingrese la API Key en el header. Ejemplo: X-API-KEY: {tu-clave}"
    });

    // Requerir API-KEY en todos los endpoints
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name = "X-API-KEY",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });

    // Filtros de whitelist
    c.DocumentFilter<MostrarSoloRutasPorConfigDocumentFilter>();
    c.DocumentFilter<MostrarSoloSchemasPorConfigDocumentFilter>();

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

var app = builder.Build();

// Habilitar archivos estáticos (para servir /images y /swagger/custom.css)
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("v1/swagger.json", "EvolApp API v2");
    c.DocumentTitle = "EvolApp API - Documentación";
    c.InjectStylesheet("../swagger/custom.css");
});

// ✅ ORDEN CORRECTO PARA CORS + PRE-FLIGHT
app.UseRouting();

// CORS antes de cualquier middleware que pueda cortar el request (ApiKey, Auth, etc.)
app.UseCors("AllowAll");

// 🔐 Middleware de API-KEY (debe permitir OPTIONS dentro del middleware)
app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
