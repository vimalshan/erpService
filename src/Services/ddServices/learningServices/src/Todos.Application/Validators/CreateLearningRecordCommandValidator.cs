using FluentValidation;
using Todos.Application.Commands;

namespace Todos.Application.Validators;

/// <summary>
/// Validator for CreateLearningRecordCommand
/// </summary>
public class CreateLearningRecordCommandValidator : AbstractValidator<CreateLearningRecordCommand>
{
    public CreateLearningRecordCommandValidator()
    {
        RuleFor(x => x.RequestNumber)
            .GreaterThan(0)
            .WithMessage("Request number must be greater than 0");

        RuleFor(x => x.LetId)
            .GreaterThan(0)
            .WithMessage("LET ID must be greater than 0");

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0)
            .WithMessage("ModifiedBy must be a valid user ID");

        RuleFor(x => x.EmployeeId)
            .MaximumLength(30)
            .WithMessage("Employee ID cannot exceed 30 characters")
            .When(x => !string.IsNullOrEmpty(x.EmployeeId));

        RuleFor(x => x.SpecificNeed)
            .MaximumLength(2000)
            .WithMessage("Specific need cannot exceed 2000 characters")
            .When(x => !string.IsNullOrEmpty(x.SpecificNeed));
    }
}
