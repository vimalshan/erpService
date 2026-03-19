using MediatR;
using StrategicStock.Application.DTOs;

namespace StrategicStock.Application.Queries.GetStrategicStockById;

public sealed record GetStrategicStockByIdQuery(int StrategicStockId) : IRequest<StrategicStockDto?>;
