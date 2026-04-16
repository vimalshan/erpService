using MediatR;

namespace SettingsService.Domain.Events;

public interface IDomainEvent : INotification { }

public record UserCreatedEvent(int UserId, string Username, string Email) : IDomainEvent;
public record UserDeactivatedEvent(int UserId, string Username) : IDomainEvent;
public record UserPreferenceUpdatedEvent(int UserId, string PreferenceKey, string? OldValue, string? NewValue) : IDomainEvent;
public record RoleCreatedEvent(int RoleId, string RoleName, string RoleCode) : IDomainEvent;
