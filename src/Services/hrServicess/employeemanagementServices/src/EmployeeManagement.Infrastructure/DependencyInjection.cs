using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Domain.Interfaces;
using EmployeeManagement.Infrastructure.Dapper;
using EmployeeManagement.Infrastructure.Identity;
using EmployeeManagement.Infrastructure.Messaging;
using EmployeeManagement.Infrastructure.Persistence;
using EmployeeManagement.Infrastructure.Persistence.Repositories;
using EmployeeManagement.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace EmployeeManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IPromotionRepository, PromotionRepository>();
        services.AddScoped<ITransferRepository, TransferRepository>();
        services.AddScoped<IProbationRepository, ProbationRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Services
        services.AddSingleton<IDapperQueryService, DapperQueryService>();
        services.AddSingleton<IBlobStorageService, BlobStorageService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        // RabbitMQ — optional: degrade gracefully if broker is unavailable
        services.AddSingleton<IConnection?>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<RabbitMqMessagePublisher>>();
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = configuration["RabbitMQ:Host"] ?? "localhost",
                    Port = int.Parse(configuration["RabbitMQ:Port"] ?? "5672"),
                    UserName = configuration["RabbitMQ:Username"] ?? "guest",
                    Password = configuration["RabbitMQ:Password"] ?? "guest",
                    RequestedConnectionTimeout = TimeSpan.FromSeconds(5)
                };
                return factory.CreateConnectionAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "RabbitMQ unavailable at startup — messaging disabled. Start RabbitMQ and restart the service to enable it.");
                return null;
            }
        });

        services.AddSingleton<IChannel?>(sp =>
        {
            var connection = sp.GetService<IConnection>();
            if (connection is null) return null;
            return connection.CreateChannelAsync().GetAwaiter().GetResult();
        });

        services.AddScoped<IMessagePublisher, RabbitMqMessagePublisher>();
        services.AddHostedService<EmployeeEventConsumer>();

        return services;
    }
}
