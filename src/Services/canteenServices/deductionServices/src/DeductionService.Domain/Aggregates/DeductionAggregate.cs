using DeductionService.Domain.Common;
using DeductionService.Domain.Entities;
using DeductionService.Domain.Events;
using DeductionService.Domain.Exceptions;
using DeductionService.Domain.ValueObjects;

namespace DeductionService.Domain.Aggregates;

/// <summary>
/// Aggregate root that manages the lifecycle of payroll deductions for an employee.
/// </summary>
public class DeductionAggregate : BaseEntity
{
    private readonly List<AdhocPayDeduction> _deductions = [];
    private readonly List<AdhocPayDeductionHistory> _history = [];

    public long EmployeeSystemId { get; private set; }
    public string? CompanyCode { get; private set; }
    public long? CanteenUnit { get; private set; }

    public IReadOnlyCollection<AdhocPayDeduction> Deductions => _deductions.AsReadOnly();
    public IReadOnlyCollection<AdhocPayDeductionHistory> History => _history.AsReadOnly();

    private DeductionAggregate() { }

    public static DeductionAggregate Create(long employeeSystemId, string companyCode, long canteenUnit)
    {
        return new DeductionAggregate
        {
            EmployeeSystemId = employeeSystemId,
            CompanyCode = companyCode,
            CanteenUnit = canteenUnit
        };
    }

    public AdhocPayDeduction AddDeduction(
        long systemId,
        decimal payAmount,
        string earningDeductionCode,
        long enteredByUserId)
    {
        var deduction = AdhocPayDeduction.Create(
            systemId,
            CanteenUnit,
            payAmount,
            earningDeductionCode,
            EmployeeSystemId,
            enteredByUserId);

        _deductions.Add(deduction);
        AddDomainEvent(new DeductionCreatedEvent(systemId, EmployeeSystemId, payAmount));
        return deduction;
    }

    public void ProcessMonthlyDeductions(MonthYear period, long processedByUserId)
    {
        var activeDeductions = _deductions.Where(d => d.CancelFlag != "Y").ToList();

        foreach (var deduction in activeDeductions)
        {
            var historyEntry = AdhocPayDeductionHistory.CreateFromDeduction(deduction);
            _history.Add(historyEntry);
        }

        AddDomainEvent(new MonthlyDeductionProcessedEvent(
            EmployeeSystemId,
            period.ToString(),
            activeDeductions.Sum(d => d.PayAmount ?? 0),
            processedByUserId));
    }
}
