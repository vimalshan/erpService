using EmployeeRelations.Domain.Common;
using EmployeeRelations.Domain.Events;

namespace EmployeeRelations.Domain.Aggregates;

/// <summary>Aggregate root for Survey management.</summary>
public class SurveyMaster : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string Image { get; private set; } = string.Empty;
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public DateTime? ClosureDate { get; private set; }
    public string AutoLock { get; private set; } = "N";
    public string? Flag { get; private set; }
    public long? TemplateId { get; private set; }

    private readonly List<SurveyQuestion> _questions = new();
    private readonly List<SurveyResponseMain> _responses = new();

    public IReadOnlyCollection<SurveyQuestion> Questions => _questions.AsReadOnly();
    public IReadOnlyCollection<SurveyResponseMain> Responses => _responses.AsReadOnly();

    protected SurveyMaster() { }

    public SurveyMaster(long id, string name, string image, DateTime startDate, DateTime? endDate, string autoLock)
    {
        Id = id;
        Name = name;
        Image = image;
        StartDate = startDate;
        EndDate = endDate;
        AutoLock = autoLock;
        AddDomainEvent(new SurveyCreatedEvent(id, name, startDate));
    }

    public void AddQuestion(long questId, string questName, string questType, long sectionId, bool mandatory, long sortOrder)
    {
        _questions.Add(new SurveyQuestion(questId, Id, questName, questType, sectionId, mandatory, sortOrder));
    }

    public void Close(DateTime closureDate)
    {
        ClosureDate = closureDate;
        Flag = "C";
        AddDomainEvent(new SurveyClosedEvent(Id, closureDate));
    }

    public void AddResponse(long responseId, long empSysId, long updatedBy)
    {
        _responses.Add(new SurveyResponseMain(responseId, Id, empSysId, updatedBy));
    }
}
