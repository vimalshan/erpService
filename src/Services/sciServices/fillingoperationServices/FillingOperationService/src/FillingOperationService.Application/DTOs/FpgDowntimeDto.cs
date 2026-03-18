namespace FillingOperationService.Application.DTOs;

public record FpgDowntimeDto(
    int FpgId,
    int? FillingPointGroupId,
    DateTime StartDateTime,
    DateTime EndDateTime,
    string? DowntimeType,
    string? NoOfFillingPoints
);
