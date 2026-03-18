using FluentValidation;

namespace TrainingDevelopment.Application.Features.TrainingDetails.Commands.CreateTrainingDetail;

public class CreateTrainingDetailCommandValidator : AbstractValidator<CreateTrainingDetailCommand>
{
    public CreateTrainingDetailCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Training ID must be greater than 0.");
        RuleFor(x => x.FinancialYear).GreaterThan(0).WithMessage("Financial year is required.");
        RuleFor(x => x.EmployeeSysId).GreaterThan(0).WithMessage("Employee ID is required.");
        RuleFor(x => x.TrainingNeed).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.GapArea).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.Mode).InclusiveBetween(1, 2).WithMessage("Mode must be 1 (On-The-Job) or 2 (Classroom).");
        RuleFor(x => x.ProgramId).GreaterThan(0);
        RuleFor(x => x.ProgramDescription).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.PlannedFrom).NotEmpty();
        RuleFor(x => x.PlannedTo).GreaterThan(x => x.PlannedFrom)
            .WithMessage("Planned end date must be after planned start date.");
    }
}
