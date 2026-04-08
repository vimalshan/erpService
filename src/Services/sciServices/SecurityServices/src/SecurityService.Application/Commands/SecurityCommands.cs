using MediatR;
using SecurityService.Application.DTOs;

namespace SecurityService.Application.Commands.Users;

// ── Create User ──────────────────────────────────────────────
public record CreateUserCommand(
    long UserId,
    string UserCode,
    string? UserName,
    string? Email,
    long? Phone,
    DateTime StartDate,
    string? UserType,
    string? CreatedBy) : IRequest<UserDto>;

// ── Update User ──────────────────────────────────────────────
public record UpdateUserCommand(
    long UserId,
    string? UserName,
    string? Email,
    long? Phone,
    string? UserType,
    string UpdatedBy,
    long UpdatedByNum) : IRequest<UserDto>;

// ── Deactivate User ──────────────────────────────────────────
public record DeactivateUserCommand(long UserId, DateTime EndDate) : IRequest<bool>;

// ── Assign Role ──────────────────────────────────────────────
public record AssignRoleCommand(
    long UserId,
    long RoleId,
    DateTime StartDate,
    DateTime? EndDate,
    string AssignedBy) : IRequest<bool>;

// ── Revoke Role ──────────────────────────────────────────────
public record RevokeRoleCommand(long UserId, long RoleId) : IRequest<bool>;

// ── Create Role ──────────────────────────────────────────────
public record CreateRoleCommand(
    long RoleId,
    string RoleName,
    string? CreatedBy) : IRequest<RoleDto>;

// ── Update Role ──────────────────────────────────────────────
public record UpdateRoleCommand(
    long RoleId,
    string RoleName,
    string UpdatedBy,
    long UpdatedByNum) : IRequest<RoleDto>;
