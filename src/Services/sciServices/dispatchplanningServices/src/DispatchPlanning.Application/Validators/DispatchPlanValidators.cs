using FluentValidation;
using DispatchPlanning.Application.Features.DispatchPlans.Commands;

namespace DispatchPlanning.Application.Validators;

public class CreateDispatchPlanValidator : AbstractValidator<CreateDispatchPlanCommand>
{
    public CreateDispatchPlanValidator()
    {
        RuleFor(x => x.PlanType)
            .Must(t => t == 'I' || t == 'S')
            .WithMessage("PlanType must be 'I' (Itemwise) or 'S' (SubGroupwise).");

        RuleFor(x => x.PlanMonth)
            .NotEmpty()
            .WithMessage("PlanMonth is required.");

        RuleFor(x => x.CompanyUnitId)
            .GreaterThan(0)
            .WithMessage("CompanyUnitId must be positive.");

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0)
            .WithMessage("ModifiedBy must be a valid user ID.");
    }
}

public class AddDispatchPlanItemValidator : AbstractValidator<AddDispatchPlanItemCommand>
{
    public AddDispatchPlanItemValidator()
    {
        RuleFor(x => x.PlanHeaderId).GreaterThan(0);
        RuleFor(x => x.BreakupItemId).GreaterThan(0);
        RuleFor(x => x.ModifiedBy).GreaterThan(0);

        RuleFor(x => x.TargetWeek1).GreaterThanOrEqualTo(0).When(x => x.TargetWeek1.HasValue);
        RuleFor(x => x.TargetWeek2).GreaterThanOrEqualTo(0).When(x => x.TargetWeek2.HasValue);
        RuleFor(x => x.TargetWeek3).GreaterThanOrEqualTo(0).When(x => x.TargetWeek3.HasValue);
        RuleFor(x => x.TargetWeek4).GreaterThanOrEqualTo(0).When(x => x.TargetWeek4.HasValue);
        RuleFor(x => x.TargetWeek5).GreaterThanOrEqualTo(0).When(x => x.TargetWeek5.HasValue);
    }
}
