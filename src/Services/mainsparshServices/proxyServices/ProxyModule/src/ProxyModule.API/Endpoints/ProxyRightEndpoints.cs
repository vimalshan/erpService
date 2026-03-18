using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProxyModule.Application.Commands.CreateProxyRight;
using ProxyModule.Application.Commands.DeactivateProxyRight;
using ProxyModule.Application.Commands.UpdateProxyRight;
using ProxyModule.Application.DTOs;
using ProxyModule.Application.Queries.GetActiveProxyRights;
using ProxyModule.Application.Queries.GetProxyRightById;
using ProxyModule.Application.Queries.GetProxyRightsByUser;

namespace ProxyModule.API.Endpoints;

public static class ProxyRightEndpoints
{
    public static void MapProxyRightEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/proxy-rights")
            .WithTags("ProxyRights-MinimalAPI")
            .RequireAuthorization();

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetProxyRightByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetProxyRightByIdV2")
        .Produces<ProxyRightDto>()
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/user/{proxyUserId:long}", async (long proxyUserId, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetProxyRightsByUserQuery(proxyUserId), ct);
            return Results.Ok(result);
        })
        .WithName("GetProxyRightsByUserV2")
        .Produces<IEnumerable<ProxyRightDto>>();

        group.MapGet("/active", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetActiveProxyRightsQuery(), ct);
            return Results.Ok(result);
        })
        .WithName("GetActiveProxyRightsV2")
        .Produces<IEnumerable<ProxyRightDto>>();

        group.MapPost("/", async ([FromBody] CreateProxyRightDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var command = new CreateProxyRightCommand(
                dto.ProxyUserId, dto.DelegatedUserId, dto.ProxyStartDate,
                dto.ProxyEndDate, dto.ProxyType, dto.Scope, dto.Notes, dto.CreatedBy);

            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/proxy-rights/{result.ProxyId}", result);
        })
        .WithName("CreateProxyRightV2")
        .Produces<ProxyRightDto>(StatusCodes.Status201Created);

        group.MapPut("/{id:long}", async (long id, [FromBody] UpdateProxyRightDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var command = new UpdateProxyRightCommand(
                id, dto.ProxyStartDate, dto.ProxyEndDate, dto.ProxyType,
                dto.Scope, dto.Notes, dto.UpdatedBy);

            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        })
        .WithName("UpdateProxyRightV2")
        .Produces<ProxyRightDto>();

        group.MapDelete("/{id:long}", async (long id, [FromQuery] long updatedBy, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new DeactivateProxyRightCommand(id, updatedBy), ct);
            return Results.NoContent();
        })
        .WithName("DeactivateProxyRightV2")
        .Produces(StatusCodes.Status204NoContent);
    }
}
