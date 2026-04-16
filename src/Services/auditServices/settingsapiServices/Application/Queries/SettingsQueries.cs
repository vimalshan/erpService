using SettingsService.Application.DTOs;
using SettingsService.Domain.Interfaces;
using MediatR;

namespace SettingsService.Application.Queries;

public record GetUserByIdQuery(int UserId) : IRequest<UserDto?>;
public record GetAllUsersQuery() : IRequest<IEnumerable<UserDto>>;
public record GetAllRolesQuery() : IRequest<IEnumerable<RoleDto>>;
public record GetUserPreferencesQuery(int UserId) : IRequest<IEnumerable<UserPreferenceDto>>;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly ISettingsDomainRepository _repo;
    public GetUserByIdQueryHandler(ISettingsDomainRepository repo) { _repo = repo; }

    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        var u = await _repo.GetUserByIdAsync(request.UserId);
        if (u == null) return null;
        return new UserDto(u.UserId, u.Username, u.Email, u.FirstName, u.LastName, u.IsActive,
            u.LastLoginDate, u.CreatedDate, u.ModifiedDate, u.Phone, u.Position, u.Department,
            u.TimeZone, u.Language, u.IsEmailVerified, u.TwoFactorEnabled);
    }
}

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
{
    private readonly ISettingsDomainRepository _repo;
    public GetAllUsersQueryHandler(ISettingsDomainRepository repo) { _repo = repo; }

    public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken ct)
    {
        var users = await _repo.GetAllUsersAsync();
        return users.Select(u => new UserDto(u.UserId, u.Username, u.Email, u.FirstName, u.LastName,
            u.IsActive, u.LastLoginDate, u.CreatedDate, u.ModifiedDate, u.Phone, u.Position,
            u.Department, u.TimeZone, u.Language, u.IsEmailVerified, u.TwoFactorEnabled));
    }
}

public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, IEnumerable<RoleDto>>
{
    private readonly ISettingsDomainRepository _repo;
    public GetAllRolesQueryHandler(ISettingsDomainRepository repo) { _repo = repo; }

    public async Task<IEnumerable<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken ct)
    {
        var roles = await _repo.GetAllRolesAsync();
        return roles.Select(r => new RoleDto(r.RoleId, r.RoleName, r.RoleCode, r.Description,
            r.IsActive, r.IsSystemRole, r.Permissions));
    }
}

public class GetUserPreferencesQueryHandler : IRequestHandler<GetUserPreferencesQuery, IEnumerable<UserPreferenceDto>>
{
    private readonly ISettingsDomainRepository _repo;
    public GetUserPreferencesQueryHandler(ISettingsDomainRepository repo) { _repo = repo; }

    public async Task<IEnumerable<UserPreferenceDto>> Handle(GetUserPreferencesQuery request, CancellationToken ct)
    {
        var prefs = await _repo.GetUserPreferencesAsync(request.UserId);
        return prefs.Select(p => new UserPreferenceDto(p.UserPreferenceId, p.UserId, p.PreferenceKey,
            p.PreferenceValue, p.PreferenceType, p.Category, p.IsActive));
    }
}
