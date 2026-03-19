using MediatR;

namespace StrategicStock.Application.Commands.CloseStrategicStock;

public sealed record CloseStrategicStockCommand(int StrategicStockId, int? ModifiedByUserId) : IRequest;
