using HotChocolate.Authorization;
using MediatR;
using StrategicStock.Application.Commands.CloseStrategicStock;
using StrategicStock.Application.Commands.CreateStrategicStock;
using StrategicStock.Application.Commands.UpdateStrategicStock;

namespace StrategicStock.API.GraphQL.Mutations;

[Authorize]
public sealed class StrategicStockMutationType
{
    public async Task<int> CreateStrategicStock(
        CreateStrategicStockCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> UpdateStrategicStock(
        UpdateStrategicStockCommand input, [Service] IMediator mediator, CancellationToken ct)
    {
        await mediator.Send(input, ct);
        return true;
    }

    public async Task<bool> CloseStrategicStock(
        int strategicStockId, int? userId, [Service] IMediator mediator, CancellationToken ct)
    {
        await mediator.Send(new CloseStrategicStockCommand(strategicStockId, userId), ct);
        return true;
    }
}
