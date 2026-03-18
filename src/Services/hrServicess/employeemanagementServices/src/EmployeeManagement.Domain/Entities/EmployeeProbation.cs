using EmployeeManagement.Domain.Common;
using EmployeeManagement.Domain.Events;

namespace EmployeeManagement.Domain.Entities;

public sealed class EmployeeProbation : BaseEntity
{
    public long ProbationId { get; private set; }
    public long EmployeeId { get; private set; }
    public long UnitId { get; private set; }
    public long GradeId { get; private set; }
    public DateTime DueDate { get; private set; }
    public char ProbationStatus { get; private set; }  // A=Confirmed, B=Extended, C=Terminated
    public bool IsExtended { get; private set; }
    public DateTime ProbationDate { get; private set; }
    public char? SalaryChange { get; private set; }
    public char? GradeChange { get; private set; }
    public string? Rating { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long CreatedBy { get; private set; }
    public long? LastModifiedBy { get; private set; }
    public DateTime? LastModifiedOn { get; private set; }

    private EmployeeProbation() { }

    public static EmployeeProbation Create(long id, long employeeId, long unitId, long gradeId,
        DateTime dueDate, DateTime probationDate, long createdBy)
    {
        return new EmployeeProbation
        {
            ProbationId = id, EmployeeId = employeeId, UnitId = unitId, GradeId = gradeId,
            DueDate = dueDate, ProbationStatus = 'P', IsExtended = false,
            ProbationDate = probationDate, CreatedBy = createdBy, CreatedOn = DateTime.UtcNow
        };
    }

    public void Review(char status, string? rating, long modifiedBy)
    {
        ProbationStatus = status;
        Rating = rating;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }

    public void Extend(long modifiedBy)
    {
        IsExtended = true;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
