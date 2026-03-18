using BankService.Domain.Entities;

namespace BankService.Domain.Interfaces;

public interface IBankMasterRepository
{
    Task<BankMaster?> GetByCodeAsync(string trustCode, string bankCode, CancellationToken ct = default);
    Task<IReadOnlyList<BankMaster>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<BankMaster>> GetByTrustCodeAsync(string trustCode, CancellationToken ct = default);
    Task AddAsync(BankMaster bank, CancellationToken ct = default);
    void Update(BankMaster bank);
    void Delete(BankMaster bank);
}
