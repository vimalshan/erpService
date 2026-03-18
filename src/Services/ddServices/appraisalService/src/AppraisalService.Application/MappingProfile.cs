using System;
using AutoMapper;
using AppraisalService.Application.CQRS.Queries;
using AppraisalService.Application.DTOs;
using AppraisalService.Domain.Entities;
using AppraisalService.Domain;

namespace AppraisalService.Application;

/// <summary>
/// AutoMapper configuration profiles
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Entity to DTO mappings
        CreateMap<AppraisalBandEntity, AppraisalBandDto>();

        CreateMap<AppraisalMainEntity, AppraisalMainDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Code));

        CreateMap<AppraisalMainEntity, AppraisalDetailedDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Code))
            .ForMember(dest => dest.Compensation, opt => opt.MapFrom(src => src.Compensation))
            .ForMember(dest => dest.Benefits, opt => opt.MapFrom(src => src.Benefits))
            .ForMember(dest => dest.CompetencyAssessments, opt => opt.MapFrom(src => src.CompetencyAssessments));

        CreateMap<CompensationDetails, CompensationDto>();
        CreateMap<BenefitsAvailability, BenefitsDto>();

        CreateMap<CompetencyAssessmentEntity, CompetencyAssessmentDto>();
        CreateMap<EmployeeGoalEntity, EmployeeGoalDto>();
    }
}
