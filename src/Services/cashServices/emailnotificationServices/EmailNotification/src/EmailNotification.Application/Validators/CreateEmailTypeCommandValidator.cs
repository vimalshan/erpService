using FluentValidation;

namespace EmailNotification.Application.Validators;

/// <summary>
/// Validator for CreateEmailTypeCommand
/// </summary>
public class CreateEmailTypeCommandValidator : AbstractValidator<Commands.CreateEmailTypeCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateEmailTypeCommandValidator class
    /// </summary>
    public CreateEmailTypeCommandValidator()
    {
        RuleFor(x => x.EmailName)
            .NotEmpty().WithMessage("Email name is required")
            .MaximumLength(500).WithMessage("Email name cannot exceed 500 characters");

        RuleFor(x => x.EmailType)
            .NotEmpty().WithMessage("Email type is required")
            .Must(x => x == "D" || x == "E").WithMessage("Email type must be 'D' (Daily) or 'E' (Event)");

        RuleFor(x => x.EmailProcName)
            .NotEmpty().WithMessage("Procedure name is required")
            .MaximumLength(100).WithMessage("Procedure name cannot exceed 100 characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be a valid user ID");
    }
}
