using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MediatR;
using FluentValidation;

#nullable enable

namespace MasterData.Application.Behaviors
{
    /// <summary>
    /// Validation behavior for MediatR pipeline
    /// </summary>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken))
            );

            var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }

            return await next();
        }
    }

    /// <summary>
    /// Logging behavior for MediatR pipeline
    /// </summary>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Handling {requestName}");

            try
            {
                var response = await next();
                Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Completed {requestName}");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Error in {requestName}: {ex.Message}");
                throw;
            }
        }
    }

    /// <summary>
    /// Performance monitoring behavior for MediatR pipeline
    /// </summary>
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var startTime = DateTime.UtcNow;
            var response = await next();
            var elapsedTime = DateTime.UtcNow - startTime;

            if (elapsedTime.TotalMilliseconds > 1000)
            {
                var requestName = typeof(TRequest).Name;
                Console.WriteLine($"[PERFORMANCE] {requestName} took {elapsedTime.TotalMilliseconds}ms");
            }

            return response;
        }
    }
}

namespace MasterData.Application.Validators
{
    using Commands.CompanyUnit;
    using Commands.Location;
    using Commands.Supplier;
    using Commands.State;
    using Commands.City;

    /// <summary>
    /// Validator for CreateCompanyUnitCommand
    /// </summary>
    public class CreateCompanyUnitValidator : AbstractValidator<CreateCompanyUnitCommand>
    {
        public CreateCompanyUnitValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required")
                .Length(1, 3).WithMessage("Code must be between 1 and 3 characters");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(1000).WithMessage("Name must not exceed 1000 characters");
        }
    }

    /// <summary>
    /// Validator for CreateLocationCommand
    /// </summary>
    public class CreateLocationValidator : AbstractValidator<CreateLocationCommand>
    {
        public CreateLocationValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters");
        }
    }

    /// <summary>
    /// Validator for CreateSupplierCommand
    /// </summary>
    public class CreateSupplierValidator : AbstractValidator<CreateSupplierCommand>
    {
        public CreateSupplierValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required")
                .MaximumLength(25).WithMessage("Code must not exceed 25 characters");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

            RuleFor(x => x.EntryId)
                .NotEmpty().WithMessage("Entry ID is required")
                .MaximumLength(25).WithMessage("Entry ID must not exceed 25 characters");

            RuleFor(x => x.EntryNumber)
                .GreaterThan(0).WithMessage("Entry Number must be greater than 0");
        }
    }

    /// <summary>
    /// Validator for CreateStateCommand
    /// </summary>
    public class CreateStateValidator : AbstractValidator<CreateStateCommand>
    {
        public CreateStateValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required")
                .MaximumLength(100).WithMessage("Code must not exceed 100 characters");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters");
        }
    }

    /// <summary>
    /// Validator for CreateCityCommand
    /// </summary>
    public class CreateCityValidator : AbstractValidator<CreateCityCommand>
    {
        public CreateCityValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required")
                .MaximumLength(100).WithMessage("Code must not exceed 100 characters");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

            RuleFor(x => x.StateCode)
                .NotEmpty().WithMessage("State Code is required")
                .MaximumLength(100).WithMessage("State Code must not exceed 100 characters");
        }
    }
}
