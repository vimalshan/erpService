using FluentValidation;
using BookingService.Application.Commands;

namespace BookingService.Application.Validators;

public class CreateBookingValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingValidator()
    {
        RuleFor(x => x.EmployeeSysId).NotEmpty().WithMessage("Employee ID is required");
        RuleFor(x => x.Type).NotEmpty().Must(t => t is "TKT" or "STY" or "CAB")
            .WithMessage("Type must be TKT, STY, or CAB");
        RuleFor(x => x.Through).NotEmpty();
        RuleFor(x => x.TpStatus).NotEmpty();
    }
}

public class ConfirmBookingValidator : AbstractValidator<ConfirmBookingCommand>
{
    public ConfirmBookingValidator()
    {
        RuleFor(x => x.BookId).NotEmpty();
        RuleFor(x => x.Mode).NotEmpty();
        RuleFor(x => x.Cost).NotEmpty();
        RuleFor(x => x.StartDate).LessThan(x => x.EndDate);
    }
}
