using MediatR;
using AdminService.Application.DTOs;

namespace AdminService.Application.Commands.UserMaps;

public record CreateAdminUserMapCommand(
    string AdminMapId,
    string AdminBookType,
    string AdminMode,
    string AdminEmpSysId,
    string AdminId,
    string AdminLastModifiedBy
) : IRequest<AdminUserMapDto>;

public record UpdateAdminUserMapCommand(
    string AdminMapId,
    string AdminBookType,
    string AdminMode,
    string AdminEmpSysId,
    string AdminId,
    string AdminLastModifiedBy
) : IRequest<AdminUserMapDto>;

public record DeleteAdminUserMapCommand(string AdminMapId) : IRequest<bool>;
