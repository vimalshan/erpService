namespace PurchaseSalesService.Application.DTOs;

public sealed record SaleMainDto(
    long SerialNumber,
    long TrackingNumber,
    long TransactionNumber,
    long PurposeCode,
    long StageCode,
    string? IsoNumber,
    DateTime? IsoDate,
    string? ProductDescription,
    string UserId,
    long UserNumber,
    DateTime UpdatedAt,
    char? CancelFlag,
    string? VehicleCustomer,
    IReadOnlyCollection<SaleSubDto> SubItems
);

public sealed record SaleSubDto(
    long? ReferenceNumber,
    long? SerialNumber,
    string? ProductCode,
    decimal? ProductQuantity,
    string? ProductGrade,
    string? UserComment,
    string? CheckbookInvoice,
    char? CancelFlag
);
