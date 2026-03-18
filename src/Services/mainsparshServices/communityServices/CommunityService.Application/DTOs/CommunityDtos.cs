namespace CommunityService.Application.DTOs;

public record CreateCommunityDto(
    string CommunityCode,
    string CommunityName,
    string? CommunityDescription,
    string CommunityType,
    string? CommunityIcon,
    string? CommunityBanner,
    string PrivacyLevel,
    long OwnerId);

public record UpdateCommunityDto(
    long CommunityId,
    string CommunityName,
    string? CommunityDescription,
    string PrivacyLevel);

public record CommunityDto(
    long CommunityId,
    string CommunityCode,
    string CommunityName,
    string? CommunityDescription,
    string CommunityType,
    string? CommunityIcon,
    string? CommunityBanner,
    string PrivacyLevel,
    long OwnerId,
    long? ApproverId,
    string CommunityStatus,
    int MemberCount,
    DateTime CreatedOn,
    DateTime? UpdatedOn);

public record CommunityMemberDto(
    long MemberId,
    long CommunityId,
    long UserSysId,
    string MemberRole,
    DateTime JoinDate,
    DateTime? LeaveDate,
    string MemberStatus,
    int ContributionCount);

public record AddMemberDto(
    long CommunityId,
    long UserId,
    string MemberRole);

public record RemoveMemberDto(
    long CommunityId,
    long UserId);

public record ChangeMemberRoleDto(
    long CommunityId,
    long UserId,
    string NewRole);

public record CommunityListDto(
    long CommunityId,
    string CommunityCode,
    string CommunityName,
    string CommunityType,
    string PrivacyLevel,
    int MemberCount);
