using MediatR;

namespace StrategicStock.Application.Commands.UpdateStrategicStock;

public sealed record UpdateStrategicStockCommand(
    int StrategicStockId,
    long? MaxQty,
    long? FilledQty,
    string? StockTypeCode,
    int? ModifiedByUserId) : IRequest;
