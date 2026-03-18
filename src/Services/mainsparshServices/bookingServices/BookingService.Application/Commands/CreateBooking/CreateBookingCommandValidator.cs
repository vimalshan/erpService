using FluentValidation;

namespace BookingService.Application.Commands.CreateBooking;

public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(x => x.BookingAppNo).NotEmpty().MaximumLength(50);
        RuleFor(x => x.BookingTitle).NotEmpty().MaximumLength(255);
        RuleFor(x => x.LocationCode).MaximumLength(50).When(x => x.LocationCode is not null);
        RuleFor(x => x.CreatedBy).GreaterThan(0).WithMessage("CreatedBy must be a valid user ID.");
    }
}
