using TransactionService.Infrastructure.Data;
using TransactionService.Infrastructure.Repositories;
using TransactionService.Infrastructure.MessageConsumers;
using TransactionService.Infrastructure;
using TransactionService.Domain.Interfaces;
using TransactionService.Domain.Events;
using TransactionService.Shared.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TransactionService.API.Extensions;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<TransactionDbContext>(options =>
            options.UseSqlServer(connectionString, sqlOptions =>
                sqlOptions.MigrationsAssembly("TransactionService.Infrastructure")));

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
        services.AddScoped<TransactionEventConsumer>();

        // Domain Events
        services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();

        return services;
    }

    public static IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<TransactionDbContext>();
            dbContext.Database.Migrate();

            // Seed initial data
            dbContext.SeedTransactionDataAsync().Wait();
        }

        return app;
    }
}
