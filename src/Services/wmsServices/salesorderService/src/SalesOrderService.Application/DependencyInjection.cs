using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SalesOrderService.Application.Common.Behaviours;
using SalesOrderService.Application.Common.Mappings;
using System.Reflection;

namespace SalesOrderService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });

        services.AddValidatorsFromAssembly(assembly);

        services.AddAutoMapper(typeof(SalesOrderMappingProfile));

        return services;
    }
}
