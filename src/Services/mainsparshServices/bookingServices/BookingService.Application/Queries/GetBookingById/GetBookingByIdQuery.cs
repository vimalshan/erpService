using AutoMapper;
using BookingService.Application.DTOs;
using BookingService.Domain.Interfaces;
using MediatR;

namespace BookingService.Application.Queries.GetBookingById;

public record GetBookingByIdQuery(long BookingId) : IRequest<BookingDetailDto>;

public class GetBookingByIdQueryHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetBookingByIdQuery, BookingDetailDto>
{
    public async Task<BookingDetailDto> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        var booking = await uow.Bookings.GetByIdAsync(request.BookingId, cancellationToken)
            ?? throw new KeyNotFoundException($"Booking {request.BookingId} not found.");

        return mapper.Map<BookingDetailDto>(booking);
    }
}
