using System;
using EmployeeService.Domain.ValueObjects;

namespace EmployeeService.Domain.Events;

/// <summary>
/// Domain event raised when employee CTC is incremented
/// </summary>
public class EmployeeCTCIncrementedEvent : Common.DomainEvent
{
    public long EmployeeSystemId { get; }
    public Money OldCTC { get; }
    public Money NewCTC { get; }
    public Percentage IncrementPercentage { get; }
    public DateTime EffectiveDate { get; }
    public long ApprovedBy { get; }

    public EmployeeCTCIncrementedEvent(
        long employeeSystemId,
        Money oldCtc,
        Money newCtc,
        Percentage incrementPercentage,
        DateTime effectiveDate,
        long approvedBy)
    {
        EmployeeSystemId = employeeSystemId;
        OldCTC = oldCtc;
        NewCTC = newCtc;
        IncrementPercentage = incrementPercentage;
        EffectiveDate = effectiveDate;
        ApprovedBy = approvedBy;
    }
}

/// <summary>
/// Domain event raised when employee CTC increment is rejected
/// </summary>
public class EmployeeCTCIncrementRejectedEvent : Common.DomainEvent
{
    public long EmployeeSystemId { get; }
    public Percentage ProposedIncrementPercentage { get; }
    public string Reason { get; }
    public long RejectedBy { get; }

    public EmployeeCTCIncrementRejectedEvent(
        long employeeSystemId,
        Percentage proposedIncrementPercentage,
        string reason,
        long rejectedBy)
    {
        EmployeeSystemId = employeeSystemId;
        ProposedIncrementPercentage = proposedIncrementPercentage;
        Reason = reason;
        RejectedBy = rejectedBy;
    }
}

/// <summary>
/// Domain event raised when employee CTC is modified
/// </summary>
public class EmployeeCTCModifiedEvent : Common.DomainEvent
{
    public long EmployeeSystemId { get; }
    public Money OldGrossCTC { get; }
    public Money NewGrossCTC { get; }
    public Money OldBasicSalary { get; }
    public Money NewBasicSalary { get; }
    public DateTime EffectiveDate { get; }
    public string Reason { get; }

    public EmployeeCTCModifiedEvent(
        long employeeSystemId,
        Money oldGrossCTC,
        Money newGrossCTC,
        Money oldBasicSalary,
        Money newBasicSalary,
        DateTime effectiveDate,
        string reason)
    {
        EmployeeSystemId = employeeSystemId;
        OldGrossCTC = oldGrossCTC;
        NewGrossCTC = newGrossCTC;
        OldBasicSalary = oldBasicSalary;
        NewBasicSalary = newBasicSalary;
        EffectiveDate = effectiveDate;
        Reason = reason;
    }
}
