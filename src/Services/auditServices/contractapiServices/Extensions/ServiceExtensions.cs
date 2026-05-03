using ContractService.Application.Behaviours;
using ContractService.Domain.Interfaces;
using ContractService.Infrastructure.Data;
using ContractService.Infrastructure.Messaging;
using ContractService.Infrastructure.Repositories;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContractService.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceExtensions).Assembly));
        services.AddValidatorsFromAssembly(typeof(ServiceExtensions).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        services.AddDbContext<ContractDomainDbContext>(o => o.UseSqlServer(config.GetConnectionString("DefaultConnection")));
        services.AddScoped<IContractDomainRepository, EfContractDomainRepository>();
        return services;
    }

    public static IServiceCollection AddMessagingServices(this IServiceCollection services, IConfiguration config)
    {
        if (!config.GetValue<bool>("RabbitMQ:Enabled"))
            return services;

        var virtualHost = config["RabbitMQ:VirtualHost"] ?? "/";

        services.AddMassTransit(x =>
        {
            x.AddConsumer<ContractCreatedConsumer>();
            x.AddConsumer<ContractStatusChangedConsumer>();
            x.AddConsumer<ContractRenewedConsumer>();
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
        return services;
    }

    public static IServiceCollection AddHealthCheckServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddHealthChecks().AddDbContextCheck<ContractDomainDbContext>("ContractDb");
        return services;
    }
}
