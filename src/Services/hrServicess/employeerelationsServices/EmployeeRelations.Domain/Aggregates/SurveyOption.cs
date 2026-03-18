using EmployeeRelations.Domain.Common;

namespace EmployeeRelations.Domain.Aggregates;

public class SurveyOption : BaseEntity
{
    public long OptionId { get; private set; }
    public long QuestionId { get; private set; }
    public string Description { get; private set; } = string.Empty;

    protected SurveyOption() { }

    public SurveyOption(long optionId, long questionId, string description)
    {
        OptionId = optionId;
        QuestionId = questionId;
        Description = description;
    }
}
