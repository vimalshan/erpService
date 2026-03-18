using FluentValidation;

namespace FaqServices.Application.Features.Answers.Commands.CreateAnswer;

public class CreateAnswerCommandValidator : AbstractValidator<CreateAnswerCommand>
{
    public CreateAnswerCommandValidator()
    {
        RuleFor(x => x.QuestionId).NotEmpty().WithMessage("Question id is required");
        RuleFor(x => x.AnswerText).NotEmpty().WithMessage("Answer text is required").MaximumLength(1000);
        RuleFor(x => x.AnswerTextAr).MaximumLength(1000).When(x => !string.IsNullOrWhiteSpace(x.AnswerTextAr));
        RuleFor(x => x.SortOrder).GreaterThanOrEqualTo(0);
    }
}
