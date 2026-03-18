namespace PayrollServices.Application.DTOs;

/// <summary>
/// DTO for Payroll Batch
/// </summary>
public class PayrollBatchDto
{
    public long BatchId { get; set; }
    public string BatchMonth { get; set; } = null!;
    public string Status { get; set; } = null!;
    public long CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public long? UpdatedBy { get; set; }
}
