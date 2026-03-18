using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using AutoMapper;
using MediatR;
using LoanApplication.Application.Mappings;

namespace LoanApplication.Application;

/// <summary>
/// Application layer service registration
/// </summary>
public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register MediatR
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(typeof(ApplicationServiceRegistration).Assembly);
        });

        // Register validators
        services.AddValidatorsFromAssembly(typeof(ApplicationServiceRegistration).Assembly);

        // Register AutoMapper
        services.AddAutoMapper(typeof(LoanApplicationMappingProfile));

        return services;
    }
}
