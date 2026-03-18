using CourseService.Application.DTOs;
using MediatR;

namespace CourseService.Application.Courses.Commands.CreateCourse;

public record CreateCourseCommand(
    long CourseId,
    char CourseType,
    string CourseDescription,
    string ObjectiveDescription,
    DateTime EffectiveDate,
    DateTime ClosingDate,
    DateTime StartDate,
    DateTime EndDate,
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
    string? FileName,
    string? ThumbnailPicture,
    string? CourseDuration,
    long? EvaluationId
) : IRequest<CourseDto>;
