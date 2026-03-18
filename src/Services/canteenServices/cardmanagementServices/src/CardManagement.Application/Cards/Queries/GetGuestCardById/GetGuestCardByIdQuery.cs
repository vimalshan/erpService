using MediatR;
using CardManagement.Application.Common.DTOs;

namespace CardManagement.Application.Cards.Queries.GetGuestCardById;

public record GetGuestCardByIdQuery(long CanteenUnit) : IRequest<GuestCardDto?>;
