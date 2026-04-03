using MediatR;
using LoanTransaction.Application.DTOs;

namespace LoanTransaction.Application.Commands;

public record DisburseLoanCommand(
    long ApplicationId,
    long EmployeeId,
    long LoanDefinitionId,
    long GradeId,
    long UnitId,
    long SubclassId,
    long GuarantorId,
    string DisbursementType,
    decimal PrincipalAmount,
    int InterestRate,
    int TenureMonths,
    string RecoveryMethod,
    DateTime EffectiveDate,
    DateTime FirstInstallmentDate,
    string Reason,
    string CompoundingFactor,
    string InterestFrequency,
    bool HasEmployeeInterestRate,
    long AmountEdId,
    long PrnEdId,
    long IntEdId,
    long CreatedBy
) : IRequest<long>;

public record RecordEmiPaymentCommand(
    long LoanNo,
    long InstallmentId,
    decimal PrincipalPaid,
    decimal InterestPaid,
    long PaidBy
) : IRequest<bool>;

public record CloseLoanCommand(
    long LoanNo,
    string ClosureType,
    long ClosedBy
) : IRequest<bool>;

public record AdjustLoanCommand(
    long LoanNo,
    long AdjLoanNo,
    decimal AdjPrincipalAmount,
    decimal AdjInterestAmount,
    long UpdatedBy
) : IRequest<bool>;

public record SetEmployeeInterestRateCommand(
    long LoanNo,
    int Rate,
    decimal EmiAmount,
    int NumberOfInstallments,
    long ModifiedBy
) : IRequest<bool>;

public record CreateEmiScheduleCommand(
    long LoanNo,
    long EmployeeId,
    long UnitId,
    decimal PrincipalAmount,
    int InterestRate,
    int TenureMonths,
    DateTime FirstInstallmentDate,
    long CreatedBy
) : IRequest<IEnumerable<EmiScheduleItemDto>>;
