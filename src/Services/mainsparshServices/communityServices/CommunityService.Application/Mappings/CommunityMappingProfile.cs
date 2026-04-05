namespace CommunityService.Application.Mappings;

using AutoMapper;
using DTOs;
using Domain.Entities;

public class CommunityMappingProfile : Profile
{
    public CommunityMappingProfile()
    {
        // Community Mappings
        CreateMap<Community, CommunityDto>()
            .ConstructUsing(src => new CommunityDto(
                src.CommunityId,
                src.CommunityCode.Value,
                src.CommunityName.Value,
                src.CommunityDescription,
                src.CommunityType.Value,
                src.CommunityIcon,
                src.CommunityBanner,
                src.PrivacyLevel.Value,
                src.OwnerId,
                src.ApproverId,
                src.CommunityStatus.Value,
                src.MemberCount,
                src.AuditInfo.CreatedOn,
                src.AuditInfo.UpdatedOn
            ))
            .ForAllMembers(opt => opt.Ignore());

        // CommunityMember Mappings
        CreateMap<CommunityMember, CommunityMemberDto>()
            .ConstructUsing(src => new CommunityMemberDto(
                src.MemberId,
                src.CommunityId,
                src.UserSysId,
                src.MemberRole.Value,
                src.JoinDate,
                src.LeaveDate,
                src.MemberStatus.Value,
                src.ContributionCount
            ))
            .ForAllMembers(opt => opt.Ignore());
    }
}
