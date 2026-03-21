using MediatR;
using TravelRequestService.Application.DTOs;

namespace TravelRequestService.Application.Queries;

public record GetAllTravelRequestsQuery : IRequest<IReadOnlyList<TravelRequestDto>>;
