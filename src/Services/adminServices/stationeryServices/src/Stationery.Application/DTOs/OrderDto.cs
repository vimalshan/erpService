namespace Stationery.Application.DTOs;

public record OrderDto(
    long Id,
    long LocationId,
    long VendorId,
    DateTime OrderedDate,
    DateTime DeliveryDate,
    long OrderedBy,
    IEnumerable<OrderSubDto> Details
);

public record OrderSubDto(
    long Id,
    long OrderMainId,
    long RequestSubId,
    long OrderedQty,
    long OrderPrice,
    long ActualPrice,
    DateTime? ReceivedOn,
    DateTime DeliveryDate
);
