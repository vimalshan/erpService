using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class GradeCatStayRule : AggregateRoot<string>
{
    public string GradeCategory { get; private set; } = string.Empty;
    public string ApplyToUnit { get; private set; } = string.Empty;
    public string UnitId { get; private set; } = string.Empty;
    public string ApplyToGrade { get; private set; } = string.Empty;
    public string GradeId { get; private set; } = string.Empty;
    public string TravelType { get; private set; } = string.Empty;
    public string StayType { get; private set; } = string.Empty;
    public string CityClassId { get; private set; } = string.Empty;
    public string Limit { get; private set; } = string.Empty;
    public string BookCharges { get; private set; } = string.Empty;
    public string NightStayValue { get; private set; } = string.Empty;
    public string IncidentalExpenses { get; private set; } = string.Empty;

    private GradeCatStayRule() { }

    public static GradeCatStayRule Create(string id, string gradeCategory, string applyToUnit,
        string unitId, string applyToGrade, string gradeId, string travelType, string stayType,
        string cityClassId, string limit, string bookCharges, string nightStayValue, string incidentalExpenses)
    {
        return new GradeCatStayRule
        {
            Id = id, GradeCategory = gradeCategory, ApplyToUnit = applyToUnit,
            UnitId = unitId, ApplyToGrade = applyToGrade, GradeId = gradeId,
            TravelType = travelType, StayType = stayType, CityClassId = cityClassId,
            Limit = limit, BookCharges = bookCharges, NightStayValue = nightStayValue,
            IncidentalExpenses = incidentalExpenses
        };
    }
}
