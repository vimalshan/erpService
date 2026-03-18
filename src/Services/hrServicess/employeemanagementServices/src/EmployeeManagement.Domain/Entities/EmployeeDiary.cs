using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Domain.Entities;

public sealed class EmployeeDiary : BaseEntity
{
    public long EmployeeId { get; private set; }
    public long DiaryId { get; private set; }
    public char DiaryType { get; private set; }  // A = Awards, R = Recognition
    public long SubType { get; private set; }
    public string? DiaryDate { get; private set; }  // MMYYYY
    public string? Reason { get; private set; }
    public long? UpdatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }

    private EmployeeDiary() { }

    public static EmployeeDiary Create(long employeeId, long diaryId, char diaryType,
        long subType, string? diaryDate, string? reason, long updatedBy)
        => new()
        {
            EmployeeId = employeeId, DiaryId = diaryId, DiaryType = diaryType,
            SubType = subType, DiaryDate = diaryDate, Reason = reason,
            UpdatedBy = updatedBy, UpdatedOn = DateTime.UtcNow
        };
}
