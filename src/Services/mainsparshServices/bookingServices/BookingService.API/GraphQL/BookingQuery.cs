using BookingService.Application.DTOs;
using BookingService.Application.Queries.GetAllBookings;
using BookingService.Application.Queries.GetBookingById;
using MediatR;

namespace BookingService.API.GraphQL;

[QueryType]
public class BookingQuery
{
    public async Task<BookingDetailDto?> GetBookingByIdAsync(
        long id, ISender sender, CancellationToken cancellationToken)
    {
        try
        {
            return await sender.Send(new GetBookingByIdQuery(id), cancellationToken);
        }
        catch (System.Collections.Generic.KeyNotFoundException)
        {
            return null;
        }
    }

    public async Task<IEnumerable<BookingDto>> GetBookingsAsync(
        ISender sender,
        int page = 1,
        int pageSize = 20,
        string? statusFilter = null,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetAllBookingsQuery(page, pageSize, statusFilter), cancellationToken);
        return result.Items;
    }
}
