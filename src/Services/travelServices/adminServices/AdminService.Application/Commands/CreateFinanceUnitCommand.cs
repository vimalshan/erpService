using MediatR;
using AdminService.Application.DTOs;

namespace AdminService.Application.Commands;

/// <summary>
/// Command to create a finance unit
/// </summary>
public record CreateFinanceUnitCommand(
    long UnitId,
    string? UnitCode,
    string Name,
    long? OracleCode
) : IRequest<FinanceUnitDto>;
