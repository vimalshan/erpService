using MediatR;
using AdminService.Application.DTOs;

namespace AdminService.Application.Queries;

/// <summary>
/// Query to get all admin units
/// </summary>
public record GetAllAdminUnitsQuery : IRequest<IEnumerable<AdminUnitDto>>;
