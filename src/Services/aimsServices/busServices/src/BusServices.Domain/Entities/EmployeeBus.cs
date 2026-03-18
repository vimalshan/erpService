using BusServices.Domain.Common;
using BusServices.Domain.Events;
using BusServices.Domain.Exceptions;

namespace BusServices.Domain.Entities;

/// <summary>Maps to EMPLOYEE_BUS table. Represents assignment of an employee to a bus/route.</summary>
public sealed class EmployeeBus : BaseEntity
{
    public long EmpBusId { get; private set; }
    public long EmpSysId { get; private set; }
    public int BusId { get; private set; }
    public int RouteId { get; private set; }
    public DateTime EffectiveDate { get; private set; }
    public DateTime? ClosingDate { get; private set; }
    public long LastModifiedBy { get; private set; }
    public DateTime LastModifiedOn { get; private set; }

    private EmployeeBus() { }

    public static EmployeeBus Assign(
        long empBusId,
        long empSysId,
        int busId,
        int routeId,
        long assignedBy)
    {
        if (empSysId <= 0) throw new DomainException("Invalid employee system ID.");
        if (busId <= 0) throw new DomainException("Invalid bus ID.");
        if (routeId <= 0) throw new DomainException("Invalid route ID.");

        var assignment = new EmployeeBus
        {
            EmpBusId = empBusId,
            EmpSysId = empSysId,
            BusId = busId,
            RouteId = routeId,
            EffectiveDate = DateTime.UtcNow,
            LastModifiedBy = assignedBy,
            LastModifiedOn = DateTime.UtcNow
        };

        assignment.AddDomainEvent(new EmployeeAssignedToBusEvent(empBusId, empSysId, busId, routeId, assignedBy));
        return assignment;
    }

    public void Close(DateTime closingDate, long modifiedBy)
    {
        if (closingDate < EffectiveDate)
            throw new DomainException("Closing date cannot be before effective date.");

        ClosingDate = closingDate;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
