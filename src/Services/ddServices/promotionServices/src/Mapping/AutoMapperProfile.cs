using AutoMapper;
using PromotionService.Domain.Entities;
using PromotionService.DTOs;

namespace PromotionService.Mapping;

public class PromotionAutoMapperProfile : Profile
{
    public PromotionAutoMapperProfile()
    {
        // Rating
        CreateMap<Rating, RatingDto>().ReverseMap();
        CreateMap<CreateRatingDto, Rating>()
            .ForMember(d => d.RatingId, o => o.Ignore())
            .ForMember(d => d.FinalRating, o => o.Ignore())
            .ForMember(d => d.RatingGrade, o => o.Ignore())
            .ForMember(d => d.RatingCategory, o => o.Ignore())
            .ForMember(d => d.Status, o => o.MapFrom(_ => "P"))
            .ForMember(d => d.RatedOn, o => o.MapFrom(_ => DateTime.UtcNow))
            .ForMember(d => d.CreatedOn, o => o.MapFrom(_ => DateTime.UtcNow));

        // PromotionRecommendation
        CreateMap<PromotionRecommendation, PromotionRecommendationDto>().ReverseMap();
        CreateMap<CreatePromotionRecommendationDto, PromotionRecommendation>()
            .ForMember(d => d.PromotionId, o => o.Ignore())
            .ForMember(d => d.Status, o => o.MapFrom(_ => "P"))
            .ForMember(d => d.CreatedOn, o => o.MapFrom(_ => DateTime.UtcNow));

        // IncrementRequest
        CreateMap<IncrementRequest, IncrementRequestDto>().ReverseMap();
        CreateMap<CreateIncrementRequestDto, IncrementRequest>()
            .ForMember(d => d.IncrementId, o => o.Ignore())
            .ForMember(d => d.IncrementAmount, o => o.Ignore())
            .ForMember(d => d.IncrementPercentage, o => o.Ignore())
            .ForMember(d => d.Status, o => o.MapFrom(_ => "P"))
            .ForMember(d => d.CreatedOn, o => o.MapFrom(_ => DateTime.UtcNow));

        // VTCAssessment
        CreateMap<VTCAssessment, VTCAssessmentDto>().ReverseMap();
        CreateMap<CreateVTCAssessmentDto, VTCAssessment>()
            .ForMember(d => d.VTCAssessmentId, o => o.Ignore())
            .ForMember(d => d.Status, o => o.MapFrom(_ => "P"))
            .ForMember(d => d.AssessedOn, o => o.MapFrom(_ => DateTime.UtcNow))
            .ForMember(d => d.CreatedOn, o => o.MapFrom(_ => DateTime.UtcNow));

        // DD_ schema entities to DTOs
        CreateMap<AppraisalAmount, AppraisalAmountDto>().ReverseMap();
        CreateMap<HorizontalPromotion, HorizontalPromotionDto>().ReverseMap();
        CreateMap<VTCCorrection, VTCCorrectionDto>().ReverseMap();
        CreateMap<DirectIncrement, DirectIncrementDto>().ReverseMap();
    }
}
