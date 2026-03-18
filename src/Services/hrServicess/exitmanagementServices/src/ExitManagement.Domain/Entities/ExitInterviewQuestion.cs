using ExitManagement.Domain.Common;

namespace ExitManagement.Domain.Entities;

/// <summary>
/// Maps to TT_EXIT_INTERVIEW - Exit interview survey questions.
/// </summary>
public class ExitInterviewQuestion : BaseEntity
{
    public string? QuestionId { get; private set; }
    public string? QuestionDescription { get; private set; }
    public decimal? OrderId { get; private set; }

    private ExitInterviewQuestion() { }

    public static ExitInterviewQuestion Create(string questionId, string description, decimal orderId)
    {
        return new ExitInterviewQuestion
        {
            QuestionId = questionId,
            QuestionDescription = description,
            OrderId = orderId
        };
    }
}
