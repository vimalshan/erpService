using MediatR;
using TransactionService.Application.DTOs;
using TransactionService.Application.Features.ApprovalWorkflows.Commands;
using TransactionService.Application.Features.TransactionLogs.Commands;

namespace TransactionService.API.GraphQL;

public class TransactionMutation
{
    public async Task<ApprovalWorkflowDto> SubmitWorkflow(
        SubmitWorkflowCommand input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken) =>
        await mediator.Send(input, cancellationToken);

    public async Task<ApprovalWorkflowDto> ApproveStep(
        ApproveStepCommand input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken) =>
        await mediator.Send(input, cancellationToken);

    public async Task<ApprovalWorkflowDto> RejectStep(
        RejectStepCommand input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken) =>
        await mediator.Send(input, cancellationToken);

    public async Task<TransactionLogDto> LogTransaction(
        LogTransactionCommand input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken) =>
        await mediator.Send(input, cancellationToken);
}
