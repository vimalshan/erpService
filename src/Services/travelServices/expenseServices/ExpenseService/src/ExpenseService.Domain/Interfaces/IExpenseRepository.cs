using ExpenseService.Domain.Entities;

namespace ExpenseService.Domain.Interfaces;

public interface IExpenseRepository
{
    Task<TravelExpense?> GetByIdAsync(long requestNumber, long serialNumber, CancellationToken ct = default);
    Task<IReadOnlyList<TravelExpense>> GetByRequestNumberAsync(long requestNumber, CancellationToken ct = default);
    Task<TravelExpense> AddAsync(TravelExpense expense, CancellationToken ct = default);
    Task UpdateAsync(TravelExpense expense, CancellationToken ct = default);
    Task DeleteAsync(long requestNumber, long serialNumber, CancellationToken ct = default);
}
