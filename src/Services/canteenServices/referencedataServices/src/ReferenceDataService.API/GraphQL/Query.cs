using MediatR;
using ReferenceDataService.Application.DTOs;
using ReferenceDataService.Application.Queries.GetAllLovMasters;
using ReferenceDataService.Application.Queries.GetAllLovTypeMasters;
using ReferenceDataService.Application.Queries.GetAllPathToSqlServers;
using ReferenceDataService.Application.Queries.GetLovMasterById;
using ReferenceDataService.Application.Queries.GetLovTypeMasterByCode;

namespace ReferenceDataService.API.GraphQL;

public class Query
{
    public async Task<IEnumerable<LovMasterDto>> GetLovMasters([Service] IMediator mediator)
    {
        return await mediator.Send(new GetAllLovMastersQuery());
    }

    public async Task<LovMasterDto?> GetLovMasterById([Service] IMediator mediator, string lovId)
    {
        return await mediator.Send(new GetLovMasterByIdQuery(lovId));
    }

    public async Task<IEnumerable<LovTypeMasterDto>> GetLovTypeMasters([Service] IMediator mediator)
    {
        return await mediator.Send(new GetAllLovTypeMastersQuery());
    }

    public async Task<LovTypeMasterDto?> GetLovTypeMasterByCode([Service] IMediator mediator, string lovTypeCode)
    {
        return await mediator.Send(new GetLovTypeMasterByCodeQuery(lovTypeCode));
    }

    public async Task<IEnumerable<PathToSqlServerDto>> GetPathToSqlServers([Service] IMediator mediator)
    {
        return await mediator.Send(new GetAllPathToSqlServersQuery());
    }
}
