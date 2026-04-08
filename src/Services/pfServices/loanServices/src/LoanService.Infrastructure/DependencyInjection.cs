using Azure.Storage.Blobs;
using LoanService.Domain.Interfaces;
using LoanService.Infrastructure.BlobStorage;
using LoanService.Infrastructure.Messaging;
using LoanService.Infrastructure.Messaging.Consumers;
using LoanService.Infrastructure.Persistence;
using LoanService.Infrastructure.Persistence.Dapper;
using LoanService.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace LoanService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;

        // EF Core
        services.AddDbContext<LoanDbContext>(options =>
            options.UseSqlServer(connectionString, sql =>
                sql.EnableRetryOnFailure(3)));

        // Dapper
        services.AddSingleton<ILoanDapperRepository>(_ => new LoanDapperRepository(connectionString));

        // Repositories / UoW
        services.AddScoped<ILoanRepository, LoanRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // RabbitMQ Publisher (with fallback if RabbitMQ is unavailable)
        var rabbitHost = configuration["RabbitMQ:HostName"] ?? "localhost";
        var rabbitUser = configuration["RabbitMQ:UserName"] ?? "guest";
        var rabbitPass = configuration["RabbitMQ:Password"] ?? "guest";

        services.AddSingleton<IMessagePublisher>(sp =>
        {
            try
            {
                return RabbitMqPublisher.CreateAsync(rabbitHost, rabbitUser, rabbitPass).GetAwaiter().GetResult();
            }
            catch
            {
                var logger = sp.GetRequiredService<ILogger<NullMessagePublisher>>();
                logger.LogWarning("RabbitMQ is not available. Using no-op publisher.");
                return new NullMessagePublisher(logger);
            }
        });

        // Azure Blob Storage (with no-op fallback)
        var blobConnectionString = configuration["AzureBlobStorage:ConnectionString"];
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddSingleton(new BlobServiceClient(blobConnectionString));
            services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();
        }
        else
        {
            services.AddSingleton<IBlobStorageService, NullBlobStorageService>();
        }

        // RabbitMQ Consumers (hosted services)
        services.AddSingleton<LoanPaymentConsumer>(sp =>
            new LoanPaymentConsumer(rabbitHost, rabbitUser, rabbitPass,
                sp.GetRequiredService<ILogger<LoanPaymentConsumer>>()));
        services.AddHostedService(sp => sp.GetRequiredService<LoanPaymentConsumer>());

        services.AddSingleton<LoanApprovalConsumer>(sp =>
            new LoanApprovalConsumer(rabbitHost, rabbitUser, rabbitPass,
                sp.GetRequiredService<ILogger<LoanApprovalConsumer>>()));
        services.AddHostedService(sp => sp.GetRequiredService<LoanApprovalConsumer>());

        // Polly Circuit Breaker for HttpClient
        services.AddHttpClient("LoanServiceClient")
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
