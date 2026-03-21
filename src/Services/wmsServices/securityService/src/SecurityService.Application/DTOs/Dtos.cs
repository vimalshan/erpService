namespace SecurityService.Application.DTOs;

public record UserDto(int UserId, string Username, string Email, string FullName, bool IsActive, DateTime CreatedDate, DateTime? LastLogin, List<string> Roles);
public record RoleDto(int RoleId, string RoleName, string? Description, List<string> Permissions);
public record PermissionDto(int PermissionId, string PermissionName, string? Module, string? Description);
public record LoginResponseDto(string Token, string Username, string Email, List<string> Roles, DateTime Expiration);
public record UserCreateDto(string Username, string Password, string Email, string FullName);
public record UserUpdateDto(int UserId, string Email, string FullName, bool IsActive);
public record RoleCreateDto(string RoleName, string? Description);
public record RoleUpdateDto(int RoleId, string RoleName, string? Description);
public record PermissionCreateDto(string PermissionName, string? Module, string? Description);
public record PermissionUpdateDto(int PermissionId, string PermissionName, string? Module, string? Description);
public record AssignRoleDto(int UserId, int RoleId);
public record AssignPermissionDto(int RoleId, int PermissionId);
public record LoginDto(string Username, string Password);
