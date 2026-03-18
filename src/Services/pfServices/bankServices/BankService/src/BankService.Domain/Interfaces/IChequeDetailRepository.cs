using BankService.Domain.Entities;

namespace BankService.Domain.Interfaces;

public interface IChequeDetailRepository
{
    Task<ChequeDetail?> GetByIdAsync(long chequeId, CancellationToken ct = default);
    Task<IReadOnlyList<ChequeDetail>> GetByStatusAsync(string status, CancellationToken ct = default);
    Task<IReadOnlyList<ChequeDetail>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(ChequeDetail cheque, CancellationToken ct = default);
    void Update(ChequeDetail cheque);
    void Delete(ChequeDetail cheque);
}
