using MediatR;

namespace LoanApplication.Application.Commands;

/// <summary>
/// Command to create a new loan application
/// </summary>
public class CreateLoanApplicationCommand : IRequest<long>
{
    public long EmployeeId { get; set; }
    public long LoanId { get; set; }
    public long AppliedBy { get; set; }
    public string Source { get; set; } = "SLF";
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
    public long GuarantorId { get; set; }
    public long? SecondGuarantorId { get; set; }
    public int TenureMonths { get; set; }
}

/// <summary>
/// Command to submit a loan application for approval
/// </summary>
public class SubmitLoanApplicationCommand : IRequest<bool>
{
    public long LoanApplicationId { get; set; }
    public long SubmittedBy { get; set; }
}

/// <summary>
/// Command to approve a loan application
/// </summary>
public class ApproveLoanApplicationCommand : IRequest<bool>
{
    public long LoanApplicationId { get; set; }
    public long ApprovedBy { get; set; }
    public string? Remarks { get; set; }
}

/// <summary>
/// Command to reject a loan application
/// </summary>
public class RejectLoanApplicationCommand : IRequest<bool>
{
    public long LoanApplicationId { get; set; }
    public long RejectedBy { get; set; }
    public string? Remarks { get; set; }
}

/// <summary>
/// Command to disburse a loan
/// </summary>
public class DisburseLoanCommand : IRequest<bool>
{
    public long LoanApplicationId { get; set; }
    public long DisbursingBy { get; set; }
}

/// <summary>
/// Command to set second guarantor
/// </summary>
public class SetSecondGuarantorCommand : IRequest<bool>
{
    public long LoanApplicationId { get; set; }
    public long SecondGuarantorId { get; set; }
    public long ModifiedBy { get; set; }
}

/// <summary>
/// Command to mark loan for special sanction
/// </summary>
public class MarkForSpecialSanctionCommand : IRequest<bool>
{
    public long LoanApplicationId { get; set; }
    public bool Sanctioned { get; set; }
    public long ModifiedBy { get; set; }
}
