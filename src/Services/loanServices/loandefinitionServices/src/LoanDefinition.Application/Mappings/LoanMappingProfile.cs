using AutoMapper;
using LoanDefinition.Application.DTOs;
using LoanDefinition.Domain.Entities;

namespace LoanDefinition.Application.Mappings;

public class LoanMappingProfile : Profile
{
    public LoanMappingProfile()
    {
        CreateMap<LoanTypeMaster, LoanTypeMasterDto>()
            .ForCtorParam("LoanType", opt => opt.MapFrom(s => s.Id))
            .ForCtorParam("ModifiedBy", opt => opt.MapFrom(s => s.LastModifiedBy))
            .ForCtorParam("ModifiedOn", opt => opt.MapFrom(s => s.LastModifiedOn));

        CreateMap<LoanMaster, LoanMasterDto>()
            .ForCtorParam("LoanId", opt => opt.MapFrom(s => s.Id))
            .ForCtorParam("LoanTypeName", opt => opt.MapFrom(s => s.LoanType != null ? s.LoanType.LoanName : string.Empty))
            .ForCtorParam("MinimumLimit", opt => opt.MapFrom(s => s.LoanLimit.MinimumLimit))
            .ForCtorParam("MaximumLimit", opt => opt.MapFrom(s => s.LoanLimit.MaximumLimit));

        CreateMap<LoanSubClass, LoanSubClassDto>()
            .ForCtorParam("SubClassId", opt => opt.MapFrom(s => s.Id));

        CreateMap<LoanInterestRateMaster, LoanInterestRateDto>()
            .ForCtorParam("RateId", opt => opt.MapFrom(s => s.Id));

        CreateMap<LoanLimitRangeMaster, LoanLimitRangeDto>()
            .ForCtorParam("RangeRateId", opt => opt.MapFrom(s => s.Id));

        CreateMap<LoanPerquisite, LoanPerquisiteDto>()
            .ForCtorParam("PerquisiteId", opt => opt.MapFrom(s => s.Id));

        CreateMap<LoanFestival, LoanFestivalDto>()
            .ForCtorParam("FestivalId", opt => opt.MapFrom(s => s.Id));

        CreateMap<LoanFestivalMap, LoanFestivalMapDto>()
            .ForCtorParam("MapId", opt => opt.MapFrom(s => s.Id))
            .ForCtorParam("FestivalDescription", opt => opt.MapFrom(s => s.Festival != null ? s.Festival.Description : null));

        CreateMap<LoanAccountMaster, LoanAccountMasterDto>()
            .ForCtorParam("AccountId", opt => opt.MapFrom(s => s.Id));
    }
}
