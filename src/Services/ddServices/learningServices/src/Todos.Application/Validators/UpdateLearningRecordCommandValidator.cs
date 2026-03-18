using FluentValidation;
using Todos.Application.Commands;

namespace Todos.Application.Validators;

/// <summary>
/// Validator for UpdateLearningRecordCommand
/// </summary>
public class UpdateLearningRecordCommandValidator : AbstractValidator<UpdateLearningRecordCommand>
{
    public UpdateLearningRecordCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ID is required");

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0)
            .WithMessage("ModifiedBy must be a valid user ID");

        RuleFor(x => x.BhrStatus)
            .Must(x => string.IsNullOrEmpty(x) || x == "Y" || x == "N")
            .WithMessage("BHR Status must be Y, N, or null")
            .When(x => !string.IsNullOrEmpty(x.BhrStatus));

        RuleFor(x => x.SpecificNeed)
            .MaximumLength(2000)
            .WithMessage("Specific need cannot exceed 2000 characters")
            .When(x => !string.IsNullOrEmpty(x.SpecificNeed));

        RuleFor(x => x.DevelopmentArea)
            .MaximumLength(2000)
            .WithMessage("Development area cannot exceed 2000 characters")
            .When(x => !string.IsNullOrEmpty(x.DevelopmentArea));
    }
}
