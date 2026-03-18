using MasterDataService.Application.DTOs;
using MasterDataService.Application.Features.LovMaster.Queries;
using MasterDataService.Application.Features.Configuration.Queries;
using MasterDataService.Application.Features.RateMaster.Queries;
using MasterDataService.Infrastructure.Dapper;
using MediatR;

namespace MasterDataService.API.GraphQL;

public class Query
{
    public async Task<IEnumerable<LovMasterDto>> GetLovValues(
        [Service] IMediator mediator,
        string? category = null,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetAllLovQuery(category), cancellationToken);

    public async Task<LovMasterDto?> GetLovById(
        decimal id,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetLovByIdQuery(id), cancellationToken);

    public async Task<IEnumerable<ConfigurationDto>> GetConfigurations(
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetAllConfigurationsQuery(), cancellationToken);

    public async Task<ConfigurationDto?> GetConfigurationByKey(
        string key,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetConfigurationByKeyQuery(key), cancellationToken);

    public async Task<IEnumerable<RateMasterDto>> GetRates(
        string? trustCode,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetAllRatesQuery(trustCode), cancellationToken);

    public async Task<IEnumerable<FundTypeDto>> GetFundTypes(
        [Service] IDapperQueryService dapperQueryService,
        CancellationToken cancellationToken = default)
        => await dapperQueryService.GetAllFundTypesDapperAsync(cancellationToken);

    public async Task<IEnumerable<StatusMasterDto>> GetStatuses(
        string statusType,
        [Service] IDapperQueryService dapperQueryService,
        CancellationToken cancellationToken = default)
        => await dapperQueryService.GetStatusByTypeDapperAsync(statusType, cancellationToken);
}
