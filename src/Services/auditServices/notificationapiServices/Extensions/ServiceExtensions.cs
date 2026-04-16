using NotificationService.Application.Behaviours;
using NotificationService.Domain.Interfaces;
using NotificationService.Infrastructure.Data;
using NotificationService.Infrastructure.Messaging;
using NotificationService.Infrastructure.Repositories;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace NotificationService.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceExtensions).Assembly));
        services.AddValidatorsFromAssembly(typeof(ServiceExtensions).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        services.AddDbContext<NotificationDomainDbContext>(o => o.UseSqlServer(config.GetConnectionString("DefaultConnection")));
        services.AddScoped<INotificationDomainRepository, EfNotificationDomainRepository>();
        return services;
    }

    public static IServiceCollection AddMessagingServices(this IServiceCollection services, IConfiguration config)
    {
        if (!config.GetValue<bool>("RabbitMQ:Enabled"))
            return services;

        services.AddMassTransit(x =>
        {
            x.AddConsumer<NotificationCreatedConsumer>();
            x.AddConsumer<NotificationReadConsumer>();
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(config["RabbitMQ:Host"] ?? "localhost", h =>
                {
                    h.Username(config["RabbitMQ:Username"] ?? "guest");
                    h.Password(config["RabbitMQ:Password"] ?? "guest");
                });
                cfg.ConfigureEndpoints(ctx);
            });
        });
        return services;
    }

    public static IServiceCollection AddHealthCheckServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddHealthChecks().AddDbContextCheck<NotificationDomainDbContext>("NotificationDb");
        return services;
    }
}
