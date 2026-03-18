using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using AutoMapper;
using EmployeeService.Application.Behaviors;
using EmployeeService.Application.Mappings;

namespace EmployeeService.Application;

/// <summary>
/// Extension method for registering application services
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>));
        });

        // Register Validators
        var validatorType = typeof(AbstractValidator<>);
        var validatorTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.BaseType != null 
                && t.BaseType.IsGenericType 
                && t.BaseType.GetGenericTypeDefinition() == validatorType);

        foreach (var validatorType1 in validatorTypes)
        {
            var baseType = validatorType1.BaseType;
            var genericArg = baseType?.GetGenericArguments().FirstOrDefault();
            if (genericArg != null)
            {
                var serviceType = typeof(IValidator<>).MakeGenericType(genericArg);
                services.AddScoped(serviceType, validatorType1);
            }
        }

        // Register AutoMapper
        services.AddAutoMapper(typeof(EmployeeMappingProfile));

        return services;
    }
}
