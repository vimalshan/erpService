using MediatR;

namespace BookingService.Application.Commands.SubmitBooking
{
    public record SubmitBookingCommand(long BookingId, long UpdatedBy) : IRequest;
}

namespace BookingService.Application.Commands.ApproveBooking
{
    public record ApproveBookingCommand(long BookingId, long UpdatedBy) : IRequest;
}

namespace BookingService.Application.Commands.RejectBooking
{
    public record RejectBookingCommand(long BookingId, long UpdatedBy) : IRequest;
}

namespace BookingService.Application.Commands.CancelBooking
{
    public record CancelBookingCommand(long BookingId, long UpdatedBy) : IRequest;
}
