using BankService.Domain.Entities;
using BankService.Domain.Interfaces;
using BankService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BankService.Infrastructure.Repositories;

public class BankMasterRepository(BankDbContext context) : IBankMasterRepository
{
    public async Task<BankMaster?> GetByCodeAsync(string trustCode, string bankCode, CancellationToken ct = default)
        => await context.BankMasters.FirstOrDefaultAsync(
            b => b.BankTrustCode == trustCode && b.BankCode == bankCode, ct);

    public async Task<IReadOnlyList<BankMaster>> GetAllAsync(CancellationToken ct = default)
        => await context.BankMasters.ToListAsync(ct);

    public async Task<IReadOnlyList<BankMaster>> GetByTrustCodeAsync(string trustCode, CancellationToken ct = default)
        => await context.BankMasters.Where(b => b.BankTrustCode == trustCode).ToListAsync(ct);

    public async Task AddAsync(BankMaster bank, CancellationToken ct = default)
        => await context.BankMasters.AddAsync(bank, ct);

    public void Update(BankMaster bank) => context.BankMasters.Update(bank);

    public void Delete(BankMaster bank) => context.BankMasters.Remove(bank);
}
