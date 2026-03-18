using LoanDefinition.Application.DTOs;
using LoanDefinition.Application.Features.Loans.Commands;
using LoanDefinition.Application.Features.LoanTypes.Commands;
using LoanDefinition.Application.Features.Festivals.Commands;
using MediatR;

namespace LoanDefinition.API.GraphQL;

public class LoanMutation
{
    public async Task<LoanTypeMasterDto> CreateLoanType([Service] IMediator mediator, CreateLoanTypeCommand input)
        => await mediator.Send(input);

    public async Task<LoanTypeMasterDto> UpdateLoanType([Service] IMediator mediator, UpdateLoanTypeCommand input)
        => await mediator.Send(input);

    public async Task<bool> DeleteLoanType([Service] IMediator mediator, long loanType)
        => await mediator.Send(new DeleteLoanTypeCommand(loanType));

    public async Task<LoanMasterDto> CreateLoan([Service] IMediator mediator, CreateLoanCommand input)
        => await mediator.Send(input);

    public async Task<LoanMasterDto> UpdateLoan([Service] IMediator mediator, UpdateLoanCommand input)
        => await mediator.Send(input);

    public async Task<bool> CloseLoan([Service] IMediator mediator, CloseLoanCommand input)
        => await mediator.Send(input);

    public async Task<bool> DeleteLoan([Service] IMediator mediator, long loanId)
        => await mediator.Send(new DeleteLoanCommand(loanId));

    public async Task<LoanFestivalDto> CreateFestival([Service] IMediator mediator, CreateFestivalCommand input)
        => await mediator.Send(input);

    public async Task<LoanFestivalDto> UpdateFestival([Service] IMediator mediator, UpdateFestivalCommand input)
        => await mediator.Send(input);

    public async Task<bool> DeleteFestival([Service] IMediator mediator, long festivalId)
        => await mediator.Send(new DeleteFestivalCommand(festivalId));
}
