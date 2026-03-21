using MediatR;
using TourPlanService.Application.DTOs;

namespace TourPlanService.Application.Queries.GetTourPlanById;

public sealed record GetTourPlanByIdQuery(string TpId) : IRequest<TourPlanDto?>;
