namespace AimsTransactionService.Application.Common.Interfaces;

public interface ILeaveCreditRepository
{
    Task<decimal> GetBalanceAsync(long employeeSysId, int leaveId, CancellationToken cancellationToken = default);
    Task<long> GetNextIdAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Domain.Entities.LeaveCredit credit, CancellationToken cancellationToken = default);
}
