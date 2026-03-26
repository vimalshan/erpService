using MediatR;
using ReferenceDataService.Application.Commands.CreateLovMaster;
using ReferenceDataService.Application.Commands.CreateLovTypeMaster;
using ReferenceDataService.Application.Commands.CreatePathToSqlServer;
using ReferenceDataService.Application.Commands.DeleteLovMaster;
using ReferenceDataService.Application.Commands.DeleteLovTypeMaster;
using ReferenceDataService.Application.Commands.DeletePathToSqlServer;
using ReferenceDataService.Application.Commands.UpdateLovMaster;
using ReferenceDataService.Application.Commands.UpdateLovTypeMaster;
using ReferenceDataService.Application.Commands.UpdatePathToSqlServer;
using ReferenceDataService.Application.DTOs;

namespace ReferenceDataService.API.GraphQL;

public class Mutation
{
    public async Task<LovMasterDto> CreateLovMaster([Service] IMediator mediator, string lovId, string? lovType, string? lovName)
    {
        return await mediator.Send(new CreateLovMasterCommand(lovId, lovType, lovName));
    }

    public async Task<LovMasterDto> UpdateLovMaster([Service] IMediator mediator, string lovId, string? lovType, string? lovName)
    {
        return await mediator.Send(new UpdateLovMasterCommand(lovId, lovType, lovName));
    }

    public async Task<bool> DeleteLovMaster([Service] IMediator mediator, string lovId)
    {
        return await mediator.Send(new DeleteLovMasterCommand(lovId));
    }

    public async Task<LovTypeMasterDto> CreateLovTypeMaster([Service] IMediator mediator, string lovTypeCode, string? lovTypeName)
    {
        return await mediator.Send(new CreateLovTypeMasterCommand(lovTypeCode, lovTypeName));
    }

    public async Task<LovTypeMasterDto> UpdateLovTypeMaster([Service] IMediator mediator, string lovTypeCode, string? lovTypeName)
    {
        return await mediator.Send(new UpdateLovTypeMasterCommand(lovTypeCode, lovTypeName));
    }

    public async Task<bool> DeleteLovTypeMaster([Service] IMediator mediator, string lovTypeCode)
    {
        return await mediator.Send(new DeleteLovTypeMasterCommand(lovTypeCode));
    }

    public async Task<PathToSqlServerDto> CreatePathToSqlServer([Service] IMediator mediator,
        string? companyCode, string? serverName, string? databaseName, string? userId, string? dbPassword)
    {
        return await mediator.Send(new CreatePathToSqlServerCommand(companyCode, serverName, databaseName, userId, dbPassword));
    }

    public async Task<PathToSqlServerDto> UpdatePathToSqlServer([Service] IMediator mediator,
        int id, string? serverName, string? databaseName, string? userId, string? dbPassword)
    {
        return await mediator.Send(new UpdatePathToSqlServerCommand(id, serverName, databaseName, userId, dbPassword));
    }

    public async Task<bool> DeletePathToSqlServer([Service] IMediator mediator, int id)
    {
        return await mediator.Send(new DeletePathToSqlServerCommand(id));
    }
}
