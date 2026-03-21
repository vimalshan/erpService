using ExpenseService.Domain.Entities;

namespace ExpenseService.Domain.Interfaces;

public interface ISettlementRepository
{
    Task<IReadOnlyList<ExpSettlement>> GetAllAsync(CancellationToken ct = default);
    Task<ExpSettlement> AddAsync(ExpSettlement settlement, CancellationToken ct = default);
    Task<IReadOnlyList<ExpSettlementReport>> GetReportsByRequestAsync(long requestNumber, CancellationToken ct = default);
}
