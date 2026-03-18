using ReviewService.Domain.Common;

namespace ReviewService.Domain.Entities;

/// <summary>
/// Maps to COURSE_REVIEWSUB table.
/// </summary>
public class CourseReviewSub : BaseEntity
{
    public long RvCrsId { get; private set; }
    public char RvUsrId { get; private set; }
    public long RvSrlNum { get; private set; }
    public char RvTypCod { get; private set; }
    public long RvTypNum { get; private set; }

    public CourseReviewMain? ReviewMain { get; private set; }

    private CourseReviewSub() { }

    public static CourseReviewSub Create(
        long courseId, char userId, long srlNum, char typeCode, long typeNum)
    {
        return new CourseReviewSub
        {
            RvCrsId = courseId,
            RvUsrId = userId,
            RvSrlNum = srlNum,
            RvTypCod = typeCode,
            RvTypNum = typeNum
        };
    }
}
