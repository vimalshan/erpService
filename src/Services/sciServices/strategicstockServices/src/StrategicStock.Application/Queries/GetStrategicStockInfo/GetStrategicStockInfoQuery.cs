using MediatR;
using StrategicStock.Application.DTOs;

namespace StrategicStock.Application.Queries.GetStrategicStockInfo;

public sealed record GetStrategicStockInfoQuery(int SciItemId, int CompanyUnitId)
    : IRequest<IReadOnlyList<StrategicStockInfoDto>>;
