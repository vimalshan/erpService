using FluentValidation;

namespace PFTransactionalService.Application.Commands.ProcessContribution;

public class ProcessContributionCommandValidator : AbstractValidator<ProcessContributionCommand>
{
    public ProcessContributionCommandValidator()
    {
        RuleFor(x => x.EmpSysId).GreaterThan(0);
        RuleFor(x => x.MemberNo).GreaterThan(0);
        RuleFor(x => x.TrustCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.EmpContribution).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ErContribution).GreaterThanOrEqualTo(0);
        RuleFor(x => x.VolContribution).GreaterThanOrEqualTo(0);
        RuleFor(x => x.TxnMonth).NotEmpty();
        RuleFor(x => x.ProcessedBy).GreaterThan(0);
    }
}
