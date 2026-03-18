namespace RecruitmentService.Application.DTOs;

public record ApplicationDto(
    decimal AppId,
    decimal AppSl,
    string? AppUnit,
    decimal? AppVacancyId,
    string Status,
    string? Remarks,
    decimal? UpdatedBy,
    DateTime? UpdatedOn,
    IEnumerable<ApplicationQualificationDto> Qualifications,
    IEnumerable<ApplicationTrainingDto> Trainings
);

public record ApplicationQualificationDto(
    decimal AppId,
    decimal AppQualId,
    decimal? QualCode,
    string? QualDescription,
    string? YearFrom,
    string? YearTo,
    string? InstitutionCode,
    string? InstitutionDescription,
    string? EducationType,
    decimal? SpecializationCode,
    string? SpecializationDescription,
    string? Percentage,
    decimal? DegreeCode,
    string? DegreeDescription,
    string? InstitutionOthers
);

public record ApplicationTrainingDto(
    decimal AppId,
    decimal TrainingId,
    string? Title,
    string? Duration,
    string? Institute,
    string? Location
);

public record SubmitApplicationRequest(
    decimal AppId,
    decimal AppSl,
    string? AppUnit,
    decimal VacancyId,
    IEnumerable<QualificationRequest>? Qualifications,
    IEnumerable<TrainingRequest>? Trainings
);

public record QualificationRequest(
    decimal QualId,
    decimal? QualCode,
    string? QualDescription,
    string? YearFrom,
    string? YearTo,
    string? InstitutionCode,
    string? InstitutionDescription,
    string? EducationType,
    decimal? SpecializationCode,
    string? SpecializationDescription,
    string? Percentage,
    decimal? DegreeCode,
    string? DegreeDescription,
    string? InstitutionOthers
);

public record TrainingRequest(
    decimal TrainingId,
    string? Title,
    string? Duration,
    string? Institute,
    string? Location
);

public record UpdateStatusRequest(string StatusCode, string? Remarks);
