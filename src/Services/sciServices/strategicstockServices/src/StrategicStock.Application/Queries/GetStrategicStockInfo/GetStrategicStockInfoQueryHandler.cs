using MediatR;
using StrategicStock.Application.DTOs;
using StrategicStock.Application.Interfaces;

namespace StrategicStock.Application.Queries.GetStrategicStockInfo;

public sealed class GetStrategicStockInfoQueryHandler(IDapperContext dapper)
    : IRequestHandler<GetStrategicStockInfoQuery, IReadOnlyList<StrategicStockInfoDto>>
{
    public async Task<IReadOnlyList<StrategicStockInfoDto>> Handle(
        GetStrategicStockInfoQuery request, CancellationToken cancellationToken)
    {
        return await dapper.QueryStoredProcAsync<StrategicStockInfoDto>(
            "usp_GetStrategicStockInfo",
            new { p_ItemID = request.SciItemId, p_CompanyUnitID = request.CompanyUnitId });
    }
}
