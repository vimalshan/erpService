namespace FeedbackService.Application.Validators;

using FluentValidation;
using Commands;

/// <summary>
/// Validator for CreateFeedbackCommand
/// </summary>
public class CreateFeedbackCommandValidator : AbstractValidator<CreateFeedbackCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateFeedbackCommandValidator class
    /// </summary>
    public CreateFeedbackCommandValidator()
    {
        RuleFor(x => x.FeedbackId)
            .GreaterThan(0)
            .WithMessage("Feedback ID must be greater than 0");

        RuleFor(x => x.RequestNo)
            .GreaterThan(0)
            .WithMessage("Request number must be greater than 0");

        RuleFor(x => x.ApproverSystemId)
            .GreaterThan(0)
            .WithMessage("Approver system ID must be greater than 0");

        RuleFor(x => x.Remarks)
            .MaximumLength(2000)
            .WithMessage("Remarks must not exceed 2000 characters");
    }
}
