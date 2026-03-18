using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Persistence;
using BookingService.Infrastructure.Repositories;

namespace BookingService.Infrastructure.Repositories;

public class UnitOfWork(BookingDbContext context) : IUnitOfWork
{
    private IBookingRepository? _bookings;

    public IBookingRepository Bookings => _bookings ??= new BookingRepository(context);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => context.SaveChangesAsync(cancellationToken);
}
