using MediatR;
using LoanTransaction.Application.DTOs;

namespace LoanTransaction.Application.Queries;

public record GetLoanByIdQuery(long LoanNo) : IRequest<LoanDto?>;

public record GetLoansByEmployeeQuery(long EmployeeId, bool ActiveOnly = false) : IRequest<IEnumerable<LoanDto>>;

public record GetAllLoansQuery(int PageNumber = 1, int PageSize = 20) : IRequest<PagedLoanResultDto>;

public record GetInstallmentScheduleQuery(long LoanNo) : IRequest<IEnumerable<LoanInstallmentDto>>;

public record GetPendingInstallmentsQuery(long LoanNo) : IRequest<IEnumerable<LoanInstallmentDto>>;

public record GetLoanLedgerQuery(long LoanNo) : IRequest<IEnumerable<LoanLedgerDto>>;

public record GetLoanLedgerByEmployeeQuery(long EmployeeId) : IRequest<IEnumerable<LoanLedgerDto>>;

public record GetLoanSettlementsQuery(long LoanNo) : IRequest<IEnumerable<LoanSettlementDto>>;

public record CalculateEmiQuery(
    decimal PrincipalAmount,
    int RatePerAnnum,
    int TenureMonths
) : IRequest<EmiCalculationResultDto>;
