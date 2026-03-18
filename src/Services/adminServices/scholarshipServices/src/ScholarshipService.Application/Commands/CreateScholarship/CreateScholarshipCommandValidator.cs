using FluentValidation;

namespace ScholarshipService.Application.Commands.CreateScholarship;

public class CreateScholarshipCommandValidator : AbstractValidator<CreateScholarshipCommand>
{
    public CreateScholarshipCommandValidator()
    {
        RuleFor(x => x.EmployeeSysId).GreaterThan(0);
        RuleFor(x => x.GradeId).GreaterThan(0);
        RuleFor(x => x.DependentId).GreaterThan(0);
        RuleFor(x => x.ChildName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastSchool).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastExam).NotEmpty().Must(e => e == "10" || e == "12")
            .WithMessage("LastExam must be '10' or '12'.");
        RuleFor(x => x.CgpaFlag).NotEmpty().Must(f => f == "Y" || f == "N");
        RuleFor(x => x.MarksFile).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CourseName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CourseJoinYear).GreaterThan(1900);
        RuleFor(x => x.CourseDuration).GreaterThan(0);
        RuleFor(x => x.Source).NotEmpty().MaximumLength(1);
        RuleFor(x => x.DisbursementAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.DisbursementFrequency).NotEmpty().MaximumLength(1);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}
