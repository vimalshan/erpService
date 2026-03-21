using MediatR;
using AdminService.Application.DTOs;

namespace AdminService.Application.Queries;

/// <summary>
/// Query to get all finance units
/// </summary>
public record GetAllFinanceUnitsQuery : IRequest<IEnumerable<FinanceUnitDto>>;
