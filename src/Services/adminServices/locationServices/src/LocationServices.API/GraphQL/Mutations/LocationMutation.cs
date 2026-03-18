using LocationServices.Application.Commands;
using LocationServices.Application.DTOs;
using MediatR;

namespace LocationServices.API.GraphQL.Mutations;

/// <summary>GraphQL Mutation type for LocationAppMap CRUD operations</summary>
public sealed class LocationMutation
{
    // ── Create ───────────────────────────────────────────────────────────────
    [GraphQLDescription("Create a new location-app mapping.")]
    [Error(typeof(GraphQLOperationError))]
    public async Task<LocationAppMapDto> CreateLocationAppMap(
        CreateLocationAppMapInput input,
        [Service] IMediator mediator,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new CreateLocationAppMapCommand(
                input.LocationId, input.AppName, input.SiteCategoryCode,
                input.SelfAccess, input.DeemedApproval, input.CreatedBy),
            ct);

        if (!result.IsSuccess)
            throw new GraphQLOperationError(result.Error!);

        return result.Value!;
    }

    // ── Update ───────────────────────────────────────────────────────────────
    [GraphQLDescription("Update an existing location-app mapping.")]
    [Error(typeof(GraphQLOperationError))]
    public async Task<LocationAppMapDto> UpdateLocationAppMap(
        UpdateLocationAppMapInput input,
        [Service] IMediator mediator,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new UpdateLocationAppMapCommand(
                input.LocationId, input.AppName, input.SiteCategoryCode,
                input.SelfAccess, input.DeemedApproval,
                input.IsActive ?? true, input.ModifiedBy),
            ct);

        if (!result.IsSuccess)
            throw new GraphQLOperationError(result.Error!);

        return result.Value!;
    }

    // ── Delete ───────────────────────────────────────────────────────────────
    [GraphQLDescription("Soft-delete (deactivate) a location-app mapping.")]
    [Error(typeof(GraphQLOperationError))]
    public async Task<bool> DeleteLocationAppMap(
        decimal locationId, string appName, string modifiedBy,
        [Service] IMediator mediator,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new DeleteLocationAppMapCommand(locationId, appName, modifiedBy), ct);

        if (!result.IsSuccess)
            throw new GraphQLOperationError(result.Error!);

        return true;
    }
}

// ── INPUT TYPES ──────────────────────────────────────────────────────────────
public sealed record CreateLocationAppMapInput(
    decimal LocationId, string AppName, long? SiteCategoryCode,
    string? SelfAccess, string? DeemedApproval, string CreatedBy);

public sealed record UpdateLocationAppMapInput(
    decimal LocationId, string AppName, long? SiteCategoryCode,
    string? SelfAccess, string? DeemedApproval, bool? IsActive, string ModifiedBy);

// ── CUSTOM ERROR TYPE ─────────────────────────────────────────────────────────
public sealed class GraphQLOperationError : Exception
{
    public GraphQLOperationError(string message) : base(message) { }
}
