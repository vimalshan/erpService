using FluentValidation;
using EmployeeService.Application.Commands.AssignApprover;
using EmployeeService.Application.Commands.MapCalendar;
using EmployeeService.Application.Commands.RecordTimeInfo;

namespace EmployeeService.Application.Validators;

public sealed class AssignApproverCommandValidator : AbstractValidator<AssignApproverCommand>
{
    public AssignApproverCommandValidator()
    {
        RuleFor(x => x.EmpSysId).GreaterThan(0).WithMessage("Employee ID must be positive.");
        RuleFor(x => x.ApproverSysId).GreaterThan(0).WithMessage("Approver system ID must be positive.");
        RuleFor(x => x.Level).InclusiveBetween(1, 10).WithMessage("Approver level must be between 1 and 10.");
        RuleFor(x => x.AssignedBy).GreaterThan(0).WithMessage("AssignedBy must be a valid user ID.");
    }
}

public sealed class MapCalendarCommandValidator : AbstractValidator<MapCalendarCommand>
{
    public MapCalendarCommandValidator()
    {
        RuleFor(x => x.EmpSysId).GreaterThan(0).WithMessage("Employee ID must be positive.");
        RuleFor(x => x.CalendarId).GreaterThan(0).WithMessage("Calendar ID must be positive.");
        RuleFor(x => x.MappedBy).GreaterThan(0).WithMessage("MappedBy must be a valid user ID.");
    }
}

public sealed class RecordTimeInfoCommandValidator : AbstractValidator<RecordTimeInfoCommand>
{
    private static readonly HashSet<char> _validFlags = ['P', 'A', 'L', 'H', 'W', 'X'];

    public RecordTimeInfoCommandValidator()
    {
        RuleFor(x => x.EmpSysId).GreaterThan(0);
        RuleFor(x => x.AttFlag).Must(f => _validFlags.Contains(char.ToUpperInvariant(f)))
            .WithMessage("Attendance flag must be one of P, A, L, H, W, X.");
        RuleFor(x => x.ModifiedBy).GreaterThan(0);
    }
}
