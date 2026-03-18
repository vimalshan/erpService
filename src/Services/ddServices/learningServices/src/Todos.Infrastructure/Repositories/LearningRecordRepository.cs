using Microsoft.EntityFrameworkCore;
using Todos.Domain;
using Todos.Domain.Entities;

namespace Todos.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for LearningRecord entities
/// </summary>
public class LearningRecordRepository : Persistence.EFRepository<LearningRecord>
{
    public LearningRecordRepository(Persistence.TodosDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Gets learning records by request number
    /// </summary>
    public async Task<IEnumerable<LearningRecord>> GetByRequestNumberAsync(decimal requestNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(r => r.RequestNumber.Value == requestNumber)
            .Include(r => r.SubRecords)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets learning records by employee ID
    /// </summary>
    public async Task<IEnumerable<LearningRecord>> GetByEmployeeIdAsync(string employeeId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(r => r.EmployeeId != null && r.EmployeeId.Value == employeeId)
            .Include(r => r.SubRecords)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets paginated learning records
    /// </summary>
    public async Task<(IEnumerable<LearningRecord> Items, int Total)> GetPaginatedAsync(
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        var total = await _dbSet.CountAsync(cancellationToken);
        var items = await _dbSet
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(r => r.SubRecords)
            .ToListAsync(cancellationToken);

        return (items, total);
    }
}
