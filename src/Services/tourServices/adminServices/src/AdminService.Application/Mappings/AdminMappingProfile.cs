using AutoMapper;
using AdminService.Domain.Entities;
using AdminService.Application.DTOs;

namespace AdminService.Application.Mappings;

public class AdminMappingProfile : Profile
{
    public AdminMappingProfile()
    {
        CreateMap<AdminMaster, AdminMasterDto>()
            .ForMember(d => d.AdminLocStatus, o => o.MapFrom(s => s.AdminLocStatus.HasValue ? s.AdminLocStatus.Value.ToString() : null));
        CreateMap<AdminMasterDto, AdminMaster>()
            .ForMember(d => d.AdminLocStatus, o => o.MapFrom(s => !string.IsNullOrEmpty(s.AdminLocStatus) ? s.AdminLocStatus[0] : (char?)null));
        CreateMap<AdminUserMap, AdminUserMapDto>().ReverseMap();
        CreateMap<AdminFinUserMap, AdminFinUserMapDto>().ReverseMap();
        CreateMap<AdminAccessRights, AdminAccessRightsDto>().ReverseMap();
        CreateMap<AdminAccessRightsLog, AdminAccessRightsLogDto>().ReverseMap();
    }
}
