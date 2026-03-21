using Microsoft.EntityFrameworkCore;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Persistence;

namespace BookingService.Infrastructure.Repositories;

public class BookConfirmationRepository(BookingDbContext context) : IBookConfirmationRepository
{
    public async Task<BookRequestConfirmation?> GetByIdAsync(string id, CancellationToken ct = default)
    {
        return await context.BookRequestConfirmations
            .FirstOrDefaultAsync(c => c.BookConfId == id, ct);
    }

    public async Task<IReadOnlyList<BookRequestConfirmation>> GetByBookingIdAsync(string bookingId, CancellationToken ct = default)
    {
        return await context.BookRequestConfirmations
            .Where(c => c.BookId == bookingId)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task AddAsync(BookRequestConfirmation entity, CancellationToken ct = default)
    {
        await context.BookRequestConfirmations.AddAsync(entity, ct);
    }

    public void Update(BookRequestConfirmation entity)
    {
        context.BookRequestConfirmations.Update(entity);
    }
}
