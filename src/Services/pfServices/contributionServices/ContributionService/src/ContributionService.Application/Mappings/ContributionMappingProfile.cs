using AutoMapper;
using ContributionService.Application.DTOs;
using ContributionService.Domain.Entities;

namespace ContributionService.Application.Mappings;

public class ContributionMappingProfile : Profile
{
    public ContributionMappingProfile()
    {
        CreateMap<ContributionMain, ContributionMainDto>();
        CreateMap<ContributionDetail, ContributionDetailDto>();
        CreateMap<ContributionBreakup, ContributionBreakupDto>();
        CreateMap<SuperannuationBatch, SuperannuationBatchDto>();
        CreateMap<SuperannuationContribution, SuperannuationContributionDto>();
        CreateMap<SuperannuationTrustName, SuperannuationTrustNameDto>();
    }
}
