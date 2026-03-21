using Microsoft.EntityFrameworkCore;
using ReceivingService.Domain.Interfaces;
using ReceivingService.Infrastructure.Data;

namespace ReceivingService.Infrastructure.Repositories;

public sealed class ReceivingRepository : IReceivingRepository
{
    private readonly ReceivingDbContext _context;

    public ReceivingRepository(ReceivingDbContext context)
        => _context = context;

    public async Task<Domain.Entities.Receiving?> GetByIdAsync(
        int id, CancellationToken ct = default)
        => await _context.Receivings
            .Include(r => r.Lines)
            .FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task<Domain.Entities.Receiving?> GetByNumberAsync(
        string receivingNumber, CancellationToken ct = default)
        => await _context.Receivings
            .Include(r => r.Lines)
            .FirstOrDefaultAsync(r => r.ReceivingNumber == receivingNumber, ct);

    public async Task<IEnumerable<Domain.Entities.Receiving>> GetAllAsync(
        int page, int pageSize, CancellationToken ct = default)
        => await _context.Receivings
            .Include(r => r.Lines)
            .OrderByDescending(r => r.CreatedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task<IEnumerable<Domain.Entities.Receiving>> GetByPoIdAsync(
        int poId, CancellationToken ct = default)
        => await _context.Receivings
            .Include(r => r.Lines)
            .Where(r => r.PoId == poId)
            .ToListAsync(ct);

    public async Task AddAsync(Domain.Entities.Receiving receiving, CancellationToken ct = default)
        => await _context.Receivings.AddAsync(receiving, ct);

    public void Update(Domain.Entities.Receiving receiving)
        => _context.Receivings.Update(receiving);

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _context.SaveChangesAsync(ct);
}
