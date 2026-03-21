using ExpenseService.Domain.Entities;

namespace ExpenseService.Domain.Interfaces;

public interface ICurrencyRepository
{
    Task<TravelCurrency?> GetByIdAsync(long requestNumber, int serialNumber, CancellationToken ct = default);
    Task<IReadOnlyList<TravelCurrency>> GetByRequestNumberAsync(long requestNumber, CancellationToken ct = default);
    Task<TravelCurrency> AddAsync(TravelCurrency currency, CancellationToken ct = default);
    Task UpdateAsync(TravelCurrency currency, CancellationToken ct = default);
}
