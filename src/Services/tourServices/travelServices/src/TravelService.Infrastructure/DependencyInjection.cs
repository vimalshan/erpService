using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TravelService.Application.Common.Interfaces;
using TravelService.Domain.Repositories;
using TravelService.Infrastructure.Messaging;
using TravelService.Infrastructure.Persistence;
using TravelService.Infrastructure.Persistence.Repositories;
using TravelService.Infrastructure.Services;

namespace TravelService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<TravelDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("TravelDb"),
                sql => sql.MigrationsAssembly(typeof(TravelDbContext).Assembly.FullName))
            .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning)));

        // Repositories
        services.AddScoped<ITourPlanRepository, TourPlanRepository>();
        services.AddScoped<IBatchRepository, BatchRepository>();
        services.AddScoped<IForexRepository, ForexRepository>();
        services.AddScoped<IApproverDetailRepository, ApproverDetailRepository>();

        // Services
        services.AddScoped<TravelQueryService>();
        services.AddScoped<IBlobStorageService, BlobStorageService>();

        // Messaging
        var rabbitEnabled = configuration.GetValue("RabbitMQ:Enabled", false);
        services.AddSingleton<IMessagePublisher, RabbitMqMessagePublisher>();
        if (rabbitEnabled)
            services.AddHostedService<TourPlanEventConsumer>();

        // JWT Authentication
        var jwtSection = configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSection["Key"] ?? throw new InvalidOperationException("JWT Key not configured."));

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
                ValidIssuer = jwtSection["Issuer"],
                ValidAudience = jwtSection["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };
        });

        return services;
    }
}
