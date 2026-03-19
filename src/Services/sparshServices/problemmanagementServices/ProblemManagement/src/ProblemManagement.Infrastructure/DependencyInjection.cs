using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using RabbitMQ.Client;
using ProblemManagement.Domain.Interfaces;
using ProblemManagement.Infrastructure.Data;
using ProblemManagement.Infrastructure.Messaging;
using ProblemManagement.Infrastructure.Messaging.Consumers;
using ProblemManagement.Infrastructure.Repositories;
using ProblemManagement.Infrastructure.Services;

namespace ProblemManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<ProblemManagementDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(3);
                    sqlOptions.CommandTimeout(0);
                }));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ProblemManagementDbContext>());

        // Repositories
        services.AddScoped<IProblemRepository, ProblemRepository>();
        services.AddScoped<IProblemSolutionRepository, ProblemSolutionRepository>();
        services.AddScoped<IProblemApprovalRepository, ProblemApprovalRepository>();
        services.AddScoped<ISolutionApprovalRepository, SolutionApprovalRepository>();
        services.AddScoped<ISolutionCommentRepository, SolutionCommentRepository>();
        services.AddScoped<IProblemFunctionRepository, ProblemFunctionRepository>();
        services.AddScoped<IProblemImpactRepository, ProblemImpactRepository>();
        services.AddScoped<IProblemAttachmentRepository, ProblemAttachmentRepository>();
        services.AddScoped<IDapperProblemRepository, DapperProblemRepository>();

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

            // RabbitMQ Consumers
            services.AddHostedService<ProblemCreatedConsumer>();
            services.AddHostedService<SolutionApprovedConsumer>();
        }
        else
        {
            // Register a null/mock message publisher when RabbitMQ is disabled
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
