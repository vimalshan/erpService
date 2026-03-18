using AutoMapper;
using CompensationBenefits.Application.DTOs;
using CompensationBenefits.Domain.Entities;

namespace CompensationBenefits.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<SalaryMain, SalaryDto>()
            .ForMember(d => d.Details, o => o.MapFrom(s => s.Details));
        CreateMap<SalaryDetail, SalaryDetailDto>();

        CreateMap<SalaryStructureMain, SalaryStructureDto>()
            .ForMember(d => d.Details, o => o.MapFrom(s => s.Details));
        CreateMap<SalaryStructureDetail, SalaryStructureDetailDto>();

        CreateMap<MediclaimMaster, MediclaimDto>()
            .ForMember(d => d.YearlyPremiums, o => o.MapFrom(s => s.YearlyPremiums));
        CreateMap<MediclaimYearlyPremium, MediclaimYearlyPremiumDto>();

        CreateMap<MobileConnection, MobileConnectionDto>();
        CreateMap<RetiralRangeMaster, RetiralRangeMasterDto>();
    }
}
