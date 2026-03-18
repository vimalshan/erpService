namespace UserSecurityService.Application.DTOs;

public record UserProfileDto(
    string UserId,
    decimal EmpNum,
    string UnitCode,
    string NickName,
    string UserType,
    string EmailFlag,
    string? OfficeEmail,
    string? PersonalEmail,
    DateTime EffectiveDate,
    DateTime? CloseDate,
    string? EmpName,
    string? FirstName,
    string? MiddleName,
    string? LastName,
    string? Designation,
    string? Division,
    string? PhotoPath,
    string RegStatus
);

public record UserAppsMappingDto(
    decimal EmpSysId,
    string AppCode,
    DateTime EffectiveDate,
    DateTime? CloseDate,
    decimal HrRoleId,
    string? Remarks
);

public record UserUnitMapDto(
    decimal RoleId,
    string AppCode,
    decimal EmpSysId,
    decimal OrgId,
    char UnitAll,
    string RoleType,
    DateTime EffectiveDate,
    DateTime? CloseDate,
    decimal? UnitId,
    string? Remarks
);

public record EmpPasswordChangeDto(
    decimal RecordId,
    decimal EmpSysId,
    decimal CreatedBy,
    DateTime CreatedOn
);

public record AuthTokenDto(
    string AccessToken,
    DateTime ExpiresAt,
    string TokenType = "Bearer"
);

public record LoginRequestDto(string UserId, string Password);
