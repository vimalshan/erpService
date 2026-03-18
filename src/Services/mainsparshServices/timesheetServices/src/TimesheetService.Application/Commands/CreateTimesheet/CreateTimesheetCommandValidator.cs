using FluentValidation;

namespace TimesheetService.Application.Commands.CreateTimesheet;

public sealed class CreateTimesheetCommandValidator : AbstractValidator<CreateTimesheetCommand>
{
    public CreateTimesheetCommandValidator()
    {
        RuleFor(x => x.EmployeeId).GreaterThan(0).WithMessage("EmployeeId must be a positive number.");
        RuleFor(x => x.TimesheetDate).NotEmpty().WithMessage("TimesheetDate is required.");
        RuleFor(x => x.WorkDate).NotEmpty().WithMessage("WorkDate is required.");
        RuleFor(x => x.WorkDate).LessThanOrEqualTo(x => x.TimesheetDate).WithMessage("WorkDate cannot be after TimesheetDate.");
        RuleFor(x => x.TotalHours).InclusiveBetween(0, 24).When(x => x.TotalHours.HasValue).WithMessage("TotalHours must be between 0 and 24.");
        RuleFor(x => x.CreatedBy).GreaterThan(0).WithMessage("CreatedBy must be a positive number.");
        RuleFor(x => x.WorkDescription).MaximumLength(4000).When(x => x.WorkDescription != null).WithMessage("WorkDescription must not exceed 4000 characters.");
    }
}
