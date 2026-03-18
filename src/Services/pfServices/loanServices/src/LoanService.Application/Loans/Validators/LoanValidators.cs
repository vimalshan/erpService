using FluentValidation;
using LoanService.Application.Loans.Commands;

namespace LoanService.Application.Loans.Validators;

public class CreateLoanValidator : AbstractValidator<CreateLoanCommand>
{
    public CreateLoanValidator()
    {
        RuleFor(x => x.LoanNo).GreaterThan(0);
        RuleFor(x => x.MemberId).GreaterThan(0);
        RuleFor(x => x.LoanAmount).GreaterThan(0);
        RuleFor(x => x.LoanType).GreaterThan(0);
        RuleFor(x => x.LoanReason).NotEmpty().MaximumLength(200);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
        RuleFor(x => x.TrustCode).MaximumLength(3).When(x => x.TrustCode != null);
        RuleFor(x => x.Tenure).MaximumLength(10).When(x => x.Tenure != null);
    }
}

public class ApproveLoanValidator : AbstractValidator<ApproveLoanCommand>
{
    public ApproveLoanValidator()
    {
        RuleFor(x => x.LoanNo).GreaterThan(0);
        RuleFor(x => x.ApprovalDate).NotEmpty();
    }
}

public class CloseLoanValidator : AbstractValidator<CloseLoanCommand>
{
    public CloseLoanValidator()
    {
        RuleFor(x => x.LoanNo).GreaterThan(0);
        RuleFor(x => x.ClosureDate).NotEmpty();
    }
}

public class AddRepaymentValidator : AbstractValidator<AddRepaymentCommand>
{
    public AddRepaymentValidator()
    {
        RuleFor(x => x.LoanNo).GreaterThan(0);
        RuleFor(x => x.InstallmentNo).GreaterThan(0);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.DueDate).NotEmpty();
    }
}

public class MakePaymentValidator : AbstractValidator<MakePaymentCommand>
{
    public MakePaymentValidator()
    {
        RuleFor(x => x.LoanNo).GreaterThan(0);
        RuleFor(x => x.RepaymentId).GreaterThan(0);
        RuleFor(x => x.PaidAmount).GreaterThan(0);
        RuleFor(x => x.PaidDate).NotEmpty();
    }
}
