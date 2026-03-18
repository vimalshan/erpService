using FluentValidation;

namespace FaqServices.Application.Features.Grades.Commands.UpdateGrade;

public class UpdateGradeCommandValidator : AbstractValidator<UpdateGradeCommand>
{
    public UpdateGradeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.GradeName).NotEmpty().WithMessage("Grade name is required").MaximumLength(255);
        RuleFor(x => x.Description).MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Description));
        RuleFor(x => x.SortOrder).GreaterThanOrEqualTo(0);
    }
}
