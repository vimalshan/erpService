using WorkOrderService.Domain.Entities;

namespace WorkOrderService.Domain.Interfaces;

public interface IWorkTaskRepository
{
    Task<WorkTask?> GetByIdAsync(long taskId, CancellationToken cancellationToken = default);
    Task<IEnumerable<WorkTask>> GetByWorkOrderIdAsync(long workOrderId, CancellationToken cancellationToken = default);
    Task<WorkTask> AddAsync(WorkTask workTask, CancellationToken cancellationToken = default);
    Task UpdateAsync(WorkTask workTask, CancellationToken cancellationToken = default);
    Task DeleteAsync(long taskId, CancellationToken cancellationToken = default);
}
