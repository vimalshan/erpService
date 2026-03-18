using FluentValidation;

namespace RiskService.Application.Commands.Risk;

public class CreateRiskCommandValidator : AbstractValidator<CreateRiskCommand>
{
    public CreateRiskCommandValidator()
    {
        RuleFor(x => x.EventTitle).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(4000);
        RuleFor(x => x.ApplicableTo).Must(c => "OBSU".Contains(c))
            .WithMessage("ApplicableTo must be O, B, S, or U");
        RuleFor(x => x.OrganizationId).GreaterThan(0);
        RuleFor(x => x.TypeId).GreaterThan(0);
        RuleFor(x => x.ImpactId).GreaterThan(0);
        RuleFor(x => x.ProbabilityId).GreaterThan(0);
        RuleFor(x => x.RatingId).GreaterThan(0);
        RuleFor(x => x.ResponseId).GreaterThan(0);
        RuleFor(x => x.OwnerId).GreaterThan(0);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}
