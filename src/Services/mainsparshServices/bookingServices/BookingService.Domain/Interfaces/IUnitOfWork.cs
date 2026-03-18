namespace BookingService.Domain.Interfaces;

public interface IUnitOfWork
{
    IBookingRepository Bookings { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
