using BankService.Domain.Entities;

namespace BankService.Domain.Interfaces;

public interface IChequeRegisterRepository
{
    Task<ChequeRegister?> GetByIdAsync(long registerId, CancellationToken ct = default);
    Task<IReadOnlyList<ChequeRegister>> GetByAccountIdAsync(long accountId, CancellationToken ct = default);
    Task AddAsync(ChequeRegister register, CancellationToken ct = default);
    void Update(ChequeRegister register);
}
