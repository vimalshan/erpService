namespace OrderScheduleService.Domain.Interfaces;

using OrderScheduleService.Domain.Aggregates;

public interface ITiedOrderRepository
{
    Task<TiedOrderAggregate?> GetByIdAsync(long id);
    Task<IEnumerable<TiedOrderAggregate>> GetByCustomerAsync(string customerCode);
    Task<IEnumerable<TiedOrderAggregate>> GetAllAsync();
    Task AddAsync(TiedOrderAggregate order);
    Task UpdateAsync(TiedOrderAggregate order);
    Task DeleteAsync(long id);
    Task SaveChangesAsync();
}
