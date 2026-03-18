using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using DocumentService.Application.Behaviours;
using DocumentService.Application.Validators;

namespace DocumentService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile<Mappings.LoanDocumentProfile>());

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });

        services.AddScoped<IValidator<Commands.CreateLoanDocument.CreateLoanDocumentCommand>, CreateLoanDocumentCommandValidator>();
        services.AddScoped<IValidator<Commands.UpdateLoanDocument.UpdateLoanDocumentCommand>, UpdateLoanDocumentCommandValidator>();

        return services;
    }
}
