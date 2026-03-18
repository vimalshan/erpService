using FluentValidation;

namespace AttendanceService.Application.Commands.SwipePunch;

public class RecordSwipePunchCommandValidator : AbstractValidator<RecordSwipePunchCommand>
{
    public RecordSwipePunchCommandValidator()
    {
        RuleFor(x => x.EmpSysId).GreaterThan(0).WithMessage("Employee ID must be positive.");
        RuleFor(x => x.PunchTime).LessThanOrEqualTo(DateTime.UtcNow.AddMinutes(5))
            .WithMessage("Punch time cannot be in the future.");
        RuleFor(x => x.GateNo).NotEmpty().MaximumLength(10);
        RuleFor(x => x.PunchStatus).Must(s => s == "I" || s == "O")
            .WithMessage("Punch status must be 'I' (In) or 'O' (Out).");
    }
}
