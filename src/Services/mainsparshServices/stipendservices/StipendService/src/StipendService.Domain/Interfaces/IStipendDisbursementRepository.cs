using StipendService.Domain.Entities;

namespace StipendService.Domain.Interfaces;

public interface IStipendDisbursementRepository
{
    Task<StipendDisbursement?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<StipendDisbursement>> GetByMonthYearAsync(string monthYear, CancellationToken cancellationToken = default);
    Task<IEnumerable<StipendDisbursement>> GetBySrfIdAsync(long srfId, CancellationToken cancellationToken = default);
    Task AddAsync(StipendDisbursement disbursement, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<StipendDisbursement> disbursements, CancellationToken cancellationToken = default);
    Task UpdateAsync(StipendDisbursement disbursement, CancellationToken cancellationToken = default);
    Task<bool> ExistsForMonthAsync(long srfId, long stipendId, string monthYear, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
