using AutoMapper;
using UnitService.Application.DTOs;
using UnitService.Domain.Entities;

namespace UnitService.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<EquipmentMaster, EquipmentDto>()
            .ForMember(d => d.UnitCode, opt => opt.MapFrom(s => s.UnitCode.Value));

        CreateMap<EquipmentStatus, EquipmentStatusDto>()
            .ForMember(d => d.StatusDescription, opt => opt.MapFrom(s => s.StatusDescription))
            .ForMember(d => d.StatusCode, opt => opt.MapFrom(s => s.StatusCode));

        CreateMap<AccessMaster, AccessDto>()
            .ForMember(d => d.UnitCode, opt => opt.MapFrom(s => s.UnitCode.Value))
            .ForMember(d => d.AccessType, opt => opt.MapFrom(s => s.AccessType.Value));

        CreateMap<CategoryMaster, CategoryDto>()
            .ForMember(d => d.UnitCode, opt => opt.MapFrom(s => s.UnitCode.Value));

        CreateMap<BudgetMaster, BudgetDto>()
            .ForMember(d => d.UnitCode, opt => opt.MapFrom(s => s.UnitCode.Value));
    }
}
