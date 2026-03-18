using FluentValidation;

namespace ReviewService.Application.Commands.SubmitFeedback;

public class SubmitFeedbackCommandValidator : AbstractValidator<SubmitFeedbackCommand>
{
    public SubmitFeedbackCommandValidator()
    {
        RuleFor(x => x.CourseId).GreaterThan(0).WithMessage("CourseId must be positive.");
        RuleFor(x => x.UserId).NotEmpty().MaximumLength(255).WithMessage("UserId is required and must not exceed 255 characters.");
        RuleFor(x => x.ReviewDate).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("ReviewDate cannot be in the future.");
        RuleFor(x => x.GeneralRemarks).NotEmpty().MaximumLength(255).WithMessage("GeneralRemarks is required.");
        RuleFor(x => x.RequestNum).GreaterThan(0).WithMessage("RequestNum must be positive.");
        RuleFor(x => x.OverallRating).InclusiveBetween(0, 10).WithMessage("OverallRating must be between 0 and 10.");
    }
}
