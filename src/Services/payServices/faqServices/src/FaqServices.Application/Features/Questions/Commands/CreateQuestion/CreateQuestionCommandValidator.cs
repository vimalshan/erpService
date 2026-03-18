using FluentValidation;

namespace FaqServices.Application.Features.Questions.Commands.CreateQuestion;

public class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
{
    public CreateQuestionCommandValidator()
    {
        RuleFor(x => x.GradeId).NotEmpty().WithMessage("Grade id is required");
        RuleFor(x => x.QuestionText).NotEmpty().WithMessage("Question text is required").MaximumLength(1000);
        RuleFor(x => x.QuestionTextAr).MaximumLength(1000).When(x => !string.IsNullOrWhiteSpace(x.QuestionTextAr));
        RuleFor(x => x.SortOrder).GreaterThanOrEqualTo(0);
    }
}
