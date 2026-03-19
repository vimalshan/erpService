using MediatR;
using SecurityService.Application.DTOs;

namespace SecurityService.Application.Commands.Users;

// ── Create UserMasterMap ───────────────────────────────────────────────────
public record CreateUserMapCommand(
    long UserId,
    string DepartmentCode,
    DateTime StartDate,
    DateTime? EndDate) : IRequest<UserMasterMapDto>;

// ── Update UserMasterMap ───────────────────────────────────────────────────
public record UpdateUserMapCommand(
    long MapId,
    string DepartmentCode,
    DateTime StartDate,
    DateTime? EndDate) : IRequest<UserMasterMapDto>;

// ── Delete UserMasterMap ───────────────────────────────────────────────────
public record DeleteUserMapCommand(long MapId) : IRequest<bool>;

// ── Assign Menu to Role ────────────────────────────────────────────────────
public record AssignMenuToRoleCommand(
    long RoleId,
    long MenuId,
    string AssignedBy,
    long AssignedByNum) : IRequest<bool>;

// ── Unassign Menu from Role ────────────────────────────────────────────────
public record UnassignMenuFromRoleCommand(
    long RoleId,
    long MenuId) : IRequest<bool>;
