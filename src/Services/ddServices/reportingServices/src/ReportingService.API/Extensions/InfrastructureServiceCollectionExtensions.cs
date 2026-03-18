using ReportingService.Infrastructure.Data;
using ReportingService.Infrastructure.Repositories;
using ReportingService.Infrastructure.MessageConsumers;
using ReportingService.Infrastructure;
using ReportingService.Domain.Interfaces;
using ReportingService.Domain.Events;
using ReportingService.Shared.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ReportingService.API.Extensions;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ReportingDbContext>(options =>
            options.UseSqlServer(connectionString, sqlOptions =>
                sqlOptions.MigrationsAssembly("ReportingService.Infrastructure")));

        // Unit of Work & Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // JWT Token Service
        services.AddScoped<ITokenService, JwtTokenService>();

        // Resilience Policy
        services.AddScoped<IResiliencePolicy, ResiliencePolicyService>();

        // Azure Blob Storage
        services.AddScoped<IBlobStorageService, BlobStorageService>();

        // RabbitMQ
        services.AddScoped<IRabbitMQConsumer>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var logger = sp.GetService<ILogger<RabbitMQConsumer>>();
            var rabbitmqSettings = config.GetSection("RabbitMQ");
            var hostname = rabbitmqSettings["Hostname"] ?? "localhost";
            var username = rabbitmqSettings["Username"] ?? "guest";
            var password = rabbitmqSettings["Password"] ?? "guest";
            return new RabbitMQConsumer(hostname, username, password, logger);
        });

        // Message Consumers
        services.AddScoped<AppraisalEventConsumer>();

        // Domain Events
        services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();

        return services;
    }

    public static IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ReportingDbContext>();
            dbContext.Database.Migrate();
            
            // Seed initial data
            dbContext.SeedAppraisalDataAsync().Wait();
        }

        return app;
    }
}
