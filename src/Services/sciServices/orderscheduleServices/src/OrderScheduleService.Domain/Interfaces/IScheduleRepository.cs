namespace OrderScheduleService.Domain.Interfaces;

using OrderScheduleService.Domain.Aggregates;

public interface IScheduleRepository
{
    Task<ScheduleAggregate?> GetByIdAsync(long id);
    Task<IEnumerable<ScheduleAggregate>> GetByItemAsync(decimal itemId);
    Task<IEnumerable<ScheduleAggregate>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate);
    Task AddAsync(ScheduleAggregate schedule);
    Task UpdateAsync(ScheduleAggregate schedule);
    Task DeleteAsync(long id);
    Task SaveChangesAsync();
}
