namespace TransactionService.API.GraphQL;

using MediatR;
using TransactionService.Application.Commands.AllocateBudget;
using TransactionService.Application.Commands.ApproveRequest;
using TransactionService.Application.Commands.CreateOrder;
using TransactionService.Application.Commands.ReceiveOrder;
using TransactionService.Application.Commands.SubmitRequest;

public sealed class Mutation
{
    public async Task<long> SubmitRequest(
        [Service] IMediator mediator,
        SubmitRequestCommand command,
        CancellationToken ct = default)
    {
        return await mediator.Send(command, ct);
    }

    public async Task<bool> ApproveRequest(
        [Service] IMediator mediator,
        ApproveRequestCommand command,
        CancellationToken ct = default)
    {
        return await mediator.Send(command, ct);
    }

    public async Task<long> CreateOrder(
        [Service] IMediator mediator,
        CreateOrderCommand command,
        CancellationToken ct = default)
    {
        return await mediator.Send(command, ct);
    }

    public async Task<bool> ReceiveOrder(
        [Service] IMediator mediator,
        ReceiveOrderCommand command,
        CancellationToken ct = default)
    {
        return await mediator.Send(command, ct);
    }

    public async Task<bool> AllocateDeptBudget(
        [Service] IMediator mediator,
        AllocateDeptBudgetCommand command,
        CancellationToken ct = default)
    {
        return await mediator.Send(command, ct);
    }

    public async Task<bool> AllocateUnitBudget(
        [Service] IMediator mediator,
        AllocateUnitBudgetCommand command,
        CancellationToken ct = default)
    {
        return await mediator.Send(command, ct);
    }
}
