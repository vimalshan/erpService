using Microsoft.EntityFrameworkCore;
using ClubMembershipService.Domain.Entities;
using ClubMembershipService.Domain.Interfaces;
using ClubMembershipService.Infrastructure.Data;

namespace ClubMembershipService.Infrastructure.Repositories;

public class ClubRepository : IClubRepository
{
    private readonly ClubMembershipDbContext _context;

    public ClubRepository(ClubMembershipDbContext context) => _context = context;

    public async Task<ClubMaster?> GetByIdAsync(long clubId, CancellationToken ct = default)
        => await _context.ClubMasters.FindAsync(new object[] { clubId }, ct);

    public async Task<IEnumerable<ClubMaster>> GetAllAsync(CancellationToken ct = default)
        => await _context.ClubMasters.AsNoTracking().ToListAsync(ct);

    public async Task<IEnumerable<ClubMaster>> GetActiveAsync(CancellationToken ct = default)
        => await _context.ClubMasters.AsNoTracking()
            .Where(c => c.Status == Domain.ValueObjects.ClubStatus.Active)
            .ToListAsync(ct);

    public async Task<ClubMaster> AddAsync(ClubMaster club, CancellationToken ct = default)
    {
        await _context.ClubMasters.AddAsync(club, ct);
        await _context.SaveChangesAsync(ct);
        return club;
    }

    public async Task UpdateAsync(ClubMaster club, CancellationToken ct = default)
    {
        _context.ClubMasters.Update(club);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<int> GetActiveCountAsync(CancellationToken ct = default)
        => await _context.ClubMasters.CountAsync(c => c.Status == Domain.ValueObjects.ClubStatus.Active, ct);
}
