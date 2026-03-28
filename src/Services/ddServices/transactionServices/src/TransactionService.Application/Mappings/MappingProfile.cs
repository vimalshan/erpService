using AutoMapper;
using TransactionService.Application.DTOs;
using TransactionService.Domain.Entities;

namespace TransactionService.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DemandMaster, DemandMasterDto>();
        CreateMap<SaaBudget, SaaBudgetDto>();
        CreateMap<SaaPeriod, SaaPeriodDto>();
        CreateMap<SaaLevel, SaaLevelDto>();
        CreateMap<SaaRecommend, SaaRecommendDto>();
        CreateMap<SaaSubmit, SaaSubmitDto>();
        CreateMap<SaaMailTrigger, SaaMailTriggerDto>();
    }
}
