using LoanService.Application.Common;
using LoanService.Application.DTOs;
using LoanService.Application.Loans.Commands;
using MediatR;

namespace LoanService.Api.GraphQL;

public class LoanMutation
{
    public async Task<LoanDto?> CreateLoan([Service] IMediator mediator, CreateLoanCommand input, CancellationToken ct)
    {
        var result = await mediator.Send(input, ct);
        return result.IsSuccess ? result.Data : null;
    }

    public async Task<LoanDto?> ApproveLoan([Service] IMediator mediator, long loanNo, DateTime approvalDate, CancellationToken ct)
    {
        var result = await mediator.Send(new ApproveLoanCommand { LoanNo = loanNo, ApprovalDate = approvalDate }, ct);
        return result.IsSuccess ? result.Data : null;
    }

    public async Task<LoanDto?> CloseLoan([Service] IMediator mediator, long loanNo, DateTime closureDate, CancellationToken ct)
    {
        var result = await mediator.Send(new CloseLoanCommand { LoanNo = loanNo, ClosureDate = closureDate }, ct);
        return result.IsSuccess ? result.Data : null;
    }

    public async Task<RepaymentDto?> MakePayment([Service] IMediator mediator, MakePaymentCommand input, CancellationToken ct)
    {
        var result = await mediator.Send(input, ct);
        return result.IsSuccess ? result.Data : null;
    }
}
