using MediatR;
using PurchaseOrderService.Application.Interfaces;

namespace PurchaseOrderService.Application.Queries.GetPurchaseOrders;

public class GetPurchaseOrdersQueryHandler : IRequestHandler<GetPurchaseOrdersQuery, PurchaseOrdersResponse>
{
    private readonly IPurchaseOrderReadRepository _readRepository;

    public GetPurchaseOrdersQueryHandler(IPurchaseOrderReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<PurchaseOrdersResponse> Handle(GetPurchaseOrdersQuery request, CancellationToken cancellationToken)
    {
        var items = await _readRepository.GetAllAsync(request.Page, request.PageSize, request.Status, cancellationToken);
        var totalCount = await _readRepository.GetCountAsync(request.Status, cancellationToken);

        return new PurchaseOrdersResponse
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
