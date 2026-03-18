using FluentValidation;

namespace FaqServices.Application.Features.Grades.Commands.CreateGrade;

public class CreateGradeCommandValidator : AbstractValidator<CreateGradeCommand>
{
    public CreateGradeCommandValidator()
    {
        RuleFor(x => x.GradeName)
            .NotEmpty().WithMessage("Grade name is required.")
            .MaximumLength(255).WithMessage("Grade name must not exceed 255 characters.");

        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("Sort order must be non-negative.");
    }
}
