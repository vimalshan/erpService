using EmployeeTransactionsService.Domain.Common;
using EmployeeTransactionsService.Domain.Events;

namespace EmployeeTransactionsService.Domain.Entities;

public sealed class EmployeeGradeChange : BaseEntity
{
    private EmployeeGradeChange()
    {
    }

    public decimal EmpGradeChangeId { get; private set; }
    public decimal EmpEmpSysId { get; private set; }
    public decimal EmpOldGrade { get; private set; }
    public decimal EmpNewGrade { get; private set; }
    public DateTime EmpEffDate { get; private set; }
    public string EmpStatus { get; private set; } = "P";
    public decimal? EmpCreatedBy { get; private set; }
    public DateTime EmpCreatedOn { get; private set; }
    public decimal? EmpApprovedBy { get; private set; }
    public DateTime? EmpApprovedOn { get; private set; }

    public static EmployeeGradeChange Create(decimal changeId, decimal employeeId, decimal oldGrade, decimal newGrade, DateTime effectiveDate, string status, decimal createdBy)
    {
        var entity = new EmployeeGradeChange
        {
            EmpGradeChangeId = changeId,
            EmpEmpSysId = employeeId,
            EmpOldGrade = oldGrade,
            EmpNewGrade = newGrade,
            EmpEffDate = effectiveDate,
            EmpStatus = status,
            EmpCreatedBy = createdBy,
            EmpCreatedOn = DateTime.UtcNow,
            EmpApprovedBy = status == "A" ? createdBy : null,
            EmpApprovedOn = status == "A" ? DateTime.UtcNow : null
        };

        entity.AddDomainEvent(new EmployeeGradeChangedDomainEvent(changeId, employeeId, oldGrade, newGrade));
        return entity;
    }
}