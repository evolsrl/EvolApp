using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using EvolApp.API.Data;
using EvolApp.API.Repositories;
using EvolApp.API.Repositories.Interfaces;

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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
