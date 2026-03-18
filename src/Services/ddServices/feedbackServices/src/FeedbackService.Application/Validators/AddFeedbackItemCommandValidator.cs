namespace FeedbackService.Application.Validators;

using FluentValidation;
using Commands;

/// <summary>
/// Validator for AddFeedbackItemCommand
/// </summary>
public class AddFeedbackItemCommandValidator : AbstractValidator<AddFeedbackItemCommand>
{
    /// <summary>
    /// Initializes a new instance of the AddFeedbackItemCommandValidator class
    /// </summary>
    public AddFeedbackItemCommandValidator()
    {
        RuleFor(x => x.FeedbackId)
            .GreaterThan(0)
            .WithMessage("Feedback ID must be greater than 0");

        RuleFor(x => x.QuestionNo)
            .GreaterThan(0)
            .WithMessage("Question number must be greater than 0");
    }
}
