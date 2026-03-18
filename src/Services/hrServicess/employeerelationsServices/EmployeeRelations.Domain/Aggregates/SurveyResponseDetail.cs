using EmployeeRelations.Domain.Common;

namespace EmployeeRelations.Domain.Aggregates;

public class SurveyResponseDetail : BaseEntity
{
    public long QuestionId { get; private set; }
    public long ResponseId { get; private set; }
    public string? ResponseOption { get; private set; }
    public string? ResponseText { get; private set; }

    protected SurveyResponseDetail() { }

    public SurveyResponseDetail(long responseId, long questionId, string? option, string? text)
    {
        ResponseId = responseId;
        QuestionId = questionId;
        ResponseOption = option;
        ResponseText = text;
    }
}
