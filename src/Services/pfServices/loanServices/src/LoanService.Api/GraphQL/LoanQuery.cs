using LoanService.Application.DTOs;
using LoanService.Application.Loans.Queries;
using MediatR;

namespace LoanService.Api.GraphQL;

public class LoanQuery
{
    public async Task<LoanDto?> GetLoan([Service] IMediator mediator, long loanNo, CancellationToken ct)
    {
        var result = await mediator.Send(new GetLoanByIdQuery(loanNo), ct);
        return result.IsSuccess ? result.Data : null;
    }

    public async Task<IReadOnlyList<LoanDto>?> GetLoansByMember([Service] IMediator mediator, long memberId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetLoansByMemberQuery(memberId), ct);
        return result.IsSuccess ? result.Data : null;
    }

    public async Task<IReadOnlyList<LoanDto>?> GetActiveLoans([Service] IMediator mediator, CancellationToken ct)
    {
        var result = await mediator.Send(new GetActiveLoansQuery(), ct);
        return result.IsSuccess ? result.Data : null;
    }

    public async Task<IEnumerable<ActiveLoanDto>?> GetActiveLoansSummary([Service] IMediator mediator, CancellationToken ct)
    {
        var result = await mediator.Send(new GetActiveLoansSummaryQuery(), ct);
        return result.IsSuccess ? result.Data : null;
    }
}
