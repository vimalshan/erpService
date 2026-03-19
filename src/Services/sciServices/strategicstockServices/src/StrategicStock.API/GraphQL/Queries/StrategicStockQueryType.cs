using HotChocolate.Authorization;
using StrategicStock.Application.DTOs;
using StrategicStock.Application.Queries.GetAllStrategicStocks;
using StrategicStock.Application.Queries.GetStrategicStockById;
using StrategicStock.Application.Queries.GetStrategicStockInfo;
using MediatR;

namespace StrategicStock.API.GraphQL.Queries;

[Authorize]
public sealed class StrategicStockQueryType
{
    public async Task<IReadOnlyList<StrategicStockDto>> GetStrategicStocks(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllStrategicStocksQuery(), ct);

    public async Task<StrategicStockDto?> GetStrategicStockById(
        int id, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetStrategicStockByIdQuery(id), ct);

    public async Task<IReadOnlyList<StrategicStockInfoDto>> GetStrategicStockInfo(
        int itemId, int companyUnitId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetStrategicStockInfoQuery(itemId, companyUnitId), ct);
}
