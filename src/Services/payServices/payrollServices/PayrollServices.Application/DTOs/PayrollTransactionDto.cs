namespace PayrollServices.Application.DTOs;

/// <summary>
/// DTO for Payroll Transaction
/// </summary>
public class PayrollTransactionDto
{
    public long TransactionId { get; set; }
    public long EmployeeSystemId { get; set; }
    public long BatchId { get; set; }
    public string Month { get; set; } = null!;
    public decimal GrossSalary { get; set; }
    public decimal Deductions { get; set; }
    public decimal NetSalary { get; set; }
    public string Status { get; set; } = null!;
    public long CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
}
