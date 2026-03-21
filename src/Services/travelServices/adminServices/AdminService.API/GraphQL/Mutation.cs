using AdminService.Application.Commands;
using AdminService.Application.DTOs;
using MediatR;

namespace AdminService.API.GraphQL;

/// <summary>
/// GraphQL mutation type
/// </summary>
public class Mutation
{
    public async Task<AdminUnitDto> CreateAdminUnit(
        long adminCode,
        string name,
        string? adminType,
        string? unitCode,
        long? cabUnit,
        string? imageUrl,
        long? sortOrder,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new CreateAdminUnitCommand(adminCode, name, adminType, unitCode, cabUnit, imageUrl, sortOrder);
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<AdminUnitDto> UpdateAdminUnit(
        long id,
        string name,
        string? adminType,
        string? unitCode,
        long? cabUnit,
        string? imageUrl,
        long? sortOrder,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new UpdateAdminUnitCommand(id, name, adminType, unitCode, cabUnit, imageUrl, sortOrder);
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<bool> DeleteAdminUnit(
        long id,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new DeleteAdminUnitCommand(id), cancellationToken);
    }

    public async Task<FinanceUnitDto> CreateFinanceUnit(
        long unitId,
        string? unitCode,
        string name,
        long? oracleCode,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new CreateFinanceUnitCommand(unitId, unitCode, name, oracleCode);
        return await mediator.Send(command, cancellationToken);
    }
}
