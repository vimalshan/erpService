using MediatR;
using BookingService.Application.DTOs;

namespace BookingService.Application.Queries;

public record GetBookingByIdQuery(string BookMainId) : IRequest<BookRequestMainDto?>;
public record GetAllBookingsQuery : IRequest<IReadOnlyList<BookRequestMainDto>>;
public record GetBookingsByEmployeeQuery(string EmployeeSysId) : IRequest<IReadOnlyList<BookRequestMainDto>>;
public record GetBookingConfirmationsQuery(string BookingId) : IRequest<IReadOnlyList<BookConfirmationDto>>;
