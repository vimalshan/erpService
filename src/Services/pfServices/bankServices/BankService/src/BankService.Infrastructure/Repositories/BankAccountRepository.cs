using BankService.Domain.Entities;
using BankService.Domain.Interfaces;
using BankService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BankService.Infrastructure.Repositories;

public class BankAccountRepository(BankDbContext context) : IBankAccountRepository
{
    public async Task<BankAccount?> GetByIdAsync(long accountId, CancellationToken ct = default)
        => await context.BankAccounts.FirstOrDefaultAsync(a => a.AccountId == accountId, ct);

    public async Task<BankAccount?> GetByAccountNumberAsync(string accountNumber, CancellationToken ct = default)
        => await context.BankAccounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber, ct);

    public async Task<IReadOnlyList<BankAccount>> GetAllAsync(CancellationToken ct = default)
        => await context.BankAccounts.ToListAsync(ct);

    public async Task<IReadOnlyList<BankAccount>> GetByTrustCodeAsync(string trustCode, CancellationToken ct = default)
        => await context.BankAccounts.Where(a => a.TrustCode == trustCode).ToListAsync(ct);

    public async Task AddAsync(BankAccount account, CancellationToken ct = default)
        => await context.BankAccounts.AddAsync(account, ct);

    public void Update(BankAccount account) => context.BankAccounts.Update(account);

    public void Delete(BankAccount account) => context.BankAccounts.Remove(account);
}
