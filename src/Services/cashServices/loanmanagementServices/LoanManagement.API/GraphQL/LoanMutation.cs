using LoanManagement.Application.Commands.CreateLoan;
using LoanManagement.Application.Commands.AddDisbursement;
using LoanManagement.Application.Commands.CloseLoan;
using LoanManagement.Application.DTOs;
using MediatR;

namespace LoanManagement.API.GraphQL;

public class LoanMutation
{
    public async Task<LoanDto> CreateLoan(
        [Service] IMediator mediator,
        CreateLoanInput input)
        => await mediator.Send(new CreateLoanCommand(
            input.LoanKey, input.OrgId, input.LoanAmount, input.LoanTypeId,
            input.BankId, input.CreatedBy, input.LoanDate, input.OrgCurr, input.LoanCurr));

    public async Task<DisbursementScheduleDto> AddDisbursement(
        [Service] IMediator mediator,
        decimal loanId,
        DateTime disbDate,
        decimal amount,
        decimal? excRate)
        => await mediator.Send(new AddDisbursementCommand(loanId, disbDate, amount, excRate));

    public async Task<bool> CloseLoan(
        [Service] IMediator mediator,
        decimal loanId,
        decimal modifiedBy)
        => await mediator.Send(new CloseLoanCommand(loanId, modifiedBy));
}

public record CreateLoanInput(
    string LoanKey,
    decimal OrgId,
    decimal LoanAmount,
    decimal LoanTypeId,
    decimal BankId,
    decimal CreatedBy,
    DateTime LoanDate,
    decimal? OrgCurr = null,
    decimal? LoanCurr = null);
