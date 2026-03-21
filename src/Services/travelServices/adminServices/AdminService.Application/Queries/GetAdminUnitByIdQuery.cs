using MediatR;
using AdminService.Application.DTOs;

namespace AdminService.Application.Queries;

/// <summary>
/// Query to get admin unit by ID
/// </summary>
public record GetAdminUnitByIdQuery(long Id) : IRequest<AdminUnitDto?>;
