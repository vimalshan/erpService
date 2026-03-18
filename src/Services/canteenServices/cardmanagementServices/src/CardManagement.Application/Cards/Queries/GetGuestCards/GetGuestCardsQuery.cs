using MediatR;
using CardManagement.Application.Common.DTOs;
using CardManagement.Application.Common.Models;

namespace CardManagement.Application.Cards.Queries.GetGuestCards;

public record GetGuestCardsQuery(int PageNumber = 1, int PageSize = 20, long? CanteenUnit = null)
    : IRequest<PagedResult<GuestCardDto>>;
