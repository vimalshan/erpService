using CourseService.Domain.Common;

namespace CourseService.Domain.Entities;

/// <summary>
/// Represents a course skill model entry (maps to COURSE_MODEL table).
/// </summary>
public class CourseModel : BaseEntity
{
    public long CourseId { get; private set; }
    public long SkillNumber { get; private set; }
    public long LevelNumber { get; private set; }
    public string? SkillGroup { get; private set; }

    private CourseModel() { }

    public static CourseModel Create(long courseId, long skillNumber, long levelNumber, string? skillGroup)
        => new() { CourseId = courseId, SkillNumber = skillNumber, LevelNumber = levelNumber, SkillGroup = skillGroup };
}
