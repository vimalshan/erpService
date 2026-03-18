using MediatR;
using SwipeTransactionService.Application.DTOs;
using SwipeTransactionService.Application.Features.SwipeTransactions.Commands;
using SwipeTransactionService.Application.Features.CanteenPunch.Commands;

namespace SwipeTransactionService.API.GraphQL;

public sealed class Mutation
{
    public async Task<SwipeCardUploadDto> RecordSwipeUpload(
        RecordSwipeUploadCommand input,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<CanteenPunchDto> RecordPunch(
        RecordPunchCommand input,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(input, ct);
}
