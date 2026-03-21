using AutoMapper;
using LookupService.Application.DTOs;
using LookupService.Domain.Entities;

namespace LookupService.Application.Mappings;

public class LookupMappingProfile : Profile
{
    public LookupMappingProfile()
    {
        CreateMap<LovTypeMaster, LovTypeMasterDto>();
        CreateMap<LovMaster, LovMasterDto>();
        CreateMap<LovUnitMap, LovUnitMapDto>();
        CreateMap<LovPanelMap, LovPanelMapDto>();
        CreateMap<PanelMaster, PanelMasterDto>();
        CreateMap<ProcessMaster, ProcessMasterDto>();
        CreateMap<UnitProcessMap, UnitProcessMapDto>();
        CreateMap<UnitLovAccessMaster, UnitLovAccessMasterDto>();
        CreateMap<UnitLovAccessDetail, UnitLovAccessDetailDto>();
    }
}
