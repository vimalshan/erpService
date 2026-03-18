namespace FeedbackService.Application.Validators;

using FluentValidation;
using Commands;

/// <summary>
/// Validator for SubmitFeedbackCommand
/// </summary>
public class SubmitFeedbackCommandValidator : AbstractValidator<SubmitFeedbackCommand>
{
    /// <summary>
    /// Initializes a new instance of the SubmitFeedbackCommandValidator class
    /// </summary>
    public SubmitFeedbackCommandValidator()
    {
        RuleFor(x => x.FeedbackId)
            .GreaterThan(0)
            .WithMessage("Feedback ID must be greater than 0");
    }
}
