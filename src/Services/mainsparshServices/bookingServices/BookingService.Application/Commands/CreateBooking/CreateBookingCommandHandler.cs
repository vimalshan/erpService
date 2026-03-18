using AutoMapper;
using BookingService.Application.DTOs;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using MediatR;

namespace BookingService.Application.Commands.CreateBooking;

public class CreateBookingCommandHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<CreateBookingCommand, BookingDto>
{
    public async Task<BookingDto> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = BookMain.Create(
            request.BookingAppNo,
            request.BookingTitle,
            request.LocationCode,
            request.BookingDate,
            request.CreatedBy);

        await uow.Bookings.AddAsync(booking, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        return mapper.Map<BookingDto>(booking);
    }
}
