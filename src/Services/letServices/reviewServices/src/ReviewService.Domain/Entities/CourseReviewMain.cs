using ReviewService.Domain.Common;

namespace ReviewService.Domain.Entities;

/// <summary>
/// Maps to COURSE_REVIEWMAIN table.
/// </summary>
public class CourseReviewMain : BaseEntity
{
    public long RvCrsId { get; private set; }
    public char RvUsrId { get; private set; }
    public DateTime RvRevDat { get; private set; }
    public string RvGenRem { get; private set; } = string.Empty;
    public string RqSupUsr { get; private set; } = string.Empty;
    public char RvSrlNum { get; private set; }
    public char RvSupRem { get; private set; }
    public char RvRatPer { get; private set; }
    public char RvFilNam { get; private set; }
    public DateTime RvNxtDat { get; private set; }
    public DateTime RvOrgDat { get; private set; }

    public ICollection<CourseReviewSub> ReviewSubs { get; private set; } = new List<CourseReviewSub>();

    private CourseReviewMain() { }

    public static CourseReviewMain Create(
        long courseId, char userId, DateTime reviewDate,
        string generalRemarks, string supervisorUser,
        char srlNum, char supRem, char ratPer, char fileName,
        DateTime nextDate, DateTime originalDate)
    {
        return new CourseReviewMain
        {
            RvCrsId = courseId,
            RvUsrId = userId,
            RvRevDat = reviewDate,
            RvGenRem = generalRemarks,
            RqSupUsr = supervisorUser,
            RvSrlNum = srlNum,
            RvSupRem = supRem,
            RvRatPer = ratPer,
            RvFilNam = fileName,
            RvNxtDat = nextDate,
            RvOrgDat = originalDate
        };
    }
}
