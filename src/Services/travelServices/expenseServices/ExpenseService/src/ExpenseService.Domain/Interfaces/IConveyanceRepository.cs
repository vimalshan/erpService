using ExpenseService.Domain.Entities;

namespace ExpenseService.Domain.Interfaces;

public interface IConveyanceRepository
{
    Task<TravelConveyance?> GetByIdAsync(long serialNumber, long requestNumber, CancellationToken ct = default);
    Task<IReadOnlyList<TravelConveyance>> GetByRequestNumberAsync(long requestNumber, CancellationToken ct = default);
    Task<TravelConveyance> AddAsync(TravelConveyance conveyance, CancellationToken ct = default);
    Task UpdateAsync(TravelConveyance conveyance, CancellationToken ct = default);
    Task DeleteAsync(long serialNumber, long requestNumber, CancellationToken ct = default);
}
