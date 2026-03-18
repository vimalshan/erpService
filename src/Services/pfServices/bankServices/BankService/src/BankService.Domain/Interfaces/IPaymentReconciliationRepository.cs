using BankService.Domain.Entities;

namespace BankService.Domain.Interfaces;

public interface IPaymentReconciliationRepository
{
    Task<PaymentReconciliation?> GetByIdAsync(long reconId, CancellationToken ct = default);
    Task<IReadOnlyList<PaymentReconciliation>> GetByChequeIdAsync(long chequeId, CancellationToken ct = default);
    Task AddAsync(PaymentReconciliation recon, CancellationToken ct = default);
    void Update(PaymentReconciliation recon);
}
