namespace AccessService.API.GraphQL.Inputs;

/// <summary>Input for creating a new UserMap</summary>
public class CreateUserMapInput
{
    public long EmployeeSystemId { get; set; }
}

/// <summary>Input for activating a UserMap</summary>
public class ActivateUserMapInput
{
    public long EmployeeSystemId { get; set; }
    public DateTime EffectiveDate { get; set; }
}

/// <summary>Input for deactivating (closing) a UserMap</summary>
public class DeactivateUserMapInput
{
    public long EmployeeSystemId { get; set; }
    public DateTime ClosureDate { get; set; }
}
