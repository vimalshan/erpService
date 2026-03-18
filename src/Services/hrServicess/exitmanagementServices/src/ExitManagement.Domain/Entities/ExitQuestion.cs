using ExitManagement.Domain.Common;

namespace ExitManagement.Domain.Entities;

/// <summary>
/// Maps to TT_EXIT_QUESTIONS - Exit interview question master.
/// </summary>
public class ExitQuestion : BaseEntity
{
    public string? QuestionId { get; private set; }
    public string? QuestionDescription { get; private set; }
    public decimal? QuestionOrder { get; private set; }

    private ExitQuestion() { }

    public static ExitQuestion Create(string questionId, string description, decimal order)
    {
        return new ExitQuestion
        {
            QuestionId = questionId,
            QuestionDescription = description,
            QuestionOrder = order
        };
    }

    public void Update(string description, decimal order)
    {
        QuestionDescription = description;
        QuestionOrder = order;
    }
}
