using CompensationService.Domain.Common;
using CompensationService.Domain.Events;
using CompensationService.Domain.ValueObjects;

namespace CompensationService.Domain.Entities;

/// <summary>
/// Aggregate Root for Compensation Grade
/// </summary>
public sealed class CompensationGrade : AggregateRoot
{
    public GradeCode GradeCode { get; private set; } = null!;
    public string GradeName { get; private set; } = null!;
    public int GradeLevel { get; private set; }
    public SalaryStructure SalaryStructure { get; private set; } = null!;
    public GradeStatus Status { get; private set; } = null!;
    public DateTime EffectiveFrom { get; private set; }
    public DateTime? EffectiveTo { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long? UpdatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }

    private CompensationGrade() { }

    /// <summary>
    /// Factory method to create a new compensation grade
    /// </summary>
    public static CompensationGrade Create(
        string gradeCode,
        string gradeName,
        int gradeLevel,
        decimal baseSalary,
        decimal hraPercentage,
        decimal daPercentage,
        DateTime effectiveFrom,
        long createdBy)
    {
        var grade = new CompensationGrade
        {
            GradeCode = GradeCode.Create(gradeCode),
            GradeName = gradeName ?? throw new ArgumentNullException(nameof(gradeName)),
            GradeLevel = gradeLevel,
            SalaryStructure = SalaryStructure.Create(baseSalary, hraPercentage, daPercentage),
            Status = GradeStatus.CreateActive(),
            EffectiveFrom = effectiveFrom,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow,
            Version = 1
        };

        // Raise domain event
        var @event = new CompensationGradeCreatedEvent(
            Guid.NewGuid(),
            gradeCode,
            gradeName,
            gradeLevel,
            baseSalary,
            effectiveFrom,
            createdBy);

        grade.AddDomainEvent(@event);

        return grade;
    }

    /// <summary>
    /// Update compensation grade details
    /// </summary>
    public void Update(
        string gradeName,
        decimal baseSalary,
        decimal hraPercentage,
        decimal daPercentage,
        long updatedBy)
    {
        GradeName = gradeName ?? throw new ArgumentNullException(nameof(gradeName));
        SalaryStructure = SalaryStructure.Create(baseSalary, hraPercentage, daPercentage);
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
        Version++;

        var @event = new CompensationGradeUpdatedEvent(
            Guid.NewGuid(),
            GradeCode.Value,
            gradeName,
            baseSalary,
            updatedBy);

        AddDomainEvent(@event);
    }

    /// <summary>
    /// Change grade status
    /// </summary>
    public void ChangeStatus(char newStatus, long changedBy)
    {
        var newGradeStatus = GradeStatus.Create(newStatus);
        if (Status.Value == newGradeStatus.Value)
            return;

        Status = newGradeStatus;
        UpdatedBy = changedBy;
        UpdatedOn = DateTime.UtcNow;
        Version++;

        var @event = new CompensationGradeStatusChangedEvent(
            Guid.NewGuid(),
            newStatus,
            changedBy);

        AddDomainEvent(@event);
    }

    /// <summary>
    /// Set effective end date for the grade
    /// </summary>
    public void SetEffectiveEndDate(DateTime effectiveTo, long updatedBy)
    {
        if (effectiveTo <= EffectiveFrom)
            throw new ArgumentException("Effective end date must be after effective start date");

        EffectiveTo = effectiveTo;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
        Version++;
    }

    /// <summary>
    /// Check if grade is currently active based on effective dates
    /// </summary>
    public bool IsCurrentlyActive()
    {
        var today = DateTime.UtcNow.Date;
        return Status.IsActive && 
               today >= EffectiveFrom.Date && 
               (EffectiveTo == null || today <= EffectiveTo.Value.Date);
    }
}
