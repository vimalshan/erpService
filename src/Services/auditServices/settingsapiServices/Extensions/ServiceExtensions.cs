using SettingsService.Application.Behaviours;
using SettingsService.Domain.Events;
using SettingsService.Domain.Interfaces;
using SettingsService.Infrastructure.Data;
using SettingsService.Infrastructure.Messaging;
using SettingsService.Infrastructure.Repositories;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace SettingsService.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceExtensions).Assembly));
        services.AddValidatorsFromAssembly(typeof(ServiceExtensions).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        services.AddDbContext<SettingsDomainDbContext>(o => o.UseSqlServer(config.GetConnectionString("DefaultConnection")));
        services.AddScoped<ISettingsDomainRepository, EfSettingsDomainRepository>();
        return services;
    }

    public static IServiceCollection AddMessagingServices(this IServiceCollection services, IConfiguration config)
    {
        if (!config.GetValue<bool>("RabbitMQ:Enabled"))
            return services;

        services.AddTransient<INotificationHandler<UserCreatedEvent>, UserCreatedEventHandler>();
        services.AddTransient<INotificationHandler<UserDeactivatedEvent>, UserDeactivatedEventHandler>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<UserCreatedConsumer>();
            x.AddConsumer<UserDeactivatedConsumer>();
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
        services.AddHealthChecks().AddDbContextCheck<SettingsDomainDbContext>("SettingsDb");
        return services;
    }
}
