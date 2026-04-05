using FluentValidation;

namespace TimeSheetService.Application.Commands.SubmitTimesheet;

public class SubmitTimesheetCommandValidator : AbstractValidator<SubmitTimesheetCommand>
{
    public SubmitTimesheetCommandValidator()
    {
        RuleFor(x => x.EmployeeSysId).GreaterThan(0).WithMessage("EmployeeSysId must be > 0.");
        RuleFor(x => x.TimeDate).NotEmpty().WithMessage("TimeDate is required.");
        RuleFor(x => x.TotalHours).GreaterThan(0).WithMessage("TotalHours must be > 0.");
        RuleFor(x => x.EntryTypeCode).Must(c => c == "S" || c == "M" || c == "A")
            .WithMessage("EntryTypeCode must be S, M, or A.");
        RuleFor(x => x.ModifiedBy).GreaterThan(0).WithMessage("ModifiedBy must be > 0.");
    }
}
