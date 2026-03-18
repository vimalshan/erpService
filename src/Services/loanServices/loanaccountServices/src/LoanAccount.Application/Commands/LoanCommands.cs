using MediatR;

namespace LoanAccount.Application.Commands;

/// <summary>
/// Command to create a new loan
/// </summary>
public record CreateLoanCommand(
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
    long GuarantorId,
    long CreatedBy) : IRequest<long>;

/// <summary>
/// Command to approve a loan
/// </summary>
public record ApproveLoanCommand(
    long LoanNo,
    decimal InterestRate,
    long ApprovedBy,
    string? ApprovalRemarks) : IRequest<bool>;

/// <summary>
/// Command to disburse a loan
/// </summary>
public record DisburseLoanCommand(
    long LoanNo,
    decimal Amount,
    long DisbursedBy) : IRequest<bool>;

/// <summary>
/// Command to create loan installments
/// </summary>
public record CreateLoanInstallmentsCommand(
    long LoanNo,
    int NumberOfInstallments,
    decimal EMIAmount,
    DateTime FirstInstallmentDate,
    long CreatedBy) : IRequest<bool>;

/// <summary>
/// Command to record EMI payment
/// </summary>
public record RecordEMIPaymentCommand(
    long InstallmentId,
    long LoanNo,
    decimal PrincipalPaid,
    decimal InterestPaid,
    DateTime PaymentDate,
    long PaidBy) : IRequest<bool>;

/// <summary>
/// Command to settle a loan
/// </summary>
public record SettleLoanCommand(
    long LoanNo,
    long SettledBy) : IRequest<bool>;

/// <summary>
/// Command to close a loan
/// </summary>
public record CloseLoanCommand(
    long LoanNo,
    string Reason,
    long ClosedBy) : IRequest<bool>;

/// <summary>
/// Command to record loan transaction
/// </summary>
public record RecordLoanTransactionCommand(
    long LoanNo,
    long EmpSysId,
    long EmpNo,
    long UnitId,
    char DCFlag,
    string Description,
    decimal Amount,
    string TransactionType,
    long ReferenceNo,
    long ScheduleId,
    long RecordedBy) : IRequest<bool>;

/// <summary>
/// Command to adjust principal against new loan
/// </summary>
public record AdjustPrincipalCommand(
    long CurrentLoanNo,
    long NewLoanNo,
    decimal AdjustmentAmount,
    long AdjustedBy) : IRequest<bool>;

/// <summary>
/// Command to set employee-wise interest rate
/// </summary>
public record SetEmployeeInterestRateCommand(
    long LoanNo,
    decimal InterestRate,
    decimal EMIAmount,
    int InstallmentNumbers,
    long SetBy) : IRequest<bool>;
