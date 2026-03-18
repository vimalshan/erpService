using Microsoft.EntityFrameworkCore;
using PayrollServices.Domain.Entities;
using PayrollServices.Domain.Interfaces;
using PayrollServices.Infrastructure.Data;

namespace PayrollServices.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for payroll operations
/// </summary>
public class PayrollRepository : IPayrollRepository
{
    private readonly PayrollDbContext _dbContext;

    public PayrollRepository(PayrollDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #region Batch Operations

    public async Task<PayrollBatch?> GetBatchByIdAsync(long batchId)
    {
        return await _dbContext.PayrollBatches.FindAsync(batchId);
    }

    public async Task<PayrollBatch?> GetBatchByMonthAsync(string month)
    {
        return await _dbContext.PayrollBatches.FirstOrDefaultAsync(x => x.BatchMonth == month);
    }

    public async Task<IEnumerable<PayrollBatch>> GetAllBatchesAsync()
    {
        return await _dbContext.PayrollBatches.ToListAsync();
    }

    public async Task<PayrollBatch> CreateBatchAsync(PayrollBatch batch)
    {
        _dbContext.PayrollBatches.Add(batch);
        return batch;
    }

    public async Task UpdateBatchAsync(PayrollBatch batch)
    {
        _dbContext.PayrollBatches.Update(batch);
    }

    public async Task DeleteBatchAsync(long batchId)
    {
        var batch = await GetBatchByIdAsync(batchId);
        if (batch != null)
            _dbContext.PayrollBatches.Remove(batch);
    }

    #endregion

    #region Transaction Operations

    public async Task<PayrollTransaction?> GetTransactionByIdAsync(long transactionId)
    {
        return await _dbContext.PayrollTransactions.FindAsync(transactionId);
    }

    public async Task<IEnumerable<PayrollTransaction>> GetTransactionsByBatchAsync(long batchId)
    {
        return await _dbContext.PayrollTransactions
            .Where(x => x.BatchId == batchId)
            .ToListAsync();
    }

    public async Task<IEnumerable<PayrollTransaction>> GetTransactionsByEmployeeAsync(long employeeId, string month)
    {
        return await _dbContext.PayrollTransactions
            .Where(x => x.EmployeeSystemId == employeeId && x.Month == month)
            .ToListAsync();
    }

    public async Task<PayrollTransaction> CreateTransactionAsync(PayrollTransaction transaction)
    {
        _dbContext.PayrollTransactions.Add(transaction);
        return transaction;
    }

    public async Task UpdateTransactionAsync(PayrollTransaction transaction)
    {
        _dbContext.PayrollTransactions.Update(transaction);
    }

    public async Task DeleteTransactionAsync(long transactionId)
    {
        var transaction = await GetTransactionByIdAsync(transactionId);
        if (transaction != null)
            _dbContext.PayrollTransactions.Remove(transaction);
    }

    #endregion

    #region Adjustment Operations

    public async Task<PayrollAdjustment?> GetAdjustmentByIdAsync(long adjustmentId)
    {
        return await _dbContext.PayrollAdjustments.FindAsync(adjustmentId);
    }

    public async Task<IEnumerable<PayrollAdjustment>> GetAdjustmentsByEmployeeAsync(long employeeId)
    {
        return await _dbContext.PayrollAdjustments
            .Where(x => x.EmployeeSystemId == employeeId)
            .ToListAsync();
    }

    public async Task<IEnumerable<PayrollAdjustment>> GetPendingAdjustmentsAsync()
    {
        return await _dbContext.PayrollAdjustments
            .Where(x => x.ApprovedOn == null)
            .ToListAsync();
    }

    public async Task<PayrollAdjustment> CreateAdjustmentAsync(PayrollAdjustment adjustment)
    {
        _dbContext.PayrollAdjustments.Add(adjustment);
        return adjustment;
    }

    public async Task UpdateAdjustmentAsync(PayrollAdjustment adjustment)
    {
        _dbContext.PayrollAdjustments.Update(adjustment);
    }

    public async Task DeleteAdjustmentAsync(long adjustmentId)
    {
        var adjustment = await GetAdjustmentByIdAsync(adjustmentId);
        if (adjustment != null)
            _dbContext.PayrollAdjustments.Remove(adjustment);
    }

    #endregion

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}
