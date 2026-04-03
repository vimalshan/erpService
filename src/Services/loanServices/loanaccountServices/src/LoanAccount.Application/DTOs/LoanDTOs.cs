namespace LoanAccount.Application.DTOs;

/// <summary>
/// DTO for loan creation request
/// </summary>
public record CreateLoanRequest(
    long LoanAppId,
    long EmployeeId,
    long LoanId,
    long GradeId,
    decimal PrincipalAmount,
    string DisbursementType,
    DateTime LoanDate,
    DateTime FirstInstallmentDate,
    long UnitId,
    long SubClassId,
    string Reason,
    long GuarantorId);

/// <summary>
/// DTO for loan response
/// </summary>
public record LoanResponse
{
    public long LoanNo { get; init; }
    public long EmployeeId { get; init; }
    public decimal PrincipalAmount { get; init; }
    public decimal OutstandingAmount { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime LoanDate { get; init; }
    public DateTime FirstInstallmentDate { get; init; }
    public DateTime? ClosureDate { get; init; }
}

/// <summary>
/// DTO for installment request
/// </summary>
public record CreateInstallmentRequest(
    long LoanNo,
    long InstallmentNo,
    decimal Amount,
    int InterestRatePercentage,
    DateTime DueDate);

/// <summary>
/// DTO for installment response
/// </summary>
public record InstallmentResponse
{
    public long InstallmentId { get; init; }
    public long LoanNo { get; init; }
    public long InstallmentNo { get; init; }
    public decimal Amount { get; init; }
    public decimal InterestRate { get; init; }
    public DateTime DueDate { get; init; }
    public DateTime? PaidDate { get; init; }
    public bool IsPaid { get; init; }
}

/// <summary>
/// DTO for EMI payment request
/// </summary>
public record RecordEMIPaymentRequest(
    long InstallmentId,
    decimal PrincipalPaid,
    decimal InterestPaid,
    DateTime PaymentDate);

/// <summary>
/// DTO for EMI payment response
/// </summary>
public record EMIPaymentResponse
{
    public long InstallmentId { get; init; }
    public long LoanNo { get; init; }
    public decimal PrincipalPaid { get; init; }
    public decimal InterestPaid { get; init; }
    public DateTime PaymentDate { get; init; }
    public decimal RemainingBalance { get; init; }
}

/// <summary>
/// DTO for interest rate request
/// </summary>
public record SetInterestRateRequest(
    long LoanNo,
    decimal InterestRate,
    decimal EMIAmount,
    int InstallmentNumbers);

/// <summary>
/// DTO for interest rate response
/// </summary>
public record InterestRateResponse
{
    public long RateId { get; init; }
    public long LoanNo { get; init; }
    public decimal InterestRate { get; init; }
    public decimal EMIAmount { get; init; }
    public int InstallmentNumbers { get; init; }
    public DateTime EffectiveDate { get; init; }
}

/// <summary>
/// DTO for loan settlement request
/// </summary>
public record SettleLoanRequest(
    long LoanNo,
    string SettlementType,
    decimal Amount,
    DateTime SettlementDate);

/// <summary>
/// DTO for loan settlement response
/// </summary>
public record LoanSettlementResponse
{
    public long SettlementId { get; init; }
    public long LoanNo { get; init; }
    public string SettlementType { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public DateTime SettlementDate { get; init; }
    public decimal RemainingBalance { get; init; }
}

/// <summary>
/// DTO for loan approval request
/// </summary>
public record ApproveLoanRequest(
    long LoanNo,
    decimal InterestRate,
    string? ApprovalRemarks);

/// <summary>
/// DTO for loan approval response
/// </summary>
public record LoanApprovalResponse
{
    public long LoanNo { get; init; }
    public bool IsApproved { get; init; }
    public DateTime ApprovedOn { get; init; }
    public string ApprovalRemarks { get; init; } = string.Empty;
}

/// <summary>
/// DTO for loan ledger entry response
/// </summary>
public record LoanLedgerEntryResponse
{
    public long LedgerId { get; init; }
    public long LoanNo { get; init; }
    public DateTime TransactionDate { get; init; }
    public string DCFlag { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string TransactionType { get; init; } = string.Empty;
}

/// <summary>
/// DTO for loan query response
/// </summary>
public record LoanDetailsResponse
{
    public long LoanNo { get; init; }
    public long EmployeeId { get; init; }
    public decimal PrincipalAmount { get; init; }
    public decimal DisbursedAmount { get; init; }
    public decimal OutstandingAmount { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime LoanDate { get; init; }
    public DateTime? ClosureDate { get; init; }
    public IEnumerable<InstallmentResponse> Installments { get; init; } = Enumerable.Empty<InstallmentResponse>();
    public IEnumerable<LoanLedgerEntryResponse> LedgerEntries { get; init; } = Enumerable.Empty<LoanLedgerEntryResponse>();
}
