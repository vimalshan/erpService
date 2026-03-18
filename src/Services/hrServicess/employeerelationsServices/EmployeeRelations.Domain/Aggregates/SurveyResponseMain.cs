using EmployeeRelations.Domain.Common;
using EmployeeRelations.Domain.Events;

namespace EmployeeRelations.Domain.Aggregates;

public class SurveyResponseMain : BaseEntity
{
    public long ResponseId { get; private set; }
    public long SurveyId { get; private set; }
    public long EmpSysId { get; private set; }
    public long UpdatedBy { get; private set; }
    public DateTime UpdatedOn { get; private set; }
    public string Status { get; private set; } = "P";
    public long? Skip { get; private set; }

    private readonly List<SurveyResponseDetail> _details = new();
    public IReadOnlyCollection<SurveyResponseDetail> Details => _details.AsReadOnly();

    protected SurveyResponseMain() { }

    public SurveyResponseMain(long responseId, long surveyId, long empSysId, long updatedBy)
    {
        ResponseId = responseId;
        SurveyId = surveyId;
        EmpSysId = empSysId;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
        Status = "P";
    }

    public void AddDetail(long questionId, string? option, string? text)
    {
        _details.Add(new SurveyResponseDetail(ResponseId, questionId, option, text));
    }

    public void Submit()
    {
        Status = "S";
        UpdatedOn = DateTime.UtcNow;
    }
}
