namespace AccessService.Domain.Entities;

/// <summary>
/// SPARSHMENU_ACCESS - SPARSH Menu Access Control
/// Defines granular access to SPARSH menus by unit, calendar, and grade category
/// </summary>
public class SPARSHMenuAccess : AggregateRoot
{
    public long AccessId { get; private set; }
    
    public long UnitId { get; private set; }
    
    public long CalendarId { get; private set; }
    
    public string GradeCategory { get; private set; }
    
    public long SPARSHMenuId { get; private set; }

    private SPARSHMenuAccess() { }

    public SPARSHMenuAccess(long accessId, long unitId, long calendarId, string gradeCategory, long sparshMenuId)
    {
        if (accessId <= 0)
            throw new ArgumentException("Access ID must be greater than 0", nameof(accessId));

        if (unitId <= 0)
            throw new ArgumentException("Unit ID must be greater than 0", nameof(unitId));

        if (calendarId <= 0)
            throw new ArgumentException("Calendar ID must be greater than 0", nameof(calendarId));

        if (string.IsNullOrWhiteSpace(gradeCategory) || gradeCategory.Length != 3)
            throw new ArgumentException("Grade category must be exactly 3 characters", nameof(gradeCategory));

        if (sparshMenuId <= 0)
            throw new ArgumentException("SPARSH menu ID must be greater than 0", nameof(sparshMenuId));

        AccessId = accessId;
        UnitId = unitId;
        CalendarId = calendarId;
        GradeCategory = gradeCategory;
        SPARSHMenuId = sparshMenuId;
    }

    public void UpdateAccess(long unitId, long calendarId, string gradeCategory, long sparshMenuId)
    {
        if (unitId <= 0)
            throw new ArgumentException("Unit ID must be greater than 0", nameof(unitId));

        if (calendarId <= 0)
            throw new ArgumentException("Calendar ID must be greater than 0", nameof(calendarId));

        if (string.IsNullOrWhiteSpace(gradeCategory) || gradeCategory.Length != 3)
            throw new ArgumentException("Grade category must be exactly 3 characters", nameof(gradeCategory));

        if (sparshMenuId <= 0)
            throw new ArgumentException("SPARSH menu ID must be greater than 0", nameof(sparshMenuId));

        UnitId = unitId;
        CalendarId = calendarId;
        GradeCategory = gradeCategory;
        SPARSHMenuId = sparshMenuId;
    }
}
