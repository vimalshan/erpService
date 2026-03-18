using LocationServices.Application.DTOs;
using LocationServices.Application.Queries;
using MediatR;

namespace LocationServices.API.GraphQL.Queries;

/// <summary>GraphQL Query type for LocationAppMap</summary>
public sealed class LocationQuery
{
    // ── All ──────────────────────────────────────────────────────────────────
    [GraphQLDescription("Get all location-app mappings.")]
    public async Task<IEnumerable<LocationAppMapDto>> GetLocationAppMaps(
        [Service] IMediator mediator, CancellationToken ct) =>
        (await mediator.Send(new GetAllLocationAppMapsQuery(), ct)).Value ?? [];

    // ── Active only ──────────────────────────────────────────────────────────
    [GraphQLDescription("Get all active mappings.")]
    public async Task<IEnumerable<LocationAppMapDto>> GetActiveLocationAppMaps(
        [Service] IMediator mediator, CancellationToken ct) =>
        (await mediator.Send(new GetActiveLocationAppMapsQuery(), ct)).Value ?? [];

    // ── By location ──────────────────────────────────────────────────────────
    [GraphQLDescription("Get mappings for a specific location ID.")]
    public async Task<IEnumerable<LocationAppMapDto>> GetLocationAppMapsByLocation(
        decimal locationId,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        (await mediator.Send(new GetLocationAppMapsByLocationQuery(locationId), ct)).Value ?? [];

    // ── Single ───────────────────────────────────────────────────────────────
    [GraphQLDescription("Get a single mapping by locationId + appName.")]
    public async Task<LocationAppMapDto?> GetLocationAppMap(
        decimal locationId, string appName,
        [Service] IMediator mediator,
        CancellationToken ct)
    {
        var result = await mediator.Send(new GetLocationAppMapQuery(locationId, appName), ct);
        return result.IsSuccess ? result.Value : null;
    }

    // ── Count ─────────────────────────────────────────────────────────────────
    [GraphQLDescription("Get total count of all mappings.")]
    public async Task<int> GetLocationAppMapCount(
        [Service] IMediator mediator, CancellationToken ct) =>
        (await mediator.Send(new GetLocationAppMapCountQuery(), ct)).Value;
}
