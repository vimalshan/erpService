using LoanApplication.Application.DTOs;

namespace LoanApplication.API.GraphQL.Types;

/// <summary>
/// GraphQL type for Loan Application
/// </summary>
public class LoanApplicationType
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
    public string Status { get; set; } = string.Empty;
    public string StatusDisplayName { get; set; } = string.Empty;
    public long GuarantorId { get; set; }
    public long? SecondGuarantorId { get; set; }
    public string? ApprovalRemarks { get; set; }
    public long? RequiredBy { get; set; }
    public long? ApprovedBy { get; set; }
    public DateTime? ApprovedOn { get; set; }
    public int? TenureMonths { get; set; }
    public string? SpecialSanction { get; set; }
    public DateTime CreatedAt { get; set; }
    public long CreatedBy { get; set; }
    public DateTime ModifiedAt { get; set; }
    public long ModifiedBy { get; set; }

    public static LoanApplicationType FromDto(LoanApplicationDto dto)
    {
        return new LoanApplicationType
        {
            Id = dto.Id,
            EmployeeId = dto.EmployeeId,
            LoanId = dto.LoanId,
            AppliedBy = dto.AppliedBy,
            AppliedOn = dto.AppliedOn,
            Source = dto.Source,
            Amount = dto.Amount,
            SubclassId = dto.SubclassId,
            Reason = dto.Reason,
            Status = dto.Status.ToString(),
            StatusDisplayName = dto.StatusDisplayName,
            GuarantorId = dto.GuarantorId,
            SecondGuarantorId = dto.SecondGuarantorId,
            ApprovalRemarks = dto.ApprovalRemarks,
            RequiredBy = dto.RequiredBy,
            ApprovedBy = dto.ApprovedBy,
            ApprovedOn = dto.ApprovedOn,
            TenureMonths = dto.TenureMonths,
            SpecialSanction = dto.SpecialSanction?.ToString(),
            CreatedAt = dto.CreatedAt,
            CreatedBy = dto.CreatedBy,
            ModifiedAt = dto.ModifiedAt,
            ModifiedBy = dto.ModifiedBy
        };
    }
}

/// <summary>
/// GraphQL type for eligibility check result
/// </summary>
public class EligibilityCheckType
{
    public bool IsEligible { get; set; }
    public int ServiceYears { get; set; }
    public int ActiveLoanCount { get; set; }
    public int MaxActiveLoans { get; set; }
    public int MinServiceYears { get; set; }
    public string? Reason { get; set; }
}
