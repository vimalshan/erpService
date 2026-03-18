using FluentValidation;

namespace FaqServices.Application.Features.Questions.Commands.UpdateQuestion;

public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
{
    public UpdateQuestionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.QuestionText).NotEmpty().WithMessage("Question text is required").MaximumLength(1000);
        RuleFor(x => x.QuestionTextAr).MaximumLength(1000).When(x => !string.IsNullOrWhiteSpace(x.QuestionTextAr));
        RuleFor(x => x.SortOrder).GreaterThanOrEqualTo(0);
    }
}
