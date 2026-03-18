using CourseService.Domain.Common;

namespace CourseService.Domain.Entities;

/// <summary>
/// Represents course cost details (maps to COURSE_COST table).
/// </summary>
public class CourseCost : BaseEntity
{
    public long? CourseId { get; private set; }
    public long? CostCode { get; private set; }
    public long? CostAmount { get; private set; }
    public char? CostType { get; private set; }
    public string? Remark { get; private set; }
    public string? UnitCode { get; private set; }

    private CourseCost() { }

    public static CourseCost Create(long courseId, long? costCode, long? costAmount, char? costType, string? remark, string? unitCode)
        => new() { CourseId = courseId, CostCode = costCode, CostAmount = costAmount, CostType = costType, Remark = remark, UnitCode = unitCode };
}
