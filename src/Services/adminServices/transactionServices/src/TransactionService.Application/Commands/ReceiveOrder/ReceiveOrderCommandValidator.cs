namespace TransactionService.Application.Commands.ReceiveOrder;

using FluentValidation;

public sealed class ReceiveOrderCommandValidator : AbstractValidator<ReceiveOrderCommand>
{
    public ReceiveOrderCommandValidator()
    {
        RuleFor(x => x.OrderSubId).GreaterThan(0);
        RuleFor(x => x.ReceivedQty).GreaterThan(0);
        RuleFor(x => x.ReceivedBy).GreaterThan(0);
    }
}
