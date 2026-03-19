using MasterDataService.Application.DTOs;
using MasterDataService.Application.Queries;
using MasterDataService.Application.Commands;
using MediatR;

namespace MasterDataService.API.GraphQL;

public class Query
{
    public async Task<IReadOnlyList<LovMasterDto>> GetLovMasters([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllLovMastersQuery(), ct);

    public async Task<LovMasterDto?> GetLovMasterById([Service] IMediator mediator, long lovId, CancellationToken ct)
        => await mediator.Send(new GetLovMasterByIdQuery(lovId), ct);

    public async Task<IReadOnlyList<LovMasterDto>> GetLovMastersByType([Service] IMediator mediator, string lovType, CancellationToken ct)
        => await mediator.Send(new GetLovMastersByTypeQuery(lovType), ct);

    public async Task<IReadOnlyList<LovTypeMasterDto>> GetLovTypeMasters([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllLovTypeMastersQuery(), ct);

    public async Task<IReadOnlyList<HoldTypeMasterDto>> GetHoldTypeMasters([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllHoldTypeMastersQuery(), ct);

    public async Task<IReadOnlyList<LocationScanParamDto>> GetLocationScanParams([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllLocationScanParamsQuery(), ct);

    public async Task<IReadOnlyList<ScannerMasterDto>> GetScannerMasters([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllScannerMastersQuery(), ct);
}

public class Mutation
{
    public async Task<LovMasterDto> CreateLovMaster([Service] IMediator mediator, long lovId, string lovType, string lovName, CancellationToken ct)
        => await mediator.Send(new CreateLovMasterCommand(lovId, lovType, lovName), ct);

    public async Task<LovMasterDto> UpdateLovMaster([Service] IMediator mediator, long lovId, string lovType, string lovName, CancellationToken ct)
        => await mediator.Send(new UpdateLovMasterCommand(lovId, lovType, lovName), ct);

    public async Task<bool> DeleteLovMaster([Service] IMediator mediator, long lovId, CancellationToken ct)
        => await mediator.Send(new DeleteLovMasterCommand(lovId), ct);

    public async Task<LovTypeMasterDto> CreateLovTypeMaster([Service] IMediator mediator, string typeCode, string typeName, CancellationToken ct)
        => await mediator.Send(new CreateLovTypeMasterCommand(typeCode, typeName), ct);

    public async Task<HoldTypeMasterDto> CreateHoldTypeMaster([Service] IMediator mediator, long holdId, string? holdName, string? holdCategory, CancellationToken ct)
        => await mediator.Send(new CreateHoldTypeMasterCommand(holdId, holdName, holdCategory), ct);

    public async Task<ScannerMasterDto> CreateScannerMaster([Service] IMediator mediator, long deviceId, string? deviceName, long deviceLocationId, string? devicePath, CancellationToken ct)
        => await mediator.Send(new CreateScannerMasterCommand(deviceId, deviceName, deviceLocationId, devicePath), ct);
}
