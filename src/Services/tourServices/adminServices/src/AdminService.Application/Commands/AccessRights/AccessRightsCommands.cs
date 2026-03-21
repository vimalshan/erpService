using MediatR;
using AdminService.Application.DTOs;

namespace AdminService.Application.Commands.AccessRights;

public record CreateAccessRightsCommand(
    string AdminRightsId,
    string? AdminLocationId,
    string? AdminRightsFor,
    string? AdminRightsType,
    string? AdminUserId,
    string? AdminAlertId,
    string? AdminContactNo,
    string? AdminContactDes,
    string? AdminEntBy
) : IRequest<AdminAccessRightsDto>;

public record UpdateAccessRightsCommand(
    string AdminRightsId,
    string? AdminLocationId,
    string? AdminRightsFor,
    string? AdminRightsType,
    string? AdminUserId,
    string? AdminAlertId,
    string? AdminContactNo,
    string? AdminContactDes,
    string? AdminEntBy
) : IRequest<AdminAccessRightsDto>;

public record DeleteAccessRightsCommand(string AdminRightsId) : IRequest<bool>;
