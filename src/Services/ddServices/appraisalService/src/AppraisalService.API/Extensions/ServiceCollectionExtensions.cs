using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using Azure.Storage.Blobs;
using AppraisalService.Infrastructure.Persistence.Data;
using AppraisalService.Infrastructure.Persistence.Repositories;
using AppraisalService.Infrastructure.Messaging;
using AppraisalService.Infrastructure.Storage;
using AppraisalService.Infrastructure.Authentication;
using AppraisalService.Domain;
using AppraisalService.Domain.Repositories;

namespace AppraisalService.API.Extensions;

/// <summary>
/// Dependency Injection extension for adding all services
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppraisalServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        services.AddDbContext<AppraisalDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("AppraisalDb"),
                b => b.MigrationsAssembly("AppraisalService.Infrastructure")));

        // Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // RabbitMQ
        var rabbitMQConfig = configuration.GetSection("RabbitMQ");
        var connectionFactory = new ConnectionFactory
        {
            HostName = rabbitMQConfig["HostName"] ?? "localhost",
            UserName = rabbitMQConfig["UserName"] ?? "guest",
            Password = rabbitMQConfig["Password"] ?? "guest",
            VirtualHost = rabbitMQConfig["VirtualHost"] ?? "/",
            Port = int.TryParse(rabbitMQConfig["Port"], out var port) ? port : 5672
        };

        services.AddSingleton<IConnectionFactory>(connectionFactory);
        services.AddScoped<IMessagePublisher, RabbitMQPublisher>();
        services.AddScoped<DomainEventConsumer>();

        // Azure Storage
        var storageConnectionString = configuration["AzureStorage:ConnectionString"];
        services.AddSingleton(new BlobServiceClient(storageConnectionString));
        services.AddScoped<IBlobStorageService, BlobStorageService>();

        // JWT
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}
