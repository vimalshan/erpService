using ExpenseService.Domain.Entities;
using ExpenseService.Domain.Interfaces;
using ExpenseService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseService.Infrastructure.Repositories;

public class DaSummaryRepository : IDaSummaryRepository
{
    private readonly ExpenseDbContext _context;

    public DaSummaryRepository(ExpenseDbContext context)
    {
        _context = context;
    }

    public async Task<DaSummary?> GetByRequestIdAsync(long requestId, CancellationToken ct = default)
    {
        return await _context.DaSummaries.FirstOrDefaultAsync(s => s.RequestId == requestId, ct);
    }

    public async Task<DaSummary> AddAsync(DaSummary summary, CancellationToken ct = default)
    {
        _context.DaSummaries.Add(summary);
        await _context.SaveChangesAsync(ct);
        return summary;
    }

    public async Task UpdateAsync(DaSummary summary, CancellationToken ct = default)
    {
        _context.DaSummaries.Update(summary);
        await _context.SaveChangesAsync(ct);
    }
}
