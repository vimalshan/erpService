using HotChocolate;
using LoanTransaction.Application.Commands;
using MediatR;

namespace LoanTransaction.API.GraphQL;

public class LoanMutationType
{
    public async Task<long> DisburseLoanAsync(DisburseLoanCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> RecordEmiPaymentAsync(RecordEmiPaymentCommand input, [Service] IMediator mediator, CancellationToken ct)
    {
        await mediator.Send(input, ct);
        return true;
    }

    public async Task<bool> CloseLoanAsync(CloseLoanCommand input, [Service] IMediator mediator, CancellationToken ct)
    {
        await mediator.Send(input, ct);
        return true;
    }

    public async Task<bool> AdjustLoanAsync(AdjustLoanCommand input, [Service] IMediator mediator, CancellationToken ct)
    {
        await mediator.Send(input, ct);
        return true;
    }

    public async Task<bool> SetEmployeeInterestRateAsync(SetEmployeeInterestRateCommand input, [Service] IMediator mediator, CancellationToken ct)
    {
        await mediator.Send(input, ct);
        return true;
    }

    public async Task<bool> CreateEmiScheduleAsync(CreateEmiScheduleCommand input, [Service] IMediator mediator, CancellationToken ct)
    {
        await mediator.Send(input, ct);
        return true;
    }
}
