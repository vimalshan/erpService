namespace LoanTransaction.Application.DTOs;

public class LoanDto
{
    public long LoanNo { get; set; }
    public long ApplicationId { get; set; }
    public long EmployeeId { get; set; }
    public long LoanDefinitionId { get; set; }
    public long GradeId { get; set; }
    public long UnitId { get; set; }
    public long SubclassId { get; set; }
    public long GuarantorId { get; set; }
    public string DisbursementType { get; set; } = string.Empty;
    public decimal PrincipalAmount { get; set; }
    public decimal OldPrincipalAdj { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal PrincipalOutstanding { get; set; }
    public string RecoveryMethod { get; set; } = string.Empty;
    public DateTime EffectiveDate { get; set; }
    public DateTime FirstInstallmentDate { get; set; }
    public DateTime LastInstallmentDate { get; set; }
    public DateTime? ClosureDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? ApprovalRemarks { get; set; }
    public string ClosureType { get; set; } = string.Empty;
    public bool HasEmployeeInterestRate { get; set; }
    public string CompoundingFactor { get; set; } = string.Empty;
    public string InterestFrequency { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public long ModifiedBy { get; set; }
    public DateTime ModifiedAt { get; set; }
}

public class DisburseLoanDto
{
    public long ApplicationId { get; set; }
    public long EmployeeId { get; set; }
    public long LoanDefinitionId { get; set; }
    public long GradeId { get; set; }
    public long UnitId { get; set; }
    public long SubclassId { get; set; }
    public long GuarantorId { get; set; }
    public string DisbursementType { get; set; } = "NEW";
    public decimal PrincipalAmount { get; set; }
    public int InterestRate { get; set; }
    public int TenureMonths { get; set; }
    public string RecoveryMethod { get; set; } = "EMA";
    public DateTime EffectiveDate { get; set; }
    public DateTime FirstInstallmentDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string CompoundingFactor { get; set; } = "S";
    public string InterestFrequency { get; set; } = "M";
    public bool HasEmployeeInterestRate { get; set; }
    public long AmountEdId { get; set; }
    public long PrnEdId { get; set; }
    public long IntEdId { get; set; }
    public long CreatedBy { get; set; }
}

public class RecordEmiPaymentDto
{
    public long LoanNo { get; set; }
    public long InstallmentId { get; set; }
    public decimal PrincipalPaid { get; set; }
    public decimal InterestPaid { get; set; }
    public long PaidBy { get; set; }
}

public class CloseLoanDto
{
    public long LoanNo { get; set; }
    public string ClosureType { get; set; } = "SET";
    public long ClosedBy { get; set; }
}

public class AdjustLoanDto
{
    public long LoanNo { get; set; }
    public long AdjLoanNo { get; set; }
    public decimal AdjPrincipalAmount { get; set; }
    public decimal AdjInterestAmount { get; set; }
    public long UpdatedBy { get; set; }
}

public class LoanInstallmentDto
{
    public long Id { get; set; }
    public long LoanNo { get; set; }
    public long UnitId { get; set; }
    public DateTime InstallmentDate { get; set; }
    public long InstallmentNo { get; set; }
    public decimal InstallmentAmount { get; set; }
    public decimal PrincipalOutstanding { get; set; }
    public decimal PrincipalAdjusted { get; set; }
    public decimal InterestAdjusted { get; set; }
    public decimal InterestAccrued { get; set; }
    public decimal InterestRecovered { get; set; }
    public decimal PrincipalRecovered { get; set; }
    public int InterestRate { get; set; }
    public string Remarks { get; set; } = string.Empty;
    public bool IsPaid { get; set; }
    public decimal RemainingAmount { get; set; }
}

public class LoanSettlementDto
{
    public long Id { get; set; }
    public long LoanNo { get; set; }
    public long UnitId { get; set; }
    public string SettlementType { get; set; } = string.Empty;
    public long InstallmentNo { get; set; }
    public DateTime InstallmentDate { get; set; }
    public DateTime RecoveryDate { get; set; }
    public string RecoveryType { get; set; } = string.Empty;
    public decimal InstallmentAmount { get; set; }
    public string PayType { get; set; } = string.Empty;
    public bool IsCancelled { get; set; }
    public DateTime? CancelDate { get; set; }
}

public class LoanLedgerDto
{
    public long Id { get; set; }
    public long LoanNo { get; set; }
    public long EmployeeId { get; set; }
    public long UnitId { get; set; }
    public DateTime TransactionDate { get; set; }
    public string DCFlag { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal TransactionAmount { get; set; }
    public string TransactionType { get; set; } = string.Empty;
}

public class EmiScheduleItemDto
{
    public int InstallmentNo { get; set; }
    public DateTime InstallmentDate { get; set; }
    public decimal InstallmentAmount { get; set; }
    public decimal PrincipalComponent { get; set; }
    public decimal InterestComponent { get; set; }
    public decimal PrincipalOutstanding { get; set; }
}

public class EmiCalculationResultDto
{
    public decimal EmiAmount { get; set; }
    public decimal PrincipalAmount { get; set; }
    public int RatePerAnnum { get; set; }
    public int TenureMonths { get; set; }
    public decimal TotalInterest { get; set; }
    public decimal TotalPayable { get; set; }
    public IEnumerable<EmiScheduleItemDto> Schedule { get; set; } = Enumerable.Empty<EmiScheduleItemDto>();
}

public class PagedLoanResultDto
{
    public IEnumerable<LoanDto> Items { get; set; } = Enumerable.Empty<LoanDto>();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}
