using Microsoft.EntityFrameworkCore;
using PayTransactionalService.Domain.Entities;
using PayTransactionalService.Domain.Repositories;
using PayTransactionalService.Infrastructure.Persistence;

namespace PayTransactionalService.Infrastructure.Repositories;

public class PayTransactionRepository : IPayTransactionRepository
{
    private readonly PayTransactionalDbContext _context;
    public PayTransactionRepository(PayTransactionalDbContext context) => _context = context;

    public async Task<PayTransaction?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _context.PayTransactions.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);

    public async Task<IEnumerable<PayTransaction>> GetByEmployeeAsync(long employeeSystemId, CancellationToken ct = default)
        => await _context.PayTransactions
            .Where(x => x.EmployeeSystemId == employeeSystemId && !x.IsDeleted)
            .OrderByDescending(x => x.MonthYear)
            .ToListAsync(ct);

    public async Task<IEnumerable<PayTransaction>> GetByEmployeeAndMonthAsync(long employeeSystemId, string monthYear, CancellationToken ct = default)
        => await _context.PayTransactions
            .Where(x => x.EmployeeSystemId == employeeSystemId && x.MonthYear == monthYear && !x.IsDeleted)
            .ToListAsync(ct);

    public async Task<IEnumerable<PayTransaction>> GetByBatchIdAsync(long batchId, CancellationToken ct = default)
        => await _context.PayTransactions
            .Where(x => x.BatchId == batchId && !x.IsDeleted)
            .ToListAsync(ct);

    public async Task<IEnumerable<PayTransaction>> GetByMonthYearAsync(string monthYear, CancellationToken ct = default)
        => await _context.PayTransactions
            .Where(x => x.MonthYear == monthYear && !x.IsDeleted)
            .OrderBy(x => x.EmployeeSystemId)
            .ToListAsync(ct);

    public async Task AddAsync(PayTransaction entity, CancellationToken ct = default)
    {
        await _context.PayTransactions.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(PayTransaction entity, CancellationToken ct = default)
    {
        _context.PayTransactions.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(long id, CancellationToken ct = default)
    {
        var entity = await GetByIdAsync(id, ct);
        if (entity != null) { entity.IsDeleted = true; await _context.SaveChangesAsync(ct); }
    }
}

public class PayArrearRepository : IPayArrearRepository
{
    private readonly PayTransactionalDbContext _context;
    public PayArrearRepository(PayTransactionalDbContext context) => _context = context;

    public async Task<PayArrear?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _context.PayArrears.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);

    public async Task<IEnumerable<PayArrear>> GetByEmployeeAsync(long employeeSystemId, CancellationToken ct = default)
        => await _context.PayArrears
            .Where(x => x.EmployeeSystemId == employeeSystemId && !x.IsDeleted)
            .OrderByDescending(x => x.PayDate)
            .ToListAsync(ct);

    public async Task<IEnumerable<PayArrear>> GetByEmployeeAndMonthAsync(long employeeSystemId, string monthYear, CancellationToken ct = default)
        => await _context.PayArrears
            .Where(x => x.EmployeeSystemId == employeeSystemId && x.MonthYear == monthYear && !x.IsDeleted)
            .ToListAsync(ct);

    public async Task<IEnumerable<PayArrear>> GetUnprocessedByEmployeeAsync(long employeeSystemId, CancellationToken ct = default)
        => await _context.PayArrears
            .Where(x => x.EmployeeSystemId == employeeSystemId && !x.IsProcessed && !x.IsDeleted)
            .ToListAsync(ct);

    public async Task<IEnumerable<PayArrear>> GetByTypeAsync(string type, string? monthYear = null, CancellationToken ct = default)
    {
        var query = _context.PayArrears.Where(x => x.Type == type && !x.IsDeleted);
        if (monthYear != null) query = query.Where(x => x.MonthYear == monthYear);
        return await query.OrderByDescending(x => x.PayDate).ToListAsync(ct);
    }

    public async Task AddAsync(PayArrear entity, CancellationToken ct = default)
    {
        await _context.PayArrears.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(PayArrear entity, CancellationToken ct = default)
    {
        _context.PayArrears.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(long id, CancellationToken ct = default)
    {
        var entity = await GetByIdAsync(id, ct);
        if (entity != null) { entity.IsDeleted = true; await _context.SaveChangesAsync(ct); }
    }
}

public class PayAdjustmentRepository : IPayAdjustmentRepository
{
    private readonly PayTransactionalDbContext _context;
    public PayAdjustmentRepository(PayTransactionalDbContext context) => _context = context;

    public async Task<PayAdjustment?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _context.PayAdjustments.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);

    public async Task<IEnumerable<PayAdjustment>> GetByEmployeeAsync(long employeeSystemId, CancellationToken ct = default)
        => await _context.PayAdjustments
            .Where(x => x.EmployeeSystemId == employeeSystemId && !x.IsDeleted)
            .OrderByDescending(x => x.EffectiveDate)
            .ToListAsync(ct);

    public async Task<IEnumerable<PayAdjustment>> GetPendingAsync(CancellationToken ct = default)
        => await _context.PayAdjustments
            .Where(x => x.Status == "P" && !x.IsDeleted)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(ct);

    public async Task<IEnumerable<PayAdjustment>> GetByMonthYearAsync(string monthYear, CancellationToken ct = default)
        => await _context.PayAdjustments
            .Where(x => x.MonthYear == monthYear && !x.IsDeleted)
            .ToListAsync(ct);

    public async Task AddAsync(PayAdjustment entity, CancellationToken ct = default)
    {
        await _context.PayAdjustments.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(PayAdjustment entity, CancellationToken ct = default)
    {
        _context.PayAdjustments.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(long id, CancellationToken ct = default)
    {
        var entity = await GetByIdAsync(id, ct);
        if (entity != null) { entity.IsDeleted = true; await _context.SaveChangesAsync(ct); }
    }
}

public class PayrollBatchRepository : IPayrollBatchRepository
{
    private readonly PayTransactionalDbContext _context;
    public PayrollBatchRepository(PayTransactionalDbContext context) => _context = context;

    public async Task<PayrollBatch?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _context.PayrollBatches.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);

    public async Task<PayrollBatch?> GetByMonthYearAsync(string monthYear, CancellationToken ct = default)
        => await _context.PayrollBatches.FirstOrDefaultAsync(x => x.MonthYear == monthYear && !x.IsDeleted, ct);

    public async Task<IEnumerable<PayrollBatch>> GetAllAsync(CancellationToken ct = default)
        => await _context.PayrollBatches
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.MonthYear)
            .ToListAsync(ct);

    public async Task AddAsync(PayrollBatch entity, CancellationToken ct = default)
    {
        await _context.PayrollBatches.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(PayrollBatch entity, CancellationToken ct = default)
    {
        _context.PayrollBatches.Update(entity);
        await _context.SaveChangesAsync(ct);
    }
}
