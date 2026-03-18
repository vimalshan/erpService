namespace CourseService.Application.DTOs;

public record CourseDto(
    long CourseId,
    char CourseType,
    string CourseDescription,
    string ObjectiveDescription,
    DateTime EffectiveDate,
    DateTime ClosingDate,
    DateTime StartDate,
    DateTime EndDate,
    DateTime LastDate,
    long NumberOfDays,
    char TrainingType,
    char LocationCode,
    string AddressLine1,
    string AddressLine2,
    string AddressLine3,
    long PinCode,
    string PhoneNumber,
    string? TrainerName1,
    string? TrainerName2,
    string? TrainerName3,
    string? TrainerDesignation1,
    string? TrainerDesignation2,
    string? TrainerDesignation3,
    string? TrainerContact1,
    string? TrainerContact2,
    string? TrainerContact3,
    long? TrainerCode,
    decimal? TrainerRating,
    decimal? ContentRating,
    decimal? AdminRating,
    DateTime? CancellationDate,
    string? CancellationRemark,
    string? FileName,
    string? ThumbnailPicture,
    string? CourseDuration,
    long? EvaluationId
);

public record CourseScheduleDto(
    long CourseId,
    long ScheduleSerialNumber,
    DateTime ScheduleDate,
    string StartTime,
    string EndTime,
    string LocationName,
    string TrainerName
);

public record CourseParticipantDto(
    long CourseId,
    string UserCode,
    long? NominationStatus,
    DateTime? EnrollmentDate,
    char? ApprovalStatus,
    DateTime? CancellationDate,
    string? CancellationRemark,
    char? AttendanceStatus,
    long? UserPin,
    string? ApproverCode,
    long? ApproverPin
);

public record CourseSummaryDto(
    long CourseId,
    string CourseDescription,
    char CourseType,
    DateTime StartDate,
    DateTime EndDate,
    long NumberOfDays,
    int ParticipantCount,
    int ScheduleCount,
    bool IsCancelled
);
