using EmployeePrideManagement.Application.DTOs;
using MediatR;

namespace EmployeePrideManagement.Application.Queries.GetPrideMomentById;

public record GetPrideMomentByIdQuery(decimal MomentPrideId) : IRequest<PrideMomentDto?>;
