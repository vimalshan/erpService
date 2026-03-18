using TeamServices.Domain.Common;
using TeamServices.Domain.Events;

namespace TeamServices.Domain.Entities;

public class TeamMaster : BaseEntity, IAggregateRoot
{
    public string TeamName { get; private set; } = string.Empty;

    private readonly List<TeamEmployeeMap> _employeeMaps = new();
    public IReadOnlyCollection<TeamEmployeeMap> EmployeeMaps => _employeeMaps.AsReadOnly();

    private readonly List<TeamUnitMap> _unitMaps = new();
    public IReadOnlyCollection<TeamUnitMap> UnitMaps => _unitMaps.AsReadOnly();

    private TeamMaster() { }

    public TeamMaster(long teamId, string teamName, long modifiedBy)
    {
        Id = teamId;
        TeamName = teamName ?? throw new ArgumentNullException(nameof(teamName));
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;

        AddDomainEvent(new TeamCreatedEvent(teamId, teamName));
    }

    public void UpdateName(string newName, long modifiedBy)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Team name cannot be empty.", nameof(newName));

        var oldName = TeamName;
        TeamName = newName;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;

        AddDomainEvent(new TeamUpdatedEvent(Id, oldName, newName));
    }

    public void AddEmployee(TeamEmployeeMap employeeMap)
    {
        _employeeMaps.Add(employeeMap);
        LastModifiedOn = DateTime.UtcNow;
        AddDomainEvent(new TeamMemberAddedEvent(Id, employeeMap.EmployeeSysId));
    }

    public void RemoveEmployee(long employeeMapId)
    {
        var emp = _employeeMaps.FirstOrDefault(e => e.Id == employeeMapId);
        if (emp != null)
        {
            _employeeMaps.Remove(emp);
            LastModifiedOn = DateTime.UtcNow;
            AddDomainEvent(new TeamMemberRemovedEvent(Id, emp.EmployeeSysId));
        }
    }

    public void AddUnitMap(TeamUnitMap unitMap)
    {
        _unitMaps.Add(unitMap);
        LastModifiedOn = DateTime.UtcNow;
    }
}
