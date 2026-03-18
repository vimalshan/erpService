using LocationServices.Application.Abstractions;
using LocationServices.Application.DTOs;

namespace LocationServices.Application.Commands;

// ── CREATE ──────────────────────────────────────────────────────────────────
public sealed record CreateLocationAppMapCommand(
    decimal  LocationId,
    string   AppName,
    long?    SiteCategoryCode,
    string?  SelfAccess,
    string?  DeemedApproval,
    string   CreatedBy) : ICommand<LocationAppMapDto>;

// ── UPDATE ──────────────────────────────────────────────────────────────────
public sealed record UpdateLocationAppMapCommand(
    decimal  LocationId,
    string   AppName,
    long?    SiteCategoryCode,
    string?  SelfAccess,
    string?  DeemedApproval,
    bool     IsActive,
    string   ModifiedBy) : ICommand<LocationAppMapDto>;

// ── DELETE ──────────────────────────────────────────────────────────────────
public sealed record DeleteLocationAppMapCommand(
    decimal LocationId,
    string  AppName,
    string  ModifiedBy) : ICommand;
