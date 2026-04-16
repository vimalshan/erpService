using ActionService.Application.Behaviours;
using ActionService.Data;
using ActionService.Domain.Interfaces;
using ActionService.Infrastructure.Data;
using ActionService.Infrastructure.Messaging;
using ActionService.Infrastructure.Repositories;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ActionService.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // MediatR + Pipeline Behaviours
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

        // FluentValidation
        services.AddValidatorsFromAssembly(typeof(Program).Assembly);

        // EF Core
        services.AddDbContext<ActionDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Dapper
        services.AddSingleton<DapperContext>();

        // Repositories
        services.AddScoped<IActionRepository, EfActionRepository>();

        return services;
    }

    public static IServiceCollection AddMessagingServices(this IServiceCollection services, IConfiguration configuration)
    {
        if (!configuration.GetValue<bool>("RabbitMQ:Enabled"))
            return services;

        services.AddMassTransit(x =>
        {
            x.AddConsumer<ActionCreatedConsumer>();
            x.AddConsumer<ActionCompletedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMQ:Host"], "/", h =>
                {
                    h.Username(configuration["RabbitMQ:Username"] ?? "guest");
                    h.Password(configuration["RabbitMQ:Password"] ?? "guest");
                });
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }

    public static IServiceCollection AddHealthCheckServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddDbContextCheck<ActionDbContext>("database");

        return services;
    }
}
