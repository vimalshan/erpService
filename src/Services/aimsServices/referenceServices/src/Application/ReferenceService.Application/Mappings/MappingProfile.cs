using AutoMapper;
using ReferenceService.Application.DTOs;
using ReferenceService.Domain.Entities;

namespace ReferenceService.Application.Mappings;

/// <summary>
/// AutoMapper configuration for mappings.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // LOV Type mappings
        CreateMap<LovType, LovTypeDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Values, opt => opt.MapFrom(src => src.Values));
        
        CreateMap<LovTypeDto, LovType>().ReverseMap();
        
        // LOV Value mappings
        CreateMap<LovValue, LovValueDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        
        CreateMap<LovValueDto, LovValue>().ReverseMap();
        
        // Permission Rule mappings
        CreateMap<PermissionRule, PermissionRuleDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        
        CreateMap<PermissionRuleDto, PermissionRule>().ReverseMap();
        
        // Leave Flag mappings
        CreateMap<LeaveFlag, LeaveFlagDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        
        CreateMap<LeaveFlagDto, LeaveFlag>().ReverseMap();
    }
}
