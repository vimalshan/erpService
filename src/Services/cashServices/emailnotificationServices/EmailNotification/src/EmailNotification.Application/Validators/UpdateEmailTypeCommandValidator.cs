using FluentValidation;

namespace EmailNotification.Application.Validators;

/// <summary>
/// Validator for UpdateEmailTypeCommand
/// </summary>
public class UpdateEmailTypeCommandValidator : AbstractValidator<Commands.UpdateEmailTypeCommand>
{
    /// <summary>
    /// Initializes a new instance of the UpdateEmailTypeCommandValidator class
    /// </summary>
    public UpdateEmailTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Email type ID must be greater than 0");

        RuleFor(x => x.EmailName)
            .NotEmpty().WithMessage("Email name is required")
            .MaximumLength(500).WithMessage("Email name cannot exceed 500 characters");

        RuleFor(x => x.EmailProcName)
            .NotEmpty().WithMessage("Procedure name is required")
            .MaximumLength(100).WithMessage("Procedure name cannot exceed 100 characters");

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0).WithMessage("ModifiedBy must be a valid user ID");
    }
}
