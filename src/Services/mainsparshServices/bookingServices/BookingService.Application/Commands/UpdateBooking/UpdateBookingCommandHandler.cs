using AutoMapper;
using BookingService.Application.DTOs;
using BookingService.Domain.Interfaces;
using MediatR;

namespace BookingService.Application.Commands.UpdateBooking;

public class UpdateBookingCommandHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<UpdateBookingCommand, BookingDto>
{
    public async Task<BookingDto> Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await uow.Bookings.GetByIdAsync(request.BookingId, cancellationToken)
            ?? throw new KeyNotFoundException($"Booking {request.BookingId} not found.");

        booking.Update(request.BookingTitle, request.LocationCode, request.BookingDate, request.UpdatedBy);
        uow.Bookings.Update(booking);
        await uow.SaveChangesAsync(cancellationToken);

        return mapper.Map<BookingDto>(booking);
    }
}
