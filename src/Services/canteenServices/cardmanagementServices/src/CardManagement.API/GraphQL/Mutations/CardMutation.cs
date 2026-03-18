using MediatR;
using CardManagement.Application.Cards.Commands.CreateGuestCard;
using CardManagement.Application.Cards.Commands.CloseGuestCard;
using CardManagement.Application.Cards.Commands.SettleCard;
using CardManagement.Application.Common.DTOs;

namespace CardManagement.API.GraphQL.Mutations;

public class CardMutation
{
    public async Task<GuestCardDto> CreateGuestCard(
        [Service] IMediator mediator,
        CreateGuestCardCommand command,
        CancellationToken ct = default)
        => await mediator.Send(command, ct);

    public async Task<bool> CloseGuestCard(
        [Service] IMediator mediator,
        long canteenUnit,
        CancellationToken ct = default)
        => await mediator.Send(new CloseGuestCardCommand(canteenUnit), ct);

    public async Task<CardSettlementDto> SettleCard(
        [Service] IMediator mediator,
        SettleCardCommand command,
        CancellationToken ct = default)
        => await mediator.Send(command, ct);
}
