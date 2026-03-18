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
            .ForMember(dest => dest.CommunityId, opt => opt.MapFrom(src => src.CommunityId))
            .ForMember(dest => dest.CommunityCode, opt => opt.MapFrom(src => src.CommunityCode.Value))
            .ForMember(dest => dest.CommunityName, opt => opt.MapFrom(src => src.CommunityName.Value))
            .ForMember(dest => dest.CommunityType, opt => opt.MapFrom(src => src.CommunityType.Value))
            .ForMember(dest => dest.PrivacyLevel, opt => opt.MapFrom(src => src.PrivacyLevel.Value))
            .ForMember(dest => dest.CommunityStatus, opt => opt.MapFrom(src => src.CommunityStatus.Value))
            .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.AuditInfo.CreatedOn))
            .ForMember(dest => dest.UpdatedOn, opt => opt.MapFrom(src => src.AuditInfo.UpdatedOn));

        // CommunityMember Mappings
        CreateMap<CommunityMember, CommunityMemberDto>()
            .ForMember(dest => dest.MemberRole, opt => opt.MapFrom(src => src.MemberRole.Value))
            .ForMember(dest => dest.MemberStatus, opt => opt.MapFrom(src => src.MemberStatus.Value));
    }
}
