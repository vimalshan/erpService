using Microsoft.EntityFrameworkCore;
using WorkOrderService.Domain.Entities;
using WorkOrderService.Domain.Interfaces;
using WorkOrderService.Infrastructure.Persistence;

namespace WorkOrderService.Infrastructure.Repositories;

public class WorkTaskRepository : IWorkTaskRepository
{
    private readonly WorkOrderDbContext _context;

    public WorkTaskRepository(WorkOrderDbContext context)
    {
        _context = context;
    }

    public async Task<WorkTask?> GetByIdAsync(long taskId, CancellationToken cancellationToken = default)
    {
        return await _context.WorkTasks.FindAsync([taskId], cancellationToken);
    }

    public async Task<IEnumerable<WorkTask>> GetByWorkOrderIdAsync(long workOrderId, CancellationToken cancellationToken = default)
    {
        return await _context.WorkTasks
            .Where(t => t.WorkOrderId == workOrderId)
            .ToListAsync(cancellationToken);
    }

    public async Task<WorkTask> AddAsync(WorkTask workTask, CancellationToken cancellationToken = default)
    {
        await _context.WorkTasks.AddAsync(workTask, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return workTask;
    }

    public async Task UpdateAsync(WorkTask workTask, CancellationToken cancellationToken = default)
    {
        _context.WorkTasks.Update(workTask);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(long taskId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.WorkTasks.FindAsync([taskId], cancellationToken);
        if (entity is not null)
        {
            _context.WorkTasks.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
