using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CurrencyManagement.Domain.Interfaces;
using CurrencyManagement.Infrastructure.Repositories;
using CurrencyManagement.Infrastructure.Persistence;
using CurrencyManagement.Application.Common.Interfaces;
using CurrencyManagement.Infrastructure.Dapper;
using CurrencyManagement.Infrastructure.Storage;
using CurrencyManagement.Infrastructure.Messaging;
using RabbitMQ.Client;
using Azure.Storage.Blobs;

namespace CurrencyManagement.Infrastructure;

/// <summary>
/// Extension methods for registering infrastructure layer services
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbContext
        services.AddDbContext<CurrencyDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found")));

        // Register IApplicationDbContext
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<CurrencyDbContext>());

        // Register Repositories
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
        services.AddScoped<IOrganizationCurrencyRepository, OrganizationCurrencyRepository>();

        // Register Dapper Query Services
        services.AddScoped<ICurrencyQueryService, CurrencyQueryService>();
        services.AddScoped<IExchangeRateQueryService, ExchangeRateQueryService>();

        // Register RabbitMQ (optional - gracefully handles when RabbitMQ is unavailable)
        try
        {
            var factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ:HostName"] ?? "localhost",
                UserName = configuration["RabbitMQ:UserName"] ?? "guest",
                Password = configuration["RabbitMQ:Password"] ?? "guest",
                Port = int.Parse(configuration["RabbitMQ:Port"] ?? "5672")
            };
            services.AddSingleton(factory.CreateConnection());
            services.AddScoped<IMessagePublisher, RabbitMQMessagePublisher>();
        }
        catch (Exception)
        {
            // RabbitMQ not available - register a no-op publisher
            services.AddScoped<IMessagePublisher, NoOpMessagePublisher>();
        }

        // Register Azure Blob Storage (optional)
        var blobConnectionString = configuration.GetConnectionString("BlobStorage");
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddSingleton(x => new BlobServiceClient(blobConnectionString));
        }
        else
        {
            // Register a dummy service when blob storage is not configured
            services.AddSingleton(x => new BlobServiceClient(new Uri("https://dummy.blob.core.windows.net")));
        }
        services.AddScoped<IBlobStorageService, BlobStorageService>();

        return services;
    }
}
