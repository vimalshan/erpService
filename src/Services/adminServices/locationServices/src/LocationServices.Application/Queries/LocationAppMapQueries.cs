using LocationServices.Application.Abstractions;
using LocationServices.Application.DTOs;

namespace LocationServices.Application.Queries;

// ── GET ALL ─────────────────────────────────────────────────────────────────
public sealed record GetAllLocationAppMapsQuery : IQuery<IEnumerable<LocationAppMapDto>>;

// ── GET ACTIVE ───────────────────────────────────────────────────────────────
public sealed record GetActiveLocationAppMapsQuery : IQuery<IEnumerable<LocationAppMapDto>>;

// ── GET BY LOCATION ─────────────────────────────────────────────────────────
public sealed record GetLocationAppMapsByLocationQuery(
    decimal LocationId) : IQuery<IEnumerable<LocationAppMapDto>>;

// ── GET SINGLE ───────────────────────────────────────────────────────────────
public sealed record GetLocationAppMapQuery(
    decimal LocationId,
    string  AppName) : IQuery<LocationAppMapDto>;

// ── GET COUNT ────────────────────────────────────────────────────────────────
public sealed record GetLocationAppMapCountQuery : IQuery<int>;
