using FluentValidation;

namespace LetTransactionService.Application.Commands.SubmitReview;

public class SubmitReviewCommandValidator : AbstractValidator<SubmitReviewCommand>
{
    public SubmitReviewCommandValidator()
    {
        RuleFor(x => x.ReviewSerialNumber).GreaterThan(0).WithMessage("Review serial number must be positive.");
        RuleFor(x => x.FeedbackNumber).GreaterThan(0).WithMessage("Feedback number must be positive.");
        RuleFor(x => x.ImplementationGoal).MaximumLength(4000);
        RuleFor(x => x.KeyLearning).MaximumLength(4000);
        RuleFor(x => x.KeyStepsImplementation).MaximumLength(4000);
        RuleFor(x => x.KeyOutputsExpected).MaximumLength(4000);
        RuleFor(x => x.MeasurementProcess).MaximumLength(4000);
        RuleFor(x => x.HelpRequiredFromHr).MaximumLength(4000);
    }
}
