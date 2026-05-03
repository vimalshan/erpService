using AuditService.Application.Behaviours;
using AuditService.Domain.Interfaces;
using AuditService.Infrastructure.Data;
using AuditService.Infrastructure.Messaging;
using AuditService.Infrastructure.Repositories;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AuditService.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        services.AddValidatorsFromAssembly(typeof(Program).Assembly);
        services.AddDbContext<AuditDomainDbContext>(options =>
            options
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));
        services.AddScoped<IAuditDomainRepository, EfAuditDomainRepository>();
        return services;
    }

    public static IServiceCollection AddMessagingServices(this IServiceCollection services, IConfiguration configuration)
    {
        if (!configuration.GetValue<bool>("RabbitMQ:Enabled"))
            return services;

        var virtualHost = configuration["RabbitMQ:VirtualHost"] ?? "/";

        services.AddMassTransit(x =>
        {
            x.AddConsumer<AuditCreatedConsumer>();
            x.AddConsumer<AuditStatusChangedConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMQ:Host"], virtualHost, h =>
                {
                    h.Username(configuration["RabbitMQ:Username"] ?? "guest");
                    h.Password(configuration["RabbitMQ:Password"] ?? "guest");
                    h.RequestedConnectionTimeout(TimeSpan.FromSeconds(10));
                });
                cfg.UseMessageRetry(r => r.Intervals(1000, 2000, 5000));
                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }

    public static IServiceCollection AddHealthCheckServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddDbContextCheck<AuditDomainDbContext>("database");

        return services;
    }
}
