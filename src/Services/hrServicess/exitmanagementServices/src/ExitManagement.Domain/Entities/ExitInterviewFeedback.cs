using ExitManagement.Domain.Common;

namespace ExitManagement.Domain.Entities;

/// <summary>
/// Maps to EMPLOYEE_EXIT_INT - Exit interview feedback answers.
/// </summary>
public class ExitInterviewFeedback : BaseEntity
{
    public decimal ExitNo { get; private set; }
    public decimal SerialNo { get; private set; }
    public string? QuestionId { get; private set; }
    public string? Feedback { get; private set; }

    private ExitInterviewFeedback() { }

    public static ExitInterviewFeedback Create(
        decimal exitNo,
        decimal serialNo,
        string questionId,
        string feedback,
        decimal updatedBy)
    {
        return new ExitInterviewFeedback
        {
            ExitNo = exitNo,
            SerialNo = serialNo,
            QuestionId = questionId,
            Feedback = feedback,
            UpdatedBy = updatedBy,
            UpdatedOn = DateTime.UtcNow
        };
    }

    public void UpdateFeedback(string feedback, decimal updatedBy)
    {
        Feedback = feedback;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
