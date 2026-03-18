namespace HRService.Application.DTOs;

public class DepartmentDto
{
    public Guid DepartmentId { get; set; }
    public string DepartmentCode { get; set; }
    public string DepartmentName { get; set; }
    public string? Description { get; set; }
    public Guid? ManagerId { get; set; }
    public string? ManagerName { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}

public class CreateDepartmentDto
{
    public string DepartmentCode { get; set; }
    public string DepartmentName { get; set; }
    public string? Description { get; set; }
}
