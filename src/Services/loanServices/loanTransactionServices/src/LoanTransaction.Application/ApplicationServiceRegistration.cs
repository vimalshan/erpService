using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using LoanTransaction.Application.Behaviours;
using LoanTransaction.Application.Mappings;

namespace LoanTransaction.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationServiceRegistration).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });

        services.AddValidatorsFromAssembly(typeof(ApplicationServiceRegistration).Assembly);

        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<LoanTransactionMappingProfile>();
        });

        return services;
    }
}
