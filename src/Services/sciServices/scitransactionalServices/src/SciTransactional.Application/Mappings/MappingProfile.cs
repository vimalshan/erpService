using AutoMapper;
using SciTransactional.Application.DTOs;
using SciTransactional.Domain.Entities;

namespace SciTransactional.Application.Mappings;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<SparshNavigationEntity, NavigationDto>()
            .ForMember(d => d.RequestNum, o => o.MapFrom(s => s.Id));

        CreateMap<NormsMainEntity, NormsMainDto>()
            .ForMember(d => d.NormNo, o => o.MapFrom(s => s.Id));

        CreateMap<NormsMasterEntity, NormsMasterDto>()
            .ForMember(d => d.NormId, o => o.MapFrom(s => s.Id));

        CreateMap<AdvanceLicenseEntity, AdvanceLicenseDto>()
            .ForMember(d => d.LicenseId, o => o.MapFrom(s => s.Id));

        CreateMap<AdvanceLicenseEntitlementEntity, EntitlementDto>()
            .ForMember(d => d.LicenseId, o => o.MapFrom(s => s.Id));

        CreateMap<AutoMailStatusEntity, AutoMailStatusDto>();
        CreateMap<AutoMailIdEntity, AutoMailIdDto>();

        CreateMap<ActualOrderMapEntity, OrderMapDto>();

        CreateMap<VehicleDirectEntryEntity, DirectEntryDto>();
    }
}
