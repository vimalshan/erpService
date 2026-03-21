using BookingService.Domain.Entities;

namespace BookingService.Domain.Interfaces;

public interface IBookRequestRepository
{
    Task<BookRequestMain?> GetByIdAsync(string id, CancellationToken ct = default);
    Task<IReadOnlyList<BookRequestMain>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<BookRequestMain>> GetByEmployeeAsync(string employeeSysId, CancellationToken ct = default);
    Task AddAsync(BookRequestMain entity, CancellationToken ct = default);
    void Update(BookRequestMain entity);
    void Delete(BookRequestMain entity);
}
