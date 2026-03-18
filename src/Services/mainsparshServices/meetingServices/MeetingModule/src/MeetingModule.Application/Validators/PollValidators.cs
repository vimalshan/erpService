using FluentValidation;
using MeetingModule.Application.Commands.Polls;

namespace MeetingModule.Application.Validators;

public class CreatePollValidator : AbstractValidator<CreatePollCommand>
{
    private static readonly string[] ValidPollTypes = ["MULTIPLE_CHOICE", "YES_NO", "RATING", "TEXT"];

    public CreatePollValidator()
    {
        RuleFor(x => x.Dto.MeetingId).GreaterThan(0);
        RuleFor(x => x.Dto.PollQuestion).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Dto.PollType)
            .Must(t => t is null || ValidPollTypes.Contains(t))
            .WithMessage("Poll type must be one of: MULTIPLE_CHOICE, YES_NO, RATING, TEXT");
    }
}
