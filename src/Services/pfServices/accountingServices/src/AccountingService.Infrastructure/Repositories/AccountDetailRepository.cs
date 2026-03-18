using AccountingService.Domain.Entities;
using AccountingService.Domain.Interfaces;
using AccountingService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AccountingService.Infrastructure.Repositories;

public class AccountDetailRepository : IAccountDetailRepository
{
    private readonly AccountingDbContext _context;

    public AccountDetailRepository(AccountingDbContext context)
        => _context = context;

    public async Task<AccountDetail?> GetByIdAsync(long sysId, CancellationToken ct = default)
        => await _context.AccountDetails.FindAsync([sysId], ct);

    public async Task<IEnumerable<AccountDetail>> GetByTrustCodeAsync(string trustCode, CancellationToken ct = default)
        => await _context.AccountDetails.Where(a => a.AcTrustCode == trustCode).ToListAsync(ct);

    public async Task<IEnumerable<AccountDetail>> GetByDateRangeAsync(string trustCode, DateTime from, DateTime to, CancellationToken ct = default)
        => await _context.AccountDetails
            .Where(a => a.AcTrustCode == trustCode && a.AcDocDat >= from && a.AcDocDat <= to)
            .ToListAsync(ct);

    public async Task AddAsync(AccountDetail entity, CancellationToken ct = default)
        => await _context.AccountDetails.AddAsync(entity, ct);

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _context.SaveChangesAsync(ct);
}
