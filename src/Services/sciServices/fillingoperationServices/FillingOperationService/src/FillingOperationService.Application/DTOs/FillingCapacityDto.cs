namespace FillingOperationService.Application.DTOs;

public record FillingCapacityDto(
    int FillingPointGroupId,
    int MainProductId,
    int PackageTypeId,
    int ItemCapacityId,
    int CapacityPerShift,
    int UsagePriority
);
