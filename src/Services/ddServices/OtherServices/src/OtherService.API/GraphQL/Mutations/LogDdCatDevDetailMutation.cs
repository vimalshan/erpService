using MediatR;
using OtherService.Application.CQRS.Commands.CreateLogDdCatDevDetail;
using OtherService.Application.CQRS.Commands.DeleteLogDdCatDevDetail;
using OtherService.Application.CQRS.Commands.UpdateLogDdCatDevDetail;
using OtherService.Application.DTOs;

namespace OtherService.API.GraphQL.Mutations;

[MutationType]
public sealed class LogDdCatDevDetailMutation
{
    public async Task<LogDdCatDevDetailDto> CreateLogDdCatDevDetail(
        decimal? reqNum,
        decimal? qtnNum,
        decimal? ansSrl,
        string appId,
        decimal appNum,
        DateTime? entDat,
        string? desc,
        string? need,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(
            new CreateLogDdCatDevDetailCommand(reqNum, qtnNum, ansSrl, appId, appNum, entDat, desc, need),
            ct);

    public async Task<LogDdCatDevDetailDto?> UpdateLogDdCatDevDetail(
        string appId,
        decimal appNum,
        decimal? reqNum,
        decimal? qtnNum,
        decimal? ansSrl,
        DateTime? entDat,
        string? desc,
        string? need,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(
            new UpdateLogDdCatDevDetailCommand(appId, appNum, reqNum, qtnNum, ansSrl, entDat, desc, need),
            ct);

    public async Task<bool> DeleteLogDdCatDevDetail(
        string appId,
        decimal appNum,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new DeleteLogDdCatDevDetailCommand(appId, appNum), ct);
}
