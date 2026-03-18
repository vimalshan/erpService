using TeamServices.Domain.Common;
using TeamServices.Domain.ValueObjects;

namespace TeamServices.Domain.Entities;

public class TeamUnitMap : BaseEntity
{
    public long TeamId { get; private set; }
    public long UnitId { get; private set; }
    public char GradeCategory { get; private set; }
    public long? CadreId { get; private set; }

    public TeamMaster? Team { get; private set; }

    private TeamUnitMap() { }

    public TeamUnitMap(long mapId, long teamId, long unitId, char gradeCategory, long? cadreId, long modifiedBy)
    {
        Id = mapId;
        TeamId = teamId;
        UnitId = unitId;
        GradeCategory = gradeCategory;
        CadreId = cadreId;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }

    public void UpdateGradeCategory(char newCategory, long modifiedBy)
    {
        GradeCategory = newCategory;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }

    public void UpdateCadre(long? cadreId, long modifiedBy)
    {
        CadreId = cadreId;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
