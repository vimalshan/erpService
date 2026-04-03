using HotChocolate;
using LoanTransaction.Application.DTOs;
using LoanTransaction.Application.Queries;
using MediatR;

namespace LoanTransaction.API.GraphQL;

public class LoanQueryType
{
    public async Task<LoanDto?> GetLoanByIdAsync(long loanNo, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetLoanByIdQuery(loanNo), ct);

    public async Task<IEnumerable<LoanDto>> GetLoansByEmployeeAsync(long empId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetLoansByEmployeeQuery(empId), ct);

    public async Task<PagedLoanResultDto> GetAllLoansAsync(int page, int pageSize, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllLoansQuery(page, pageSize), ct);

    public async Task<IEnumerable<LoanInstallmentDto>> GetInstallmentScheduleAsync(long loanNo, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetInstallmentScheduleQuery(loanNo), ct);

    public async Task<IEnumerable<LoanInstallmentDto>> GetPendingInstallmentsAsync(long loanNo, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetPendingInstallmentsQuery(loanNo), ct);

    public async Task<IEnumerable<LoanLedgerDto>> GetLoanLedgerAsync(long loanNo, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetLoanLedgerQuery(loanNo), ct);

    public async Task<IEnumerable<LoanLedgerDto>> GetLoanLedgerByEmployeeAsync(long empId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetLoanLedgerByEmployeeQuery(empId), ct);

    public async Task<IEnumerable<LoanSettlementDto>> GetLoanSettlementsAsync(long loanNo, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetLoanSettlementsQuery(loanNo), ct);

    public async Task<EmiCalculationResultDto> CalculateEmiAsync(
        decimal principal, int annualInterestRate, int tenureMonths,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CalculateEmiQuery(principal, annualInterestRate, tenureMonths), ct);
}
