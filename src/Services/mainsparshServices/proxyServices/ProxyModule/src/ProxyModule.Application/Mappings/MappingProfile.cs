using AutoMapper;
using ProxyModule.Application.DTOs;
using ProxyModule.Domain.Entities;

namespace ProxyModule.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProxyRight, ProxyRightDto>()
            .ForMember(dest => dest.IsCurrentlyActive, opt => opt.MapFrom(src => src.IsCurrentlyActive));

        CreateMap<CreateProxyRightDto, ProxyRight>();
    }
}
