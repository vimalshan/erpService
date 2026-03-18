using AuthProvider.Application.DTOs;
using MediatR;

namespace AuthProvider.Application.Commands;

/// <summary>CQRS Command – creates a new user (write side).</summary>
public record CreateUserCommand(
    string Username,
    string Email,
    string Password,
    string FirstName,
    string LastName) : IRequest<UserDto>;

/// <summary>CQRS Command – updates user profile.</summary>
public record UpdateUserCommand(
    Guid UserId,
    string FirstName,
    string LastName) : IRequest<UserDto>;

/// <summary>CQRS Command – soft-deletes / deactivates a user.</summary>
public record DeleteUserCommand(Guid UserId) : IRequest<bool>;

/// <summary>CQRS Command – assigns a role to a user.</summary>
public record AssignRoleCommand(Guid UserId, string RoleName) : IRequest<bool>;

/// <summary>CQRS Command – authenticates a user and returns JWT tokens.</summary>
public record LoginCommand(string UsernameOrEmail, string Password, string IpAddress) : IRequest<TokenResponseDto>;

/// <summary>CQRS Command – refreshes an access token.</summary>
public record RefreshTokenCommand(string RefreshToken, string IpAddress) : IRequest<TokenResponseDto>;

/// <summary>CQRS Command – revokes a refresh token.</summary>
public record RevokeTokenCommand(string RefreshToken, string IpAddress) : IRequest<bool>;
