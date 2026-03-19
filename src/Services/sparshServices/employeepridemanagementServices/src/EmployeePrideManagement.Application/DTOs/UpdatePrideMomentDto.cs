namespace EmployeePrideManagement.Application.DTOs;

public class UpdatePrideMomentDto
{
    public decimal MomentPrideId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Body { get; set; }
    public string Footer { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public long ModifiedBy { get; set; }
}
