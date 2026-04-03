using LoanAccount.Application.DTOs;
using LoanAccount.Application.Queries;
using MediatR;

namespace LoanAccount.API.GraphQL.Queries;

/// <summary>
/// GraphQL Query type for loan operations
/// </summary>
public class LoanQuery
{
    [GraphQLName("loanByNumber")]
    public async Task<LoanResponse?> GetLoanByNumber(long loanNo, [Service] IMediator mediator, CancellationToken cancellationToken)
    {
        var query = new GetLoanByNumberQuery(loanNo);
        return await mediator.Send(query, cancellationToken);
    }

    [GraphQLName("employeeLoans")]
    public async Task<IEnumerable<LoanResponse>> GetEmployeeLoans(long employeeId, [Service] IMediator mediator, CancellationToken cancellationToken)
    {
        var query = new GetEmployeeLoansQuery(employeeId);
        return await mediator.Send(query, cancellationToken);
    }

    [GraphQLName("unitLoans")]
    public async Task<IEnumerable<LoanResponse>> GetUnitLoans(long unitId, [Service] IMediator mediator, CancellationToken cancellationToken)
    {
        var query = new GetUnitLoansQuery(unitId);
        return await mediator.Send(query, cancellationToken);
    }

    [GraphQLName("activeLoans")]
    public async Task<IEnumerable<LoanResponse>> GetActiveLoans([Service] IMediator mediator, CancellationToken cancellationToken)
    {
        var query = new GetActiveLoansQuery();
        return await mediator.Send(query, cancellationToken);
    }

    [GraphQLName("loanDetails")]
    public async Task<LoanDetailsResponse?> GetLoanDetails(long loanNo, [Service] IMediator mediator, CancellationToken cancellationToken)
    {
        var query = new GetLoanDetailsQuery(loanNo);
        return await mediator.Send(query, cancellationToken);
    }

    [GraphQLName("loanInstallments")]
    public async Task<IEnumerable<InstallmentResponse>> GetLoanInstallments(long loanNo, [Service] IMediator mediator, CancellationToken cancellationToken)
    {
        var query = new GetLoanInstallmentsQuery(loanNo);
        return await mediator.Send(query, cancellationToken);
    }

    [GraphQLName("loanLedger")]
    public async Task<IEnumerable<LoanLedgerEntryResponse>> GetLoanLedger(long loanNo, [Service] IMediator mediator, CancellationToken cancellationToken)
    {
        var query = new GetLoanLedgerEntriesQuery(loanNo);
        return await mediator.Send(query, cancellationToken);
    }

    [GraphQLName("loanSettlements")]
    public async Task<IEnumerable<LoanSettlementResponse>> GetLoanSettlements(long loanNo, [Service] IMediator mediator, CancellationToken cancellationToken)
    {
        var query = new GetLoanSettlementsQuery(loanNo);
        return await mediator.Send(query, cancellationToken);
    }
}
