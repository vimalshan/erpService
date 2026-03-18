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
public record LoanResponse(
    long LoanNo,
    long EmployeeId,
    decimal PrincipalAmount,
    decimal OutstandingAmount,
    string Status,
    DateTime LoanDate,
    DateTime FirstInstallmentDate,
    DateTime? ClosureDate);

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
public record InstallmentResponse(
    long InstallmentId,
    long LoanNo,
    long InstallmentNo,
    decimal Amount,
    decimal InterestRate,
    DateTime DueDate,
    DateTime? PaidDate,
    bool IsPaid);

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
public record EMIPaymentResponse(
    long InstallmentId,
    long LoanNo,
    decimal PrincipalPaid,
    decimal InterestPaid,
    DateTime PaymentDate,
    decimal RemainingBalance);

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
public record InterestRateResponse(
    long RateId,
    long LoanNo,
    decimal InterestRate,
    decimal EMIAmount,
    int InstallmentNumbers,
    DateTime EffectiveDate);

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
public record LoanSettlementResponse(
    long SettlementId,
    long LoanNo,
    string SettlementType,
    decimal Amount,
    DateTime SettlementDate,
    decimal RemainingBalance);

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
public record LoanApprovalResponse(
    long LoanNo,
    bool IsApproved,
    DateTime ApprovedOn,
    string ApprovalRemarks);

/// <summary>
/// DTO for loan ledger entry response
/// </summary>
public record LoanLedgerEntryResponse(
    long LedgerId,
    long LoanNo,
    DateTime TransactionDate,
    string DCFlag,
    string Description,
    decimal Amount,
    string TransactionType);

/// <summary>
/// DTO for loan query response
/// </summary>
public record LoanDetailsResponse(
    long LoanNo,
    long EmployeeId,
    decimal PrincipalAmount,
    decimal DisbursedAmount,
    decimal OutstandingAmount,
    string Status,
    DateTime LoanDate,
    DateTime? ClosureDate,
    IEnumerable<InstallmentResponse> Installments,
    IEnumerable<LoanLedgerEntryResponse> LedgerEntries);
