namespace ObjectiveService.Domain.Entities;

/// <summary>
/// Control Point entity representing performance metrics/KPIs
/// </summary>
public class ControlPoint : BaseEntity
{
    public decimal EmployeeSysId { get; set; }
    public decimal DDYearId { get; set; }
    public string Source { get; set; } // DD, CP, PC
    public decimal RefId { get; set; }
    public decimal SerialNumber { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string UnitOfMeasurement { get; set; }
    public string UnitFrom { get; set; }
    public string UnitTo { get; set; }
    public decimal VersionNumber { get; set; }
    public decimal? Weightage { get; set; }
    public decimal? AccountabilityId { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string Status { get; set; } // A-Active, D-Deleted, M-Modified

    private ControlPoint() { }

    public ControlPoint(
        decimal employeeSysId,
        decimal ddYearId,
        string source,
        decimal refId,
        decimal serialNumber,
        string description,
        string category,
        string unitOfMeasurement,
        string unitFrom,
        string unitTo,
        decimal versionNumber,
        decimal? weightage = null,
        decimal? accountabilityId = null)
    {
        EmployeeSysId = employeeSysId;
        DDYearId = ddYearId;
        Source = source;
        RefId = refId;
        SerialNumber = serialNumber;
        Description = description;
        Category = category;
        UnitOfMeasurement = unitOfMeasurement;
        UnitFrom = unitFrom;
        UnitTo = unitTo;
        VersionNumber = versionNumber;
        Weightage = weightage;
        AccountabilityId = accountabilityId;
        Status = "A";
        ModifiedDate = DateTime.UtcNow;

        RaiseDomainEvent(new ControlPointCreatedDomainEvent(Id, employeeSysId, description));
    }

    public void Update(string description, string unitFrom, string unitTo, decimal? weightage = null)
    {
        Description = description;
        UnitFrom = unitFrom;
        UnitTo = unitTo;
        Weightage = weightage;
        ModifiedDate = DateTime.UtcNow;
        VersionNumber++;

        RaiseDomainEvent(new ControlPointModifiedDomainEvent(Id, description));
    }

    public void Delete()
    {
        Status = "D";
        ModifiedDate = DateTime.UtcNow;

        RaiseDomainEvent(new ControlPointDeletedDomainEvent(Id));
    }
}

public class ControlPointCreatedDomainEvent : DomainEventBase
{
    public decimal ControlPointId { get; }
    public decimal EmployeeSysId { get; }
    public string Description { get; }

    public ControlPointCreatedDomainEvent(decimal controlPointId, decimal employeeSysId, string description)
    {
        ControlPointId = controlPointId;
        EmployeeSysId = employeeSysId;
        Description = description;
    }
}

public class ControlPointModifiedDomainEvent : DomainEventBase
{
    public decimal ControlPointId { get; }
    public string Description { get; }

    public ControlPointModifiedDomainEvent(decimal controlPointId, string description)
    {
        ControlPointId = controlPointId;
        Description = description;
    }
}

public class ControlPointDeletedDomainEvent : DomainEventBase
{
    public decimal ControlPointId { get; }

    public ControlPointDeletedDomainEvent(decimal controlPointId)
    {
        ControlPointId = controlPointId;
    }
}
