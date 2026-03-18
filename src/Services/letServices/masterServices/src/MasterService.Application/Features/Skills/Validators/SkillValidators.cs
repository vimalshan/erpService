using FluentValidation;
using MasterService.Application.Features.Skills.Commands;

namespace MasterService.Application.Features.Skills.Validators;

public class CreateSkillCommandValidator : AbstractValidator<CreateSkillCommand>
{
    public CreateSkillCommandValidator()
    {
        RuleFor(x => x.SkillCode).GreaterThan(0).WithMessage("SkillCode must be positive.");
        RuleFor(x => x.SkillName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.SkillType).Must(t => "TBF".Contains(char.ToUpper(t)))
            .WithMessage("SkillType must be T (Technical), B (Behavioural), or F (Functional).");
        RuleFor(x => x.WeightNum).GreaterThanOrEqualTo(0).When(x => x.WeightNum.HasValue);
    }
}

public class UpdateSkillCommandValidator : AbstractValidator<UpdateSkillCommand>
{
    public UpdateSkillCommandValidator()
    {
        RuleFor(x => x.SkillCode).GreaterThan(0);
        RuleFor(x => x.SkillName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.SkillType).Must(t => "TBF".Contains(char.ToUpper(t)));
    }
}
