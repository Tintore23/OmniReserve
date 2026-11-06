using OmniReserve.Application;
using OmniReserve.Infrastructure;
using OmniReserve.Domain.Entities;
using OmniReserve.Domain.Exceptions;
using Microsoft.Extensions.Caching.StackExchangeRedis;

var builder = WebApplication.CreateBuilder(args);

// 1. Inyección de capas y servicios
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// 2. Configuración del Caché Distribuido con Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
    options.InstanceName = "OmniReserve_";
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// 3. Middlewares y tubería HTTP
app.UseMiddleware<OmniReserve.Api.Middlewares.GlobalExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // 1. Lee, inspecciona y valida el JWT Bearer
app.UseAuthorization();  // 2. Valida los permisos/roles del usuario autenticado

app.MapControllers();

app.Run();