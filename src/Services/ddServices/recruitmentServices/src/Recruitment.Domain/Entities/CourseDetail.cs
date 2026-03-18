using Recruitment.Domain.Common;

namespace Recruitment.Domain.Entities;

/// <summary>
/// CourseDetail entity representing applicant's educational background
/// </summary>
public class CourseDetail : Entity
{
    public decimal ApplicationNumber { get; private set; }
    public decimal SerialNo { get; private set; }
    public string CourseTitle { get; private set; }
    public string Duration { get; private set; }
    public string Institute { get; private set; }

    public CourseDetail(
        decimal applicationNumber,
        decimal serialNo,
        string courseTitle,
        string duration,
        string institute)
    {
        ApplicationNumber = applicationNumber;
        SerialNo = serialNo;
        CourseTitle = courseTitle;
        Duration = duration;
        Institute = institute;
        Id = serialNo;
    }
}
