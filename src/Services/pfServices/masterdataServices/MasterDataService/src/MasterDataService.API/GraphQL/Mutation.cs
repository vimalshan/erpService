using MasterDataService.Application.DTOs;
using MasterDataService.Application.Features.LovMaster.Commands;
using MasterDataService.Application.Features.Configuration.Commands;
using MediatR;

namespace MasterDataService.API.GraphQL;

public class Mutation
{
    public async Task<LovMasterDto> CreateLovValue(
        string lovCode,
        string lovDescription,
        string lovValue,
        string lovCategory,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new CreateLovCommand(lovCode, lovDescription, lovValue, lovCategory), cancellationToken);

    public async Task<bool> ActivateLovValue(
        decimal lovId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new ActivateLovCommand(lovId), cancellationToken);

    public async Task<bool> DeactivateLovValue(
        decimal lovId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new DeactivateLovCommand(lovId), cancellationToken);

    public async Task<ConfigurationDto> CreateConfiguration(
        string configKey,
        string configValue,
        string configType,
        string? description,
        long createdBy,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new CreateConfigurationCommand(configKey, configValue, configType, description, createdBy), cancellationToken);
}
