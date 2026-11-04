using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OmniReserve.Infrastructure.Security;
using OmniReserve.Application.Common.Interfaces;

using OmniReserve.Infrastructure.Persistence;
using OmniReserve.Infrastructure.Persistence.Repositories;

namespace OmniReserve.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // 1. Configurar las opciones de JWT desde appsettings.json
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        // --- Configuración de Autenticación JWT Bearer ---
        var jwtSettings = configuration.GetSection(JwtSettings.SectionName);
        var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"] 
            ?? throw new InvalidOperationException("JwtSettings:Secret is not configured."));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                ClockSkew = TimeSpan.Zero
            };
        });

        // 2. Configurar la base de datos PostgreSQL
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("OmniReserveDb")
            )
        );

        // 3. Registrar servicios, repositorios e interfaces de seguridad
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        services.AddScoped<IJwtProvider, JwtProvider>(); // <-- ¡Agregado el proveedor de JWT!
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}