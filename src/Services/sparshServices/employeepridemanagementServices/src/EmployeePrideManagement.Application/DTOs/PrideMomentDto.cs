namespace EmployeePrideManagement.Application.DTOs;

public class PrideMomentDto
{
    public decimal MomentPrideId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Body { get; set; }
    public decimal EmployeeSysId { get; set; }
    public string Footer { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public long ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
}
