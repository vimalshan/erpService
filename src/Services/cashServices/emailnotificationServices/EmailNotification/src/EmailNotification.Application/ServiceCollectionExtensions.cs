using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using AutoMapper;
using FluentValidation.AspNetCore;

namespace EmailNotification.Application;

/// <summary>
/// Extension methods for configuring application services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds application layer services to the dependency injection container
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

        // Register FluentValidation
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);

        // Register AutoMapper
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);

        // Register Domain Event Dispatcher and Handlers
        services.AddScoped<Services.IDomainEventDispatcher, Services.DomainEventDispatcher>();

        return services;
    }
}
