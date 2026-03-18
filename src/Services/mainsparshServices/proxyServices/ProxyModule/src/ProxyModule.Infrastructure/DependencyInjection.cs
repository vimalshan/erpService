using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProxyModule.Application.Interfaces;
using ProxyModule.Domain.Interfaces;
using ProxyModule.Infrastructure.Messaging;
using ProxyModule.Infrastructure.Messaging.Consumers;
using ProxyModule.Infrastructure.Persistence;
using ProxyModule.Infrastructure.Repositories;
using ProxyModule.Infrastructure.Services;

namespace ProxyModule.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<ProxyModuleDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ProxyModuleDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IProxyRightRepository, ProxyRightRepository>();
        services.AddScoped<IProxyRightReadRepository, ProxyRightReadRepository>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        // RabbitMQ
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        services.AddHostedService<ProxyRightEventConsumer>();

        // MediatR handlers from this assembly (domain event handlers)
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }
}
