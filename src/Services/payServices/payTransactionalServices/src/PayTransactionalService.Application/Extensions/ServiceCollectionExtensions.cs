using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PayTransactionalService.Application.Commands;
using PayTransactionalService.Application.Mappings;
using PayTransactionalService.Application.Validators;

namespace PayTransactionalService.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile<PayTransactionalMappingProfile>());

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

        services.AddScoped<IValidator<CreatePayTransactionCommand>, CreatePayTransactionCommandValidator>();
        services.AddScoped<IValidator<CreatePayArrearCommand>, CreatePayArrearCommandValidator>();
        services.AddScoped<IValidator<CreatePayAdjustmentCommand>, CreatePayAdjustmentCommandValidator>();
        services.AddScoped<IValidator<ProcessMonthlySalaryCommand>, ProcessMonthlySalaryCommandValidator>();

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}

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
