namespace CompensationService.Application;

using AutoMapper;
using CompensationService.Domain.Entities;
using CompensationService.Application.DTOs;

/// <summary>
/// AutoMapper profile for compensation service mappings.
/// </summary>
public class CompensationMappingProfile : Profile
{
    public CompensationMappingProfile()
    {
        // Budget mappings
        CreateMap<Budget, BudgetDto>()
            .ForMember(dest => dest.BudgetAmount, opt => opt.MapFrom(src => src.BudgetAmount.Amount))
            .ReverseMap();

        // Compensation Level mappings
        CreateMap<CompensationLevel, CompensationLevelDto>()
            .ForMember(dest => dest.MinAmount, opt => opt.MapFrom(src => src.LevelRange.MinAmount))
            .ForMember(dest => dest.MaxAmount, opt => opt.MapFrom(src => src.LevelRange.MaxAmount))
            .ReverseMap();

        // Compensation Period mappings
        CreateMap<CompensationPeriod, CompensationPeriodDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Description))
            .ReverseMap();

        // Compensation Recommendation mappings
        CreateMap<CompensationRecommendation, CompensationRecommendationDto>()
            .ForMember(dest => dest.CtcAmount, opt => opt.MapFrom(src => src.CtcAmount.Amount))
            .ForMember(dest => dest.MaximumCap, opt => opt.MapFrom(src => src.MaximumCap.Amount))
            .ForMember(dest => dest.EligibilityAmount, opt => opt.MapFrom(src => src.EligibilityAmount.Amount))
            .ForMember(dest => dest.RecommendedAmount, opt => opt.MapFrom(src => src.RecommendedAmount != null ? src.RecommendedAmount.Amount : (decimal?)null))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.StatusCode))
            .ForMember(dest => dest.StatusDescription, opt => opt.MapFrom(src => src.Status.Description))
            .ReverseMap();

        // Budget Log mappings
        CreateMap<BudgetLog, BudgetDto>()
            .ForMember(dest => dest.BudgetAmount, opt => opt.MapFrom(src => src.BudgetAmount.Amount))
            .ReverseMap();
    }
}
