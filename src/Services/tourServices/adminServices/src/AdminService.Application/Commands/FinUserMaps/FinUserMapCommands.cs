using MediatR;
using AdminService.Application.DTOs;

namespace AdminService.Application.Commands.FinUserMaps;

public record CreateAdminFinUserMapCommand(
    string FinanceMapId,
    string FinancePayUnitId,
    string FinanceEmpSysId,
    string? FinanceLastModifiedBy
) : IRequest<AdminFinUserMapDto>;

public record UpdateAdminFinUserMapCommand(
    string FinanceMapId,
    string FinancePayUnitId,
    string FinanceEmpSysId,
    string? FinanceLastModifiedBy
) : IRequest<AdminFinUserMapDto>;

public record DeleteAdminFinUserMapCommand(string FinanceMapId) : IRequest<bool>;
