using MenuAndSecurityService.Domain.Interfaces;
using MenuAndSecurityService.Infrastructure.BlobStorage;
using MenuAndSecurityService.Infrastructure.Dapper;
using MenuAndSecurityService.Infrastructure.Messaging;
using MenuAndSecurityService.Infrastructure.Messaging.Consumers;
using MenuAndSecurityService.Infrastructure.Persistence;
using MenuAndSecurityService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MenuAndSecurityService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<MenuSecurityDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(MenuSecurityDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<IRoleMenuAccessRepository, RoleMenuAccessRepository>();

        // Dapper
        services.AddSingleton<DapperContext>();
        services.AddScoped<DapperMenuRepository>();

        // RabbitMQ
        services.AddSingleton<RabbitMqConnection>();
        services.AddSingleton<IMessagePublisher, MessagePublisher>();
        services.AddHostedService<MenuAccessConsumer>();
        services.AddHostedService<RoleChangeConsumer>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        return services;
    }
}
