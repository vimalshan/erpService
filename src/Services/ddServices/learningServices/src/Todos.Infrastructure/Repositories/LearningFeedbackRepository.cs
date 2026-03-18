using Microsoft.EntityFrameworkCore;
using Todos.Domain;
using Todos.Domain.Entities;

namespace Todos.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for LearningFeedback entities
/// </summary>
public class LearningFeedbackRepository : Persistence.EFRepository<LearningFeedback>
{
    public LearningFeedbackRepository(Persistence.TodosDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Gets feedback by request number
    /// </summary>
    public async Task<IEnumerable<LearningFeedback>> GetByRequestNumberAsync(decimal requestNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(f => f.RequestNumber.Value == requestNumber)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets paginated feedback records
    /// </summary>
    public async Task<(IEnumerable<LearningFeedback> Items, int Total)> GetPaginatedAsync(
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        var total = await _dbSet.CountAsync(cancellationToken);
        var items = await _dbSet
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, total);
    }
}
