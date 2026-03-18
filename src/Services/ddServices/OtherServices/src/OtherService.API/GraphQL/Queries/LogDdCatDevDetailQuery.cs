using MediatR;
using OtherService.Application.CQRS.Queries.GetAllLogDdCatDevDetails;
using OtherService.Application.CQRS.Queries.GetLogDdCatDevDetailByKey;
using OtherService.Application.CQRS.Queries.GetLogDdCatDevDetailsByReqNum;
using OtherService.Application.DTOs;

namespace OtherService.API.GraphQL.Queries;

[QueryType]
public sealed class LogDdCatDevDetailQuery
{
    public async Task<IEnumerable<LogDdCatDevDetailDto>> GetAllLogDdCatDevDetails(
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new GetAllLogDdCatDevDetailsQuery(), ct);

    public async Task<LogDdCatDevDetailDto?> GetLogDdCatDevDetailByKey(
        string appId,
        decimal appNum,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new GetLogDdCatDevDetailByKeyQuery(appId, appNum), ct);

    public async Task<IEnumerable<LogDdCatDevDetailDto>> GetLogDdCatDevDetailsByReqNum(
        decimal reqNum,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new GetLogDdCatDevDetailsByReqNumQuery(reqNum), ct);
}
