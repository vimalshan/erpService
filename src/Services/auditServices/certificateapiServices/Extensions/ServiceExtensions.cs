using CertificateService.Application.Behaviours;
using CertificateService.Domain.Interfaces;
using CertificateService.Infrastructure.Data;
using CertificateService.Infrastructure.Messaging;
using CertificateService.Infrastructure.Repositories;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CertificateService.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        services.AddValidatorsFromAssembly(typeof(Program).Assembly);
        services.AddDbContext<CertificateDomainDbContext>(o => o.UseSqlServer(config.GetConnectionString("DefaultConnection")));
        services.AddScoped<ICertificateDomainRepository, EfCertificateDomainRepository>();
        return services;
    }

    public static IServiceCollection AddMessagingServices(this IServiceCollection services, IConfiguration config)
    {
        if (!config.GetValue<bool>("RabbitMQ:Enabled"))
            return services;

        services.AddMassTransit(x =>
        {
            x.AddConsumer<CertificateIssuedConsumer>();
            x.UsingRabbitMq((ctx, cfg) => { cfg.Host(config["RabbitMQ:Host"], "/", h => { h.Username(config["RabbitMQ:Username"] ?? "guest"); h.Password(config["RabbitMQ:Password"] ?? "guest"); }); cfg.ConfigureEndpoints(ctx); });
        });
        return services;
    }

    public static IServiceCollection AddHealthCheckServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddHealthChecks()
            .AddDbContextCheck<CertificateDomainDbContext>("database");
        return services;
    }
}
