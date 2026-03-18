namespace PayrollServices.Domain.Entities;

/// <summary>
/// Represents payroll adjustments (allowances, arrears, deductions)
/// Maps to PAY_ARR table
/// </summary>
public class PayrollAdjustment : BaseEntity
{
    public long AdjustmentId { get; init; }
    public long EmployeeSystemId { get; set; }
    public decimal Amount { get; set; }
    public AdjustmentType AdjustmentType { get; set; }
    public DateTime AdjustmentDate { get; set; }
    public string? Description { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ApprovedOn { get; set; }
    public long? ApprovedBy { get; set; }

    public static PayrollAdjustment Create(
        long adjustmentId,
        long employeeSystemId,
        decimal amount,
        AdjustmentType adjustmentType,
        string? description,
        long createdBy)
    {
        if (amount < 0 && adjustmentType != AdjustmentType.Deduction)
        {
            throw new ArgumentException("Only deductions can have negative amounts.");
        }

        var adjustment = new PayrollAdjustment
        {
            AdjustmentId = adjustmentId,
            EmployeeSystemId = employeeSystemId,
            Amount = Math.Abs(amount),
            AdjustmentType = adjustmentType,
            AdjustmentDate = DateTime.UtcNow,
            Description = description,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };

        return adjustment;
    }

    public void Approve(long approvedBy)
    {
        ApprovedBy = approvedBy;
        ApprovedOn = DateTime.UtcNow;
    }
}

/// <summary>
/// Type of payroll adjustment
/// </summary>
public enum AdjustmentType
{
    Allowance = 'A',
    Deduction = 'D',
    Arrear = 'R'
}
