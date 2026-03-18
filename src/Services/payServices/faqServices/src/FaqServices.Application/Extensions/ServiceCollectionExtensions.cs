using AutoMapper;
using FluentValidation;
using FaqServices.Application.Common.Mappings;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FaqServices.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

        // AutoMapper
        services.AddAutoMapper(typeof(FaqMappingProfile));

        // Fluent Validation
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);

        return services;
    }
}
