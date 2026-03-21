using MediatR;
using TravelRequestService.Application.DTOs;

namespace TravelRequestService.Application.Queries;

public record GetTravelRequestsByUserQuery(long UserNumber) : IRequest<IReadOnlyList<TravelRequestDto>>;
