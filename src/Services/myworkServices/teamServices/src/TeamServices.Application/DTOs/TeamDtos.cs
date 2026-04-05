namespace TeamServices.Application.DTOs;

public class TeamDto
{
    public long TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public long LastModifiedBy { get; set; }
    public DateTime LastModifiedOn { get; set; }
    public List<TeamEmployeeMapDto> EmployeeMaps { get; set; } = new();
    public List<TeamUnitMapDto> UnitMaps { get; set; } = new();
}

public class TeamEmployeeMapDto
{
    public long Id { get; set; }
    public long TeamId { get; set; }
    public long EmployeeSysId { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime? CloseDate { get; set; }
    public long LastModifiedBy { get; set; }
    public DateTime LastModifiedOn { get; set; }
}

public class TeamUnitMapDto
{
    public long MapId { get; set; }
    public long TeamId { get; set; }
    public long UnitId { get; set; }
    public string GradeCategory { get; set; } = string.Empty;
    public long? CadreId { get; set; }
    public long LastModifiedBy { get; set; }
    public DateTime LastModifiedOn { get; set; }
}
