namespace HRService.Application.DTOs;

public class SalaryDto
{
    public Guid SalaryId { get; set; }
    public Guid EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public DateTime EffectiveDate { get; set; }
    public decimal TotalBaseSalary { get; set; }
    public string Status { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class UpdateSalaryDto
{
    public Guid EmployeeId { get; set; }
    public DateTime EffectiveDate { get; set; }
    public decimal NewSalary { get; set; }
}
