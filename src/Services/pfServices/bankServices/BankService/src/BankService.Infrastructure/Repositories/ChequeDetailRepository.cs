using BankService.Domain.Entities;
using BankService.Domain.Interfaces;
using BankService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BankService.Infrastructure.Repositories;

public class ChequeDetailRepository(BankDbContext context) : IChequeDetailRepository
{
    public async Task<ChequeDetail?> GetByIdAsync(long chequeId, CancellationToken ct = default)
        => await context.ChequeDetails.FirstOrDefaultAsync(c => c.ChequeId == chequeId, ct);

    public async Task<IReadOnlyList<ChequeDetail>> GetByStatusAsync(string status, CancellationToken ct = default)
        => await context.ChequeDetails.Where(c => c.ChequeStatus == status).ToListAsync(ct);

    public async Task<IReadOnlyList<ChequeDetail>> GetAllAsync(CancellationToken ct = default)
        => await context.ChequeDetails.ToListAsync(ct);

    public async Task AddAsync(ChequeDetail cheque, CancellationToken ct = default)
        => await context.ChequeDetails.AddAsync(cheque, ct);

    public void Update(ChequeDetail cheque) => context.ChequeDetails.Update(cheque);

    public void Delete(ChequeDetail cheque) => context.ChequeDetails.Remove(cheque);
}
