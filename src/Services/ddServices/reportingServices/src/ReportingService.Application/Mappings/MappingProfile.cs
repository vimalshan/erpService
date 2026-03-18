using AutoMapper;

namespace ReportingService.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ReportingService.Domain.Entities.Appraisal, DTOs.AppraisalDto>()
            .ForMember(dst => dst.Goals, opt => opt.MapFrom(src => src.Goals))
            .ForMember(dst => dst.Performances, opt => opt.MapFrom(src => src.Performances));

        CreateMap<ReportingService.Domain.Entities.AppraisalGoal, DTOs.AppraisalGoalDto>();

        CreateMap<ReportingService.Domain.Entities.AppraiseePerformance, DTOs.AppraiseePerformanceDto>();

        CreateMap<ReportingService.Domain.Entities.DDRating, DTOs.DDRatingDto>();
    }
}
