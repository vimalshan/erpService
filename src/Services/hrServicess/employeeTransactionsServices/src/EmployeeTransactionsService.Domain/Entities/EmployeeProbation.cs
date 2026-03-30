using EmployeeTransactionsService.Domain.Common;
using EmployeeTransactionsService.Domain.Events;

namespace EmployeeTransactionsService.Domain.Entities;

public sealed class EmployeeProbation : BaseEntity
{
    private EmployeeProbation()
    {
    }

    public decimal ProbId { get; private set; }
    public decimal ProbEmpSysId { get; private set; }
    public DateTime? ProbDueDate { get; private set; }
    public decimal? ProbDdRequestNo { get; private set; }
    public string? ProbFinStatus { get; private set; }
    public DateTime? ProbReviewDate { get; private set; }
    public DateTime? ProbNxtReviewDate { get; private set; }
    public DateTime? ProbConfDate { get; private set; }

    public static EmployeeProbation CreateInitial(decimal probationId, decimal employeeId, DateTime dueDate)
    {
        return new EmployeeProbation
        {
            ProbId = probationId,
            ProbEmpSysId = employeeId,
            ProbDueDate = dueDate
        };
    }

    public void Review(string finalStatus, DateTime? confirmationDate, DateTime? nextReviewDate)
    {
        ProbFinStatus = finalStatus;
        ProbReviewDate = DateTime.UtcNow;
        ProbNxtReviewDate = nextReviewDate;
        ProbConfDate = finalStatus == "A" ? (confirmationDate ?? DateTime.UtcNow) : ProbConfDate;

        AddDomainEvent(new ProbationReviewedDomainEvent(ProbId, ProbEmpSysId, finalStatus));
    }
}