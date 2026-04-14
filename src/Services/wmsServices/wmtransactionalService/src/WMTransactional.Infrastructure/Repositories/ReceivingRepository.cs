using Microsoft.EntityFrameworkCore;
using WMTransactional.Domain.Entities;
using WMTransactional.Domain.Interfaces;
using WMTransactional.Infrastructure.Persistence;

namespace WMTransactional.Infrastructure.Repositories;

public class ReceivingRepository : IReceivingRepository
{
    private readonly WMTransactionalDbContext _context;

    public ReceivingRepository(WMTransactionalDbContext context)
    {
        _context = context;
    }

    public async Task<Receiving?> GetByIdAsync(int receivingId, CancellationToken ct = default)
    {
        return await _context.Receivings
            .Include(r => r.Lines)
            .FirstOrDefaultAsync(r => r.ReceivingId == receivingId, ct);
    }

    public async Task<Receiving?> GetByNumberAsync(string receivingNumber, CancellationToken ct = default)
    {
        return await _context.Receivings
            .Include(r => r.Lines)
            .FirstOrDefaultAsync(r => r.ReceivingNumber == receivingNumber, ct);
    }

    public async Task<IEnumerable<Receiving>> GetByPurchaseOrderAsync(int poId, CancellationToken ct = default)
    {
        return await _context.Receivings
            .Include(r => r.Lines)
            .Where(r => r.PoId == poId)
            .OrderByDescending(r => r.CreatedDate)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<Receiving>> GetByStatusAsync(string status, CancellationToken ct = default)
    {
        return await _context.Receivings
            .Include(r => r.Lines)
            .Where(r => r.Status == status)
            .OrderByDescending(r => r.CreatedDate)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<Receiving>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Receivings
            .Include(r => r.Lines)
            .OrderByDescending(r => r.CreatedDate)
            .ToListAsync(ct);
    }

    public async Task AddAsync(Receiving receiving, CancellationToken ct = default)
    {
        await _context.Receivings.AddAsync(receiving, ct);
    }

    public Task UpdateAsync(Receiving receiving, CancellationToken ct = default)
    {
        _context.Receivings.Update(receiving);
        return Task.CompletedTask;
    }
}
