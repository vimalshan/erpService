using BankService.Domain.Entities;
using BankService.Domain.Interfaces;
using BankService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BankService.Infrastructure.Repositories;

public class ChequeRegisterRepository(BankDbContext context) : IChequeRegisterRepository
{
    public async Task<ChequeRegister?> GetByIdAsync(long registerId, CancellationToken ct = default)
        => await context.ChequeRegisters.FirstOrDefaultAsync(r => r.RegisterId == registerId, ct);

    public async Task<IReadOnlyList<ChequeRegister>> GetByAccountIdAsync(long accountId, CancellationToken ct = default)
        => await context.ChequeRegisters.Where(r => r.AccountId == accountId).ToListAsync(ct);

    public async Task AddAsync(ChequeRegister register, CancellationToken ct = default)
        => await context.ChequeRegisters.AddAsync(register, ct);

    public void Update(ChequeRegister register) => context.ChequeRegisters.Update(register);
}
