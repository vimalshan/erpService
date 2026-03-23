namespace AccessService.API.GraphQL.Payloads;

using AccessService.API.GraphQL.Types;

/// <summary>Payload returned after a mutation that produces no meaningful return value</summary>
public class MutationPayload
{
    public bool Success { get; init; }
    public string? Message { get; init; }

    public static MutationPayload Ok(string? message = null) =>
        new() { Success = true, Message = message ?? "Operation completed successfully." };

    public static MutationPayload Fail(string message) =>
        new() { Success = false, Message = message };
}

/// <summary>Payload returned after creating a UserMap</summary>
public class CreateUserMapPayload
{
    public bool Success { get; init; }
    public Guid? Id { get; init; }
    public string? Message { get; init; }

    public static CreateUserMapPayload Ok(Guid id) =>
        new() { Success = true, Id = id, Message = "UserMap created successfully." };

    public static CreateUserMapPayload Fail(string message) =>
        new() { Success = false, Id = null, Message = message };
}

/// <summary>Payload returned after assigning a UserRole</summary>
public class AssignUserRolePayload
{
    public bool Success { get; init; }
    public int? RoleId { get; init; }
    public string? Message { get; init; }

    public static AssignUserRolePayload Ok(int roleId) =>
        new() { Success = true, RoleId = roleId, Message = "Role assigned successfully." };

    public static AssignUserRolePayload Fail(string message) =>
        new() { Success = false, RoleId = null, Message = message };
}
