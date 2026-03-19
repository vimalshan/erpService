using MediatR;
using SecurityService.Application.DTOs;

namespace SecurityService.Application.Queries;

// ── UserMasterMap ─────────────────────────────────────────────────────────
public record GetUserMapsQuery() : IRequest<IEnumerable<UserMasterMapDto>>;
public record GetUserMapsByUserQuery(long UserId) : IRequest<IEnumerable<UserMasterMapDto>>;
public record GetUserMapByIdQuery(long MapId) : IRequest<UserMasterMapDto?>;

// ── Paginated / Search Users ──────────────────────────────────────────────
public record SearchUsersQuery(
    string? SearchTerm,
    int Page = 1,
    int PageSize = 20,
    bool ActiveOnly = true) : IRequest<PagedResult<UserListDto>>;

// ── Access tree ───────────────────────────────────────────────────────────
public record GetUserAccessTreeQuery(long UserId) : IRequest<UserAccessTreeDto>;

// ── Security Stats ────────────────────────────────────────────────────────
public record GetSecurityStatsQuery() : IRequest<SecurityStatsDto>;
