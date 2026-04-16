using FindingsAPI.Gateway.Application.Behaviours;
using FindingsAPI.Gateway.Domain.Interfaces;
using FindingsAPI.Gateway.Infrastructure.Data;
using FindingsAPI.Gateway.Infrastructure.Messaging;
using FindingsAPI.Gateway.Infrastructure.Repositories;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FindingsAPI.Gateway.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceExtensions).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

        services.AddDbContext<FindingsDomainDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
        services.AddScoped<IFindingsDomainRepository, EfFindingsDomainRepository>();

        return services;
    }

    public static IServiceCollection AddMessagingServices(this IServiceCollection services, IConfiguration config)
    {
        if (!config.GetValue<bool>("RabbitMQ:Enabled"))
            return services;

        services.AddMassTransit(x =>
        {
            x.AddConsumer<FindingCreatedConsumer>();
            x.AddConsumer<FindingClosedConsumer>();
            x.AddConsumer<FindingResponseAddedConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(config["RabbitMQ:Host"] ?? "localhost", "/", h =>
                {
                    h.Username(config["RabbitMQ:Username"] ?? "guest");
                    h.Password(config["RabbitMQ:Password"] ?? "guest");
                });
                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }

    public static IServiceCollection AddHealthCheckServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddHealthChecks()
            .AddSqlServer(config.GetConnectionString("DefaultConnection")!, name: "database", tags: new[] { "db", "sql" });
        return services;
    }
}
