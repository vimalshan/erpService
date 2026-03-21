using MediatR;
using AdminService.Application.DTOs;

namespace AdminService.Application.Commands;

/// <summary>
/// Command to create an admin unit
/// </summary>
public record CreateAdminUnitCommand(
    long AdminCode,
    string Name,
    string? AdminType,
    string? UnitCode,
    long? CabUnit,
    string? ImageUrl,
    long? SortOrder
) : IRequest<AdminUnitDto>;
