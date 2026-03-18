namespace PayrollServices.Application.DTOs;

/// <summary>
/// DTO for Payroll Adjustment (Allowances, Deductions, Arrears)
/// </summary>
public class PayrollAdjustmentDto
{
    public long AdjustmentId { get; set; }
    public long EmployeeSystemId { get; set; }
    public decimal Amount { get; set; }
    public string AdjustmentType { get; set; } = null!;
    public DateTime AdjustmentDate { get; set; }
    public string? Description { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ApprovedOn { get; set; }
    public long? ApprovedBy { get; set; }
}
