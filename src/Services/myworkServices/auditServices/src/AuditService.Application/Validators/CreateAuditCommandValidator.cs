using FluentValidation;
using AuditService.Application.Commands.Audits;

namespace AuditService.Application.Validators;

public class CreateAuditCommandValidator : AbstractValidator<CreateAuditCommand>
{
    public CreateAuditCommandValidator()
    {
        RuleFor(x => x.AuditId).GreaterThan(0);
        RuleFor(x => x.AuditName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.AuditUnit).GreaterThan(0);
        RuleFor(x => x.AuditFrom).LessThan(x => x.AuditTo).WithMessage("AuditFrom must be before AuditTo.");
        RuleFor(x => x.AuditDefLocation).NotEmpty().MaximumLength(50);
        RuleFor(x => x.AuditPlanFrom).LessThan(x => x.AuditPlanTo).WithMessage("PlanFrom must be before PlanTo.");
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}
