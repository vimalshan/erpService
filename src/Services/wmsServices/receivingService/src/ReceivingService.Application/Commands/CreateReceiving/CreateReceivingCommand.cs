using MediatR;
using ReceivingService.Application.DTOs;

namespace ReceivingService.Application.Commands.CreateReceiving;

/// <summary>Command to create a new Receiving document.</summary>
public sealed record CreateReceivingCommand(
    string ReceivingNumber,
    int PoId,
    int WarehouseId,
    string? Notes,
    string? CreatedBy,
    IReadOnlyList<CreateReceivingLineRequest> Lines
) : IRequest<ReceivingDto>;

public sealed record CreateReceivingLineRequest(
    int PoLineId,
    int ProductId,
    int BinId,
    decimal QuantityReceived,
    string? LotNumber,
    DateOnly? ExpiryDate,
    string? Notes
);
