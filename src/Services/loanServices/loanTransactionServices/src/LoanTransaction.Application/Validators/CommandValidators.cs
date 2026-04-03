using FluentValidation;
using LoanTransaction.Application.Commands;

namespace LoanTransaction.Application.Validators;

public class DisburseLoanCommandValidator : AbstractValidator<DisburseLoanCommand>
{
    public DisburseLoanCommandValidator()
    {
        RuleFor(x => x.ApplicationId).GreaterThan(0).WithMessage("Application ID must be > 0.");
        RuleFor(x => x.EmployeeId).GreaterThan(0).WithMessage("Employee ID must be > 0.");
        RuleFor(x => x.LoanDefinitionId).GreaterThan(0).WithMessage("Loan Definition ID must be > 0.");
        RuleFor(x => x.PrincipalAmount).GreaterThan(0).WithMessage("Principal amount must be > 0.");
        RuleFor(x => x.InterestRate).GreaterThanOrEqualTo(0).LessThanOrEqualTo(100).WithMessage("Interest rate must be 0-100.");
        RuleFor(x => x.TenureMonths).GreaterThan(0).LessThanOrEqualTo(360).WithMessage("Tenure must be 1-360 months.");
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DisbursementType).Must(v => v == "NEW" || v == "ADJ")
            .WithMessage("Disbursement type must be NEW or ADJ.");
        RuleFor(x => x.RecoveryMethod).Must(v => new[] { "RBM", "EM1", "EMA", "FPI" }.Contains(v))
            .WithMessage("Recovery method must be RBM, EM1, EMA, or FPI.");
        RuleFor(x => x.CreatedBy).GreaterThan(0).WithMessage("Created By must be > 0.");
    }
}

public class RecordEmiPaymentCommandValidator : AbstractValidator<RecordEmiPaymentCommand>
{
    public RecordEmiPaymentCommandValidator()
    {
        RuleFor(x => x.LoanNo).GreaterThan(0);
        RuleFor(x => x.InstallmentId).GreaterThan(0);
        RuleFor(x => x.PrincipalPaid).GreaterThanOrEqualTo(0);
        RuleFor(x => x.InterestPaid).GreaterThanOrEqualTo(0);
        RuleFor(x => x).Must(x => x.PrincipalPaid + x.InterestPaid > 0)
            .WithMessage("At least one of PrincipalPaid or InterestPaid must be > 0.");
        RuleFor(x => x.PaidBy).GreaterThan(0);
    }
}

public class CloseLoanCommandValidator : AbstractValidator<CloseLoanCommand>
{
    public CloseLoanCommandValidator()
    {
        RuleFor(x => x.LoanNo).GreaterThan(0);
        RuleFor(x => x.ClosureType).Must(v => new[] { "SET", "WOF", "ADJ", "LIV" }.Contains(v))
            .WithMessage("Closure type must be SET, WOF, ADJ, or LIV.");
        RuleFor(x => x.ClosedBy).GreaterThan(0);
    }
}

public class AdjustLoanCommandValidator : AbstractValidator<AdjustLoanCommand>
{
    public AdjustLoanCommandValidator()
    {
        RuleFor(x => x.LoanNo).GreaterThan(0);
        RuleFor(x => x.AdjLoanNo).GreaterThan(0);
        RuleFor(x => x.AdjPrincipalAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.AdjInterestAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
    }
}

public class CalculateEmiQueryValidator : AbstractValidator<Queries.CalculateEmiQuery>
{
    public CalculateEmiQueryValidator()
    {
        RuleFor(x => x.PrincipalAmount).GreaterThan(0);
        RuleFor(x => x.RatePerAnnum).GreaterThanOrEqualTo(0).LessThanOrEqualTo(100);
        RuleFor(x => x.TenureMonths).GreaterThan(0).LessThanOrEqualTo(360);
    }
}
