using AutoMapper;
using BookingService.Application.Common;
using BookingService.Application.DTOs;
using BookingService.Domain.Interfaces;
using MediatR;

namespace BookingService.Application.Queries.GetAllBookings;

public class GetAllBookingsQueryHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetAllBookingsQuery, PagedResponse<BookingDto>>
{
    public async Task<PagedResponse<BookingDto>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
    {
        var bookings = await uow.Bookings.GetAllAsync(request.Page, request.PageSize, request.StatusFilter, cancellationToken);
        var total = await uow.Bookings.CountAsync(request.StatusFilter, cancellationToken);
        var dtos = mapper.Map<IEnumerable<BookingDto>>(bookings);
        return PagedResponse<BookingDto>.Create(dtos, request.Page, request.PageSize, total);
    }
}
