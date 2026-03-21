using AutoMapper;
using InsuranceService.Domain.Entities;

namespace InsuranceService.Application.DTOs;

public class InsuranceMappingProfile : Profile
{
    public InsuranceMappingProfile()
    {
        CreateMap<TravelInsurance, TravelInsuranceDto>()
            .ForMember(d => d.CompanyCode, o => o.MapFrom(s => s.CompanyCode.Value))
            .ForMember(d => d.InsuranceType, o => o.MapFrom(s => s.InsuranceType.Value))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.Value));
    }
}
