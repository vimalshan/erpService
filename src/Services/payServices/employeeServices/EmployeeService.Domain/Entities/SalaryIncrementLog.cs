using System;
using EmployeeService.Domain.Common;
using EmployeeService.Domain.ValueObjects;

namespace EmployeeService.Domain.Entities;

/// <summary>
/// Entity to track salary increment history
/// </summary>
public class SalaryIncrementLog : BaseEntity
{
    public long EmployeeSystemId { get; private set; }
    public Money OldCTC { get; private set; } = new(0);
    public Money NewCTC { get; private set; } = new(0);
    public Percentage IncrementPercentage { get; private set; } = new(0);
    public DateTime EffectiveDate { get; private set; }
    public long ApprovedBy { get; private set; }
    public DateTime ApprovedOn { get; private set; }
    public string? ApprovalComments { get; private set; }
    public string Status { get; private set; } = "Approved"; // Approved, Pending, Rejected

    private SalaryIncrementLog() { }

    /// <summary>
    /// Create a new salary increment log entry
    /// </summary>
    public SalaryIncrementLog(
        long employeeSystemId,
        Money oldCTC,
        Money newCTC,
        Percentage incrementPercentage,
        DateTime effectiveDate,
        long approvedBy,
        string? approvalComments = null)
    {
        if (employeeSystemId <= 0)
            throw new ArgumentException("Employee System ID must be greater than 0", nameof(employeeSystemId));

        if (oldCTC.Amount < 0 || newCTC.Amount < 0)
            throw new ArgumentException("CTC amounts cannot be negative");

        EmployeeSystemId = employeeSystemId;
        OldCTC = oldCTC;
        NewCTC = newCTC;
        IncrementPercentage = incrementPercentage;
        EffectiveDate = effectiveDate;
        ApprovedBy = approvedBy;
        ApprovedOn = DateTime.UtcNow;
        ApprovalComments = approvalComments;
        Status = "Approved";
    }

    /// <summary>
    /// Calculate the absolute increment amount
    /// </summary>
    public Money GetIncrementAmount()
    {
        return new Money(NewCTC.Amount - OldCTC.Amount);
    }

    /// <summary>
    /// Get the increment as a percentage of old CTC
    /// </summary>
    public Percentage GetActualIncrementPercentage()
    {
        if (OldCTC.Amount == 0)
            return new Percentage(0);

        var percentageValue = ((NewCTC.Amount - OldCTC.Amount) / OldCTC.Amount) * 100;
        return new Percentage(percentageValue);
    }

    /// <summary>
    /// Mark this increment as rejected
    /// </summary>
    public void MarkAsRejected(long rejectedBy, string reason)
    {
        Status = "Rejected";
        ApprovedBy = rejectedBy;
        ApprovedOn = DateTime.UtcNow;
        ApprovalComments = reason;
    }

    /// <summary>
    /// Mark this increment as pending approval
    /// </summary>
    public void MarkAsPending()
    {
        Status = "Pending";
    }
}
