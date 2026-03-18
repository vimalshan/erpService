namespace MedicineManagement.Application.DTOs;

public record PurchaseMainDto(
    string CompanyCode, long TransactionNumber, string VendorName,
    string InvoiceNumber, DateTime InvoiceDate, decimal InvoiceAmount,
    char CancelFlag, IReadOnlyList<PurchaseSubDto> LineItems);

public record PurchaseSubDto(
    string CompanyCode, long TransactionNumber, string SerialNumber,
    string MedicineCode, string PackagingType, long? PackagingQuantity,
    long? PackagingNos, long? TotalQuantity, DateTime? ManufacturingDate,
    DateTime? ExpiryDate, string? LotNumber);

public record CreatePurchaseDto(
    string CompanyCode, long TransactionNumber, string VendorName,
    string InvoiceNumber, DateTime InvoiceDate, decimal InvoiceAmount,
    List<CreatePurchaseLineItemDto> LineItems);

public record CreatePurchaseLineItemDto(
    string SerialNumber, string MedicineCode, string PackagingType,
    long? PackagingQuantity, long? PackagingNos, long? TotalQuantity,
    DateTime? ManufacturingDate, DateTime? ExpiryDate, string? LotNumber);
