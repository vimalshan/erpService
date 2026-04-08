using FluentValidation;

namespace PFTransactionalService.Application.Commands.ApplyInterest;

public class ApplyInterestCommandValidator : AbstractValidator<ApplyInterestCommand>
{
    public ApplyInterestCommandValidator()
    {
        RuleFor(x => x.EmpSysId).GreaterThan(0);
        RuleFor(x => x.InterestAmount).GreaterThan(0);
        RuleFor(x => x.ProcessedBy).GreaterThan(0);
    }
}
