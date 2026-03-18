using MediatR;
using CardManagement.Application.Cards.Queries.GetGuestCards;
using CardManagement.Application.Cards.Queries.GetGuestCardById;
using CardManagement.Application.Common.DTOs;

namespace CardManagement.API.GraphQL.Queries;

public class CardQuery
{
    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<GuestCardDto>> GetGuestCards(
        [Service] IMediator mediator,
        int pageNumber = 1,
        int pageSize = 50,
        long? canteenUnit = null,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetGuestCardsQuery(pageNumber, pageSize, canteenUnit), ct);
        return result.Items;
    }

    public async Task<GuestCardDto?> GetGuestCardById(
        [Service] IMediator mediator,
        long canteenUnit,
        CancellationToken ct = default)
        => await mediator.Send(new GetGuestCardByIdQuery(canteenUnit), ct);
}
