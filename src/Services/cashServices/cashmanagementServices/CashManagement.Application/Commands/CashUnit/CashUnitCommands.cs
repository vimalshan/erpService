using MediatR;
using CashManagement.Application.DTOs;

namespace CashManagement.Application.Commands.CashUnit;

public record CreateCashUnitCommand(
    long CashUnitId,
    string Name,
    string Code,
    string? Location,
    long? InChargeEmployeeId,
    decimal OpeningBalance,
    long CreatedBy
) : IRequest<CashUnitDto>;

public record UpdateCashUnitStatusCommand(
    long CashUnitId,
    bool IsActive,
    long UpdatedBy
) : IRequest<bool>;
