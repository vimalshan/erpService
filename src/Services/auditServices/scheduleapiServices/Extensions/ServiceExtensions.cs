using ScheduleService.Application.Behaviours;
using ScheduleService.Domain.Events;
using ScheduleService.Domain.Interfaces;
using ScheduleService.Infrastructure.Data;
using ScheduleService.Infrastructure.Messaging;
using ScheduleService.Infrastructure.Repositories;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ScheduleService.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceExtensions).Assembly));
        services.AddValidatorsFromAssembly(typeof(ServiceExtensions).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        services.AddDbContext<ScheduleDomainDbContext>(o => o.UseSqlServer(config.GetConnectionString("DefaultConnection")));
        services.AddScoped<IScheduleDomainRepository, EfScheduleDomainRepository>();
        return services;
    }

    public static IServiceCollection AddMessagingServices(this IServiceCollection services, IConfiguration config)
    {
        if (!config.GetValue<bool>("RabbitMQ:Enabled"))
        {
            // Register in-memory bus so IPublishEndpoint is available for MediatR handlers
            services.AddMassTransit(x =>
            {
                x.AddConsumer<AuditScheduledConsumer>();
                x.AddConsumer<AuditCompletedConsumer>();
                x.UsingInMemory((ctx, cfg) => cfg.ConfigureEndpoints(ctx));
            });

            // Register MediatR bridge handlers
            services.AddTransient<INotificationHandler<AuditScheduledEvent>, AuditScheduledEventHandler>();
            services.AddTransient<INotificationHandler<AuditCompletedEvent>, AuditCompletedEventHandler>();
            return services;
        }

        var virtualHost = config["RabbitMQ:VirtualHost"] ?? "/";

        services.AddMassTransit(x =>
        {
            x.AddConsumer<AuditScheduledConsumer>();
            x.AddConsumer<AuditCompletedConsumer>();
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(config["RabbitMQ:Host"] ?? "localhost", virtualHost, h =>
                {
                    h.Username(config["RabbitMQ:Username"] ?? "guest");
                    h.Password(config["RabbitMQ:Password"] ?? "guest");
                });
                cfg.ConfigureEndpoints(ctx);
            });
        });

        // Register MediatR bridge handlers that forward domain events to RabbitMQ via IPublishEndpoint
        services.AddTransient<INotificationHandler<AuditScheduledEvent>, AuditScheduledEventHandler>();
        services.AddTransient<INotificationHandler<AuditCompletedEvent>, AuditCompletedEventHandler>();

        return services;
    }

    public static IServiceCollection AddHealthCheckServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddHealthChecks().AddDbContextCheck<ScheduleDomainDbContext>("ScheduleDb");
        return services;
    }
}
