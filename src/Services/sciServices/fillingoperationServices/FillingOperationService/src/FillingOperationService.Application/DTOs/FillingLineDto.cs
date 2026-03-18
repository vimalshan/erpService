namespace FillingOperationService.Application.DTOs;

public record FillingLineDto(
    int FillingLineId,
    int FillingPlantId,
    string FillingLineName,
    int NoOfFillingPoints,
    int? PackageTypeId,
    string? IsClosed,
    DateTime CreationDate
);
