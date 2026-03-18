using FaqServices.Domain.Common;
using FaqServices.Domain.Events;

namespace FaqServices.Domain.Entities;

/// <summary>
/// Represents a FAQ question (maps to FAQ_QUESTION table).
/// </summary>
public class FaqQuestion : BaseEntity
{
    public string GradeId { get; private set; } = string.Empty;
    public string QuestionText { get; private set; } = string.Empty;
    public string? QuestionTextAr { get; private set; }
    public int SortOrder { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string? ImageBlobUrl { get; private set; }

    // Navigation
    public FaqGrade? Grade { get; private set; }
    private readonly List<FaqAnswer> _answers = new();
    public IReadOnlyCollection<FaqAnswer> Answers => _answers.AsReadOnly();

    private FaqQuestion() { }

    public static FaqQuestion Create(
        string gradeId,
        string questionText,
        string? questionTextAr = null,
        int sortOrder = 0,
        string? createdBy = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(gradeId);
        ArgumentException.ThrowIfNullOrWhiteSpace(questionText);

        var question = new FaqQuestion
        {
            GradeId = gradeId,
            QuestionText = questionText.Trim(),
            QuestionTextAr = questionTextAr?.Trim(),
            SortOrder = sortOrder,
            IsActive = true
        };
        question.SetAuditFields(createdBy);
        question.AddDomainEvent(new FaqQuestionCreatedEvent(question.PK, question.GradeId, question.QuestionText));
        return question;
    }

    public void Update(string questionText, string? questionTextAr, int sortOrder, string? updatedBy = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(questionText);
        QuestionText = questionText.Trim();
        QuestionTextAr = questionTextAr?.Trim();
        SortOrder = sortOrder;
        MarkUpdated(updatedBy);
        AddDomainEvent(new FaqQuestionUpdatedEvent(PK, QuestionText));
    }

    public void SetImageUrl(string blobUrl, string? updatedBy = null)
    {
        ImageBlobUrl = blobUrl;
        MarkUpdated(updatedBy);
    }

    public void Deactivate(string? updatedBy = null)
    {
        IsActive = false;
        MarkUpdated(updatedBy);
    }
}
