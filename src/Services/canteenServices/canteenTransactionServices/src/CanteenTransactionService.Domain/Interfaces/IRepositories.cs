using CanteenTransactionService.Domain.Entities;

namespace CanteenTransactionService.Domain.Interfaces;

public interface ICanteenDaconRepository
{
    Task<CanteenDacon?> GetBySerialNumberAsync(long serialNumber, CancellationToken ct = default);
    Task<IEnumerable<CanteenDacon>> GetByEmployeeAsync(long employeeSysId, string fromDate, string toDate, CancellationToken ct = default);
    Task<IEnumerable<CanteenDacon>> GetByCompanyAndDateAsync(long companyCode, string swipeDate, CancellationToken ct = default);
    Task<long> GetNextSerialNumberAsync(CancellationToken ct = default);
    Task AddAsync(CanteenDacon entity, CancellationToken ct = default);
    void Update(CanteenDacon entity);
    void Delete(CanteenDacon entity);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

public interface IDailyAvailedRepository
{
    Task<DailyAvailed?> GetBySerialNumberAsync(long serialNumber, CancellationToken ct = default);
    Task<IEnumerable<DailyAvailed>> GetByEmployeeAsync(long employeeSysId, string fromDate, string toDate, CancellationToken ct = default);
    Task<IEnumerable<DailyAvailed>> GetByCompanyAndDateAsync(long companyCode, string swipeDate, CancellationToken ct = default);
    Task<long> GetNextSerialNumberAsync(CancellationToken ct = default);
    Task AddAsync(DailyAvailed entity, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

public interface IMisBatchSubmissionRepository
{
    Task<MisBatchSubmission?> GetBySerialNumberAsync(long serialNumber, CancellationToken ct = default);
    Task<IEnumerable<MisBatchSubmission>> GetByBatchNumberAsync(long batchNumber, CancellationToken ct = default);
    Task<IEnumerable<MisBatchSubmission>> GetPendingAsync(CancellationToken ct = default);
    Task<IEnumerable<MisBatchSubmission>> GetByCompanyAndDateAsync(long companyCode, DateTime fromDate, DateTime toDate, CancellationToken ct = default);
    Task AddAsync(MisBatchSubmission entity, CancellationToken ct = default);
    void Update(MisBatchSubmission entity);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
