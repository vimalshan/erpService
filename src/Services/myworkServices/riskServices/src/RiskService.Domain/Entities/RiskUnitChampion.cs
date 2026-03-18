using RiskService.Domain.Common;

namespace RiskService.Domain.Entities;

public class RiskUnitChampion : BaseEntity
{
    public long EmployeeSysId { get; set; }
    public char ChampionType { get; set; }  // O/B/S/U/A
    public long OrganizationId { get; set; }
    public long BusinessId { get; set; }
    public long DivisionId { get; set; }
    public long UnitId { get; set; }
}
