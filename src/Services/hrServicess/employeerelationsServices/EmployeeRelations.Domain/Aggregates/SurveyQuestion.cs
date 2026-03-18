using EmployeeRelations.Domain.Common;

namespace EmployeeRelations.Domain.Aggregates;

public class SurveyQuestion : BaseEntity
{
    public long QuestId { get; private set; }
    public long SurveyId { get; private set; }
    public string QuestName { get; private set; } = string.Empty;
    public string QuestType { get; private set; } = string.Empty;
    public int? MaxOptLimit { get; private set; }
    public long SectionId { get; private set; }
    public bool Mandatory { get; private set; }
    public long SortOrder { get; private set; }
    public int? MinOptLimit { get; private set; }

    private readonly List<SurveyOption> _options = new();
    public IReadOnlyCollection<SurveyOption> Options => _options.AsReadOnly();

    protected SurveyQuestion() { }

    public SurveyQuestion(long questId, long surveyId, string questName, string questType, long sectionId, bool mandatory, long sortOrder)
    {
        QuestId = questId;
        SurveyId = surveyId;
        QuestName = questName;
        QuestType = questType;
        SectionId = sectionId;
        Mandatory = mandatory;
        SortOrder = sortOrder;
    }

    public void AddOption(long optionId, string description) => _options.Add(new SurveyOption(optionId, QuestId, description));
}
