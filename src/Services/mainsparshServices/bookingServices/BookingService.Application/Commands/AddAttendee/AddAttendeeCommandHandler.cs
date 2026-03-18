using AutoMapper;
using BookingService.Application.DTOs;
using BookingService.Domain.Interfaces;
using MediatR;

namespace BookingService.Application.Commands.AddAttendee;

public class AddAttendeeCommandHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<AddAttendeeCommand, AttendeeDto>
{
    public async Task<AttendeeDto> Handle(AddAttendeeCommand request, CancellationToken cancellationToken)
    {
        var booking = await uow.Bookings.GetByIdAsync(request.BookingId, cancellationToken)
            ?? throw new KeyNotFoundException($"Booking {request.BookingId} not found.");

        var attendee = booking.AddAttendee(request.AttendeeSysId, request.CreatedBy);
        await uow.SaveChangesAsync(cancellationToken);

        return mapper.Map<AttendeeDto>(attendee);
    }
}
