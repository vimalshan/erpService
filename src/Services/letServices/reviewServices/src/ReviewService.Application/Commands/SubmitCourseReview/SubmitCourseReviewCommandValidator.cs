using FluentValidation;

namespace ReviewService.Application.Commands.SubmitCourseReview;

public class SubmitCourseReviewCommandValidator : AbstractValidator<SubmitCourseReviewCommand>
{
    private static readonly char[] ValidStatuses = ['A', 'I', 'P', 'C'];

    public SubmitCourseReviewCommandValidator()
    {
        RuleFor(x => x.ReviewSrlNum).GreaterThan(0).WithMessage("ReviewSrlNum must be positive.");
        RuleFor(x => x.Status).Must(s => ValidStatuses.Contains(s))
            .WithMessage("Status must be one of: A, I, P, C.");
        RuleFor(x => x.Remarks1).MaximumLength(4000).When(x => x.Remarks1 is not null);
        RuleFor(x => x.Remarks2).MaximumLength(4000).When(x => x.Remarks2 is not null);
    }
}
