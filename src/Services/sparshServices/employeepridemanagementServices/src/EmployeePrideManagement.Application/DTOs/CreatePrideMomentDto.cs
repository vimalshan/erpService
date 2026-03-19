namespace EmployeePrideManagement.Application.DTOs;

public class CreatePrideMomentDto
{
    public string Title { get; set; } = string.Empty;
    public string? Body { get; set; }
    public decimal EmployeeSysId { get; set; }
    public string Footer { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public long ModifiedBy { get; set; }
}
