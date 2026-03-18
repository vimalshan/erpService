using FluentValidation;

namespace FaqServices.Application.Features.Answers.Commands.UpdateAnswer;

public class UpdateAnswerCommandValidator : AbstractValidator<UpdateAnswerCommand>
{
    public UpdateAnswerCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.AnswerText).NotEmpty().WithMessage("Answer text is required").MaximumLength(1000);
        RuleFor(x => x.AnswerTextAr).MaximumLength(1000).When(x => !string.IsNullOrWhiteSpace(x.AnswerTextAr));
        RuleFor(x => x.SortOrder).GreaterThanOrEqualTo(0);
    }
}
