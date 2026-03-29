using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Masters.Application.Interfaces;
using Masters.Infrastructure.Persistence;
using Masters.Infrastructure.Persistence.Repositories;
using Masters.Infrastructure.Messaging;
using Masters.Infrastructure.Messaging.Consumers;
using Masters.Infrastructure.Storage;
using Polly;
using Polly.Extensions.Http;

namespace Masters.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<MastersDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(MastersDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<ILovTypeMasterRepository, LovTypeMasterRepository>();
        services.AddScoped<ILovMasterRepository, LovMasterRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // RabbitMQ
        var rabbitMqConnection = configuration.GetSection("RabbitMQ:ConnectionString").Value 
            ?? "amqp://guest:guest@localhost:5672";
        services.AddSingleton<IMessagePublisher>(sp => 
            new RabbitMqPublisher(rabbitMqConnection, sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<RabbitMqPublisher>>()));

        // RabbitMQ consumers
        services.AddSingleton<LovTypeMasterCreatedConsumer>(sp =>
            new LovTypeMasterCreatedConsumer(
                rabbitMqConnection,
                sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<LovTypeMasterCreatedConsumer>>()));
        services.AddHostedService(sp => sp.GetRequiredService<LovTypeMasterCreatedConsumer>());

        services.AddSingleton<LovMasterCreatedConsumer>(sp =>
            new LovMasterCreatedConsumer(
                rabbitMqConnection,
                sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<LovMasterCreatedConsumer>>()));
        services.AddHostedService(sp => sp.GetRequiredService<LovMasterCreatedConsumer>());

        // Azure Blob Storage
        var blobStorageConnection = configuration.GetSection("AzureStorage:ConnectionString").Value 
            ?? "UseDevelopmentStorage=true";
        services.AddSingleton<IBlobStorageService>(sp => 
            new BlobStorageService(blobStorageConnection, sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<BlobStorageService>>()));

        // HTTP Client - Polly v8 uses different resilience pipeline approach
        services.AddHttpClient("MastersApiClient");

        return services;
    }
}
