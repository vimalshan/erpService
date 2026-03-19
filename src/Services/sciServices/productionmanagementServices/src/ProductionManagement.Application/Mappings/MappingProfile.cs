using AutoMapper;
using ProductionManagement.Application.DTOs;
using ProductionManagement.Domain.Entities;

namespace ProductionManagement.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ProductionPlant
        CreateMap<ProductionPlant, ProductionPlantDto>()
            .ForMember(d => d.CreatedBy, o => o.MapFrom(s => s.SciUserIdCreated))
            .ForMember(d => d.ModifiedBy, o => o.MapFrom(s => s.SciUserIdModified));

        // ProductionPlan
        CreateMap<ProductionPlan, ProductionPlanDto>()
            .ForMember(d => d.ModifiedBy, o => o.MapFrom(s => s.SciUserIdModified));

        // ProductionPlanEntry
        CreateMap<ProductionPlanEntry, ProductionPlanEntryDto>();

        // ProductionPlantProductMap
        CreateMap<ProductionPlantProductMap, ProductionPlantProductMapDto>();

        // MamProductionDet
        CreateMap<MamProductionDet, MamProductionDetDto>();

        // MamProductionMap
        CreateMap<MamProductionMap, MamProductionMapDto>();

        // NormsMain
        CreateMap<NormsMain, NormsMainDto>()
            .ForMember(d => d.NormsMasters, o => o.MapFrom(s => s.NormsMasters));

        // NormsMaster
        CreateMap<NormsMaster, NormsMasterDto>();
    }
}
