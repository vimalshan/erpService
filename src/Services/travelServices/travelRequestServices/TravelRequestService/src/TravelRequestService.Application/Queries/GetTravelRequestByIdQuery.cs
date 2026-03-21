using MediatR;
using TravelRequestService.Application.DTOs;

namespace TravelRequestService.Application.Queries;

public record GetTravelRequestByIdQuery(long PlanNumber, string CompanyCode) : IRequest<TravelRequestDto?>;
