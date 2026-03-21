using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShipmentService.Application.Common.Interfaces;
using ShipmentService.Infrastructure.Data;
using ShipmentService.Infrastructure.Messaging.Options;
using ShipmentService.Infrastructure.Messaging.RabbitMQ;
using ShipmentService.Infrastructure.Messaging.RabbitMQ.Consumers;
using ShipmentService.Infrastructure.Repositories;
using ShipmentService.Infrastructure.Storage;
using ShipmentService.Infrastructure.Storage.Options;

namespace ShipmentService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Entity Framework
        services.AddDbContext<ShipmentDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("ShipmentDb"),
                sql => sql.MigrationsAssembly(typeof(ShipmentDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IShipmentRepository, ShipmentRepository>();
        services.AddScoped<DapperShipmentRepository>();

        // RabbitMQ
        services.Configure<RabbitMQOptions>(opts => configuration.GetSection(RabbitMQOptions.SectionName).Bind(opts));
        services.AddSingleton<IMessagePublisher, RabbitMQPublisher>();
        services.AddHostedService<ShipmentCreatedConsumer>();
        services.AddHostedService<ShipmentStatusUpdateConsumer>();

        // Blob Storage
        services.Configure<BlobStorageOptions>(opts => configuration.GetSection(BlobStorageOptions.SectionName).Bind(opts));
        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        return services;
    }
}
