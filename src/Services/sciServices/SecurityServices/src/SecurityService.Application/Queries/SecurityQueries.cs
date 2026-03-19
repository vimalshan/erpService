using MediatR;
using SecurityService.Application.DTOs;

namespace SecurityService.Application.Queries;

// ── Users ────────────────────────────────────────────────────
public record GetUserByIdQuery(long UserId) : IRequest<UserDto?>;
public record GetUserByCodeQuery(string UserCode) : IRequest<UserDto?>;
public record GetAllUsersQuery(bool ActiveOnly = false) : IRequest<IEnumerable<UserListDto>>;

// ── Roles ────────────────────────────────────────────────────
public record GetAllRolesQuery() : IRequest<IEnumerable<RoleDto>>;
public record GetRoleByIdQuery(long RoleId) : IRequest<RoleDto?>;

// ── User Roles ───────────────────────────────────────────────
public record GetUserRolesQuery(long UserId) : IRequest<IEnumerable<UserRoleDto>>;

// ── Menus ───────────────────────────────────────────────────
public record GetAllMenusQuery() : IRequest<IEnumerable<MenuDto>>;
public record GetMenusByRoleQuery(long RoleId) : IRequest<IEnumerable<MenuDto>>;
