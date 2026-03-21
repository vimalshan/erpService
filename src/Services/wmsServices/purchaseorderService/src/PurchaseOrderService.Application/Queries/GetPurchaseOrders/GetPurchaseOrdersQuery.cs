using MediatR;
using PurchaseOrderService.Application.DTOs;

namespace PurchaseOrderService.Application.Queries.GetPurchaseOrders;

public record GetPurchaseOrdersQuery : IRequest<PurchaseOrdersResponse>
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public string? Status { get; init; }
}

public record PurchaseOrdersResponse
{
    public IEnumerable<PurchaseOrderSummaryDto> Items { get; init; } = Enumerable.Empty<PurchaseOrderSummaryDto>();
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
}
