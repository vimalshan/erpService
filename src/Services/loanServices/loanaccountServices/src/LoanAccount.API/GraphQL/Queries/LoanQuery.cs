using LoanAccount.Application.DTOs;
using LoanAccount.Application.Queries;
using MediatR;

namespace LoanAccount.API.GraphQL.Queries;

/// <summary>
/// GraphQL Query type for loan operations
/// </summary>
public class LoanQuery
{
    private readonly IMediator _mediator;

    public LoanQuery(IMediator mediator)
    {
        _mediator = mediator;
    }

    [GraphQLName("loanByNumber")]
    public async Task<LoanResponse?> GetLoanByNumber(long loanNo, CancellationToken cancellationToken)
    {
        var query = new GetLoanByNumberQuery(loanNo);
        return await _mediator.Send(query, cancellationToken);
    }

    [GraphQLName("employeeLoans")]
    public async Task<IEnumerable<LoanResponse>> GetEmployeeLoans(long employeeId, CancellationToken cancellationToken)
    {
        var query = new GetEmployeeLoansQuery(employeeId);
        return await _mediator.Send(query, cancellationToken);
    }

    [GraphQLName("unitLoans")]
    public async Task<IEnumerable<LoanResponse>> GetUnitLoans(long unitId, CancellationToken cancellationToken)
    {
        var query = new GetUnitLoansQuery(unitId);
        return await _mediator.Send(query, cancellationToken);
    }

    [GraphQLName("activeLoans")]
    public async Task<IEnumerable<LoanResponse>> GetActiveLoans(CancellationToken cancellationToken)
    {
        var query = new GetActiveLoansQuery();
        return await _mediator.Send(query, cancellationToken);
    }

    [GraphQLName("loanDetails")]
    public async Task<LoanDetailsResponse?> GetLoanDetails(long loanNo, CancellationToken cancellationToken)
    {
        var query = new GetLoanDetailsQuery(loanNo);
        return await _mediator.Send(query, cancellationToken);
    }

    [GraphQLName("loanInstallments")]
    public async Task<IEnumerable<InstallmentResponse>> GetLoanInstallments(long loanNo, CancellationToken cancellationToken)
    {
        var query = new GetLoanInstallmentsQuery(loanNo);
        return await _mediator.Send(query, cancellationToken);
    }

    [GraphQLName("loanLedger")]
    public async Task<IEnumerable<LoanLedgerEntryResponse>> GetLoanLedger(long loanNo, CancellationToken cancellationToken)
    {
        var query = new GetLoanLedgerEntriesQuery(loanNo);
        return await _mediator.Send(query, cancellationToken);
    }

    [GraphQLName("loanSettlements")]
    public async Task<IEnumerable<LoanSettlementResponse>> GetLoanSettlements(long loanNo, CancellationToken cancellationToken)
    {
        var query = new GetLoanSettlementsQuery(loanNo);
        return await _mediator.Send(query, cancellationToken);
    }
}
