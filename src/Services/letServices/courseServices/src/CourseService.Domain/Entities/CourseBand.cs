using CourseService.Domain.Common;

namespace CourseService.Domain.Entities;

/// <summary>
/// Represents the course band mapping (maps to COURSE_BAND table).
/// </summary>
public class CourseBand : BaseEntity
{
    public long? CourseBandCourseId { get; private set; }
    public long? BandId { get; private set; }

    private CourseBand() { }

    public static CourseBand Create(long courseBandCourseId, long bandId)
        => new() { CourseBandCourseId = courseBandCourseId, BandId = bandId };
}
