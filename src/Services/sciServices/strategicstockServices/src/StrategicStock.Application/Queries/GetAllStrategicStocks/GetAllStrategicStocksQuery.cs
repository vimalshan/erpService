using MediatR;
using StrategicStock.Application.DTOs;

namespace StrategicStock.Application.Queries.GetAllStrategicStocks;

public sealed record GetAllStrategicStocksQuery : IRequest<IReadOnlyList<StrategicStockDto>>;
