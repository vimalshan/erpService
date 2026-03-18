using CompensationService.Domain.Common;

namespace CompensationService.Domain.Events;

/// <summary>
/// Event raised when a compensation grade is created
/// </summary>
public sealed class CompensationGradeCreatedEvent : DomainEvent
{
    public string GradeCode { get; }
    public string GradeName { get; }
    public int GradeLevel { get; }
    public decimal BaseSalary { get; }
    public DateTime EffectiveFrom { get; }
    public long CreatedBy { get; }

    public CompensationGradeCreatedEvent(
        Guid gradeId,
        string gradeCode,
        string gradeName,
        int gradeLevel,
        decimal baseSalary,
        DateTime effectiveFrom,
        long createdBy) : base(gradeId)
    {
        GradeCode = gradeCode;
        GradeName = gradeName;
        GradeLevel = gradeLevel;
        BaseSalary = baseSalary;
        EffectiveFrom = effectiveFrom;
        CreatedBy = createdBy;
    }
}
