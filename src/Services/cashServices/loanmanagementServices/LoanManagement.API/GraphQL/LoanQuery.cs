using LoanManagement.Application.DTOs;
using LoanManagement.Application.Queries.GetAllLoans;
using LoanManagement.Application.Queries.GetLoanById;
using LoanManagement.Application.Queries.GetRepaymentSchedule;
using MediatR;

namespace LoanManagement.API.GraphQL;

public class LoanQuery
{
    public async Task<IEnumerable<LoanDto>> GetLoans(
        [Service] IMediator mediator,
        decimal? orgId = null)
        => await mediator.Send(new GetAllLoansQuery(orgId));

    public async Task<LoanDto?> GetLoanById(
        [Service] IMediator mediator,
        decimal loanId)
        => await mediator.Send(new GetLoanByIdQuery(loanId));

    public async Task<IEnumerable<RepaymentScheduleDto>> GetRepaymentSchedule(
        [Service] IMediator mediator,
        decimal loanId)
        => await mediator.Send(new GetRepaymentScheduleQuery(loanId));
}
