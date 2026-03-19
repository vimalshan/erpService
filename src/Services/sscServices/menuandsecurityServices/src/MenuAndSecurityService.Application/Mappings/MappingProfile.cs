using AutoMapper;
using MenuAndSecurityService.Application.DTOs;
using MenuAndSecurityService.Domain.Entities;

namespace MenuAndSecurityService.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<MenuMaster, MenuMasterDto>().ReverseMap();
        CreateMap<RoleMenuAccess, RoleMenuAccessDto>()
            .ForMember(d => d.MenuName, opt => opt.MapFrom(s => s.Menu != null ? s.Menu.MenuName : null))
            .ReverseMap();
    }
}
