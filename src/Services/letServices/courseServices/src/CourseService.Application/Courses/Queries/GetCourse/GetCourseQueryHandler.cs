using CourseService.Application.DTOs;
using CourseService.Domain.Aggregates;
using CourseService.Domain.Interfaces;
using MediatR;

namespace CourseService.Application.Courses.Queries.GetCourse;

public class GetCourseQueryHandler(ICourseRepository repository) : IRequestHandler<GetCourseQuery, CourseDto?>
{
    public async Task<CourseDto?> Handle(GetCourseQuery query, CancellationToken ct)
    {
        var course = await repository.GetByIdAsync(query.CourseId, ct);
        return course is null ? null : MapToDto(course);
    }

    internal static CourseDto MapToDto(CourseAggregate c) => new(
        c.CourseId, c.CourseType, c.CourseDescription, c.ObjectiveDescription,
        c.EffectiveDate, c.ClosingDate, c.Duration.StartDate, c.Duration.EndDate,
        c.LastDate, c.Duration.NumberOfDays, c.TrainingType,
        c.Address.LocationCode, c.Address.AddressLine1, c.Address.AddressLine2, c.Address.AddressLine3,
        c.Address.PinCode, c.Address.PhoneNumber,
        c.TrainerInfo.TrainerName1, c.TrainerInfo.TrainerName2, c.TrainerInfo.TrainerName3,
        c.TrainerInfo.TrainerDesignation1, c.TrainerInfo.TrainerDesignation2, c.TrainerInfo.TrainerDesignation3,
        c.TrainerInfo.TrainerContact1, c.TrainerInfo.TrainerContact2, c.TrainerInfo.TrainerContact3,
        c.TrainerInfo.TrainerCode,
        c.TrainerRating, c.ContentRating, c.AdminRating,
        c.CancellationDate, c.CancellationRemark,
        c.FileName, c.ThumbnailPicture, c.Duration.DurationDisplay, c.EvaluationId);
}
