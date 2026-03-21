using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecurityService.Application.Interfaces;
using SecurityService.Domain.Interfaces;
using SecurityService.Infrastructure.Dapper;
using SecurityService.Infrastructure.Messaging;
using SecurityService.Infrastructure.Messaging.Consumers;
using SecurityService.Infrastructure.Persistence;
using SecurityService.Infrastructure.Repositories;
using SecurityService.Infrastructure.Services;

namespace SecurityService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<SecurityDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("SecurityDb")));

        // Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();

        // Dapper queries
        services.AddScoped<IDapperUserQueries, DapperUserQueries>();

        // Services
        services.AddSingleton<ITokenService, JwtTokenService>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

        // RabbitMQ Consumers
        services.AddHostedService<UserCreatedConsumer>();
        services.AddHostedService<UserDeactivatedConsumer>();

        return services;
    }
}
