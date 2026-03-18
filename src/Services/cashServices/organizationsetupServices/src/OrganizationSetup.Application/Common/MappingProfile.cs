using AutoMapper;
using OrganizationSetup.Application.DTOs;
using OrganizationSetup.Domain.Entities;

namespace OrganizationSetup.Application.Common;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DealRole, RoleDto>().ReverseMap();
        CreateMap<DealUserMap, UserMapDto>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName.Value))
            .ReverseMap();
        CreateMap<DealOrgParams, OrgParamsDto>()
            .ForMember(dest => dest.OrgParamType, opt => opt.MapFrom(src => src.OrgParamType.Value))
            .ReverseMap();
        CreateMap<DealPpLimit, PpLimitDto>()
            .ForMember(dest => dest.PpTranType, opt => opt.MapFrom(src => src.PpTranType.Value))
            .ReverseMap();
    }
}
