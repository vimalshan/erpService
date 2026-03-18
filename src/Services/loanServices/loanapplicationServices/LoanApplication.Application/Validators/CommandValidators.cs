using FluentValidation;
using LoanApplication.Application.Commands;

namespace LoanApplication.Application.Validators;

/// <summary>
/// Validator for CreateLoanApplicationCommand
/// </summary>
public class CreateLoanApplicationCommandValidator : AbstractValidator<CreateLoanApplicationCommand>
{
    public CreateLoanApplicationCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0)
            .WithMessage("Employee ID must be greater than 0");

        RuleFor(x => x.LoanId)
            .GreaterThan(0)
            .WithMessage("Loan ID must be greater than 0");

        RuleFor(x => x.AppliedBy)
            .GreaterThan(0)
            .WithMessage("Applied By must be greater than 0");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Reason is required")
            .MaximumLength(200)
            .WithMessage("Reason cannot exceed 200 characters");

        RuleFor(x => x.GuarantorId)
            .GreaterThan(0)
            .WithMessage("Guarantor ID must be greater than 0");

        RuleFor(x => x)
            .Must(x => x.EmployeeId != x.GuarantorId)
            .WithMessage("Guarantor cannot be the same as applicant");

        RuleFor(x => x.TenureMonths)
            .GreaterThan(0)
            .WithMessage("Tenure must be greater than 0");

        RuleFor(x => x.Source)
            .Must(x => x == "DIR" || x == "SLF")
            .WithMessage("Source must be either DIR (Directorate) or SLF (Self Loan)");
    }
}

/// <summary>
/// Validator for SubmitLoanApplicationCommand
/// </summary>
public class SubmitLoanApplicationCommandValidator : AbstractValidator<SubmitLoanApplicationCommand>
{
    public SubmitLoanApplicationCommandValidator()
    {
        RuleFor(x => x.LoanApplicationId)
            .GreaterThan(0)
            .WithMessage("Loan Application ID must be greater than 0");

        RuleFor(x => x.SubmittedBy)
            .GreaterThan(0)
            .WithMessage("Submitted By must be greater than 0");
    }
}

/// <summary>
/// Validator for ApproveLoanApplicationCommand
/// </summary>
public class ApproveLoanApplicationCommandValidator : AbstractValidator<ApproveLoanApplicationCommand>
{
    public ApproveLoanApplicationCommandValidator()
    {
        RuleFor(x => x.LoanApplicationId)
            .GreaterThan(0)
            .WithMessage("Loan Application ID must be greater than 0");

        RuleFor(x => x.ApprovedBy)
            .GreaterThan(0)
            .WithMessage("Approved By must be greater than 0");

        RuleFor(x => x.Remarks)
            .MaximumLength(200)
            .WithMessage("Remarks cannot exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.Remarks));
    }
}

/// <summary>
/// Validator for RejectLoanApplicationCommand
/// </summary>
public class RejectLoanApplicationCommandValidator : AbstractValidator<RejectLoanApplicationCommand>
{
    public RejectLoanApplicationCommandValidator()
    {
        RuleFor(x => x.LoanApplicationId)
            .GreaterThan(0)
            .WithMessage("Loan Application ID must be greater than 0");

        RuleFor(x => x.RejectedBy)
            .GreaterThan(0)
            .WithMessage("Rejected By must be greater than 0");

        RuleFor(x => x.Remarks)
            .MaximumLength(200)
            .WithMessage("Remarks cannot exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.Remarks));
    }
}

/// <summary>
/// Validator for DisburseLoanCommand
/// </summary>
public class DisburseLoanCommandValidator : AbstractValidator<DisburseLoanCommand>
{
    public DisburseLoanCommandValidator()
    {
        RuleFor(x => x.LoanApplicationId)
            .GreaterThan(0)
            .WithMessage("Loan Application ID must be greater than 0");

        RuleFor(x => x.DisbursingBy)
            .GreaterThan(0)
            .WithMessage("Disbursing By must be greater than 0");
    }
}

/// <summary>
/// Validator for SetSecondGuarantorCommand
/// </summary>
public class SetSecondGuarantorCommandValidator : AbstractValidator<SetSecondGuarantorCommand>
{
    public SetSecondGuarantorCommandValidator()
    {
        RuleFor(x => x.LoanApplicationId)
            .GreaterThan(0)
            .WithMessage("Loan Application ID must be greater than 0");

        RuleFor(x => x.SecondGuarantorId)
            .GreaterThan(0)
            .WithMessage("Second Guarantor ID must be greater than 0");

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0)
            .WithMessage("Modified By must be greater than 0");
    }
}

/// <summary>
/// Validator for MarkForSpecialSanctionCommand
/// </summary>
public class MarkForSpecialSanctionCommandValidator : AbstractValidator<MarkForSpecialSanctionCommand>
{
    public MarkForSpecialSanctionCommandValidator()
    {
        RuleFor(x => x.LoanApplicationId)
            .GreaterThan(0)
            .WithMessage("Loan Application ID must be greater than 0");

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0)
            .WithMessage("Modified By must be greater than 0");
    }
}
