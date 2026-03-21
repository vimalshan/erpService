namespace ReceivingService.Application.DTOs;

public sealed record ReceivingDto(
    int Id,
    string ReceivingNumber,
    int PoId,
    int WarehouseId,
    DateTime ReceivedDate,
    string Status,
    string? Notes,
    string? CreatedBy,
    DateTime CreatedDate,
    IReadOnlyList<ReceivingLineDto> Lines
);

public sealed record ReceivingLineDto(
    int Id,
    int ReceivingId,
    int PoLineId,
    int ProductId,
    int BinId,
    decimal QuantityReceived,
    string? LotNumber,
    DateOnly? ExpiryDate,
    string? Notes
);
