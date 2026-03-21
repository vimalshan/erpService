using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Persistence;

namespace BookingService.Infrastructure.Repositories;

public class UnitOfWork(BookingDbContext context, IBookRequestRepository bookRequests, IBookConfirmationRepository bookConfirmations) : IUnitOfWork
{
    public IBookRequestRepository BookRequests { get; } = bookRequests;
    public IBookConfirmationRepository BookConfirmations { get; } = bookConfirmations;

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await context.SaveChangesAsync(ct);
    }
}
