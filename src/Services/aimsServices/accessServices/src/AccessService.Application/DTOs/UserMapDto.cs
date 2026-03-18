namespace AccessService.Application.DTOs;

/// <summary>
/// DTOs for UserMap entity
/// </summary>

public class CreateUserMapDto
{
    public long EmployeeSystemId { get; set; }
}

public class UpdateUserMapDto
{
    public DateTime? EffectiveDate { get; set; }
    
    public DateTime? ClosureDate { get; set; }
}

public class UserMapDto
{
    public long EmployeeSystemId { get; set; }
    
    public DateTime? EffectiveDate { get; set; }
    
    public DateTime? ClosureDate { get; set; }
    
    public long? ModifiedBy { get; set; }
    
    public DateTime? ModifiedOn { get; set; }
    
    public bool IsActive { get; set; }
}
