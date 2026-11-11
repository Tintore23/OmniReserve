using OmniReserve.Application;
using OmniReserve.Infrastructure;
using OmniReserve.Domain.Entities;
using OmniReserve.Domain.Exceptions;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.AspNetCore.RateLimiting; // 1. Directiva using para Rate Limiting

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

// 3. Configuración del Rate Limiter (Políticas Global y de Autenticación)
builder.Services.AddRateLimiter(options =>
{
    // Establecer el código de estado HTTP cuando se excede el límite
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // Política Global (100 peticiones por minuto)
    options.AddFixedWindowLimiter("GlobalLimit", policyOptions =>
    {
        policyOptions.PermitLimit = 100;
        policyOptions.Window = TimeSpan.FromMinutes(1);
        policyOptions.QueueLimit = 0;
    });

    // Política Restrictiva para Login contra Fuerza Bruta (3 peticiones cada 30 segundos)
    options.AddFixedWindowLimiter("AuthLimit", policyOptions =>
    {
        policyOptions.PermitLimit = 3;
        policyOptions.Window = TimeSpan.FromSeconds(30);
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