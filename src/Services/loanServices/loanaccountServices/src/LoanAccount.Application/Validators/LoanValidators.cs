using FluentValidation;
using LoanAccount.Application.Commands;
using MediatR;

namespace LoanAccount.Application.Validators;

/// <summary>
/// Validator for CreateLoanCommand
/// </summary>
public class CreateLoanCommandValidator : AbstractValidator<CreateLoanCommand>
{
    public CreateLoanCommandValidator()
    {
        RuleFor(x => x.LoanAppId).GreaterThan(0).WithMessage("Loan Application ID must be greater than 0");
        RuleFor(x => x.EmployeeId).GreaterThan(0).WithMessage("Employee ID must be greater than 0");
        RuleFor(x => x.PrincipalAmount).GreaterThan(0).WithMessage("Principal amount must be greater than 0");
        RuleFor(x => x.DisbursementType)
            .NotEmpty()
            .Must(x => x == "NEW" || x == "ADJ")
            .WithMessage("Disbursement type must be either NEW or ADJ");
        RuleFor(x => x.Reason).NotEmpty().WithMessage("Reason is required");
        RuleFor(x => x.GuarantorId).GreaterThan(0).WithMessage("Guarantor ID must be greater than 0");
    }
}

/// <summary>
/// Validator for ApproveLoanCommand
/// </summary>
public class ApproveLoanCommandValidator : AbstractValidator<ApproveLoanCommand>
{
    public ApproveLoanCommandValidator()
    {
        RuleFor(x => x.LoanNo).GreaterThan(0).WithMessage("Loan number must be greater than 0");
        RuleFor(x => x.InterestRate)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Interest rate must be between 0 and 100");
        RuleFor(x => x.ApprovedBy).GreaterThan(0).WithMessage("Approver ID must be greater than 0");
    }
}

/// <summary>
/// Validator for DisburseLoanCommand
/// </summary>
public class DisburseLoanCommandValidator : AbstractValidator<DisburseLoanCommand>
{
    public DisburseLoanCommandValidator()
    {
        RuleFor(x => x.LoanNo).GreaterThan(0).WithMessage("Loan number must be greater than 0");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Disbursement amount must be greater than 0");
        RuleFor(x => x.DisbursedBy).GreaterThan(0).WithMessage("Disburser ID must be greater than 0");
    }
}

/// <summary>
/// Validator for RecordEMIPaymentCommand
/// </summary>
public class RecordEMIPaymentCommandValidator : AbstractValidator<RecordEMIPaymentCommand>
{
    public RecordEMIPaymentCommandValidator()
    {
        RuleFor(x => x.InstallmentId).GreaterThan(0).WithMessage("Installment ID must be greater than 0");
        RuleFor(x => x.LoanNo).GreaterThan(0).WithMessage("Loan number must be greater than 0");
        RuleFor(x => x.PrincipalPaid).GreaterThanOrEqualTo(0).WithMessage("Principal paid must be non-negative");
        RuleFor(x => x.InterestPaid).GreaterThanOrEqualTo(0).WithMessage("Interest paid must be non-negative");
        RuleFor(x => x.PaidBy).GreaterThan(0).WithMessage("Payment made by user ID must be greater than 0");
    }
}

/// <summary>
/// Validator for SettleLoanCommand
/// </summary>
public class SettleLoanCommandValidator : AbstractValidator<SettleLoanCommand>
{
    public SettleLoanCommandValidator()
    {
        RuleFor(x => x.LoanNo).GreaterThan(0).WithMessage("Loan number must be greater than 0");
        RuleFor(x => x.SettledBy).GreaterThan(0).WithMessage("Settlement user ID must be greater than 0");
    }
}

/// <summary>
/// Validator for CloseLoanCommand
/// </summary>
public class CloseLoanCommandValidator : AbstractValidator<CloseLoanCommand>
{
    public CloseLoanCommandValidator()
    {
        RuleFor(x => x.LoanNo).GreaterThan(0).WithMessage("Loan number must be greater than 0");
        RuleFor(x => x.Reason).NotEmpty().WithMessage("Closure reason is required");
        RuleFor(x => x.ClosedBy).GreaterThan(0).WithMessage("Closed by user ID must be greater than 0");
    }
}
