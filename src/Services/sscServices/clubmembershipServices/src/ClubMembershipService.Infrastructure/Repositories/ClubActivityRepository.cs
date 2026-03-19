using Microsoft.EntityFrameworkCore;
using ClubMembershipService.Domain.Entities;
using ClubMembershipService.Domain.Interfaces;
using ClubMembershipService.Infrastructure.Data;

namespace ClubMembershipService.Infrastructure.Repositories;

public class ClubActivityRepository : IClubActivityRepository
{
    private readonly ClubMembershipDbContext _context;

    public ClubActivityRepository(ClubMembershipDbContext context) => _context = context;

    public async Task<ClubActivity?> GetByIdAsync(long activityId, CancellationToken ct = default)
        => await _context.ClubActivities.FindAsync(new object[] { activityId }, ct);

    public async Task<IEnumerable<ClubActivity>> GetByClubIdAsync(long clubId, CancellationToken ct = default)
        => await _context.ClubActivities.AsNoTracking()
            .Where(a => a.ClubId == clubId).ToListAsync(ct);

    public async Task<IEnumerable<ClubActivity>> GetAllAsync(CancellationToken ct = default)
        => await _context.ClubActivities.AsNoTracking().ToListAsync(ct);

    public async Task<ClubActivity> AddAsync(ClubActivity activity, CancellationToken ct = default)
    {
        await _context.ClubActivities.AddAsync(activity, ct);
        await _context.SaveChangesAsync(ct);
        return activity;
    }

    public async Task UpdateAsync(ClubActivity activity, CancellationToken ct = default)
    {
        _context.ClubActivities.Update(activity);
        await _context.SaveChangesAsync(ct);
    }
}
