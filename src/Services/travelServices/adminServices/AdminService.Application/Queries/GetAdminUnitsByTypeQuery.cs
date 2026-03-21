using MediatR;
using AdminService.Application.DTOs;

namespace AdminService.Application.Queries;

/// <summary>
/// Query to get admin units by type
/// </summary>
public record GetAdminUnitsByTypeQuery(string AdminType) : IRequest<IEnumerable<AdminUnitDto>>;
