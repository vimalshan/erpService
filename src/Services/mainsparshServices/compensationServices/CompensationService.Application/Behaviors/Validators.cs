using FluentValidation;
using CompensationService.Application.Commands;

namespace CompensationService.Application.Behaviors;

/// <summary>
/// Validator for CreateCompensationGradeCommand
/// </summary>
public class CreateCompensationGradeCommandValidator : AbstractValidator<CreateCompensationGradeCommand>
{
    public CreateCompensationGradeCommandValidator()
    {
        RuleFor(x => x.GradeCode)
            .NotEmpty().WithMessage("Grade code is required")
            .MaximumLength(50).WithMessage("Grade code must not exceed 50 characters");

        RuleFor(x => x.GradeName)
            .NotEmpty().WithMessage("Grade name is required")
            .MaximumLength(255).WithMessage("Grade name must not exceed 255 characters");

        RuleFor(x => x.GradeLevel)
            .GreaterThan(0).WithMessage("Grade level must be greater than 0");

        RuleFor(x => x.BaseSalary)
            .GreaterThan(0).WithMessage("Base salary must be greater than 0");

        RuleFor(x => x.HraPercentage)
            .InclusiveBetween(0, 100).WithMessage("HRA percentage must be between 0 and 100");

        RuleFor(x => x.DaPercentage)
            .InclusiveBetween(0, 100).WithMessage("DA percentage must be between 0 and 100");

        RuleFor(x => x.EffectiveFrom)
            .NotEmpty().WithMessage("Effective from date is required");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("Created by user ID must be valid");
    }
}

/// <summary>
/// Validator for UpdateCompensationGradeCommand
/// </summary>
public class UpdateCompensationGradeCommandValidator : AbstractValidator<UpdateCompensationGradeCommand>
{
    public UpdateCompensationGradeCommandValidator()
    {
        RuleFor(x => x.GradeId)
            .GreaterThan(0).WithMessage("Grade ID must be valid");

        RuleFor(x => x.GradeName)
            .NotEmpty().WithMessage("Grade name is required")
            .MaximumLength(255).WithMessage("Grade name must not exceed 255 characters");

        RuleFor(x => x.BaseSalary)
            .GreaterThan(0).WithMessage("Base salary must be greater than 0");

        RuleFor(x => x.HraPercentage)
            .InclusiveBetween(0, 100).WithMessage("HRA percentage must be between 0 and 100");

        RuleFor(x => x.DaPercentage)
            .InclusiveBetween(0, 100).WithMessage("DA percentage must be between 0 and 100");

        RuleFor(x => x.UpdatedBy)
            .GreaterThan(0).WithMessage("Updated by user ID must be valid");
    }
}

/// <summary>
/// Validator for ChangeCompensationGradeStatusCommand
/// </summary>
public class ChangeCompensationGradeStatusCommandValidator : AbstractValidator<ChangeCompensationGradeStatusCommand>
{
    public ChangeCompensationGradeStatusCommandValidator()
    {
        RuleFor(x => x.GradeId)
            .GreaterThan(0).WithMessage("Grade ID must be valid");

        RuleFor(x => x.NewStatus)
            .Must(x => x == 'A' || x == 'I').WithMessage("Status must be 'A' (Active) or 'I' (Inactive)");

        RuleFor(x => x.ChangedBy)
            .GreaterThan(0).WithMessage("Changed by user ID must be valid");
    }
}
