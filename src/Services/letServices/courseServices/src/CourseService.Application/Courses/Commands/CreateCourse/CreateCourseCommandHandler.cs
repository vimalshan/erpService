using CourseService.Application.DTOs;
using CourseService.Domain.Aggregates;
using CourseService.Domain.Interfaces;
using CourseService.Domain.ValueObjects;
using MediatR;

namespace CourseService.Application.Courses.Commands.CreateCourse;

public class CreateCourseCommandHandler(ICourseRepository repository) : IRequestHandler<CreateCourseCommand, CourseDto>
{
    public async Task<CourseDto> Handle(CreateCourseCommand cmd, CancellationToken ct)
    {
        var address = new CourseAddress(cmd.LocationCode, cmd.AddressLine1, cmd.AddressLine2, cmd.AddressLine3, cmd.PinCode, cmd.PhoneNumber);
        var duration = new CourseDuration(cmd.StartDate, cmd.EndDate, cmd.NumberOfDays, cmd.CourseDuration);
        var trainerInfo = new TrainerInfo(
            cmd.TrainerName1, cmd.TrainerName2, cmd.TrainerName3,
            cmd.TrainerDesignation1, cmd.TrainerDesignation2, cmd.TrainerDesignation3,
            cmd.TrainerContact1, cmd.TrainerContact2, cmd.TrainerContact3,
            cmd.TrainerCode);

        var course = CourseAggregate.Create(
            cmd.CourseId, cmd.CourseType, cmd.CourseDescription, cmd.ObjectiveDescription,
            cmd.EffectiveDate, cmd.ClosingDate, cmd.EffectiveDate,
            cmd.TrainingType, address, duration, trainerInfo,
            cmd.FileName, cmd.ThumbnailPicture, cmd.EvaluationId);

        await repository.AddAsync(course, ct);
        return MapToDto(course);
    }

    private static CourseDto MapToDto(CourseAggregate c) => new(
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
