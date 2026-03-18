using CourseService.Domain.Common;
using CourseService.Domain.Entities;
using CourseService.Domain.Events;
using CourseService.Domain.Exceptions;
using CourseService.Domain.ValueObjects;

namespace CourseService.Domain.Aggregates;

/// <summary>
/// Course Aggregate Root - encapsulates COURSE_MAST and all child entities.
/// </summary>
public class CourseAggregate : BaseEntity
{
    // Identity
    public long CourseId { get; private set; }
    public char CourseType { get; private set; }
    public string CourseDescription { get; private set; } = string.Empty;
    public string ObjectiveDescription { get; private set; } = string.Empty;

    // Dates
    public DateTime EffectiveDate { get; private set; }
    public DateTime ClosingDate { get; private set; }
    public DateTime LastDate { get; private set; }
    public DateTime? CancellationDate { get; private set; }
    public string? CancellationRemark { get; private set; }
    public DateTime? PendingDate { get; private set; }

    // Training
    public char TrainingType { get; private set; }

    // Value Objects
    public CourseAddress Address { get; private set; } = null!;
    public CourseDuration Duration { get; private set; } = null!;
    public TrainerInfo TrainerInfo { get; private set; } = null!;

    // Ratings
    public decimal? TrainerRating { get; private set; }
    public decimal? ContentRating { get; private set; }
    public decimal? AdminRating { get; private set; }
    public long? EvaluationId { get; private set; }

    // File / Media
    public string? FileName { get; private set; }
    public string? ThumbnailPicture { get; private set; }

    // Child collections
    private readonly List<CourseSchedule> _schedules = [];
    private readonly List<CourseParticipant> _participants = [];
    private readonly List<CourseBand> _bands = [];
    private readonly List<CourseCost> _costs = [];
    private readonly List<CourseModel> _models = [];

    public IReadOnlyList<CourseSchedule> Schedules => _schedules.AsReadOnly();
    public IReadOnlyList<CourseParticipant> Participants => _participants.AsReadOnly();
    public IReadOnlyList<CourseBand> Bands => _bands.AsReadOnly();
    public IReadOnlyList<CourseCost> Costs => _costs.AsReadOnly();
    public IReadOnlyList<CourseModel> Models => _models.AsReadOnly();

    private CourseAggregate() { }

    public static CourseAggregate Create(
        long courseId,
        char courseType,
        string courseDescription,
        string objectiveDescription,
        DateTime effectiveDate,
        DateTime closingDate,
        DateTime lastDate,
        char trainingType,
        CourseAddress address,
        CourseDuration duration,
        TrainerInfo trainerInfo,
        string? fileName = null,
        string? thumbnailPicture = null,
        long? evaluationId = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(courseDescription);
        ArgumentNullException.ThrowIfNull(address);
        ArgumentNullException.ThrowIfNull(duration);
        ArgumentNullException.ThrowIfNull(trainerInfo);

        var course = new CourseAggregate
        {
            CourseId = courseId,
            CourseType = courseType,
            CourseDescription = courseDescription,
            ObjectiveDescription = objectiveDescription,
            EffectiveDate = effectiveDate,
            ClosingDate = closingDate,
            LastDate = lastDate,
            TrainingType = trainingType,
            Address = address,
            Duration = duration,
            TrainerInfo = trainerInfo,
            FileName = fileName,
            ThumbnailPicture = thumbnailPicture,
            EvaluationId = evaluationId
        };

        course.AddDomainEvent(new CourseCreatedEvent(courseId, courseDescription));
        return course;
    }

    public void Cancel(DateTime cancellationDate, string cancellationRemark)
    {
        if (CancellationDate.HasValue)
            throw new CourseDomainException("Course is already cancelled.");

        CancellationDate = cancellationDate;
        CancellationRemark = cancellationRemark;
        AddDomainEvent(new CourseCancelledEvent(CourseId, cancellationRemark));
    }

    public CourseSchedule AddSchedule(long serialNumber, DateTime scheduleDate, string startTime, string endTime, string locationName, string trainerName)
    {
        if (CancellationDate.HasValue)
            throw new CourseDomainException("Cannot add schedule to a cancelled course.");

        var schedule = CourseSchedule.Create(CourseId, serialNumber, scheduleDate, startTime, endTime, locationName, trainerName);
        _schedules.Add(schedule);
        AddDomainEvent(new CourseScheduleAddedEvent(CourseId, serialNumber, scheduleDate));
        return schedule;
    }

    public CourseParticipant RegisterParticipant(string userCode, long? nominationStatus, DateTime enrollmentDate, char? approvalStatus)
    {
        if (CancellationDate.HasValue)
            throw new CourseDomainException("Cannot register participant for a cancelled course.");

        if (enrollmentDate > ClosingDate)
            throw new CourseDomainException("Enrollment date is after the course closing date.");

        var existing = _participants.FirstOrDefault(p => p.UserCode == userCode && p.CancellationDate == null);
        if (existing is not null)
            throw new CourseDomainException($"Participant '{userCode}' is already registered for this course.");

        var participant = CourseParticipant.Register(CourseId, userCode, nominationStatus, enrollmentDate, approvalStatus);
        _participants.Add(participant);
        AddDomainEvent(new ParticipantRegisteredEvent(CourseId, userCode, enrollmentDate));
        return participant;
    }

    public void CancelParticipant(string userCode, DateTime cancellationDate, string cancellationRemark)
    {
        var participant = _participants.FirstOrDefault(p => p.UserCode == userCode && p.CancellationDate == null)
            ?? throw new CourseDomainException($"Active participant '{userCode}' not found in course.");

        participant.Cancel(cancellationDate, cancellationRemark);
        AddDomainEvent(new ParticipantCancelledEvent(CourseId, userCode, cancellationDate));
    }

    public void UpdateAttendance(string userCode, char attendanceStatus)
    {
        var participant = _participants.FirstOrDefault(p => p.UserCode == userCode)
            ?? throw new CourseDomainException($"Participant '{userCode}' not found in course.");

        participant.UpdateAttendance(attendanceStatus);
        AddDomainEvent(new AttendanceUpdatedEvent(CourseId, userCode, attendanceStatus));
    }

    public void UpdateRatings(decimal? trainerRating, decimal? contentRating, decimal? adminRating)
    {
        TrainerRating = trainerRating;
        ContentRating = contentRating;
        AdminRating = adminRating;
    }
}
