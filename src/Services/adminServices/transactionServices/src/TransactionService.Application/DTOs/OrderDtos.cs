namespace TransactionService.Application.DTOs;

public sealed record OrderMainDto(
    long OrderMainId,
    long LocationId,
    long VendorId,
    DateTime DeliveryDate,
    DateTime OrderedDate,
    long OrderedBy,
    List<OrderSubDto> Details);

public sealed record OrderSubDto(
    long OrderSubId,
    long OrderMainId,
    long RequestSubId,
    long OrderedQty,
    DateTime? ReceivedOn,
    long ReceivedBy,
    long OrderPrice,
    long ActualPrice,
    DateTime ReceivedDate,
    DateTime DeliveryDate,
    long? ReceiptEntryBy,
    DateTime? ReceiptEntryOn);

public sealed record OrderSummaryDto(
    long OrderMainId,
    long VendorId,
    long LocationId,
    DateTime OrderedDate,
    DateTime DeliveryDate,
    int TotalItems,
    int ReceivedItems);
