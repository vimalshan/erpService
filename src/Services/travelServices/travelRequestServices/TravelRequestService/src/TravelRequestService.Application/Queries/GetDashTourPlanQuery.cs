using MediatR;
using TravelRequestService.Application.DTOs;

namespace TravelRequestService.Application.Queries;

public record GetDashTourPlanQuery : IRequest<IReadOnlyList<DashTourPlanDto>>;
