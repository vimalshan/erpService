namespace FillingOperationService.Application.DTOs;

public record FillingPlantDto(
    int FillingPlantId,
    int CompanyUnitId,
    string FillingPlantName,
    string Location,
    DateTime CreationDate,
    int SciUserIdCreated
);
