using BookingService.Domain.Aggregates;
using BookingService.Domain.Entities;

namespace BookingService.Domain.Interfaces;

public interface IBookingRepository
{
    Task<BookingAggregate?> GetByIdAsync(long bookingNumber, CancellationToken ct = default);
    Task AddAsync(BookingAggregate booking, CancellationToken ct = default);
    Task UpdateAsync(BookingAggregate booking, CancellationToken ct = default);
    Task<IEnumerable<BookingRequest>> GetByUserAsync(string userCode, CancellationToken ct = default);
    Task<long> GetNextBookingNumberAsync(CancellationToken ct = default);
}

public interface IBookingConfirmationRepository
{
    Task<BookingConfirmation?> GetByIdAsync(long confirmationNumber, CancellationToken ct = default);
    Task AddAsync(BookingConfirmation confirmation, CancellationToken ct = default);
    Task<long> GetNextConfirmationNumberAsync(CancellationToken ct = default);
}

public interface ICouponRepository
{
    Task<CouponMain?> GetByIdAsync(long couponId, CancellationToken ct = default);
    Task AddAsync(CouponMain coupon, CancellationToken ct = default);
}

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    void AddDomainEvents(IEnumerable<Common.IDomainEvent> events);
}
