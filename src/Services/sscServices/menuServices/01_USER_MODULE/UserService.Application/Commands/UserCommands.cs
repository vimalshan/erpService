using MediatR;
using UserService.Application.DTOs;

namespace UserService.Application.Commands;

/// <summary>
/// Create User command
/// </summary>
public class CreateUserCommand : IRequest<long>
{
    public required string UserName { get; init; }
    public required string Password { get; init; }
    public required string EmailId { get; init; }
    public required long EnteredBy { get; init; }
    public string? SparchUserId { get; init; }
    public long? HrEmpSysId { get; init; }
}

/// <summary>
/// Update User command
/// </summary>
public class UpdateUserCommand : IRequest<bool>
{
    public long UserId { get; init; }
    public required string UserName { get; init; }
    public required string EmailId { get; init; }
    public string? SparchUserId { get; init; }
}

/// <summary>
/// Deactivate User command
/// </summary>
public class DeactivateUserCommand : IRequest<bool>
{
    public long UserId { get; init; }
}

/// <summary>
/// Assign Role to User command
/// </summary>
public class AssignRoleToUserCommand : IRequest<bool>
{
    public long UserId { get; init; }
    public long RoleId { get; init; }
    public bool IsDefault { get; init; }
}

/// <summary>
/// Assign Organization to User command
/// </summary>
public class AssignOrganizationToUserCommand : IRequest<bool>
{
    public long UserId { get; init; }
    public required string BusinessUnitId { get; init; }
}

/// <summary>
/// Assign Location to User command
/// </summary>
public class AssignLocationToUserCommand : IRequest<bool>
{
    public long UserId { get; init; }
    public int LocationId { get; init; }
}

/// <summary>
/// Login User command
/// </summary>
public class LoginUserCommand : IRequest<LoginResponse>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}
