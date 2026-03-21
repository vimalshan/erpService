namespace EmployeeService.Application.DTOs;

public sealed record EmployeeDto
{
    public int EmployeeId { get; init; }
    public int? UserId { get; init; }
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string EmployeeCode { get; init; } = null!;
    public DateTime HireDate { get; init; }
    public string? JobTitle { get; init; }
    public string? Department { get; init; }
    public int? WarehouseId { get; init; }
    public string? Phone { get; init; }
    public string? Email { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime ModifiedDate { get; init; }
}
