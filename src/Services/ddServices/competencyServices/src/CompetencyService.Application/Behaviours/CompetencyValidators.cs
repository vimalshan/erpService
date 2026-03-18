using FluentValidation;
using CompetencyService.Application.Commands.Competencies;

namespace CompetencyService.Application.Behaviours;

public class CreateCompetencyCommandValidator : AbstractValidator<CreateCompetencyCommand>
{
    public CreateCompetencyCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Competency ID must be positive.");
        RuleFor(x => x.Name).NotEmpty().MaximumLength(4000);
        RuleFor(x => x.EffectiveDate).NotEmpty();
    }
}

public class UpdateCompetencyCommandValidator : AbstractValidator<UpdateCompetencyCommand>
{
    public UpdateCompetencyCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(4000);
        RuleFor(x => x.EffectiveDate).NotEmpty();
        RuleFor(x => x.ClosureDate)
            .GreaterThanOrEqualTo(x => x.EffectiveDate)
            .When(x => x.ClosureDate.HasValue)
            .WithMessage("Closure date must be after effective date.");
    }
}
