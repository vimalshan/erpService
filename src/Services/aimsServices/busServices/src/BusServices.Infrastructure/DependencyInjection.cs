using BusServices.Domain.Interfaces;
using BusServices.Infrastructure.Messaging;
using BusServices.Infrastructure.Messaging.RabbitMQ;
using BusServices.Infrastructure.Persistence;
using BusServices.Infrastructure.Persistence.Dapper;
using BusServices.Infrastructure.Persistence.Seed;
using BusServices.Infrastructure.Repositories;
using BusServices.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusServices.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        // EF Core
        services.AddDbContext<BusDbContext>(opts =>
            opts.UseSqlServer(
                config.GetConnectionString("BusDb"),
                sql => sql.MigrationsAssembly(typeof(DependencyInjection).Assembly.FullName)));

        // Repositories
        services.AddScoped<IBusRepository, BusRepository>();
        services.AddScoped<IBusRouteRepository, BusRouteRepository>();
        services.AddScoped<IEmployeeBusRepository, EmployeeBusRepository>();
        services.AddScoped<IBusArrivalRepository, BusArrivalRepository>();
        services.AddScoped<IBusDeductionRateRepository, BusDeductionRateRepository>();

        // Dapper
        services.AddSingleton<BusDapperQueries>();

        // Seeder
        services.AddTransient<BusDbSeeder>();

        // RabbitMQ
        services.Configure<RabbitMQSettings>(config.GetSection("RabbitMQ"));
        services.AddSingleton<IMessagePublisher, RabbitMQPublisher>();
        services.AddHostedService<RabbitMQConsumerHostedService>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        return services;
    }
}
