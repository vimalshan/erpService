using FluentValidation;

namespace PFTransactionalService.Application.Commands.ProcessWithdrawal;

public class ProcessWithdrawalCommandValidator : AbstractValidator<ProcessWithdrawalCommand>
{
    public ProcessWithdrawalCommandValidator()
    {
        RuleFor(x => x.EmpSysId).GreaterThan(0);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.SettlementType).NotEmpty().MaximumLength(10);
        RuleFor(x => x.ApprovedBy).GreaterThan(0);
    }
}
