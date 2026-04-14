using MediatR;
using WMTransactional.Application.DTOs;

namespace WMTransactional.Application.Commands.CreateShipment;

public record CreateShipmentCommand : IRequest<ShipmentDto>
{
    public string ShipmentNumber { get; init; } = null!;
    public int SoId { get; init; }
    public string? TrackingNumber { get; init; }
    public string? Carrier { get; init; }
    public string? Notes { get; init; }
    public string? CreatedBy { get; init; }
    public List<CreateShipmentLineItem> Lines { get; init; } = [];
}

public record CreateShipmentLineItem
{
    public int SoLineId { get; init; }
    public int ProductId { get; init; }
    public int BinId { get; init; }
    public decimal QuantityShipped { get; init; }
    public string? LotNumber { get; init; }
    public DateTime? ExpiryDate { get; init; }
    public string? Notes { get; init; }
}
