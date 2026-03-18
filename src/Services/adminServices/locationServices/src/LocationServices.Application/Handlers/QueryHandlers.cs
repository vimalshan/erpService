using LocationServices.Application.Abstractions;
using LocationServices.Application.DTOs;
using LocationServices.Application.Handlers;
using LocationServices.Application.Queries;
using LocationServices.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace LocationServices.Application.Handlers;

// ── GET ALL ──────────────────────────────────────────────────────────────────
public sealed class GetAllLocationAppMapsQueryHandler
    : IQueryHandler<GetAllLocationAppMapsQuery, IEnumerable<LocationAppMapDto>>
{
    private readonly ILocationAppMapReadRepository _readRepo;

    public GetAllLocationAppMapsQueryHandler(ILocationAppMapReadRepository readRepo)
        => _readRepo = readRepo;

    public async Task<Result<IEnumerable<LocationAppMapDto>>> Handle(
        GetAllLocationAppMapsQuery request, CancellationToken cancellationToken)
    {
        var items = await _readRepo.GetAllAsync(cancellationToken);
        return Result<IEnumerable<LocationAppMapDto>>.Success(items.Select(m => m.ToDto()));
    }
}

// ── GET ACTIVE ───────────────────────────────────────────────────────────────
public sealed class GetActiveLocationAppMapsQueryHandler
    : IQueryHandler<GetActiveLocationAppMapsQuery, IEnumerable<LocationAppMapDto>>
{
    private readonly ILocationAppMapReadRepository _readRepo;

    public GetActiveLocationAppMapsQueryHandler(ILocationAppMapReadRepository readRepo)
        => _readRepo = readRepo;

    public async Task<Result<IEnumerable<LocationAppMapDto>>> Handle(
        GetActiveLocationAppMapsQuery request, CancellationToken cancellationToken)
    {
        var items = await _readRepo.GetActiveMappingsAsync(cancellationToken);
        return Result<IEnumerable<LocationAppMapDto>>.Success(items.Select(m => m.ToDto()));
    }
}

// ── GET BY LOCATION ──────────────────────────────────────────────────────────
public sealed class GetLocationAppMapsByLocationQueryHandler
    : IQueryHandler<GetLocationAppMapsByLocationQuery, IEnumerable<LocationAppMapDto>>
{
    private readonly ILocationAppMapReadRepository _readRepo;

    public GetLocationAppMapsByLocationQueryHandler(ILocationAppMapReadRepository readRepo)
        => _readRepo = readRepo;

    public async Task<Result<IEnumerable<LocationAppMapDto>>> Handle(
        GetLocationAppMapsByLocationQuery request, CancellationToken cancellationToken)
    {
        var items = await _readRepo.GetByLocationIdAsync(request.LocationId, cancellationToken);
        return Result<IEnumerable<LocationAppMapDto>>.Success(items.Select(m => m.ToDto()));
    }
}

// ── GET SINGLE ───────────────────────────────────────────────────────────────
public sealed class GetLocationAppMapQueryHandler
    : IQueryHandler<GetLocationAppMapQuery, LocationAppMapDto>
{
    private readonly ILocationAppMapReadRepository _readRepo;

    public GetLocationAppMapQueryHandler(ILocationAppMapReadRepository readRepo)
        => _readRepo = readRepo;

    public async Task<Result<LocationAppMapDto>> Handle(
        GetLocationAppMapQuery request, CancellationToken cancellationToken)
    {
        var item = await _readRepo.GetMappingAsync(request.LocationId, request.AppName, cancellationToken);
        return item is null
            ? Result<LocationAppMapDto>.Failure($"Mapping {request.LocationId}/{request.AppName} not found.")
            : Result<LocationAppMapDto>.Success(item.ToDto());
    }
}

// ── GET COUNT ─────────────────────────────────────────────────────────────────
public sealed class GetLocationAppMapCountQueryHandler
    : IQueryHandler<GetLocationAppMapCountQuery, int>
{
    private readonly ILocationAppMapReadRepository _readRepo;

    public GetLocationAppMapCountQueryHandler(ILocationAppMapReadRepository readRepo)
        => _readRepo = readRepo;

    public async Task<Result<int>> Handle(
        GetLocationAppMapCountQuery request, CancellationToken cancellationToken)
    {
        var count = await _readRepo.GetTotalCountAsync(cancellationToken);
        return Result<int>.Success(count);
    }
}

// ── SHARED MAPPING ───────────────────────────────────────────────────────────
file static class Extensions
{
    public static LocationAppMapDto ToDto(this LocationAppMapReadModel m) => new(
        m.LocationId, m.AppName, m.SiteCategoryCode, m.SelfAccess,
        m.DeemedApproval, m.IsActive, m.CreatedDate, m.CreatedBy, m.ModifiedDate, m.ModifiedBy);
}
