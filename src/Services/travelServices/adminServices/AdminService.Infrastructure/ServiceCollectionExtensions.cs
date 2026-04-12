using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Azure.Storage.Blobs;
using AdminService.Infrastructure.Persistence;
using AdminService.Infrastructure.Repositories;
using AdminService.Infrastructure.Services;
using AdminService.Infrastructure.Messaging;
using AdminService.Infrastructure.Azure;
using AdminService.Domain.Interfaces;
using AdminService.Application.Mappings;
using MediatR;

namespace AdminService.Infrastructure;

/// <summary>
/// Dependency injection configuration extension
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add infrastructure services to the container
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");

        services.AddDbContext<AdminServiceDbContext>(options =>
            options.UseSqlServer(connectionString, sqlOptions =>
                sqlOptions.MigrationsAssembly(typeof(AdminServiceDbContext).Assembly.FullName)
            )
        );

        // Unit of Work and Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAdminUnitRepository, AdminUnitRepository>();
        services.AddScoped<IFinanceUnitRepository, FinanceUnitRepository>();

        // AutoMapper
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        // MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<MappingProfile>();
            cfg.RegisterServicesFromAssemblyContaining<RabbitMQPublisher>();
        });

        // JWT Token Service
        services.AddScoped<ITokenService, TokenService>();

        // RabbitMQ (Lazy-loaded to allow startup without RabbitMQ running)
        var rabbitMqSettings = configuration.GetSection("RabbitMQ");
        var factory = new ConnectionFactory
        {
            HostName = rabbitMqSettings["HostName"] ?? "localhost",
            UserName = rabbitMqSettings["UserName"] ?? "guest",
            Password = rabbitMqSettings["Password"] ?? "guest",
            VirtualHost = rabbitMqSettings["VirtualHost"] ?? "/",
            DispatchConsumersAsync = true
        };
        // Use Lazy<IConnection> to defer connection creation until first use
        services.AddSingleton(new Lazy<IConnection>(() => factory.CreateConnection()));
        services.AddScoped<IMessagePublisher, RabbitMQPublisher>();
        services.AddScoped<AdminUnitEventConsumer>();

        // Azure Blob Storage (optional - check if properly configured)
        var blobStorageConnectionString = configuration.GetConnectionString("AzureBlobStorage");
        if (!string.IsNullOrEmpty(blobStorageConnectionString) && 
            !blobStorageConnectionString.Contains("youraccount") && 
            !blobStorageConnectionString.Contains("yourkey"))
        {
            services.AddSingleton(new BlobServiceClient(blobStorageConnectionString));
            services.AddScoped<IBlobStorageService, BlobStorageService>();
        }
        else
        {
            // Register no-op implementation for development without Azure
            services.AddScoped<IBlobStorageService>(sp => new NoOpBlobStorageService());
        }

        return services;
    }
}
