using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Aggregates;
using TransactionService.Domain.Interfaces;
using TransactionService.Infrastructure.Persistence;

namespace TransactionService.Infrastructure.Repositories;

public sealed class SupplierJVRepository : ISupplierJVRepository
{
    private readonly TransactionDbContext _context;

    public SupplierJVRepository(TransactionDbContext context) => _context = context;

    public async Task<SupplierJournalVoucher?> GetByIdAsync(long jvId, CancellationToken cancellationToken = default)
        => await _context.SupplierJournalVouchers
            .Include(j => j.Lines)
            .FirstOrDefaultAsync(j => j.JvId == jvId, cancellationToken);

    public async Task<IEnumerable<SupplierJournalVoucher>> GetByVendorIdAsync(long vendorId, CancellationToken cancellationToken = default)
        => await _context.SupplierJournalVouchers
            .Include(j => j.Lines)
            .Where(j => j.JvVendorId == vendorId)
            .OrderByDescending(j => j.JvDate)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<SupplierJournalVoucher>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        => await _context.SupplierJournalVouchers
            .Include(j => j.Lines)
            .OrderByDescending(j => j.JvDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
        => await _context.SupplierJournalVouchers.CountAsync(cancellationToken);

    public async Task AddAsync(SupplierJournalVoucher jv, CancellationToken cancellationToken = default)
        => await _context.SupplierJournalVouchers.AddAsync(jv, cancellationToken);

    public void Update(SupplierJournalVoucher jv)
        => _context.SupplierJournalVouchers.Update(jv);

    public async Task<bool> ExistsAsync(long jvId, CancellationToken cancellationToken = default)
        => await _context.SupplierJournalVouchers.AnyAsync(j => j.JvId == jvId, cancellationToken);
}
