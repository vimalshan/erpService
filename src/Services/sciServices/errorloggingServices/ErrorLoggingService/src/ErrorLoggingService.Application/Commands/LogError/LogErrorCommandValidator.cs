using FluentValidation;

namespace ErrorLoggingService.Application.Commands.LogError;

public sealed class LogErrorCommandValidator : AbstractValidator<LogErrorCommand>
{
    public LogErrorCommandValidator()
    {
        RuleFor(x => x.ErrorMessage)
            .NotEmpty().WithMessage("Error message is required.")
            .MaximumLength(4000).WithMessage("Error message must not exceed 4000 characters.");

        RuleFor(x => x.StoredProcedureName)
            .NotEmpty().WithMessage("Stored procedure name is required.")
            .MaximumLength(100).WithMessage("Stored procedure name must not exceed 100 characters.");
    }
}
