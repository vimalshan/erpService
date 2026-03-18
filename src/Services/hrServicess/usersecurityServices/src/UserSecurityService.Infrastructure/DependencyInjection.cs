using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserSecurityService.Application.Common;
using UserSecurityService.Domain.Interfaces;
using UserSecurityService.Infrastructure.Dapper;
using UserSecurityService.Infrastructure.Messaging;
using UserSecurityService.Infrastructure.Persistence;
using UserSecurityService.Infrastructure.Repositories;
using UserSecurityService.Infrastructure.Services;

namespace UserSecurityService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<UserSecurityDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sql => sql.EnableRetryOnFailure(3)));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UserSecurityDbContext>());

        // Repositories
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        services.AddScoped<IUserAppsMappingRepository, UserAppsMappingRepository>();
        services.AddScoped<IEmpPasswordChangeRepository, EmpPasswordChangeRepository>();

        // Dapper
        services.AddScoped<DapperUserRepository>();

        // Services
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IBlobStorageService, BlobStorageService>();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        // RabbitMQ consumers (hosted services)
        services.AddHostedService<UserCreatedConsumer>();
        services.AddHostedService<PasswordChangedConsumer>();

        return services;
    }
}
