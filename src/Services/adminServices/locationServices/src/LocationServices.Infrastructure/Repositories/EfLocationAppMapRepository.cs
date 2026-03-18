using LocationServices.Domain.Entities;
using LocationServices.Domain.Repositories;
using LocationServices.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LocationServices.Infrastructure.Repositories;

public sealed class EfLocationAppMapRepository : ILocationAppMapRepository
{
    private readonly LocationDbContext _ctx;
    public EfLocationAppMapRepository(LocationDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<LocationAppMapAggregate>> GetAllAsync(CancellationToken ct = default) =>
        await _ctx.LocationAppMaps.ToListAsync(ct);

    public async Task<LocationAppMapAggregate?> GetMappingAsync(
        decimal locationId, string appName, CancellationToken ct = default) =>
        await _ctx.LocationAppMaps
            .FirstOrDefaultAsync(x => x.LocationId == locationId && x.AppName == appName, ct);

    public async Task<bool> ExistsAsync(
        decimal locationId, string appName, CancellationToken ct = default) =>
        await _ctx.LocationAppMaps
            .AnyAsync(x => x.LocationId == locationId && x.AppName == appName, ct);

    public async Task<IEnumerable<LocationAppMapAggregate>> GetByLocationIdAsync(
        decimal locationId, CancellationToken ct = default) =>
        await _ctx.LocationAppMaps
            .Where(x => x.LocationId == locationId)
            .ToListAsync(ct);

    public async Task<IEnumerable<LocationAppMapAggregate>> GetByAppNameAsync(
        string appName, CancellationToken ct = default) =>
        await _ctx.LocationAppMaps
            .Where(x => x.AppName == appName)
            .ToListAsync(ct);

    public async Task<IEnumerable<LocationAppMapAggregate>> GetActiveMappingsAsync(
        CancellationToken ct = default) =>
        await _ctx.LocationAppMaps
            .Where(x => x.IsActive)
            .ToListAsync(ct);

    public async Task AddAsync(LocationAppMapAggregate aggregate, CancellationToken ct = default)
        => await _ctx.LocationAppMaps.AddAsync(aggregate, ct);

    public void Update(LocationAppMapAggregate aggregate)
        => _ctx.LocationAppMaps.Update(aggregate);

    public void Delete(LocationAppMapAggregate aggregate)
        => _ctx.LocationAppMaps.Remove(aggregate);
}
