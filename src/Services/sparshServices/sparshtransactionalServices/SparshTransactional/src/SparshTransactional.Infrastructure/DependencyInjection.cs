using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using RabbitMQ.Client;
using SparshTransactional.Domain.Interfaces;
using SparshTransactional.Infrastructure.Data;
using SparshTransactional.Infrastructure.Messaging;
using SparshTransactional.Infrastructure.Messaging.Consumers;
using SparshTransactional.Infrastructure.Repositories;
using SparshTransactional.Infrastructure.Services;

namespace SparshTransactional.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<SparshTransactionalDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(3);
                    sqlOptions.CommandTimeout(0);
                }));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<SparshTransactionalDbContext>());

        // Repositories
        services.AddScoped<IScholarshipMasterRepository, ScholarshipMasterRepository>();
        services.AddScoped<IEligibilityCriteriaRepository, EligibilityCriteriaRepository>();
        services.AddScoped<IScholarshipApplicationRepository, ScholarshipApplicationRepository>();
        services.AddScoped<IScholarshipDisbursementRepository, ScholarshipDisbursementRepository>();
        services.AddScoped<IDapperScholarshipRepository, DapperScholarshipRepository>();

        // MediatR for domain event handlers in Infrastructure assembly
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        // Azure Blob Storage
        var blobConnectionString = configuration.GetConnectionString("AzureBlobStorage");
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddSingleton(new BlobServiceClient(blobConnectionString));
            services.AddScoped<IBlobStorageService, AzureBlobStorageService>();
        }
        else
        {
            services.AddScoped<IBlobStorageService, AzureBlobStorageService>();
            services.AddSingleton(new BlobServiceClient("UseDevelopmentStorage=true"));
        }

        // RabbitMQ
        var rabbitConfig = configuration.GetSection("RabbitMQ");
        var enableRabbitMq = configuration.GetValue("Features:EnableRabbitMq", false);

        if (enableRabbitMq)
        {
            services.AddSingleton<IConnectionFactory>(_ => new ConnectionFactory
            {
                HostName = rabbitConfig["HostName"] ?? "localhost",
                UserName = rabbitConfig["UserName"] ?? "guest",
                Password = rabbitConfig["Password"] ?? "guest",
                Port = int.TryParse(rabbitConfig["Port"], out var port) ? port : 5672
            });

            services.AddSingleton<IMessagePublisher>(sp =>
            {
                var factory = sp.GetRequiredService<IConnectionFactory>();
                var connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
                return new RabbitMqPublisher(connection);
            });

            services.AddHostedService<ApplicationApprovedConsumer>();
            services.AddHostedService<DisbursementCompletedConsumer>();
        }
        else
        {
            services.AddSingleton<IMessagePublisher, NoOpMessagePublisher>();
        }

        // Polly Circuit Breaker for HTTP clients
        services.AddHttpClient("ExternalApi")
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }
}
