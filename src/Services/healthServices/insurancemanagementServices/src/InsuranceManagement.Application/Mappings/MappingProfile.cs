using AutoMapper;
using InsuranceManagement.Domain.Entities;
using InsuranceManagement.Application.DTOs;

namespace InsuranceManagement.Application.Mappings;

/// <summary>
/// AutoMapper profile for mapping between domain entities and DTOs
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // InsurancePlan mappings
        CreateMap<InsurancePlan, InsurancePlanDto>()
            .ForMember(dest => dest.InsurancePlanId, opt => opt.MapFrom(src => src.InsurancePlanId))
            .ReverseMap();

        CreateMap<CreateInsurancePlanDto, InsurancePlan>()
            .ConstructUsing(src => new InsurancePlan(
                src.PlanName,
                src.PlanDescription,
                src.PremiumRate,
                src.MinPremium,
                src.MaxPremium,
                src.CoverageDetails,
                createdBy: 0)); // Will be set by handler

        // InsuranceEnrollment mappings
        CreateMap<InsuranceEnrollment, InsuranceEnrollmentDto>()
            .ForMember(dest => dest.CoverageType, opt => opt.MapFrom(src => src.CoverageType.Value))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Value))
            .ForMember(dest => dest.Plan, opt => opt.MapFrom(src => src.InsurancePlan))
            .ReverseMap();

        CreateMap<InsuranceEnrollment, InsuranceEnrollmentDetailDto>()
            .ForMember(dest => dest.CoverageType, opt => opt.MapFrom(src => src.CoverageType.Value))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Value))
            .ForMember(dest => dest.Plan, opt => opt.MapFrom(src => src.InsurancePlan))
            .ForMember(dest => dest.Claims, opt => opt.MapFrom(src => src.Claims));

        CreateMap<CreateInsuranceEnrollmentDto, InsuranceEnrollment>();

        // InsuranceClaim mappings
        CreateMap<InsuranceClaim, InsuranceClaimDto>()
            .ForMember(dest => dest.ClaimType, opt => opt.MapFrom(src => src.ClaimType.Value))
            .ForMember(dest => dest.ClaimAmount, opt => opt.MapFrom(src => src.ClaimAmount.Amount))
            .ForMember(dest => dest.ReimbursableAmount, opt => opt.MapFrom(src => src.ReimbursableAmount.Amount))
            .ForMember(dest => dest.ApprovedAmount, opt => opt.MapFrom(src => src.ApprovedAmount.Amount))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Value))
            .ReverseMap();

        CreateMap<SubmitInsuranceClaimDto, InsuranceClaim>();
    }
}
