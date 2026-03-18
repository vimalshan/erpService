namespace TrainingDevelopment.Application.DTOs;

public record TrainingDetailDto(
    decimal Id,
    decimal FinancialYear,
    decimal EmployeeSysId,
    string TrainingNeed,
    string GapArea,
    decimal Mode,
    string ModeDisplay,
    decimal ProgramId,
    string ProgramDescription,
    DateTime PlannedFrom,
    DateTime PlannedTo,
    string Status,
    string StatusDisplay,
    DateTime? ActualFrom,
    DateTime? ActualTo,
    decimal? InstituteId,
    string? InstituteDescription,
    decimal? TrainerId,
    string? TrainerDescription,
    decimal? PlaceId,
    string? Place,
    decimal? Cost,
    string? DroppedRemarks,
    decimal? LastModifiedBy,
    DateTime? LastModifiedOn
);
