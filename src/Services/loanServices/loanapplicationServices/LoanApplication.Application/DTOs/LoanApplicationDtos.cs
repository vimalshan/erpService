namespace LoanApplication.Application.DTOs;

/// <summary>
/// DTO for Loan Application
/// </summary>
public class LoanApplicationDto
{
    public long Id { get; set; }
    public long EmployeeId { get; set; }
    public long LoanId { get; set; }
    public long AppliedBy { get; set; }
    public DateTime AppliedOn { get; set; }
    public string Source { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public long? SubclassId { get; set; }
    public string Reason { get; set; } = string.Empty;
    public char Status { get; set; }
    public string StatusDisplayName { get; set; } = string.Empty;
    public long GuarantorId { get; set; }
    public long? SecondGuarantorId { get; set; }
    public string? ApprovalRemarks { get; set; }
    public long? RequiredBy { get; set; }
    public long? ApprovedBy { get; set; }
    public DateTime? ApprovedOn { get; set; }
    public int? TenureMonths { get; set; }
    public char? SpecialSanction { get; set; }
    public DateTime CreatedAt { get; set; }
    public long CreatedBy { get; set; }
    public DateTime ModifiedAt { get; set; }
    public long ModifiedBy { get; set; }
}

/// <summary>
/// Create Loan Application Request DTO
/// </summary>
public class CreateLoanApplicationDto
{
    public long EmployeeId { get; set; }
    public long LoanId { get; set; }
    public string Source { get; set; } = "SLF"; // Default to Self Loan
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
    public long GuarantorId { get; set; }
    public long? SecondGuarantorId { get; set; }
    public int TenureMonths { get; set; }
}

/// <summary>
/// Update Loan Application Request DTO
/// </summary>
public class UpdateLoanApplicationDto
{
    public long Id { get; set; }
    public string Reason { get; set; } = string.Empty;
    public long? SecondGuarantorId { get; set; }
    public int? TenureMonths { get; set; }
}

/// <summary>
/// Approve Loan Application Request DTO
/// </summary>
public class ApproveLoanApplicationDto
{
    public long Id { get; set; }
    public long ApprovedBy { get; set; }
    public string? Remarks { get; set; }
}

/// <summary>
/// Reject Loan Application Request DTO
/// </summary>
public class RejectLoanApplicationDto
{
    public long Id { get; set; }
    public long RejectedBy { get; set; }
    public string? Remarks { get; set; }
}

/// <summary>
/// Disburse Loan Application Request DTO
/// </summary>
public class DisburseLoanApplicationDto
{
    public long Id { get; set; }
    public long DisbursingBy { get; set; }
}

/// <summary>
/// Loan Application Response DTO
/// </summary>
public class LoanApplicationResponseDto
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public LoanApplicationDto? Data { get; set; }
    public List<string> Errors { get; set; } = new();
}

/// <summary>
/// Paginated Loan Application Response
/// </summary>
public class PaginatedLoanApplicationDto
{
    public List<LoanApplicationDto> Items { get; set; } = new();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
