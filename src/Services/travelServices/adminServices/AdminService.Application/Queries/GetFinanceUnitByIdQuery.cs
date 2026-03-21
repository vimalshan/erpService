using MediatR;
using AdminService.Application.DTOs;

namespace AdminService.Application.Queries;

/// <summary>
/// Query to get finance unit by ID
/// </summary>
public record GetFinanceUnitByIdQuery(long Id) : IRequest<FinanceUnitDto?>;
