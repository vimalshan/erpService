using MediatR;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using ReferenceService.Application.Behaviors;
using ReferenceService.Application.Mappings;

namespace ReferenceService.Application;

/// <summary>
/// Extension methods for registering application services.
/// </summary>
public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register MediatR
        services.AddMediatR(cfg => 
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationServiceCollectionExtensions).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        });
        
        // Register AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));
        
        return services;
    }
}
