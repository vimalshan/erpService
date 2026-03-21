using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RackingSystem.Application.Common.Interfaces;
using RackingSystem.Domain.Interfaces;
using RackingSystem.Infrastructure.Persistence;
using RackingSystem.Infrastructure.Repositories;
using RackingSystem.Infrastructure.Services;
using RackingSystem.Infrastructure.Settings;

namespace RackingSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());

        // Repositories & UoW
        services.AddScoped<IRackRepository, RackRepository>();
        services.AddScoped<IShelfRepository, ShelfRepository>();
        services.AddScoped<IBinRepository, BinRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Services
        services.AddScoped<IBlobStorageService, BlobStorageService>();
        services.AddScoped<IMessagePublisher, RabbitMQPublisher>();

        // Settings
        services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQ"));
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

        // RabbitMQ consumer background service
        services.AddHostedService<RabbitMQConsumerService>();

        return services;
    }
}
