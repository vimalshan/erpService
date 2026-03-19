namespace BatchAndEnvelopeService.Application.DTOs;

public record EnvelopeDto(
    long EnvelopeId,
    string EnvelopeType,
    long CreatedBy,
    DateTime CreatedOn,
    long? ReceivedBy,
    DateTime? ReceivedOn,
    string SummaryFlag,
    long? CancelledBy,
    DateTime? CancelledOn,
    long? ConfirmedBy,
    DateTime? ConfirmedOn,
    long? ScanLotNo,
    long LocationId,
    IEnumerable<EnvelopeDetailDto> Details
);

public record EnvelopeDetailDto(
    long EnvDetId,
    long EnvelopeId,
    string EnvelopeType,
    int DocumentId,
    long CreatedBy,
    DateTime CreatedOn,
    string ReceiveFlag
);
