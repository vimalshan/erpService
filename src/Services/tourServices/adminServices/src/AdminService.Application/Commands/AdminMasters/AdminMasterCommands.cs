using MediatR;
using AdminService.Application.DTOs;

namespace AdminService.Application.Commands.AdminMasters;

// Create
public record CreateAdminMasterCommand(
    string AdminId,
    string AdminName,
    string AdminPic,
    string AdminUnitId,
    string AdminUnitHeadSysId,
    string? AdminLocStatus
) : IRequest<AdminMasterDto>;

// Update
public record UpdateAdminMasterCommand(
    string AdminId,
    string AdminName,
    string AdminPic,
    string AdminUnitId,
    string AdminUnitHeadSysId,
    string? AdminLocStatus
) : IRequest<AdminMasterDto>;

// Delete
public record DeleteAdminMasterCommand(string AdminId) : IRequest<bool>;
