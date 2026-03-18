using AutoMapper;
using Todos.Application.DTOs;
using Todos.Domain.Entities;

namespace Todos.Application.Mappers;

/// <summary>
/// AutoMapper profile for domain to DTO mappings
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Learning Record mappings
        CreateMap<LearningRecord, LearningRecordDto>()
            .ForMember(dest => dest.BhrStatus, opt => opt.MapFrom(src => src.BhrStatus != null ? src.BhrStatus.Value : (char?)null))
            .ReverseMap();

        // Learning Sub Record mappings
        CreateMap<LearningSubRecord, LearningSubRecordDto>()
            .ReverseMap();

        // Learning Feedback mappings
        CreateMap<LearningFeedback, LearningFeedbackDto>()
            .ForMember(dest => dest.FeedbackStatus, opt => opt.MapFrom(src => src.FeedbackStatus != null ? src.FeedbackStatus.Value : (char?)null))
            .ForMember(dest => dest.AppraiserNeedStatus, opt => opt.MapFrom(src => src.AppraiserNeedStatus != null ? src.AppraiserNeedStatus.Value : (char?)null))
            .ReverseMap();

        // Development Category Detail mappings
        CreateMap<DevelopmentCategoryDetail, DevelopmentCategoryDetailDto>()
            .ReverseMap();
    }
}
