using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TaxService.Application.Commands;
using TaxService.Application.DTOs;
using TaxService.Application.Mappings;
using TaxService.Application.Validators;

namespace TaxService.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register AutoMapper
        services.AddAutoMapper(cfg => cfg.AddProfile<TaxMappingProfile>());

        // Register MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

        // Register FluentValidation validators
        services.AddScoped<IValidator<CreateTaxMarginalDetailCommand>, CreateTaxMarginalDetailCommandValidator>();
        services.AddScoped<IValidator<CreateConditionalMasterCommand>, CreateConditionalMasterCommandValidator>();
        services.AddScoped<IValidator<CreateTaxExemptionDto>, CreateTaxExemptionDtoValidator>();
        services.AddScoped<IValidator<CreateTaxDeductionDto>, CreateTaxDeductionDtoValidator>();

        // Register validation behavior
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}

/// <summary>
/// MediatR pipeline behavior for validation
/// </summary>
public class ValidationBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Any())
            throw new ValidationException(failures);

        return await next();
    }
}
