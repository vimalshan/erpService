using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Domain.Entities;

public sealed class EmployeeCareer : BaseEntity
{
    public long CareerId { get; private set; }
    public long EmployeeId { get; private set; }
    public string? Business { get; private set; }
    public string? Unit { get; private set; }
    public DateTime? From { get; private set; }
    public DateTime? To { get; private set; }
    public string? EmployeeNo { get; private set; }
    public long? GradeId { get; private set; }
    public string? GradeOther { get; private set; }
    public string? Designation { get; private set; }
    public long? DivisionId { get; private set; }
    public string? DivisionOther { get; private set; }
    public long? ProcessId { get; private set; }
    public string? ProcessOther { get; private set; }
    public long? DepartmentId { get; private set; }
    public string? DepartmentOther { get; private set; }
    public string? Reason { get; private set; }
    public long? ModifiedBy { get; private set; }
    public DateTime? ModifiedOn { get; private set; }

    private EmployeeCareer() { }

    public static EmployeeCareer Create(long careerId, long employeeId, string? business, string? unit,
        DateTime? from, DateTime? to, string? employeeNo, long? gradeId, string? designation,
        long? divisionId, long? departmentId, string? reason, long modifiedBy)
    {
        return new EmployeeCareer
        {
            CareerId = careerId, EmployeeId = employeeId, Business = business, Unit = unit,
            From = from, To = to, EmployeeNo = employeeNo, GradeId = gradeId,
            Designation = designation, DivisionId = divisionId, DepartmentId = departmentId,
            Reason = reason, ModifiedBy = modifiedBy, ModifiedOn = DateTime.UtcNow
        };
    }
}
