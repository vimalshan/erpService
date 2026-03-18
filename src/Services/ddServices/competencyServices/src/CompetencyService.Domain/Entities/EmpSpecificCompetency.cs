using CompetencyService.Domain.Common;
using CompetencyService.Domain.Exceptions;

namespace CompetencyService.Domain.Entities;

/// <summary>Maps to EMP_SPECIFIC_COMPETENCY — employee-assigned competencies.</summary>
public class EmpSpecificCompetency : AuditableEntity
{
    public decimal EmpSysId { get; private set; }
    public decimal CompetencyId { get; private set; }
    public char CompetencyType { get; private set; }   // COMPETENCY_TYPE char(1)
    public decimal YearId { get; private set; }        // DD_YEARID

    private EmpSpecificCompetency() { }

    public static EmpSpecificCompetency Assign(
        decimal empSysId, decimal competencyId, char type, decimal yearId, decimal? modifiedBy)
    {
        var e = new EmpSpecificCompetency
        {
            EmpSysId = empSysId,
            CompetencyId = competencyId,
            CompetencyType = type,
            YearId = yearId
        };
        e.SetAudit(modifiedBy);
        return e;
    }
}
