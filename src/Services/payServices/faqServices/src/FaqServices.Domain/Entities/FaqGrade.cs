using FaqServices.Domain.Common;
using FaqServices.Domain.Events;

namespace FaqServices.Domain.Entities;

/// <summary>
/// Represents a grade/category for FAQs (maps to FAQ_GRADE table).
/// </summary>
public class FaqGrade : BaseEntity
{
    public string GradeName { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public int SortOrder { get; private set; }
    public bool IsActive { get; private set; } = true;

    // Navigation
    private readonly List<FaqQuestion> _questions = new();
    public IReadOnlyCollection<FaqQuestion> Questions => _questions.AsReadOnly();

    private FaqGrade() { } // EF constructor

    public static FaqGrade Create(string gradeName, string? description = null, int sortOrder = 0, string? createdBy = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(gradeName);

        var grade = new FaqGrade
        {
            GradeName = gradeName.Trim(),
            Description = description?.Trim(),
            SortOrder = sortOrder,
            IsActive = true
        };
        grade.SetAuditFields(createdBy);
        grade.AddDomainEvent(new FaqGradeCreatedEvent(grade.PK, grade.GradeName));
        return grade;
    }

    public void Update(string gradeName, string? description, int sortOrder, string? updatedBy = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(gradeName);
        GradeName = gradeName.Trim();
        Description = description?.Trim();
        SortOrder = sortOrder;
        MarkUpdated(updatedBy);
        AddDomainEvent(new FaqGradeUpdatedEvent(PK, GradeName));
    }

    public void Deactivate(string? updatedBy = null)
    {
        IsActive = false;
        MarkUpdated(updatedBy);
    }

    public void Activate(string? updatedBy = null)
    {
        IsActive = true;
        MarkUpdated(updatedBy);
    }
}
