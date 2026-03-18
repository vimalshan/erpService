using CourseService.Domain.Common;
using CourseService.Domain.ValueObjects;

namespace CourseService.Domain.Entities;

/// <summary>
/// Represents a Course Schedule session (maps to COURSE_SCHEDULE table).
/// </summary>
public class CourseSchedule : BaseEntity
{
    public long CourseId { get; private set; }
    public long ScheduleSerialNumber { get; private set; }
    public DateTime ScheduleDate { get; private set; }
    public string StartTime { get; private set; } = string.Empty;
    public string EndTime { get; private set; } = string.Empty;
    public string LocationName { get; private set; } = string.Empty;
    public string TrainerName { get; private set; } = string.Empty;

    private CourseSchedule() { }

    public static CourseSchedule Create(
        long courseId,
        long scheduleSerialNumber,
        DateTime scheduleDate,
        string startTime,
        string endTime,
        string locationName,
        string trainerName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(startTime);
        ArgumentException.ThrowIfNullOrWhiteSpace(endTime);
        ArgumentException.ThrowIfNullOrWhiteSpace(locationName);

        return new CourseSchedule
        {
            CourseId = courseId,
            ScheduleSerialNumber = scheduleSerialNumber,
            ScheduleDate = scheduleDate,
            StartTime = startTime,
            EndTime = endTime,
            LocationName = locationName,
            TrainerName = trainerName
        };
    }

    public void Update(DateTime scheduleDate, string startTime, string endTime, string locationName, string trainerName)
    {
        ScheduleDate = scheduleDate;
        StartTime = startTime;
        EndTime = endTime;
        LocationName = locationName;
        TrainerName = trainerName;
    }
}
