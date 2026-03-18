using MediatR;
using Stationery.Application.DTOs;
using Stationery.Domain.Interfaces;

namespace Stationery.Application.Features.Items.Queries;

public record GetLowStockItemsQuery(long Threshold) : IRequest<IEnumerable<LowStockItemDto>>;

public class GetLowStockItemsQueryHandler : IRequestHandler<GetLowStockItemsQuery, IEnumerable<LowStockItemDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetLowStockItemsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<LowStockItemDto>> Handle(GetLowStockItemsQuery request, CancellationToken cancellationToken)
    {
        var sql = "EXEC [dbo].[GetLowStockItems] @Threshold = @Threshold";
        return await _unitOfWork.QueryAsync<LowStockItemDto>(sql, new { request.Threshold });
    }
}
