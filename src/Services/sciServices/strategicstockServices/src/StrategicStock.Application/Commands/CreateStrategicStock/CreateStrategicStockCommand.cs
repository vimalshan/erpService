using MediatR;

namespace StrategicStock.Application.Commands.CreateStrategicStock;

public sealed record CreateStrategicStockCommand(
    int StrategicStockId,
    int SciItemId,
    int? CompanyUnitId,
    string? StockTypeCode,
    long? MaxQty,
    string? EffectiveDate,
    int? CreatedByUserId) : IRequest<int>;
