using FluentValidation;

namespace CardManagement.Application.Cards.Commands.CreateGuestCard;

public class CreateGuestCardCommandValidator : AbstractValidator<CreateGuestCardCommand>
{
    public CreateGuestCardCommandValidator()
    {
        RuleFor(x => x.CanteenUnit).GreaterThan(0).WithMessage("Canteen unit must be positive.");
        RuleFor(x => x.CardSequence).GreaterThan(0).WithMessage("Card sequence must be positive.");
        RuleFor(x => x.CardNumber).NotEmpty().MaximumLength(20).WithMessage("Card number is required (max 20 chars).");
        RuleFor(x => x.CardName).NotEmpty().MaximumLength(50).WithMessage("Card name is required (max 50 chars).");
        RuleFor(x => x.CardType).MaximumLength(1).When(x => x.CardType != null);
        RuleFor(x => x.ReportingUnit).MaximumLength(3).When(x => x.ReportingUnit != null);
        RuleFor(x => x.EffectiveDate).NotEmpty().LessThanOrEqualTo(DateTime.UtcNow.AddDays(1));
    }
}
