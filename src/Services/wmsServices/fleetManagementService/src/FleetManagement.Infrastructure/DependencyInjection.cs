using FleetManagement.Application.Interfaces;
using FleetManagement.Domain.Interfaces;
using FleetManagement.Infrastructure.Data;
using FleetManagement.Infrastructure.Repositories;
using FleetManagement.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace FleetManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<FleetDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sql => sql.EnableRetryOnFailure(3)));

        // Repositories & UoW
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IDriverRepository, DriverRepository>();
        services.AddScoped<IRouteRepository, RouteRepository>();
        services.AddScoped<ITripRepository, TripRepository>();
        services.AddScoped<IMaintenanceLogRepository, MaintenanceLogRepository>();
        services.AddScoped<IFuelLogRepository, FuelLogRepository>();

        // Dapper
        services.AddScoped<IDapperQueryService, DapperQueryService>();

        // Blob Storage
        services.AddSingleton<IBlobStorageService, BlobStorageService>();

        // RabbitMQ Publisher (lazy connection — won't fail if RabbitMQ is unavailable)
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

        // Register Infrastructure MediatR handlers (domain event → RabbitMQ bridge)
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        // RabbitMQ Consumers
        services.AddHostedService<MaintenanceScheduleConsumer>();
        services.AddHostedService<TripCompletedConsumer>();

        // Polly Circuit Breaker for HTTP clients
        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        var circuitBreakerPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

        services.AddHttpClient("ExternalApi")
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(circuitBreakerPolicy);

        return services;
    }
}
