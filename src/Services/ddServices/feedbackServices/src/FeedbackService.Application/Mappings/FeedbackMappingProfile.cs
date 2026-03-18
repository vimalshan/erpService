namespace FeedbackService.Application.Mappings;

using AutoMapper;
using Domain.Aggregates;
using Domain.Entities;
using DTOs;

/// <summary>
/// AutoMapper profile for feedback entities
/// </summary>
public class FeedbackMappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the FeedbackMappingProfile class
    /// </summary>
    public FeedbackMappingProfile()
    {
        CreateMap<Feedback, FeedbackDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status!.Value))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<FeedbackItem, FeedbackItemDto>();
    }
}
