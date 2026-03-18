using BookingService.Domain.Entities;
using BookingService.Domain.ValueObjects;

namespace BookingService.Domain.Interfaces;

public interface IBookingRepository
{
    Task<BookMain?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<BookMain?> GetByAppNoAsync(string appNo, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookMain>> GetAllAsync(int page, int pageSize, string? statusFilter, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookMain>> GetBookingsByStatusAsync(BookingStatus status, CancellationToken cancellationToken = default);
    Task<int> CountAsync(string? statusFilter, CancellationToken cancellationToken = default);
    Task AddAsync(BookMain booking, CancellationToken cancellationToken = default);
    void Update(BookMain booking);
    Task UpdateAsync(BookMain booking, CancellationToken cancellationToken = default);
    void Remove(BookMain booking);
}
