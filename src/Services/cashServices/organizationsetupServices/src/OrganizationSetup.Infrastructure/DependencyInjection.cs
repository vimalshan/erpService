using Azure.Storage.Blobs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrganizationSetup.Application.Interfaces;
using OrganizationSetup.Infrastructure.Messaging;
using OrganizationSetup.Infrastructure.Persistence;
using OrganizationSetup.Infrastructure.Services;

namespace OrganizationSetup.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<OrganizationSetupDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection") ?? 
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CASHDB;Integrated Security=True;",
                sqlOptions => sqlOptions.MigrationsAssembly("OrganizationSetup.Infrastructure")
            )
        );

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Azure Blob Storage
        var blobConnectionString = configuration.GetConnectionString("BlobStorage");
        if (!string.IsNullOrEmpty(blobConnectionString))
        {
            services.AddScoped(_ => new BlobServiceClient(blobConnectionString));
            services.AddScoped<IBlobStorageService, AzureBlobStorageService>();
        }
        else
        {
            services.AddScoped<IBlobStorageService, NullBlobStorageService>();
        }

        // RabbitMQ
        services.Configure<RabbitMqSettings>(configuration.GetSection(RabbitMqSettings.SectionName));
        services.AddSingleton<RabbitMqMessagePublisher>();
        services.AddSingleton<IMessagePublisher>(sp => sp.GetRequiredService<RabbitMqMessagePublisher>());

        // Register domain event handlers from Infrastructure assembly
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }
}

