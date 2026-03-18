using LoanAccount.Application.DTOs;
using MediatR;

namespace LoanAccount.Application.Queries;

/// <summary>
/// Query to get loan by loan number
/// </summary>
public record GetLoanByNumberQuery(long LoanNo) : IRequest<LoanResponse?>;

/// <summary>
/// Query to get all loans for an employee
/// </summary>
public record GetEmployeeLoansQuery(long EmployeeId) : IRequest<IEnumerable<LoanResponse>>;

/// <summary>
/// Query to get all loans in a unit
/// </summary>
public record GetUnitLoansQuery(long UnitId) : IRequest<IEnumerable<LoanResponse>>;

/// <summary>
/// Query to get active loans
/// </summary>
public record GetActiveLoansQuery() : IRequest<IEnumerable<LoanResponse>>;

/// <summary>
/// Query to get loan details with installments
/// </summary>
public record GetLoanDetailsQuery(long LoanNo) : IRequest<LoanDetailsResponse?>;

/// <summary>
/// Query to get installments for a loan
/// </summary>
public record GetLoanInstallmentsQuery(long LoanNo) : IRequest<IEnumerable<InstallmentResponse>>;

/// <summary>
/// Query to get pending installments for a loan
/// </summary>
public record GetPendingInstallmentsQuery(long LoanNo) : IRequest<IEnumerable<InstallmentResponse>>;

/// <summary>
/// Query to get loan ledger entries
/// </summary>
public record GetLoanLedgerEntriesQuery(long LoanNo) : IRequest<IEnumerable<LoanLedgerEntryResponse>>;

/// <summary>
/// Query to get employee ledger entries
/// </summary>
public record GetEmployeeLedgerEntriesQuery(long EmployeeId) : IRequest<IEnumerable<LoanLedgerEntryResponse>>;

/// <summary>
/// Query to get loan interest rate
/// </summary>
public record GetLoanInterestRateQuery(long LoanNo) : IRequest<InterestRateResponse?>;

/// <summary>
/// Query to get loan settlements
/// </summary>
public record GetLoanSettlementsQuery(long LoanNo) : IRequest<IEnumerable<LoanSettlementResponse>>;

/// <summary>
/// Query to get settlements between dates
/// </summary>
public record GetSettlementsBetweenDatesQuery(DateTime StartDate, DateTime EndDate) 
    : IRequest<IEnumerable<LoanSettlementResponse>>;

/// <summary>
/// Query to calculate outstanding balance
/// </summary>
public record CalculateOutstandingBalanceQuery(long LoanNo) : IRequest<decimal>;

/// <summary>
/// Query to get loan delinquency report
/// </summary>
public record GetDelinquencyReportQuery(int MonthsOverdue) : IRequest<IEnumerable<LoanResponse>>;
