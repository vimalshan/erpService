using BookingService.Application.DTOs;
using BookingService.Application.Common;
using MediatR;

namespace BookingService.Application.Queries.GetAllBookings;

public record GetAllBookingsQuery(int Page = 1, int PageSize = 20, string? StatusFilter = null)
    : IRequest<PagedResponse<BookingDto>>;
