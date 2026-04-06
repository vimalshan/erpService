using PayTransactionalService.Domain.Entities;

namespace PayTransactionalService.Domain.Repositories;

public interface IPayTransactionRepository
{
    Task<PayTransaction?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<PayTransaction>> GetByEmployeeAsync(long employeeSystemId, CancellationToken ct = default);
    Task<IEnumerable<PayTransaction>> GetByEmployeeAndMonthAsync(long employeeSystemId, string monthYear, CancellationToken ct = default);
    Task<IEnumerable<PayTransaction>> GetByBatchIdAsync(long batchId, CancellationToken ct = default);
    Task<IEnumerable<PayTransaction>> GetByMonthYearAsync(string monthYear, CancellationToken ct = default);
    Task AddAsync(PayTransaction entity, CancellationToken ct = default);
    Task UpdateAsync(PayTransaction entity, CancellationToken ct = default);
    Task DeleteAsync(long id, CancellationToken ct = default);
}

public interface IPayArrearRepository
{
    Task<PayArrear?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<PayArrear>> GetByEmployeeAsync(long employeeSystemId, CancellationToken ct = default);
    Task<IEnumerable<PayArrear>> GetByEmployeeAndMonthAsync(long employeeSystemId, string monthYear, CancellationToken ct = default);
    Task<IEnumerable<PayArrear>> GetUnprocessedByEmployeeAsync(long employeeSystemId, CancellationToken ct = default);
    Task<IEnumerable<PayArrear>> GetByTypeAsync(string type, string? monthYear = null, CancellationToken ct = default);
    Task AddAsync(PayArrear entity, CancellationToken ct = default);
    Task UpdateAsync(PayArrear entity, CancellationToken ct = default);
    Task DeleteAsync(long id, CancellationToken ct = default);
}

public interface IPayAdjustmentRepository
{
    Task<PayAdjustment?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<PayAdjustment>> GetByEmployeeAsync(long employeeSystemId, CancellationToken ct = default);
    Task<IEnumerable<PayAdjustment>> GetPendingAsync(CancellationToken ct = default);
    Task<IEnumerable<PayAdjustment>> GetByMonthYearAsync(string monthYear, CancellationToken ct = default);
    Task AddAsync(PayAdjustment entity, CancellationToken ct = default);
    Task UpdateAsync(PayAdjustment entity, CancellationToken ct = default);
    Task DeleteAsync(long id, CancellationToken ct = default);
}

public interface IPayrollBatchRepository
{
    Task<PayrollBatch?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<PayrollBatch?> GetByMonthYearAsync(string monthYear, CancellationToken ct = default);
    Task<IEnumerable<PayrollBatch>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(PayrollBatch entity, CancellationToken ct = default);
    Task UpdateAsync(PayrollBatch entity, CancellationToken ct = default);
}
