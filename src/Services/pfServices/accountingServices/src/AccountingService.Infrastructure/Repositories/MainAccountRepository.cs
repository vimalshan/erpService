using AccountingService.Domain.Entities;
using AccountingService.Domain.Interfaces;
using AccountingService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AccountingService.Infrastructure.Repositories;

public class MainAccountRepository : IMainAccountRepository
{
    private readonly AccountingDbContext _context;

    public MainAccountRepository(AccountingDbContext context)
        => _context = context;

    public async Task<MainAccount?> GetByCodeAsync(string code, CancellationToken ct = default)
        => await _context.MainAccounts.FindAsync([code], ct);

    public async Task<IEnumerable<MainAccount>> GetAllAsync(CancellationToken ct = default)
        => await _context.MainAccounts.ToListAsync(ct);

    public async Task AddAsync(MainAccount entity, CancellationToken ct = default)
        => await _context.MainAccounts.AddAsync(entity, ct);

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _context.SaveChangesAsync(ct);
}
