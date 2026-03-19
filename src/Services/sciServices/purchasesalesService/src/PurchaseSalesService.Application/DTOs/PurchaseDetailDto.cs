namespace PurchaseSalesService.Application.DTOs;

public sealed record PurchaseDetailDto(
    long SerialNumber,
    long TrackingNumber,
    long TransactionNumber,
    long PurposeCode,
    long StageCode,
    long? OracleMerchandise,
    string? SupplierCode,
    string? TonNumLoaded,
    string? TonNumUnloaded,
    string? UserId,
    long? UserNumber,
    DateTime UpdatedAt,
    char? CancelFlag
);
