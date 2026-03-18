namespace RecruitmentService.Application.DTOs;

public record VacancyDto(
    decimal VacancyId,
    string VacancyUnit,
    decimal VacancyGrade,
    decimal VacancyPositionId,
    string VacancyName,
    string? VacancyReporting,
    decimal VacancyLocation,
    decimal VacancyProcess,
    string VacancyAge,
    string VacancyExperience,
    string VacancyQualification,
    string? VacancyNarration1,
    string? VacancyNarration2,
    string? VacancyNarration3,
    string? VacancyNarration4,
    string? VacancyAttachment,
    DateTime? VacancyLastDate,
    bool AdvertiseIntranet,
    bool AdvertiseInternet,
    string LiveStatus,
    decimal? NumberOfOpenings,
    decimal? CtcFrom,
    decimal? CtcTo,
    string? Designation,
    string? VacancyType,
    bool InternalReferralAllowed,
    string? InternalReferralEmail,
    DateTime? PostedDate,
    string? Remarks,
    bool DisabilityFlag
);

public record VacancySummaryDto(
    decimal VacancyId,
    string VacancyName,
    string? Designation,
    string VacancyUnit,
    decimal VacancyLocation,
    decimal VacancyProcess,
    DateTime? VacancyLastDate,
    string LiveStatus,
    decimal? NumberOfOpenings,
    decimal? CtcFrom,
    decimal? CtcTo
);

public record CreateVacancyRequest(
    decimal VacancyId,
    string VacancyUnit,
    decimal VacancyGrade,
    decimal VacancyPositionId,
    string VacancyName,
    string? VacancyReporting,
    decimal VacancyLocation,
    decimal VacancyProcess,
    string VacancyAge,
    string VacancyExperience,
    string VacancyQualification,
    string? VacancyNarration1,
    string? VacancyNarration2,
    string? VacancyNarration3,
    string? VacancyNarration4,
    DateTime? VacancyLastDate,
    decimal VacancyUnitId,
    string? VacancyType,
    string? Designation,
    decimal? NumberOfOpenings,
    decimal? CtcFrom,
    decimal? CtcTo,
    bool AllowUploadResume,
    bool InternalReferralAllowed,
    string? InternalReferralEmail
);

public record UpdateVacancyRequest(
    string VacancyName,
    string VacancyAge,
    string VacancyExperience,
    string VacancyQualification,
    string? VacancyNarration1,
    string? VacancyNarration2,
    string? VacancyNarration3,
    string? VacancyNarration4,
    DateTime? VacancyLastDate
);
