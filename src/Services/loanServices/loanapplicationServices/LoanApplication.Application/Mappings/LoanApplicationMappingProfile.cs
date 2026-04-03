using AutoMapper;
using LoanApplication.Domain.Aggregates;
using LoanApplication.Domain.Interfaces;
using LoanApplication.Application.DTOs;
using LoanApplication.Application.Queries;

namespace LoanApplication.Application.Mappings;

/// <summary>
/// AutoMapper profile for loan application mappings
/// </summary>
public class LoanApplicationMappingProfile : Profile
{
    public LoanApplicationMappingProfile()
    {
        // Domain to DTO
        CreateMap<LoanApplicationAggregate, LoanApplicationDto>()
            .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.Source.Value))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount.Amount))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Value))
            .ForMember(dest => dest.StatusDisplayName, opt => opt.MapFrom(src =>
                src.Status.Value == 'C' ? "Created" :
                src.Status.Value == 'P' ? "Pending" :
                src.Status.Value == 'A' ? "Approved" :
                src.Status.Value == 'R' ? "Rejected" :
                src.Status.Value == 'D' ? "Disbursed" : "Unknown"));

        // Domain Service Result to DTO
        CreateMap<EligibilityCheckResult, EligibilityCheckDto>();
    }
}

/// <summary>
/// Custom resolver for status display name
/// </summary>
public class StatusNameResolver : IValueResolver<LoanApplicationAggregate, LoanApplicationDto, string>
{
    public string Resolve(LoanApplicationAggregate source, LoanApplicationDto destination, string destMember, ResolutionContext context)
    {
        return source.Status.Value switch
        {
            'C' => "Created",
            'P' => "Pending",
            'A' => "Approved",
            'R' => "Rejected",
            'D' => "Disbursed",
            _ => "Unknown"
        };
    }
}
