using FluentValidation;

namespace StrategicStock.Application.Commands.CloseStrategicStock;

public sealed class CloseStrategicStockCommandValidator : AbstractValidator<CloseStrategicStockCommand>
{
    public CloseStrategicStockCommandValidator()
    {
        RuleFor(x => x.StrategicStockId).GreaterThan(0);
    }
}
