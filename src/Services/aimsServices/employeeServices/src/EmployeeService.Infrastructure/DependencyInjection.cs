using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using EmployeeService.Application.Interfaces;
using EmployeeService.Domain.Interfaces;
using EmployeeService.Infrastructure.Dapper;
using EmployeeService.Infrastructure.Messaging;
using EmployeeService.Infrastructure.Persistence;
using EmployeeService.Infrastructure.Repositories;
using EmployeeService.Infrastructure.Services;
using EmployeeService.Infrastructure.Storage;

namespace EmployeeService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<EmployeeDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("EmployeeDb"),
                sql => sql.MigrationsAssembly(typeof(EmployeeDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IEmployeeTimeInfoRepository, EmployeeTimeInfoRepository>();
        services.AddScoped<IEmployeeApproverRepository, EmployeeApproverRepository>();
        services.AddScoped<IEmployeeCalendarRepository, EmployeeCalendarRepository>();
        services.AddScoped<IEmployeePatternRepository, EmployeePatternRepository>();
        services.AddScoped<IEmployeeShiftRepository, EmployeeShiftRepository>();
        services.AddScoped<IEmployeeShiftPatternRepository, EmployeeShiftPatternRepository>();

        // Dapper
        services.AddSingleton<DapperService>();

        // RabbitMQ
        services.AddSingleton<IConnectionFactory>(_ =>
        {
            var cfg = configuration.GetSection("RabbitMQ");
            return new ConnectionFactory
            {
                HostName = cfg["Host"] ?? "localhost",
                Port = int.TryParse(cfg["Port"], out var port) ? port : 5672,
                UserName = cfg["Username"] ?? "guest",
                Password = cfg["Password"] ?? "guest",
                VirtualHost = cfg["VirtualHost"] ?? "/"
            };
        });
        services.AddSingleton<RabbitMqPublisher>();
        services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();
        services.AddSingleton<AttendanceFlagConsumer>();
        services.AddSingleton<ApproverAssignmentConsumer>();
        services.AddHostedService<RabbitMqConsumerHostedService>();

        // Blob Storage
        services.AddSingleton<BlobStorageService>();

        return services;
    }
}
