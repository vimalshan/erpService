using Ardalis.GuardClauses;
using Azure.Identity;
using Azure.Storage.Blobs;
using LoanAccount.Domain.Interfaces;
using LoanAccount.Infrastructure.Persistence;
using LoanAccount.Infrastructure.Resilience;
using LoanAccount.Infrastructure.Services;
using LoanAccount.Infrastructure.UnitOfWork;
using LoanAccount.Infrastructure.Messaging;
using LoanAccount.Infrastructure.EventPublishing;
using LoanAccount.Infrastructure.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Polly.CircuitBreaker;
using RabbitMQ.Client;
using RabbitMQ.Client;

namespace LoanAccount.Infrastructure.Extensions;

/// <summary>
/// Extension methods for registering infrastructure services
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    /// <summary>
    /// Registers infrastructure services including DbContext and repositories
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        Guard.Against.Null(services, nameof(services));
        Guard.Against.Null(configuration, nameof(configuration));

        var connectionString = configuration.GetConnectionString("LoanAccountDb");
        Guard.Against.NullOrEmpty(connectionString, nameof(connectionString));

        // Register domain event publishing interceptor
        services.AddScoped<DomainEventPublishingInterceptor>();

        // Register DbContext with domain event interceptor
        services.AddDbContext<LoanAccountDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                connectionString,
                sqlOptions => sqlOptions.MigrationsAssembly("LoanAccount.Infrastructure"));
            options.AddInterceptors(sp.GetRequiredService<DomainEventPublishingInterceptor>());
        });

        // Register Unit of Work
        services.AddScoped<ILoanUnitOfWork, LoanUnitOfWork>();

        // Register repositories
        services.AddScoped<ILoanMainRepository, Repositories.LoanMainRepository>();
        services.AddScoped<ILoanInstallmentRepository, Repositories.LoanInstallmentRepository>();
        services.AddScoped<ILoanEmployeeInterestRateRepository, Repositories.LoanEmployeeInterestRateRepository>();
        services.AddScoped<ILoanLedgerRepository, Repositories.LoanLedgerRepository>();
        services.AddScoped<ILoanSettlementRepository, Repositories.LoanSettlementRepository>();

        // Register Azure Blob Storage (skip if not configured)
        var azureSettings = configuration.GetSection("AzureStorage");
        var blobConnectionString = azureSettings.GetValue<string>("ConnectionString");
        var containerName = azureSettings.GetValue<string>("ContainerName") ?? "loan-documents";

        // Only register if connection string is properly configured
        if (!string.IsNullOrEmpty(blobConnectionString) && !blobConnectionString.Contains("storageaccount"))
        {
            try
            {
                var blobContainerClient = new BlobContainerClient(
                    new Uri(blobConnectionString),
                    new DefaultAzureCredential());

                services.AddSingleton(blobContainerClient);
                services.AddScoped<IBlobStorageService, AzureBlobStorageService>();
            }
            catch
            {
                // Azure Storage not available, skip registration
            }
        }

        // Register HTTP client
        services.AddHttpClient("LoanApiClient");

        // Register RabbitMQ (optional - gracefully handle if not available)
        var rabbitMQSettings = configuration.GetSection("RabbitMQ");
        var rabbitMQHost = rabbitMQSettings.GetValue<string>("Host") ?? "localhost";
        var rabbitMQPort = rabbitMQSettings.GetValue<int>("Port", 5672);
        var rabbitMQUsername = rabbitMQSettings.GetValue<string>("Username") ?? "guest";
        var rabbitMQPassword = rabbitMQSettings.GetValue<string>("Password") ?? "guest";
        var rabbitMQVirtualHost = rabbitMQSettings.GetValue<string>("VirtualHost") ?? "/";

        try
        {
            var factory = new ConnectionFactory
            {
                HostName = rabbitMQHost,
                Port = rabbitMQPort,
                UserName = rabbitMQUsername,
                Password = rabbitMQPassword,
                VirtualHost = rabbitMQVirtualHost,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };

            var rabbitMQConnection = factory.CreateConnection();
            services.AddSingleton(rabbitMQConnection);
            services.AddScoped<IEventPublisher, RabbitMQEventPublisher>();
            services.AddScoped<IEventConsumer, RabbitMQEventConsumer>();
        }
        catch
        {
            // RabbitMQ not available - application will continue without messaging
            // Services can handle missing IEventPublisher and IEventConsumer gracefully
        }

        // Register Health Checks
        var healthChecks = services.AddHealthChecks()
            .AddCheck<LoanDatabaseHealthCheck>("LoanDatabase", tags: new[] { "database" });
        
        // Only add RabbitMQ health check if RabbitMQ is available
        try
        {
            var testFactory = new ConnectionFactory
            {
                HostName = rabbitMQHost,
                Port = rabbitMQPort,
                UserName = rabbitMQUsername,
                Password = rabbitMQPassword,
                VirtualHost = rabbitMQVirtualHost
            };
            testFactory.CreateConnection().Close();
            healthChecks.AddCheck<RabbitMQHealthCheck>("RabbitMQ", tags: new[] { "messaging" });
        }
        catch
        {
            // RabbitMQ not available, skip health check
        }

        return services;
    }
}
