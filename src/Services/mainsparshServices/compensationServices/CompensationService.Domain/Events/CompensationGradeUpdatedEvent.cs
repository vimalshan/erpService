using CompensationService.Domain.Common;

namespace CompensationService.Domain.Events;

/// <summary>
/// Event raised when a compensation grade is updated
/// </summary>
public sealed class CompensationGradeUpdatedEvent : DomainEvent
{
    public string GradeCode { get; }
    public string GradeName { get; }
    public decimal BaseSalary { get; }
    public long UpdatedBy { get; }

    public CompensationGradeUpdatedEvent(
        Guid gradeId,
        string gradeCode,
        string gradeName,
        decimal baseSalary,
        long updatedBy) : base(gradeId)
    {
        GradeCode = gradeCode;
        GradeName = gradeName;
        BaseSalary = baseSalary;
        UpdatedBy = updatedBy;
    }
}
