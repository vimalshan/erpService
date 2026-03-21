using MediatR;
using AdminService.Application.DTOs;

namespace AdminService.Application.Commands;

/// <summary>
/// Command to update an admin unit
/// </summary>
public record UpdateAdminUnitCommand(
    long Id,
    string Name,
    string? AdminType,
    string? UnitCode,
    long? CabUnit,
    string? ImageUrl,
    long? SortOrder
) : IRequest<AdminUnitDto>;
