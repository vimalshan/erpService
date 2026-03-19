using Azure.Storage.Blobs;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MasterDataService.Application.Interfaces;
using MasterDataService.Domain.Interfaces;
using MasterDataService.Infrastructure.Messaging.Consumers;
using MasterDataService.Infrastructure.Persistence.Dapper;
using MasterDataService.Infrastructure.Persistence.EfCore;
using MasterDataService.Infrastructure.Repositories;
using MasterDataService.Infrastructure.Services;
using Polly;
using Polly.Extensions.Http;

namespace MasterDataService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;

        // EF Core
        services.AddDbContext<MasterDataDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Dapper
        services.AddSingleton<IDapperContext>(new DapperContext(connectionString));
        services.AddScoped<IDapperQueryService, DapperQueryService>();

        // Repositories & UoW
        services.AddScoped<ILovMasterRepository, LovMasterRepository>();
        services.AddScoped<ILovTypeMasterRepository, LovTypeMasterRepository>();
        services.AddScoped<IHoldTypeMasterRepository, HoldTypeMasterRepository>();
        services.AddScoped<ILocationScanParamRepository, LocationScanParamRepository>();
        services.AddScoped<IScannerMasterRepository, ScannerMasterRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Azure Blob Storage
        var blobConnectionString = configuration.GetValue<string>("AzureBlobStorage:ConnectionString");
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddSingleton(new BlobServiceClient(blobConnectionString));
        }
        else
        {
            services.AddSingleton(new BlobServiceClient("UseDevelopmentStorage=true"));
        }
        services.AddScoped<IBlobStorageService, AzureBlobStorageService>();

        // MassTransit + RabbitMQ
        services.AddMassTransit(x =>
        {
            x.AddConsumer<MasterDataUpdatedConsumer>();
            x.AddConsumer<MasterDataSyncRequestConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitHost = configuration.GetValue<string>("RabbitMQ:Host") ?? "localhost";
                var rabbitUser = configuration.GetValue<string>("RabbitMQ:Username") ?? "guest";
                var rabbitPass = configuration.GetValue<string>("RabbitMQ:Password") ?? "guest";

                cfg.Host(rabbitHost, "/", h =>
                {
                    h.Username(rabbitUser);
                    h.Password(rabbitPass);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        // Disable MassTransit health checks (RabbitMQ not available locally)
        services.Configure<MassTransit.MassTransitHostOptions>(options =>
        {
            options.WaitUntilStarted = false;
        });

        // Polly Circuit Breaker for HTTP clients
        services.AddHttpClient("ExternalAPI")
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
