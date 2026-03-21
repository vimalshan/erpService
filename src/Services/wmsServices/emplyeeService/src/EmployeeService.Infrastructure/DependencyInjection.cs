using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EmployeeService.Application.Interfaces;
using EmployeeService.Domain.Interfaces;
using EmployeeService.Infrastructure.Dapper;
using EmployeeService.Infrastructure.HealthChecks;
using EmployeeService.Infrastructure.Messaging;
using EmployeeService.Infrastructure.Persistence;
using EmployeeService.Infrastructure.Repositories;
using EmployeeService.Infrastructure.Services;

namespace EmployeeService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<EmployeeDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(EmployeeDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();

        // Dapper
        services.AddSingleton<DapperContext>();
        services.AddScoped<DapperEmployeeRepository>();

        // Messaging
        services.AddSingleton<IMessagePublisher, RabbitMqMessagePublisher>();
        services.AddHostedService<EmployeeMessageConsumer>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        // Health Checks
        services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("database");

        return services;
    }
}
