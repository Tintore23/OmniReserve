using OmniReserve.Application;
using OmniReserve.Infrastructure;
using OmniReserve.Domain.Entities;
using OmniReserve.Domain.Exceptions;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.AspNetCore.RateLimiting; // 1. Agrega esta directiva using

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

// 3. Configuración del Rate Limiter Global
builder.Services.AddRateLimiter(options =>
{
    // Establecer el código de estado HTTP cuando se excede el límite
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // Cambiar AddFixedWindowPolicy por AddFixedWindowLimiter
    options.AddFixedWindowLimiter("GlobalLimit", policyOptions =>
    {
        policyOptions.PermitLimit = 100;
        policyOptions.Window = TimeSpan.FromMinutes(1);
        policyOptions.QueueLimit = 0;
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. Middlewares y tubería HTTP
app.UseMiddleware<OmniReserve.Api.Middlewares.GlobalExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 5. Habilitar el Middleware de Rate Limiter
// Colócalo antes de la autenticación y autorización para rechazar peticiones excesivas anticipadamente
app.UseRateLimiter();

app.UseAuthentication(); // 1. Lee, inspecciona y valida el JWT Bearer
app.UseAuthorization();  // 2. Valida los permisos/roles del usuario autenticado

app.MapControllers();

app.Run();