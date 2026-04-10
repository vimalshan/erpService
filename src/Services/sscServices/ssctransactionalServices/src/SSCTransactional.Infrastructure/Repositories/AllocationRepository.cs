using Microsoft.EntityFrameworkCore;
using SSCTransactional.Domain.Aggregates;
using SSCTransactional.Domain.Interfaces;
using SSCTransactional.Infrastructure.Persistence;

namespace SSCTransactional.Infrastructure.Repositories;

public class AllocationRepository : IAllocationRepository
{
    private readonly ApplicationDbContext _context;

    public AllocationRepository(ApplicationDbContext context) => _context = context;

    public async Task<AllocationAggregate?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _context.Allocations.Include(a => a.DefectiveAttachments)
            .FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task<IEnumerable<AllocationAggregate>> GetAllAsync(CancellationToken ct = default)
        => await _context.Allocations.Include(a => a.DefectiveAttachments)
            .OrderByDescending(a => a.AllocatedOn).ToListAsync(ct);

    public async Task<IEnumerable<AllocationAggregate>> GetByDocIdAsync(long docId, CancellationToken ct = default)
        => await _context.Allocations.Include(a => a.DefectiveAttachments)
            .Where(a => a.DocId == docId).ToListAsync(ct);

    public async Task<IEnumerable<AllocationAggregate>> GetByGroupIdAsync(long groupId, CancellationToken ct = default)
        => await _context.Allocations.Where(a => a.GroupId == groupId).ToListAsync(ct);

    public async Task<IEnumerable<AllocationAggregate>> GetPendingByGroupAsync(long groupId, CancellationToken ct = default)
        => await _context.Allocations.Where(a => a.GroupId == groupId && a.ActionFlag == "N").ToListAsync(ct);

    public async Task AddAsync(AllocationAggregate allocation, CancellationToken ct = default)
        => await _context.Allocations.AddAsync(allocation, ct);

    public Task UpdateAsync(AllocationAggregate allocation, CancellationToken ct = default)
    {
        _context.Allocations.Update(allocation);
        return Task.CompletedTask;
    }

    public async Task<long> GetNextIdAsync(CancellationToken ct = default)
    {
        var maxId = await _context.Allocations.MaxAsync(a => (long?)a.Id, ct) ?? 0;
        return maxId + 1;
    }
}
