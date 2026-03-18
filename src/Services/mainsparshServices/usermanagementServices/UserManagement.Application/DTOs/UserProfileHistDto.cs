namespace UserManagement.Application.DTOs;

public record UserProfileHistDto(
    long HistId,
    long PolicyId,
    long UserSysId,
    string? ProfileField,
    string? OldValue,
    string? NewValue,
    string? ChangeReason,
    long ChangedBy,
    DateTime ChangedOn);

public record CreateUserProfileHistDto(
    long PolicyId,
    long UserSysId,
    string? ProfileField,
    string? OldValue,
    string? NewValue,
    string? ChangeReason,
    long ChangedBy);
