namespace UserManagement.Application.DTOs;

public record UserPolicyDto(
    long PolicyId,
    long UserSysId,
    string PolicyCode,
    string? PolicyType,
    int? DataRetentionDays,
    int? SessionTimeoutMins,
    int? MaxLoginAttempts,
    string PolicyStatus,
    DateOnly EffectiveFrom,
    DateOnly? EffectiveTo,
    long CreatedBy,
    DateTime CreatedOn,
    long? UpdatedBy,
    DateTime? UpdatedOn);

public record CreateUserPolicyDto(
    long UserSysId,
    string PolicyCode,
    string? PolicyType,
    DateOnly EffectiveFrom,
    long CreatedBy,
    int? DataRetentionDays = null,
    int? SessionTimeoutMins = null,
    int? MaxLoginAttempts = null);

public record UpdateUserPolicyDto(
    string? PolicyType,
    int? DataRetentionDays,
    int? SessionTimeoutMins,
    int? MaxLoginAttempts,
    DateOnly? EffectiveTo,
    long UpdatedBy);
