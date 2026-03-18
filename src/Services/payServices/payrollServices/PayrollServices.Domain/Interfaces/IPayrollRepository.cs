using PayrollServices.Domain.Entities;

namespace PayrollServices.Domain.Interfaces;

/// <summary>
/// Repository interface for payroll operations
/// </summary>
public interface IPayrollRepository
{
    // Batch operations
    Task<PayrollBatch?> GetBatchByIdAsync(long batchId);
    Task<PayrollBatch?> GetBatchByMonthAsync(string month);
    Task<IEnumerable<PayrollBatch>> GetAllBatchesAsync();
    Task<PayrollBatch> CreateBatchAsync(PayrollBatch batch);
    Task UpdateBatchAsync(PayrollBatch batch);
    Task DeleteBatchAsync(long batchId);

    // Transaction operations
    Task<PayrollTransaction?> GetTransactionByIdAsync(long transactionId);
    Task<IEnumerable<PayrollTransaction>> GetTransactionsByBatchAsync(long batchId);
    Task<IEnumerable<PayrollTransaction>> GetTransactionsByEmployeeAsync(long employeeId, string month);
    Task<PayrollTransaction> CreateTransactionAsync(PayrollTransaction transaction);
    Task UpdateTransactionAsync(PayrollTransaction transaction);
    Task DeleteTransactionAsync(long transactionId);

    // Adjustment operations
    Task<PayrollAdjustment?> GetAdjustmentByIdAsync(long adjustmentId);
    Task<IEnumerable<PayrollAdjustment>> GetAdjustmentsByEmployeeAsync(long employeeId);
    Task<IEnumerable<PayrollAdjustment>> GetPendingAdjustmentsAsync();
    Task<PayrollAdjustment> CreateAdjustmentAsync(PayrollAdjustment adjustment);
    Task UpdateAdjustmentAsync(PayrollAdjustment adjustment);
    Task DeleteAdjustmentAsync(long adjustmentId);

    // Unit of work
    Task<int> SaveChangesAsync();
}
