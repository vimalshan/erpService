using BookingService.Application.DTOs;
using MediatR;

namespace BookingService.Application.Commands.AddAttendee;

public record AddAttendeeCommand(long BookingId, long AttendeeSysId, long CreatedBy) : IRequest<AttendeeDto>;
