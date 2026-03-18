using LocationServices.Domain.Repositories;
using LocationServices.Infrastructure.Data;
using LocationServices.Infrastructure.Messaging;
using LocationServices.Infrastructure.Repositories;
using LocationServices.Infrastructure.Resilience;
using LocationServices.Infrastructure.Storage;
using LocationServices.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LocationServices.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration config)
    {
        // ── Entity Framework Core ────────────────────────────────────────────
        services.AddDbContext<LocationDbContext>(opt =>
            opt.UseSqlServer(config.GetConnectionString("LocationDb"),
                sql => sql.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null)));

        // ── Repositories & Unit of Work ──────────────────────────────────────
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        services.AddScoped<ILocationAppMapReadRepository, DapperLocationAppMapReadRepository>();

        // ── RabbitMQ Messaging ───────────────────────────────────────────────
        services.Configure<RabbitMQOptions>(config.GetSection("RabbitMQ"));
        services.AddSingleton<IMessagePublisher, RabbitMQPublisher>();
        services.AddSingleton<RabbitMQConsumer>();

        // ── Azure Blob Storage ───────────────────────────────────────────────
        services.Configure<BlobStorageOptions>(config.GetSection("BlobStorage"));
        services.AddScoped<IBlobStorageService, BlobStorageService>();

        // ── Polly Resilience ─────────────────────────────────────────────────
        services.AddSingleton<IResilienceService, ResilienceService>();

        return services;
    }
}
