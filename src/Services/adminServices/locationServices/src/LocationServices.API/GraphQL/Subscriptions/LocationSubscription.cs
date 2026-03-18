using LocationServices.Application.DTOs;

namespace LocationServices.API.GraphQL.Subscriptions;

/// <summary>
/// GraphQL Subscription type — Banana Cake Pop / real-time clients subscribe here.
/// Backed by HotChocolate InMemory subscriptions (swap for Redis in production).
/// </summary>
public sealed class LocationSubscription
{
    // ── On Created ───────────────────────────────────────────────────────────
    [Subscribe]
    [Topic(Topics.LocationCreated)]
    [GraphQLDescription("Fired whenever a new location-app mapping is created.")]
    public LocationAppMapDto OnLocationAppMapCreated(
        [EventMessage] LocationAppMapDto created) => created;

    // ── On Updated ───────────────────────────────────────────────────────────
    [Subscribe]
    [Topic(Topics.LocationUpdated)]
    [GraphQLDescription("Fired whenever an existing mapping is updated.")]
    public LocationAppMapDto OnLocationAppMapUpdated(
        [EventMessage] LocationAppMapDto updated) => updated;

    // ── On Deleted ───────────────────────────────────────────────────────────
    [Subscribe]
    [Topic(Topics.LocationDeleted)]
    [GraphQLDescription("Fired whenever a mapping is deactivated/deleted.")]
    public DeletedEvent OnLocationAppMapDeleted(
        [EventMessage] DeletedEvent deleted) => deleted;
}

// ── TOPIC CONSTANTS ───────────────────────────────────────────────────────────
public static class Topics
{
    public const string LocationCreated = "LOCATION_APP_MAP_CREATED";
    public const string LocationUpdated = "LOCATION_APP_MAP_UPDATED";
    public const string LocationDeleted = "LOCATION_APP_MAP_DELETED";
}

public sealed record DeletedEvent(decimal LocationId, string AppName, string DeletedBy);
