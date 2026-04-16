namespace SettingsService.Application.DTOs;

public record UserDto(
    int UserId, string Username, string Email, string FirstName, string LastName, bool IsActive,
    DateTime? LastLoginDate, DateTime CreatedDate, DateTime ModifiedDate, string? Phone,
    string? Position, string? Department, string? TimeZone, string? Language,
    bool IsEmailVerified, bool TwoFactorEnabled);

public record CreateUserDto(
    string Username, string Email, string FirstName, string LastName, string Password,
    string? Phone, string? Position, string? Department, string? TimeZone, string? Language, int? CreatedBy);

public record UpdateUserDto(
    int UserId, string Username, string Email, string FirstName, string LastName, bool IsActive,
    string? Phone, string? Position, string? Department, string? TimeZone, string? Language, int? ModifiedBy);

public record RoleDto(
    int RoleId, string RoleName, string RoleCode, string? Description, bool IsActive,
    bool IsSystemRole, string? Permissions);

public record CreateRoleDto(
    string RoleName, string RoleCode, string? Description, bool IsSystemRole, string? Permissions, int? CreatedBy);

public record UserPreferenceDto(
    int UserPreferenceId, int UserId, string PreferenceKey, string? PreferenceValue,
    string PreferenceType, string? Category, bool IsActive);

public record SetUserPreferenceDto(
    int UserId, string PreferenceKey, string? PreferenceValue, string PreferenceType, string? Category, int? ModifiedBy);
