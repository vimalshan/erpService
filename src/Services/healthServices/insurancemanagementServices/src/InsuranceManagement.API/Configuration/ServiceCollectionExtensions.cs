using MediatR;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using InsuranceManagement.Application.Mappings;

namespace InsuranceManagement.API.Configuration;

/// <summary>
/// Extension methods for registering application services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add Insurance Management application services
    /// </summary>
    public static IServiceCollection AddInsuranceManagementApplicationServices(
        this IServiceCollection services)
    {
        // MediatR - Register handlers from both API and Application assemblies
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
            // Also register from Application assembly for handlers and behaviors
            config.RegisterServicesFromAssembly(typeof(InsuranceManagement.Application.CQRS.Handlers.CreateInsurancePlanCommandHandler).Assembly);
            
            // Register resilience behavior
            config.AddOpenBehavior(typeof(InsuranceManagement.Application.CQRS.Behaviors.ResiliencePolicyBehavior<,>));
        });

        // Validators
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);

        return services;
    }

    /// <summary>
    /// Add AutoMapper configuration
    /// </summary>
    public static IServiceCollection AddAutoMapperConfiguration(
        this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        return services;
    }

    /// <summary>
    /// Add JWT Authentication
    /// </summary>
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var authority = configuration["Authentication:Authority"] 
            ?? "https://localhost:7001";
        var audience = configuration["Authentication:Audience"] 
            ?? "insurance-api";

        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = authority;
                options.Audience = audience;
                options.TokenValidationParameters.ValidateAudience = false;
                options.TokenValidationParameters.ValidateIssuer = false;
            });

        services.AddAuthorization();

        return services;
    }
}
