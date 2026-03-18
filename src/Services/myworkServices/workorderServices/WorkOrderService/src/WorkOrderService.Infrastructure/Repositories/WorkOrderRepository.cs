using Microsoft.EntityFrameworkCore;
using WorkOrderService.Domain.Entities;
using WorkOrderService.Domain.Interfaces;
using WorkOrderService.Infrastructure.Persistence;

namespace WorkOrderService.Infrastructure.Repositories;

public class WorkOrderRepository : IWorkOrderRepository
{
    private readonly WorkOrderDbContext _context;

    public WorkOrderRepository(WorkOrderDbContext context)
    {
        _context = context;
    }

    public async Task<WorkOrder?> GetByIdAsync(long workOrderId, CancellationToken cancellationToken = default)
    {
        return await _context.WorkOrders.FindAsync([workOrderId], cancellationToken);
    }

    public async Task<WorkOrder?> GetByIdWithTasksAsync(long workOrderId, CancellationToken cancellationToken = default)
    {
        return await _context.WorkOrders
            .Include(w => w.Tasks)
            .FirstOrDefaultAsync(w => w.WorkOrderId == workOrderId, cancellationToken);
    }

    public async Task<IEnumerable<WorkOrder>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.WorkOrders
            .Include(w => w.Tasks)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<WorkOrder>> GetByStatusAsync(char statusCode, CancellationToken cancellationToken = default)
    {
        var status = Domain.ValueObjects.WorkOrderStatus.FromCode(statusCode);
        return await _context.WorkOrders
            .Include(w => w.Tasks)
            .Where(w => w.WorkOrderStatus == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<WorkOrder> AddAsync(WorkOrder workOrder, CancellationToken cancellationToken = default)
    {
        await _context.WorkOrders.AddAsync(workOrder, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return workOrder;
    }

    public async Task UpdateAsync(WorkOrder workOrder, CancellationToken cancellationToken = default)
    {
        _context.WorkOrders.Update(workOrder);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(long workOrderId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.WorkOrders.FindAsync([workOrderId], cancellationToken);
        if (entity is not null)
        {
            _context.WorkOrders.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
