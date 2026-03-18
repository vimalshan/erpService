namespace AuthProvider.Application.DTOs;

public record UserDto(
    Guid Id,
    string Username,
    string Email,
    string FirstName,
    string LastName,
    bool IsActive,
    bool IsEmailVerified,
    DateTime CreatedAt,
    DateTime? LastLoginAt,
    IEnumerable<string> Roles);

public record CreateUserDto(
    string Username,
    string Email,
    string Password,
    string FirstName,
    string LastName);

public record UpdateUserDto(
    Guid Id,
    string FirstName,
    string LastName);

public record LoginRequestDto(string UsernameOrEmail, string Password);

public record TokenResponseDto(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    string TokenType = "Bearer");

public record AssignRoleDto(Guid UserId, string RoleName);

public record PagedResult<T>(IEnumerable<T> Items, int TotalCount, int Page, int PageSize);
