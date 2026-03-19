namespace BatchAndEnvelopeService.Application.DTOs;

public record ScanLotDto(
    long ScanLotNo,
    long UserId,
    string Status,
    int DeviceNo,
    DateTime? CloseDate,
    DateTime? CreatedOn,
    long? DeviceId,
    string? ScanFlag
);
