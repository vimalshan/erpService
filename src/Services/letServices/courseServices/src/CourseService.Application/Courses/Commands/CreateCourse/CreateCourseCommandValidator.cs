using FluentValidation;

namespace CourseService.Application.Courses.Commands.CreateCourse;

public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(x => x.CourseId).GreaterThan(0).WithMessage("Course ID must be positive.");
        RuleFor(x => x.CourseDescription).NotEmpty().MaximumLength(255);
        RuleFor(x => x.ObjectiveDescription).NotEmpty().MaximumLength(255);
        RuleFor(x => x.StartDate).LessThan(x => x.EndDate).WithMessage("Start date must be before end date.");
        RuleFor(x => x.EffectiveDate).NotEmpty();
        RuleFor(x => x.ClosingDate).GreaterThan(x => x.EffectiveDate);
        RuleFor(x => x.NumberOfDays).GreaterThan(0);
        RuleFor(x => x.AddressLine1).NotEmpty().MaximumLength(255);
        RuleFor(x => x.PinCode).GreaterThan(0);
        RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(255);
        RuleFor(x => x.CourseType).Must(t => "IEOB".Contains(t)).WithMessage("Course type must be I, E, O, or B.");
        RuleFor(x => x.TrainingType).Must(t => "COJW".Contains(t)).WithMessage("Training type must be C, O, J, or W.");
    }
}
