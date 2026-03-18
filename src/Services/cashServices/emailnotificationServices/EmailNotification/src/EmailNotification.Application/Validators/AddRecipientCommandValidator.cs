using FluentValidation;

namespace EmailNotification.Application.Validators;

/// <summary>
/// Validator for AddRecipientCommand
/// </summary>
public class AddRecipientCommandValidator : AbstractValidator<Commands.AddRecipientCommand>
{
    /// <summary>
    /// Initializes a new instance of the AddRecipientCommandValidator class
    /// </summary>
    public AddRecipientCommandValidator()
    {
        RuleFor(x => x.EmailTypeId)
            .GreaterThan(0).WithMessage("Email type ID must be greater than 0");

        RuleFor(x => x.EmailAddress)
            .NotEmpty().WithMessage("Email address is required")
            .EmailAddress().WithMessage("Email address format is invalid")
            .MaximumLength(200).WithMessage("Email address cannot exceed 200 characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be a valid user ID");

        RuleFor(x => x.RecipientName)
            .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.RecipientName))
            .WithMessage("Recipient name cannot exceed 100 characters");
    }
}
