using BookingService.Domain.Entities;

namespace BookingService.Domain.Interfaces;

public interface IBookConfirmationRepository
{
    Task<BookRequestConfirmation?> GetByIdAsync(string id, CancellationToken ct = default);
    Task<IReadOnlyList<BookRequestConfirmation>> GetByBookingIdAsync(string bookingId, CancellationToken ct = default);
    Task AddAsync(BookRequestConfirmation entity, CancellationToken ct = default);
    void Update(BookRequestConfirmation entity);
}
