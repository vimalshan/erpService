using BookingService.Domain.Entities;
using BookingService.Infrastructure.Data;
using HotChocolate;
using HotChocolate.Data;
using Microsoft.EntityFrameworkCore;

namespace BookingService.API.GraphQL.Queries;

public class BookingQuery
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<BookingRequest> GetBookings([Service] BookingDbContext context)
        => context.BookingRequests.AsNoTracking();

    public async Task<BookingRequest?> GetBookingByNumber(
        [Service] BookingDbContext context,
        long bookingNumber,
        CancellationToken ct)
        => await context.BookingRequests
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.BkBokNum == (decimal)bookingNumber, ct);

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<BookingConfirmation> GetConfirmations([Service] BookingDbContext context)
        => context.BookingConfirmations.AsNoTracking();
}
