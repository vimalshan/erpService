using FluentValidation;
using TransactionProcessing.Application.Commands;

namespace TransactionProcessing.Application.Validators;

public sealed class ProcessDealSettlementValidator : AbstractValidator<ProcessDealSettlementCommand>
{
    public ProcessDealSettlementValidator()
    {
        RuleFor(x => x.DealId).GreaterThan(0);
        RuleFor(x => x.SetId).GreaterThan(0);
        RuleFor(x => x.SettlementType).Must(t => t is "U" or "C" or "R")
            .WithMessage("Settlement type must be U (Utilization), C (Cancellation), or R (Rollover)");
        RuleFor(x => x.SettlementAmount).GreaterThan(0);
        RuleFor(x => x.NetAmount).GreaterThan(0);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}

public sealed class ProcessLoanDisbursementValidator : AbstractValidator<ProcessLoanDisbursementCommand>
{
    public ProcessLoanDisbursementValidator()
    {
        RuleFor(x => x.LoanId).GreaterThan(0);
        RuleFor(x => x.DisbId).GreaterThan(0);
        RuleFor(x => x.DisbAmount).GreaterThan(0);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}

public sealed class ProcessLoanRepaymentValidator : AbstractValidator<ProcessLoanRepaymentCommand>
{
    public ProcessLoanRepaymentValidator()
    {
        RuleFor(x => x.LoanId).GreaterThan(0);
        RuleFor(x => x.RepayId).GreaterThan(0);
        RuleFor(x => x.RepayAmount).GreaterThan(0);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}

public sealed class ProcessCashTransferValidator : AbstractValidator<ProcessCashTransferCommand>
{
    public ProcessCashTransferValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.SourceService).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}
