using UserSecurityService.Application.DTOs;
using UserSecurityService.Domain.Entities;

namespace UserSecurityService.Application.Mappings;

/// <summary>Safe, source-generated-style manual mapping — no reflection-based AutoMapper.</summary>
public static class UserSecurityMapper
{
    public static UserProfileDto ToDto(this UserProfilePfs src) => new(
        src.EmUsrId, src.EmEmpNum, src.EmUntCod, src.EmNickNam,
        src.EmUsrTyp, src.EmEmlFlg, src.EmOEmlId, src.EmPEmlId,
        src.EmEffDat, src.EmClsDat, src.EmEmpNam,
        src.EmFrsNam, src.EmMidNam, src.EmLstNam,
        src.EmEmpDsg, src.EmDivNam, src.EmPhtPth, src.EmRegStatus);

    public static UserAppsMappingDto ToDto(this UserAppsMap src) => new(
        src.UserEmpSysId, src.UserApps, src.UserEffDate,
        src.UserClsDate, src.UserHrRoleId, src.UserRemarks);

    public static UserUnitMapDto ToDto(this UserUnitMap src) => new(
        src.RoleId, src.RoleApps, src.RoleEmpSysId, src.RoleOrgId,
        src.RoleUnitAll, src.RoleType, src.RoleEffDate, src.RoleClsDate,
        src.RoleUnitId, src.RoleRemarks);

    public static EmpPasswordChangeDto ToDto(this EmpPasswordChange src) => new(
        src.EpwdId, src.EpwdEmpSysId, src.EpwdCreatedBy, src.EpwdCreatedOn);

    public static IEnumerable<UserProfileDto> ToDto(this IEnumerable<UserProfilePfs> src)
        => src.Select(x => x.ToDto());

    public static IEnumerable<UserAppsMappingDto> ToDto(this IEnumerable<UserAppsMap> src)
        => src.Select(x => x.ToDto());
}
