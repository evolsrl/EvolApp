using EvolApp.API.Data;
using EvolApp.API.Repositories;
using EvolApp.API.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Data;
using EvolApp.API.Swagger;

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
        .AllowAnyMethod());
});

builder.Services.AddScoped<IDbConnection>(sp =>
{
    var cs = sp.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection");
    return new SqlConnection(cs);
});

builder.Services.AddScoped<IAfiliadoRepository, AfiliadoRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IEleccionRepository, EleccionRepository>();
builder.Services.AddScoped<IVotacionRepository, VotacionRepository>();
builder.Services.AddScoped<IPrestamoRepository, PrestamoRepository>();
builder.Services.AddScoped<IGeneralesRepository, GeneralesRepository>();
builder.Services.AddScoped<ICargosRepository, CargosRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 🔐 SWAGGER + API-KEY
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EvolApp API",
        Version = "v1"
    });

    c.OperationFilter<JsonBodyExampleOperationFilter>();

    // Definición del esquema de seguridad por API-KEY
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "Ingrese la API Key en el header. Ejemplo: X-API-KEY: {tu-clave}",
        Name = "X-API-KEY",                // 👈 CAMBIÁ esto si tu header se llama distinto
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });

    // Requerir API-KEY en todos los endpoints
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"          // 👈 mismo ID que arriba
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

// 🔐 Middleware de API-KEY
app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
