using FaqServices.Domain.Common;
using FaqServices.Domain.Events;

namespace FaqServices.Domain.Entities;

/// <summary>
/// Represents a FAQ answer (maps to FAQ_ANSWERS table).
/// </summary>
public class FaqAnswer : BaseEntity
{
    public string QuestionId { get; private set; } = string.Empty;
    public string AnswerText { get; private set; } = string.Empty;
    public string? AnswerTextAr { get; private set; }
    public bool IsCorrect { get; private set; }
    public int SortOrder { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string? ImageBlobUrl { get; private set; }

    // Navigation
    public FaqQuestion? Question { get; private set; }

    private FaqAnswer() { }

    public static FaqAnswer Create(
        string questionId,
        string answerText,
        string? answerTextAr = null,
        bool isCorrect = false,
        int sortOrder = 0,
        string? createdBy = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(questionId);
        ArgumentException.ThrowIfNullOrWhiteSpace(answerText);

        var answer = new FaqAnswer
        {
            QuestionId = questionId,
            AnswerText = answerText.Trim(),
            AnswerTextAr = answerTextAr?.Trim(),
            IsCorrect = isCorrect,
            SortOrder = sortOrder,
            IsActive = true
        };
        answer.SetAuditFields(createdBy);
        answer.AddDomainEvent(new FaqAnswerCreatedEvent(answer.PK, answer.QuestionId, answer.AnswerText));
        return answer;
    }

    public void Update(string answerText, string? answerTextAr, bool isCorrect, int sortOrder, string? updatedBy = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(answerText);
        AnswerText = answerText.Trim();
        AnswerTextAr = answerTextAr?.Trim();
        IsCorrect = isCorrect;
        SortOrder = sortOrder;
        MarkUpdated(updatedBy);
        AddDomainEvent(new FaqAnswerUpdatedEvent(PK, AnswerText));
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
