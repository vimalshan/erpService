using EmployeeRelations.Domain.Common;
using EmployeeRelations.Domain.Events;

namespace EmployeeRelations.Domain.Aggregates;

/// <summary>Aggregate root for a disciplinary case.</summary>
public class DisciplinaryMain : AggregateRoot
{
    public long UnitId { get; private set; }
    public DateTime Date { get; private set; }
    public string Details { get; private set; } = string.Empty;
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long? ModifiedBy { get; private set; }
    public DateTime? ModifiedOn { get; private set; }

    private readonly List<DisciplinaryEmp> _employees = new();
    private readonly List<DisciplinaryAction> _actions = new();

    public IReadOnlyCollection<DisciplinaryEmp> Employees => _employees.AsReadOnly();
    public IReadOnlyCollection<DisciplinaryAction> Actions => _actions.AsReadOnly();

    protected DisciplinaryMain() { }

    public DisciplinaryMain(long id, long unitId, DateTime date, string details, long createdBy)
    {
        Id = id;
        UnitId = unitId;
        Date = date;
        Details = details;
        CreatedBy = createdBy;
        CreatedOn = DateTime.UtcNow;
        AddDomainEvent(new DisciplinaryCaseCreatedEvent(id, unitId, date));
    }

    public void AddEmployee(long empSysId) => _employees.Add(new DisciplinaryEmp(Id, empSysId));

    public void AddAction(long actionId, long empSysId, long typeId, DateTime actionDate, string remarks, long createdBy)
    {
        var action = new DisciplinaryAction(actionId, Id, empSysId, typeId, actionDate, remarks, createdBy);
        _actions.Add(action);
        AddDomainEvent(new DisciplinaryActionAddedEvent(actionId, Id, empSysId, actionDate));
    }

    public void Update(string details, long modifiedBy)
    {
        Details = details;
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
    }
}
