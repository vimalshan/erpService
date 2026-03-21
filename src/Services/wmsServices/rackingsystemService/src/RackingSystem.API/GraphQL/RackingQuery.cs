using RackingSystem.Application.Common.Interfaces;
using RackingSystem.Application.Features.Bins.Commands;
using RackingSystem.Application.Features.Bins.DTOs;
using RackingSystem.Application.Features.Bins.Queries;
using RackingSystem.Application.Features.Racks.DTOs;
using RackingSystem.Domain.Entities;
using RackingSystem.Domain.Interfaces;

namespace RackingSystem.API.GraphQL;

/// <summary>Hot Chocolate GraphQL Query type for the Racking System.</summary>
public sealed class RackingQuery
{
    public async Task<IEnumerable<RackDto>> GetRacksAsync(
        [Service] IUnitOfWork uow,
        int? zoneId = null,
        CancellationToken ct = default)
    {
        var racks = zoneId.HasValue
            ? await uow.Racks.GetByZoneIdAsync(zoneId.Value, ct)
            : await uow.Racks.GetAllAsync(ct);

        return racks.Select(r => new RackDto(
            r.Id, r.ZoneId, r.Code, r.RackType, r.MaxLoadWeight, r.IsActive,
            r.CreatedDate, r.ModifiedDate,
            r.Shelves.Select(s => new ShelfSummaryDto(s.Id, s.ShelfLevel, s.ShelfPosition, s.Code, s.IsActive))));
    }

    public async Task<BinDto?> GetBinAsync(
        [Service] IUnitOfWork uow,
        int id,
        CancellationToken ct = default)
    {
        var bin = await uow.Bins.GetByIdAsync(id, ct);
        if (bin is null) return null;
        var util = await uow.Bins.GetBinUtilizationAsync(id, ct);
        return MapBin(bin, util);
    }

    public async Task<IEnumerable<BinDto>> GetBinsAsync(
        [Service] IUnitOfWork uow,
        int? zoneId = null,
        string? status = null,
        CancellationToken ct = default)
    {
        var bins = zoneId.HasValue
            ? await uow.Bins.GetByZoneIdAsync(zoneId.Value, ct)
            : status != null
                ? await uow.Bins.GetByStatusAsync(status, ct)
                : await uow.Bins.GetAllAsync(ct);

        return bins.Select(b => MapBin(b, null));
    }

    private static BinDto MapBin(Bin b, decimal? util) => new(
        b.Id, b.ZoneId, b.ShelfId, b.Code, b.Barcode, b.BinType,
        b.CapacityQty, b.CapacityWeight, b.CapacityVolume,
        b.Status, b.IsActive, util, b.CreatedDate, b.ModifiedDate);
}
