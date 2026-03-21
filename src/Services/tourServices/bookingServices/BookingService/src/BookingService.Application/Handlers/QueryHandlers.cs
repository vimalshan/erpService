using AutoMapper;
using MediatR;
using BookingService.Application.DTOs;
using BookingService.Application.Queries;
using BookingService.Domain.Interfaces;

namespace BookingService.Application.Handlers;

public class GetBookingByIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetBookingByIdQuery, BookRequestMainDto?>
{
    public async Task<BookRequestMainDto?> Handle(GetBookingByIdQuery request, CancellationToken ct)
    {
        var entity = await unitOfWork.BookRequests.GetByIdAsync(request.BookMainId, ct);
        return entity is null ? null : mapper.Map<BookRequestMainDto>(entity);
    }
}

public class GetAllBookingsHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllBookingsQuery, IReadOnlyList<BookRequestMainDto>>
{
    public async Task<IReadOnlyList<BookRequestMainDto>> Handle(GetAllBookingsQuery request, CancellationToken ct)
    {
        var entities = await unitOfWork.BookRequests.GetAllAsync(ct);
        return mapper.Map<IReadOnlyList<BookRequestMainDto>>(entities);
    }
}

public class GetBookingsByEmployeeHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetBookingsByEmployeeQuery, IReadOnlyList<BookRequestMainDto>>
{
    public async Task<IReadOnlyList<BookRequestMainDto>> Handle(GetBookingsByEmployeeQuery request, CancellationToken ct)
    {
        var entities = await unitOfWork.BookRequests.GetByEmployeeAsync(request.EmployeeSysId, ct);
        return mapper.Map<IReadOnlyList<BookRequestMainDto>>(entities);
    }
}

public class GetBookingConfirmationsHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetBookingConfirmationsQuery, IReadOnlyList<BookConfirmationDto>>
{
    public async Task<IReadOnlyList<BookConfirmationDto>> Handle(GetBookingConfirmationsQuery request, CancellationToken ct)
    {
        var entities = await unitOfWork.BookConfirmations.GetByBookingIdAsync(request.BookingId, ct);
        return mapper.Map<IReadOnlyList<BookConfirmationDto>>(entities);
    }
}
