using MediatR;
using ReceivingService.Application.Commands.CancelReceiving;
using ReceivingService.Application.Commands.CloseReceiving;
using ReceivingService.Application.Commands.CreateReceiving;
using ReceivingService.Application.DTOs;

namespace ReceivingService.API.GraphQL;

/// <summary>Hot Chocolate GraphQL mutation type for Receiving.</summary>
public sealed class ReceivingMutation
{
    public async Task<ReceivingDto> CreateReceivingAsync(
        CreateReceivingCommand command,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(command, ct);

    public async Task<ReceivingDto> CloseReceivingAsync(
        int id,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new CloseReceivingCommand(id), ct);

    public async Task<ReceivingDto> CancelReceivingAsync(
        int id,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new CancelReceivingCommand(id), ct);
}
