using FluentValidation;

namespace TimeAttendance.Application.AbsenteeismDetails.Commands.CreateAbsenteeismDetail;

public class CreateAbsenteeismDetailCommandValidator : AbstractValidator<CreateAbsenteeismDetailCommand>
{
    public CreateAbsenteeismDetailCommandValidator()
    {
        RuleFor(x => x.UnitId).GreaterThan(0).WithMessage("UnitId must be greater than 0.");
        RuleFor(x => x.Year).InclusiveBetween(2000, 2100).WithMessage("Year must be between 2000 and 2100.");
        RuleFor(x => x.Month).InclusiveBetween(1, 12).WithMessage("Month must be between 1 and 12.");
        RuleFor(x => x.TotalManDays).GreaterThanOrEqualTo(0).WithMessage("Total man days cannot be negative.");
        RuleFor(x => x.AbsentManDays).GreaterThanOrEqualTo(0).WithMessage("Absent man days cannot be negative.");
        RuleFor(x => x.AbsentManDays).LessThanOrEqualTo(x => x.TotalManDays)
            .WithMessage("Absent man days cannot exceed total man days.");
        RuleFor(x => x.GradeCategory).NotEmpty().MaximumLength(3).WithMessage("Grade category must be 1-3 characters.");
        RuleFor(x => x.FunctionId).GreaterThan(0).WithMessage("FunctionId must be greater than 0.");
        RuleFor(x => x.Gender).Must(g => g == 'M' || g == 'F' || g == 'O')
            .WithMessage("Gender must be M, F, or O.");
    }
}
