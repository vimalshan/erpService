namespace BatchAndEnvelopeService.Application.DTOs;

public record BatchDto(
    long BatchId,
    long CreatedBy,
    DateTime CreatedOn,
    long LocationId,
    long ReceivedBy,
    DateTime ReceivedOn,
    string PodNo,
    string SummaryFlag,
    long? CancelBy,
    DateTime? CancelDate,
    long? ConfirmedBy,
    DateTime? ConfirmedOn,
    string? CourierName,
    string ScanFlag,
    IEnumerable<BatchDetailDto> Details
);

public record BatchDetailDto(
    int BatchDetId,
    long BatchId,
    int EnvelopeId,
    long CreatedBy,
    DateTime CreatedOn,
    string ReceiveFlag,
    long? ReceivedBy,
    DateTime? ReceivedOn
);
