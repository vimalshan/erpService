using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Masters.Application.Interfaces;
using Masters.Infrastructure.Persistence;
using Masters.Infrastructure.Persistence.Repositories;
using Masters.Infrastructure.Messaging;
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
