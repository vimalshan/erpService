using EmployeeTransactionsService.Domain.Common;

namespace EmployeeTransactionsService.Domain.Entities;

public sealed class EmployeeGrade : BaseEntity
{
    private EmployeeGrade()
    {
    }

    public decimal GradeEmpSysId { get; private set; }
    public decimal GradeTranId { get; private set; }
    public decimal GradeId { get; private set; }
    public DateTime? GradeEffDate { get; private set; }
    public DateTime? GradeClsDate { get; private set; }
    public string? GradeRemarks { get; private set; }
    public string? GradeLivFlag { get; private set; }
    public decimal? GradeUpdatedBy { get; private set; }
    public DateTime? GradeUpdatedOn { get; private set; }
    public string? GradeProbation { get; private set; }

    public static EmployeeGrade Create(decimal employeeId, decimal tranId, decimal gradeId, DateTime effectiveDate, decimal updatedBy, string probationFlag, string? remarks = null)
    {
        return new EmployeeGrade
        {
            GradeEmpSysId = employeeId,
            GradeTranId = tranId,
            GradeId = gradeId,
            GradeEffDate = effectiveDate,
            GradeRemarks = remarks,
            GradeLivFlag = "Y",
            GradeUpdatedBy = updatedBy,
            GradeUpdatedOn = DateTime.UtcNow,
            GradeProbation = probationFlag
        };
    }

    public void Close(DateTime closeDate, decimal updatedBy)
    {
        GradeClsDate = closeDate;
        GradeLivFlag = "N";
        GradeUpdatedBy = updatedBy;
        GradeUpdatedOn = DateTime.UtcNow;
    }
}