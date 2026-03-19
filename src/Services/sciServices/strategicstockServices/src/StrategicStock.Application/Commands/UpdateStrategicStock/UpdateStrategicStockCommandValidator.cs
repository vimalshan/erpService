using FluentValidation;

namespace StrategicStock.Application.Commands.UpdateStrategicStock;

public sealed class UpdateStrategicStockCommandValidator : AbstractValidator<UpdateStrategicStockCommand>
{
    public UpdateStrategicStockCommandValidator()
    {
        RuleFor(x => x.StrategicStockId).GreaterThan(0);
        RuleFor(x => x.MaxQty).GreaterThanOrEqualTo(0).When(x => x.MaxQty.HasValue);
        RuleFor(x => x.FilledQty).GreaterThanOrEqualTo(0).When(x => x.FilledQty.HasValue);
        RuleFor(x => x.StockTypeCode).MaximumLength(2).When(x => x.StockTypeCode is not null);
    }
}
