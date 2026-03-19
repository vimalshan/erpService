using Microsoft.EntityFrameworkCore;
using ClubMembershipService.Domain.Entities;
using ClubMembershipService.Domain.Interfaces;
using ClubMembershipService.Infrastructure.Data;

namespace ClubMembershipService.Infrastructure.Repositories;

public class ClubMembershipRepository : IClubMembershipRepository
{
    private readonly ClubMembershipDbContext _context;

    public ClubMembershipRepository(ClubMembershipDbContext context) => _context = context;

    public async Task<ClubMembership?> GetByIdAsync(long membershipId, CancellationToken ct = default)
        => await _context.ClubMemberships.FindAsync(new object[] { membershipId }, ct);

    public async Task<IEnumerable<ClubMembership>> GetByClubIdAsync(long clubId, CancellationToken ct = default)
        => await _context.ClubMemberships.AsNoTracking()
            .Where(m => m.ClubId == clubId).ToListAsync(ct);

    public async Task<IEnumerable<ClubMembership>> GetByMemberIdAsync(long memberId, CancellationToken ct = default)
        => await _context.ClubMemberships.AsNoTracking()
            .Where(m => m.MemberId == memberId).ToListAsync(ct);

    public async Task<bool> ExistsActiveAsync(long clubId, long memberId, CancellationToken ct = default)
        => await _context.ClubMemberships
            .AnyAsync(m => m.ClubId == clubId && m.MemberId == memberId
                && m.Status == Domain.ValueObjects.MembershipStatus.Active, ct);

    public async Task<ClubMembership> AddAsync(ClubMembership membership, CancellationToken ct = default)
    {
        await _context.ClubMemberships.AddAsync(membership, ct);
        await _context.SaveChangesAsync(ct);
        return membership;
    }

    public async Task UpdateAsync(ClubMembership membership, CancellationToken ct = default)
    {
        _context.ClubMemberships.Update(membership);
        await _context.SaveChangesAsync(ct);
    }
}
