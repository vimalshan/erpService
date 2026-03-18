using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DevelopmentService.Domain.Interfaces;
using DevelopmentService.Infrastructure.Data;
using DevelopmentService.Infrastructure.Messaging;
using DevelopmentService.Infrastructure.Messaging.Consumers;
using DevelopmentService.Infrastructure.Repositories;
using DevelopmentService.Infrastructure.Storage;

namespace DevelopmentService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<DevelopmentDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(DevelopmentDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<ILetPlanRepository, LetPlanRepository>();
        services.AddScoped<ILetBhrPlanRepository, LetBhrPlanRepository>();
        services.AddScoped<ICompetencyRepository, CompetencyRepository>();

        // Message Publisher — null-safe when RabbitMQ is unavailable
        services.AddSingleton<IMessagePublisher>(sp =>
        {
            var cfg    = sp.GetRequiredService<IConfiguration>();
            var logger = sp.GetRequiredService<ILogger<RabbitMqPublisher>>();
            try
            {
                return RabbitMqPublisher.CreateAsync(cfg, logger).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "RabbitMQ unavailable at startup — publishing will be skipped.");
                return new NullMessagePublisher(sp.GetRequiredService<ILogger<NullMessagePublisher>>());
            }
        });

        // RabbitMQ Consumers
        services.AddHostedService<LearningPlanCreatedConsumer>();
        services.AddHostedService<BhrPlanApprovedConsumer>();

        // Blob Storage
        services.AddScoped<IBlobStorageService, BlobStorageService>();

        return services;
    }
}
