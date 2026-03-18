using WorkOrderService.Domain.Entities;

namespace WorkOrderService.Domain.Interfaces;

public interface IWorkOrderRepository
{
    Task<WorkOrder?> GetByIdAsync(long workOrderId, CancellationToken cancellationToken = default);
    Task<WorkOrder?> GetByIdWithTasksAsync(long workOrderId, CancellationToken cancellationToken = default);
    Task<IEnumerable<WorkOrder>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<WorkOrder>> GetByStatusAsync(char statusCode, CancellationToken cancellationToken = default);
    Task<WorkOrder> AddAsync(WorkOrder workOrder, CancellationToken cancellationToken = default);
    Task UpdateAsync(WorkOrder workOrder, CancellationToken cancellationToken = default);
    Task DeleteAsync(long workOrderId, CancellationToken cancellationToken = default);
}
