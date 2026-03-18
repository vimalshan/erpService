using MediatR;
using DevelopmentService.Application.DTOs;

namespace DevelopmentService.Application.Queries.GetPlans;

public record GetPlansQuery(string? UserId, char? Status) : IRequest<IEnumerable<LetPlanDto>>;
