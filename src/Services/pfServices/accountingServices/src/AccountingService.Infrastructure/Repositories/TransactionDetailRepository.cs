using AccountingService.Domain.Entities;
using AccountingService.Domain.Interfaces;
using AccountingService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AccountingService.Infrastructure.Repositories;

public class TransactionDetailRepository : ITransactionDetailRepository
{
    private readonly AccountingDbContext _context;

    public TransactionDetailRepository(AccountingDbContext context)
        => _context = context;

    public async Task<TransactionDetail?> GetByIdAsync(string trustCode, int transactionId, CancellationToken ct = default)
        => await _context.TransactionDetails.FindAsync([trustCode, transactionId], ct);

    public async Task<IEnumerable<TransactionDetail>> GetByTrustCodeAsync(string trustCode, CancellationToken ct = default)
        => await _context.TransactionDetails.Where(t => t.TdTrustCode == trustCode).ToListAsync(ct);

    public async Task AddAsync(TransactionDetail entity, CancellationToken ct = default)
        => await _context.TransactionDetails.AddAsync(entity, ct);

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _context.SaveChangesAsync(ct);
}
