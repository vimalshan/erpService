using CourseService.Domain.Common;
using CourseService.Domain.Enums;

namespace CourseService.Domain.Entities;

/// <summary>
/// Represents a course participant (maps to COURSE_PARTICIPANT_MGT table).
/// </summary>
public class CourseParticipant : BaseEntity
{
    public long CourseId { get; private set; }
    public long? NominationNumber { get; private set; }
    public string UserCode { get; private set; } = string.Empty;
    public DateTime? CancellationDate { get; private set; }
    public string? CancellationRemark { get; private set; }
    public DateTime? EnrollmentDate { get; private set; }
    public char? ApprovalStatus { get; private set; }
    public char? CancelApproval { get; private set; }
    public long? UserPin { get; private set; }
    public string? ApproverCode { get; private set; }
    public long? ApproverPin { get; private set; }
    public long? NominationStatus { get; private set; }
    public long? RequestNumber { get; private set; }
    public char? Type { get; private set; }
    public string? CourseDescription { get; private set; }
    public string? TrainingDate { get; private set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public char? AttendanceStatus { get; private set; }

    private CourseParticipant() { }

    public static CourseParticipant Register(
        long courseId,
        string userCode,
        long? nominationStatus,
        DateTime enrollmentDate,
        char? approvalStatus)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userCode);

        return new CourseParticipant
        {
            CourseId = courseId,
            UserCode = userCode,
            NominationStatus = nominationStatus,
            EnrollmentDate = enrollmentDate,
            ApprovalStatus = approvalStatus
        };
    }

    public void Cancel(DateTime cancellationDate, string cancellationRemark)
    {
        CancellationDate = cancellationDate;
        CancellationRemark = cancellationRemark;
    }

    public void UpdateAttendance(char status)
    {
        AttendanceStatus = status;
    }

    public void Approve(string approverCode, long approverPin)
    {
        ApproverCode = approverCode;
        ApproverPin = approverPin;
        ApprovalStatus = 'Y';
    }
}
