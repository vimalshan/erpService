using MemberService.Domain.Aggregates;
using MemberService.Domain.Enums;
using MemberService.Domain.Interfaces;
using MemberService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MemberService.Infrastructure.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly MemberDbContext _context;

    public MemberRepository(MemberDbContext context) => _context = context;

    public async Task<Member?> GetByIdAsync(long memberNo, CancellationToken ct = default) =>
        await _context.Members
            .Include(m => m.Nominees)
            .Include(m => m.PayrollRecords)
            .Include(m => m.Contacts)
            .FirstOrDefaultAsync(m => m.MemberNo == memberNo, ct);

    public async Task<Member?> GetByEmployeeSysIdAsync(long employeeSysId, CancellationToken ct = default) =>
        await _context.Members
            .Include(m => m.Nominees)
            .Include(m => m.Contacts)
            .FirstOrDefaultAsync(m => m.EmployeeSysId == employeeSysId && m.Status == MemberStatus.Active, ct);

    public async Task<IReadOnlyList<Member>> GetAllActiveAsync(CancellationToken ct = default) =>
        await _context.Members
            .Include(m => m.Nominees)
            .Where(m => m.Status == MemberStatus.Active)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Member>> GetByTrustCodeAsync(string trustCode, CancellationToken ct = default) =>
        await _context.Members
            .Include(m => m.Nominees)
            .Where(m => m.TrustCode == trustCode && m.Status == MemberStatus.Active)
            .ToListAsync(ct);

    public async Task<bool> ExistsByEmployeeSysIdAsync(long employeeSysId, CancellationToken ct = default) =>
        await _context.Members
            .AnyAsync(m => m.EmployeeSysId == employeeSysId && m.Status == MemberStatus.Active, ct);

    public async Task<long> GetNextMemberNumberAsync(CancellationToken ct = default)
    {
        var max = await _context.Members.MaxAsync(m => (long?)m.MemberNo, ct);
        return (max ?? 0) + 1;
    }

    public async Task AddAsync(Member member, CancellationToken ct = default) =>
        await _context.Members.AddAsync(member, ct);

    public Task UpdateAsync(Member member, CancellationToken ct = default)
    {
        _context.Members.Update(member);
        return Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        await _context.SaveChangesAsync(ct);
}
