using FluentValidation;

namespace AccountingService.Application.Features.GlPosting.Commands.PostGlEntry;

public class PostGlEntryCommandValidator : AbstractValidator<PostGlEntryCommand>
{
    public PostGlEntryCommandValidator()
    {
        RuleFor(x => x.AccountCode).NotEmpty().MaximumLength(10);
        RuleFor(x => x.PostingDate).NotEmpty();
        RuleFor(x => x.ReferenceId).GreaterThan(0);
        RuleFor(x => x).Must(x => x.DebitAmount > 0 || x.CreditAmount > 0)
            .WithMessage("Either DebitAmount or CreditAmount must be greater than zero.");
        RuleFor(x => x.DebitAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CreditAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Remarks).MaximumLength(200).When(x => x.Remarks != null);
    }
}
