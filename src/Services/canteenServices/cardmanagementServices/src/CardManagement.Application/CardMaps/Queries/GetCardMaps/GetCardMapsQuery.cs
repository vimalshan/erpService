using MediatR;
using CardManagement.Application.Common.DTOs;

namespace CardManagement.Application.CardMaps.Queries.GetCardMaps;

public record GetCardMapsQuery(long CanteenUnit, bool ActiveOnly = false) : IRequest<IEnumerable<CanteenCardMapDto>>;
