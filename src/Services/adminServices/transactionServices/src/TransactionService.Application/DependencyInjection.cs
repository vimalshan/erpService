namespace TransactionService.Application;

using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TransactionService.Application.Behaviours;
using TransactionService.Application.Commands.SubmitRequest;
using TransactionService.Application.Mappings;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(SubmitRequestCommand).Assembly;

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
        });

        services.AddValidatorsFromAssembly(assembly);
        services.AddAutoMapper(cfg => cfg.AddProfile<TransactionMappingProfile>());

        return services;
    }
}
