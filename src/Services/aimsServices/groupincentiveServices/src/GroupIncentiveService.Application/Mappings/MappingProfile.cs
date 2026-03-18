using AutoMapper;
using GroupIncentiveService.Application.DTOs;
using GroupIncentiveService.Domain.Entities;

namespace GroupIncentiveService.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<GroupMaster, GroupMasterDto>();
        CreateMap<GroupEmployeeMap, GroupEmployeeMapDto>();
        CreateMap<GroupIncentiveMain, GroupIncentiveMainDto>()
            .ForCtorParam("GroupName", opt => opt.MapFrom(src => src.Group != null ? src.Group.GroupName : null))
            .ForCtorParam("Details", opt => opt.MapFrom(src => src.Details.ToList()));
        CreateMap<GroupIncentiveDet, GroupIncentiveDetDto>();
        CreateMap<GroupIncentiveBreak, GroupIncentiveBreakDto>();
        CreateMap<GroupIncentiveApproval, GroupIncentiveApprovalDto>();
    }
}
