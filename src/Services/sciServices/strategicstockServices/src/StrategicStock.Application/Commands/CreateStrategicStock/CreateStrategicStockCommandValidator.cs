using FluentValidation;

namespace StrategicStock.Application.Commands.CreateStrategicStock;

public sealed class CreateStrategicStockCommandValidator : AbstractValidator<CreateStrategicStockCommand>
{
    public CreateStrategicStockCommandValidator()
    {
        RuleFor(x => x.StrategicStockId).GreaterThan(0);
        RuleFor(x => x.SciItemId).GreaterThan(0);
        RuleFor(x => x.StockTypeCode).MaximumLength(2).When(x => x.StockTypeCode is not null);
        RuleFor(x => x.MaxQty).GreaterThanOrEqualTo(0).When(x => x.MaxQty.HasValue);
    }
}
