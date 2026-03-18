namespace ObjectiveService.Domain.Entities;

/// <summary>
/// Control Point Request aggregate for requesting/modifying control points
/// </summary>
public class ControlPointRequest : BaseEntity
{
    public decimal EmployeeSysId { get; set; }
    public decimal DDYearId { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? SubmittedOn { get; set; }
    public string Status { get; set; } // P-Pending, A-Approved, R-Returned, O-Pending with other, N-Not Submitted
    public string? Remarks { get; set; }
    public char? SubordinateFlag { get; set; }

    private readonly List<ControlPointRequestDetail> _details = new();
    public IReadOnlyList<ControlPointRequestDetail> Details => _details.AsReadOnly();

    private readonly List<ControlPointRequestApproval> _approvals = new();
    public IReadOnlyList<ControlPointRequestApproval> Approvals => _approvals.AsReadOnly();

    private ControlPointRequest() { }

    public ControlPointRequest(decimal employeeSysId, decimal ddYearId)
    {
        EmployeeSysId = employeeSysId;
        DDYearId = ddYearId;
        CreatedOn = DateTime.UtcNow;
        Status = "N"; // Not Submitted

        RaiseDomainEvent(new ControlPointRequestCreatedDomainEvent(Id, employeeSysId));
    }

    public void AddDetail(ControlPointRequestDetail detail)
    {
        if (detail == null)
            throw new ArgumentNullException(nameof(detail));

        _details.Add(detail);
    }

    public void Submit()
    {
        if (_details.Count == 0)
            throw new InvalidOperationException("Cannot submit a request without details");

        Status = "P"; // Pending
        SubmittedOn = DateTime.UtcNow;

        RaiseDomainEvent(new ControlPointRequestSubmittedDomainEvent(Id, EmployeeSysId));
    }

    public void Approve(decimal approverSysId, string remarks = null)
    {
        Status = "A";
        RaiseDomainEvent(new ControlPointRequestApprovedDomainEvent(Id, EmployeeSysId, approverSysId));
    }

    public void ReturnForRevision(string remarks)
    {
        Status = "R";
        Remarks = remarks;

        RaiseDomainEvent(new ControlPointRequestReturnedDomainEvent(Id, EmployeeSysId, remarks));
    }

    public void AddApproval(ControlPointRequestApproval approval)
    {
        _approvals.Add(approval);
    }
}

/// <summary>
/// Represents a detail item in a control point request
/// </summary>
public class ControlPointRequestDetail : BaseEntity
{
    public decimal ControlPointRequestId { get; set; }
    public decimal? ControlPointId { get; set; }
    public decimal DDYearId { get; set; }
    public decimal EmployeeSysId { get; set; }
    public string Source { get; set; } // DD, CP, PC
    public decimal ReferenceId { get; set; }
    public decimal SerialNumber { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string UnitOfMeasurement { get; set; }
    public string UnitFrom { get; set; }
    public string UnitTo { get; set; }
    public decimal VersionNumber { get; set; }
    public decimal? Weightage { get; set; }
    public decimal? AccountabilityId { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string AppStatus { get; set; } // M-Modified, D-Deleted, N-Newly Added, null-Unchanged

    private ControlPointRequestDetail() { }

    public ControlPointRequestDetail(
        decimal controlPointRequestId,
        decimal ddYearId,
        decimal employeeSysId,
        string source,
        decimal referenceId,
        decimal serialNumber,
        string description,
        string category,
        string unitOfMeasurement,
        string unitFrom,
        string unitTo,
        decimal versionNumber,
        decimal? weightage = null,
        decimal? accountabilityId = null,
        string appStatus = "N")
    {
        ControlPointRequestId = controlPointRequestId;
        DDYearId = ddYearId;
        EmployeeSysId = employeeSysId;
        Source = source;
        ReferenceId = referenceId;
        SerialNumber = serialNumber;
        Description = description;
        Category = category;
        UnitOfMeasurement = unitOfMeasurement;
        UnitFrom = unitFrom;
        UnitTo = unitTo;
        VersionNumber = versionNumber;
        Weightage = weightage;
        AccountabilityId = accountabilityId;
        AppStatus = appStatus;
        ModifiedDate = DateTime.UtcNow;
    }
}

/// <summary>
/// Represents an approval in a control point request workflow
/// </summary>
public class ControlPointRequestApproval : BaseEntity
{
    public decimal ControlPointRequestId { get; set; }
    public decimal ApproverSysId { get; set; }
    public string? Status { get; set; } // A-Approved, R-Returned
    public string? Remarks { get; set; }

    private ControlPointRequestApproval() { }

    public ControlPointRequestApproval(decimal controlPointRequestId, decimal approverSysId)
    {
        ControlPointRequestId = controlPointRequestId;
        ApproverSysId = approverSysId;
    }

    public void Approve(string remarks = null)
    {
        Status = "A";
        Remarks = remarks;
    }

    public void Return(string remarks)
    {
        Status = "R";
        Remarks = remarks;
    }
}

// Domain Events
public class ControlPointRequestCreatedDomainEvent : DomainEventBase
{
    public decimal ControlPointRequestId { get; }
    public decimal EmployeeSysId { get; }

    public ControlPointRequestCreatedDomainEvent(decimal controlPointRequestId, decimal employeeSysId)
    {
        ControlPointRequestId = controlPointRequestId;
        EmployeeSysId = employeeSysId;
    }
}

public class ControlPointRequestSubmittedDomainEvent : DomainEventBase
{
    public decimal ControlPointRequestId { get; }
    public decimal EmployeeSysId { get; }

    public ControlPointRequestSubmittedDomainEvent(decimal controlPointRequestId, decimal employeeSysId)
    {
        ControlPointRequestId = controlPointRequestId;
        EmployeeSysId = employeeSysId;
    }
}

public class ControlPointRequestApprovedDomainEvent : DomainEventBase
{
    public decimal ControlPointRequestId { get; }
    public decimal EmployeeSysId { get; }
    public decimal ApproverSysId { get; }

    public ControlPointRequestApprovedDomainEvent(decimal controlPointRequestId, decimal employeeSysId, decimal approverSysId)
    {
        ControlPointRequestId = controlPointRequestId;
        EmployeeSysId = employeeSysId;
        ApproverSysId = approverSysId;
    }
}

public class ControlPointRequestReturnedDomainEvent : DomainEventBase
{
    public decimal ControlPointRequestId { get; }
    public decimal EmployeeSysId { get; }
    public string Remarks { get; }

    public ControlPointRequestReturnedDomainEvent(decimal controlPointRequestId, decimal employeeSysId, string remarks)
    {
        ControlPointRequestId = controlPointRequestId;
        EmployeeSysId = employeeSysId;
        Remarks = remarks;
    }
}
