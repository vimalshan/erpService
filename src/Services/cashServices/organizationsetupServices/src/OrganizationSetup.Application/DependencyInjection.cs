using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OrganizationSetup.Application.Behaviors;
using OrganizationSetup.Application.Common;

namespace OrganizationSetup.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        // Validators
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        // AutoMapper
        services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

        return services;
    }
}

