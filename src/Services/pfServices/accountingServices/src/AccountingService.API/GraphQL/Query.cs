using AccountingService.Application.DTOs;
using AccountingService.Application.Features.GlPosting.Queries.GetTrialBalance;
using AccountingService.Application.Features.MainAccounts.Queries.GetMainAccounts;
using AccountingService.Application.Features.Transactions.Queries.GetTransactions;
using MediatR;

namespace AccountingService.API.GraphQL;

public class Query
{
    /// <summary>Get all main accounts</summary>
    public async Task<IEnumerable<MainAccountDto>> GetMainAccounts(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetMainAccountsQuery(), ct);

    /// <summary>Get transactions for a trust</summary>
    public async Task<IEnumerable<TransactionDetailDto>> GetTransactions(
        [Service] IMediator mediator, string trustCode, CancellationToken ct)
        => await mediator.Send(new GetTransactionsQuery(trustCode), ct);

    /// <summary>Get GL trial balance</summary>
    public async Task<IEnumerable<TrialBalanceDto>> GetTrialBalance(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetTrialBalanceQuery(), ct);
}
