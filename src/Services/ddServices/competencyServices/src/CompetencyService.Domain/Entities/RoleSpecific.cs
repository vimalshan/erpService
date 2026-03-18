using CompetencyService.Domain.Common;

namespace CompetencyService.Domain.Entities;

/// <summary>Maps to ROLE_SPECIFIC — role-specific competency assignment.</summary>
public class RoleSpecific : AuditableEntity
{
    public decimal EmpSysId { get; private set; }
    public decimal CompetencyId { get; private set; }
    public DateTime? EffFrom { get; private set; }
    public DateTime? EffTo { get; private set; }

    private RoleSpecific() { }

    public static RoleSpecific Create(
        decimal empSysId, decimal competencyId,
        DateTime? effFrom, DateTime? effTo, decimal? modifiedBy)
    {
        var e = new RoleSpecific
        {
            EmpSysId = empSysId,
            CompetencyId = competencyId,
            EffFrom = effFrom,
            EffTo = effTo
        };
        e.SetAudit(modifiedBy);
        return e;
    }

    public void Expire(DateTime effTo, decimal? modifiedBy)
    {
        EffTo = effTo;
        SetAudit(modifiedBy);
    }
}
