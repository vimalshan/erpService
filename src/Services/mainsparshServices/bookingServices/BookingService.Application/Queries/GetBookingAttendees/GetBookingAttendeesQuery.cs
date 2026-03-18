using AutoMapper;
using BookingService.Application.DTOs;
using BookingService.Domain.Interfaces;
using MediatR;

namespace BookingService.Application.Queries.GetBookingAttendees;

public record GetBookingAttendeesQuery(long BookingId) : IRequest<IEnumerable<AttendeeDto>>;

public class GetBookingAttendeesQueryHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetBookingAttendeesQuery, IEnumerable<AttendeeDto>>
{
    public async Task<IEnumerable<AttendeeDto>> Handle(GetBookingAttendeesQuery request, CancellationToken cancellationToken)
    {
        var booking = await uow.Bookings.GetByIdAsync(request.BookingId, cancellationToken)
            ?? throw new KeyNotFoundException($"Booking {request.BookingId} not found.");

        return mapper.Map<IEnumerable<AttendeeDto>>(booking.Attendees);
    }
}
