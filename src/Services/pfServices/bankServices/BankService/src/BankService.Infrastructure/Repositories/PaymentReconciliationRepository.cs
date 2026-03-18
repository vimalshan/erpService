using BankService.Domain.Entities;
using BankService.Domain.Interfaces;
using BankService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BankService.Infrastructure.Repositories;

public class PaymentReconciliationRepository(BankDbContext context) : IPaymentReconciliationRepository
{
    public async Task<PaymentReconciliation?> GetByIdAsync(long reconId, CancellationToken ct = default)
        => await context.PaymentReconciliations.FirstOrDefaultAsync(r => r.ReconId == reconId, ct);

    public async Task<IReadOnlyList<PaymentReconciliation>> GetByChequeIdAsync(long chequeId, CancellationToken ct = default)
        => await context.PaymentReconciliations.Where(r => r.ChequeId == chequeId).ToListAsync(ct);

    public async Task AddAsync(PaymentReconciliation recon, CancellationToken ct = default)
        => await context.PaymentReconciliations.AddAsync(recon, ct);

    public void Update(PaymentReconciliation recon) => context.PaymentReconciliations.Update(recon);
}
