using SettingsService.Application.DTOs;
using SettingsService.Domain.Entities;
using SettingsService.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace SettingsService.Application.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly ISettingsDomainRepository _repo;
    private readonly IMediator _mediator;
    public CreateUserCommandHandler(ISettingsDomainRepository repo, IMediator mediator) { _repo = repo; _mediator = mediator; }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken ct)
    {
        var d = request.Dto;
        var hasher = new PasswordHasher<User>();
        var user = User.Create(d.Username, d.Email, d.FirstName, d.LastName, "", d.CreatedBy);
        user.PasswordHash = hasher.HashPassword(user, d.Password);
        user.Phone = d.Phone; user.Position = d.Position; user.Department = d.Department;
        user.TimeZone = d.TimeZone ?? "UTC"; user.Language = d.Language ?? "EN";

        var created = await _repo.AddUserAsync(user);
        foreach (var evt in created.DomainEvents) await _mediator.Publish(evt, ct);
        created.ClearDomainEvents();
        return MapToDto(created);
    }

    internal static UserDto MapToDto(User u) => new(u.UserId, u.Username, u.Email, u.FirstName, u.LastName,
        u.IsActive, u.LastLoginDate, u.CreatedDate, u.ModifiedDate, u.Phone, u.Position,
        u.Department, u.TimeZone, u.Language, u.IsEmailVerified, u.TwoFactorEnabled);
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly ISettingsDomainRepository _repo;
    public UpdateUserCommandHandler(ISettingsDomainRepository repo) { _repo = repo; }

    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken ct)
    {
        var d = request.Dto;
        var e = await _repo.GetUserByIdAsync(d.UserId) ?? throw new System.Collections.Generic.KeyNotFoundException($"User {d.UserId} not found");
        e.Username = d.Username; e.Email = d.Email; e.FirstName = d.FirstName; e.LastName = d.LastName;
        e.IsActive = d.IsActive; e.Phone = d.Phone; e.Position = d.Position; e.Department = d.Department;
        e.TimeZone = d.TimeZone; e.Language = d.Language; e.ModifiedDate = DateTime.UtcNow; e.ModifiedBy = d.ModifiedBy;
        await _repo.UpdateUserAsync(e);
        return CreateUserCommandHandler.MapToDto(e);
    }
}

public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand, bool>
{
    private readonly ISettingsDomainRepository _repo;
    private readonly IMediator _mediator;
    public DeactivateUserCommandHandler(ISettingsDomainRepository repo, IMediator mediator) { _repo = repo; _mediator = mediator; }

    public async Task<bool> Handle(DeactivateUserCommand request, CancellationToken ct)
    {
        var e = await _repo.GetUserByIdAsync(request.UserId) ?? throw new System.Collections.Generic.KeyNotFoundException($"User {request.UserId} not found");
        e.Deactivate(request.ModifiedBy);
        await _repo.UpdateUserAsync(e);
        foreach (var evt in e.DomainEvents) await _mediator.Publish(evt, ct);
        e.ClearDomainEvents();
        return true;
    }
}

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleDto>
{
    private readonly ISettingsDomainRepository _repo;
    private readonly IMediator _mediator;
    public CreateRoleCommandHandler(ISettingsDomainRepository repo, IMediator mediator) { _repo = repo; _mediator = mediator; }

    public async Task<RoleDto> Handle(CreateRoleCommand request, CancellationToken ct)
    {
        var d = request.Dto;
        var role = new Role
        {
            RoleName = d.RoleName, RoleCode = d.RoleCode, Description = d.Description,
            IsSystemRole = d.IsSystemRole, Permissions = d.Permissions,
            IsActive = true, CreatedBy = d.CreatedBy, ModifiedBy = d.CreatedBy,
            CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow
        };
        var created = await _repo.AddRoleAsync(role);
        return new RoleDto(created.RoleId, created.RoleName, created.RoleCode, created.Description,
            created.IsActive, created.IsSystemRole, created.Permissions);
    }
}

public class SetUserPreferenceCommandHandler : IRequestHandler<SetUserPreferenceCommand, UserPreferenceDto>
{
    private readonly ISettingsDomainRepository _repo;
    public SetUserPreferenceCommandHandler(ISettingsDomainRepository repo) { _repo = repo; }

    public async Task<UserPreferenceDto> Handle(SetUserPreferenceCommand request, CancellationToken ct)
    {
        var d = request.Dto;
        var existing = (await _repo.GetUserPreferencesAsync(d.UserId))
            .FirstOrDefault(p => p.PreferenceKey == d.PreferenceKey);

        if (existing != null)
        {
            existing.PreferenceValue = d.PreferenceValue; existing.PreferenceType = d.PreferenceType;
            existing.Category = d.Category; existing.ModifiedDate = DateTime.UtcNow; existing.ModifiedBy = d.ModifiedBy;
            await _repo.UpdatePreferenceAsync(existing);
            return new UserPreferenceDto(existing.UserPreferenceId, existing.UserId, existing.PreferenceKey,
                existing.PreferenceValue, existing.PreferenceType, existing.Category, existing.IsActive);
        }

        var pref = new UserPreference
        {
            UserId = d.UserId, PreferenceKey = d.PreferenceKey, PreferenceValue = d.PreferenceValue,
            PreferenceType = d.PreferenceType, Category = d.Category, IsActive = true,
            CreatedBy = d.ModifiedBy, ModifiedBy = d.ModifiedBy,
            CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow
        };
        var created = await _repo.AddPreferenceAsync(pref);
        return new UserPreferenceDto(created.UserPreferenceId, created.UserId, created.PreferenceKey,
            created.PreferenceValue, created.PreferenceType, created.Category, created.IsActive);
    }
}
