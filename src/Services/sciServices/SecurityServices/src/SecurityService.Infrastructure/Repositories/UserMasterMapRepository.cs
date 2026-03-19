using Microsoft.EntityFrameworkCore;
using SecurityService.Application.Interfaces;
using SecurityService.Domain.Entities;
using SecurityService.Infrastructure.Data;

namespace SecurityService.Infrastructure.Repositories;

public sealed class UserMasterMapRepository : IUserMasterMapRepository
{
    private readonly SecurityDbContext _db;

    public UserMasterMapRepository(SecurityDbContext db) => _db = db;

    public async Task<IEnumerable<UserMasterMap>> GetAllAsync(CancellationToken ct = default)
        => await _db.UserMasterMaps.AsNoTracking().ToListAsync(ct);

    public async Task<IEnumerable<UserMasterMap>> GetByUserIdAsync(long userId, CancellationToken ct = default)
        => await _db.UserMasterMaps.AsNoTracking().Where(m => m.UserId == userId).ToListAsync(ct);

    public Task<UserMasterMap?> GetByIdAsync(long mapId, CancellationToken ct = default)
        => _db.UserMasterMaps.FirstOrDefaultAsync(m => m.MapId == mapId, ct);

    public async Task<long> AddAsync(UserMasterMap map, CancellationToken ct = default)
    {
        _db.UserMasterMaps.Add(map);
        await _db.SaveChangesAsync(ct);
        return map.MapId;
    }

    public async Task UpdateAsync(UserMasterMap map, CancellationToken ct = default)
    {
        _db.UserMasterMaps.Update(map);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(long mapId, CancellationToken ct = default)
    {
        var map = await _db.UserMasterMaps.FindAsync(new object[] { mapId }, ct);
        if (map is not null)
        {
            _db.UserMasterMaps.Remove(map);
            await _db.SaveChangesAsync(ct);
        }
    }
}
