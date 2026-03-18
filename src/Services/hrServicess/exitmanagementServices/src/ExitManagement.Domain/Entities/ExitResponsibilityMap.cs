using ExitManagement.Domain.Common;

namespace ExitManagement.Domain.Entities;

/// <summary>
/// Maps to TT_EMPLOYEE_EXITRESPEX - Exit responsibility checklist mapping.
/// </summary>
public class ExitResponsibilityMap : BaseEntity
{
    public decimal? TtId { get; private set; }
    public decimal? EmployeeSysId { get; private set; }
    public decimal? ChecklistMapId { get; private set; }
    public string? Primary { get; private set; }
    public string? Secondary { get; private set; }
    public string? FunctionalHead { get; private set; }

    private ExitResponsibilityMap() { }

    public static ExitResponsibilityMap Create(
        decimal ttId,
        decimal employeeSysId,
        decimal checklistMapId,
        string? primary,
        string? secondary,
        string? functionalHead)
    {
        return new ExitResponsibilityMap
        {
            TtId = ttId,
            EmployeeSysId = employeeSysId,
            ChecklistMapId = checklistMapId,
            Primary = primary,
            Secondary = secondary,
            FunctionalHead = functionalHead
        };
    }
}
