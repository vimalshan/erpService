using FluentValidation;

namespace LetTransactionService.Application.Commands.SubmitFeedback;

public class SubmitFeedbackCommandValidator : AbstractValidator<SubmitFeedbackCommand>
{
    public SubmitFeedbackCommandValidator()
    {
        RuleFor(x => x.FeedbackNumber).GreaterThan(0).WithMessage("Feedback number must be positive.");
        RuleFor(x => x.NominationNumber).GreaterThan(0).WithMessage("Nomination number must be positive.");
        RuleFor(x => x.Details).NotEmpty().WithMessage("At least one feedback detail is required.");
        RuleForEach(x => x.Details).ChildRules(detail =>
        {
            detail.RuleFor(d => d.FeedbackType).GreaterThan(0);
            detail.RuleFor(d => d.Rating).InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");
        });
    }
}
