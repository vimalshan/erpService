using MediatR;
using WMTransactional.Application.DTOs;

namespace WMTransactional.Application.Commands.CreatePurchaseOrder;

public record CreatePurchaseOrderCommand : IRequest<PurchaseOrderDto>
{
    public string PoNumber { get; init; } = null!;
    public int SupplierId { get; init; }
    public DateTime? ExpectedDate { get; init; }
    public string? Notes { get; init; }
    public string? CreatedBy { get; init; }
    public List<CreatePurchaseOrderLineItem> Lines { get; init; } = [];
}

public record CreatePurchaseOrderLineItem
{
    public int ProductId { get; init; }
    public decimal QuantityOrdered { get; init; }
    public decimal? UnitPrice { get; init; }
    public string? Notes { get; init; }
}
