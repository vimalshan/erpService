using Azure.Storage.Blobs;
using CategoryAndVendorService.Application.Interfaces;
using CategoryAndVendorService.Domain.Interfaces;
using CategoryAndVendorService.Infrastructure.Messaging;
using CategoryAndVendorService.Infrastructure.Persistence;
using CategoryAndVendorService.Infrastructure.Repositories;
using CategoryAndVendorService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CategoryAndVendorService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;

        // EF Core
        services.AddDbContext<CategoryVendorDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Unit of Work
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<CategoryVendorDbContext>());

        // Dapper
        services.AddSingleton(new DapperQueryService(connectionString));

        // Repositories
        services.AddScoped<IMainCategoryRepository, MainCategoryRepository>();
        services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
        services.AddScoped<IVendorDocumentRepository, VendorDocumentRepository>();
        services.AddScoped<ISupportDocumentRepository, SupportDocumentRepository>();
        services.AddScoped<ISupportDocumentCounterRepository, SupportDocumentCounterRepository>();

        // Azure Blob Storage
        var blobConnectionString = configuration.GetValue<string>("AzureBlobStorage:ConnectionString");
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddSingleton(new BlobServiceClient(blobConnectionString));
            services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();
        }

        // RabbitMQ
        var rabbitHost = configuration.GetValue<string>("RabbitMQ:HostName") ?? "localhost";
        var rabbitUser = configuration.GetValue<string>("RabbitMQ:UserName") ?? "guest";
        var rabbitPass = configuration.GetValue<string>("RabbitMQ:Password") ?? "guest";

        services.AddSingleton<IMessagePublisher>(sp =>
        {
            try
            {
                return RabbitMqPublisher.CreateAsync(rabbitHost, rabbitUser, rabbitPass).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                var logger = sp.GetRequiredService<ILogger<RabbitMqPublisher>>();
                logger.LogWarning(ex, "RabbitMQ is not available. Using no-op publisher.");
                return new NoOpMessagePublisher();
            }
        });

        // Message Consumers
        services.AddHostedService(sp => new VendorDocumentApprovalConsumer(
            sp.GetRequiredService<IServiceScopeFactory>(),
            sp.GetRequiredService<ILogger<VendorDocumentApprovalConsumer>>(),
            rabbitHost, rabbitUser, rabbitPass));

        services.AddHostedService(sp => new CategorySyncConsumer(
            sp.GetRequiredService<IServiceScopeFactory>(),
            sp.GetRequiredService<ILogger<CategorySyncConsumer>>(),
            rabbitHost, rabbitUser, rabbitPass));

        return services;
    }
}
