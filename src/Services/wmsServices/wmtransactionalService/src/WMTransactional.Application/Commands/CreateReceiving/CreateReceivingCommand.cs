using MediatR;
using WMTransactional.Application.DTOs;

namespace WMTransactional.Application.Commands.CreateReceiving;

public record CreateReceivingCommand : IRequest<ReceivingDto>
{
    public string ReceivingNumber { get; init; } = null!;
    public int PoId { get; init; }
    public string? Notes { get; init; }
    public string? CreatedBy { get; init; }
    public List<CreateReceivingLineItem> Lines { get; init; } = [];
}

public record CreateReceivingLineItem
{
    public int PoLineId { get; init; }
    public int ProductId { get; init; }
    public int BinId { get; init; }
    public decimal QuantityReceived { get; init; }
    public string? LotNumber { get; init; }
    public DateTime? ExpiryDate { get; init; }
    public string? Notes { get; init; }
}
