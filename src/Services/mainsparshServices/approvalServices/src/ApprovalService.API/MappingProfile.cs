using AutoMapper;
using ApprovalService.Application.DTOs;
using ApprovalService.Domain.Entities;

/// <summary>
/// AutoMapper Profile for domain-to-DTO mappings
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ApprovalMaster mappings
        CreateMap<ApprovalMaster, ApprovalMasterDto>()
            .ForMember(dest => dest.Status, 
                opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Approvers,
                opt => opt.MapFrom(src => src.Approvers));

        CreateMap<ApprovalMasterDto, ApprovalMaster>().ReverseMap();

        // ApproverEmployee mappings
        CreateMap<ApproverEmployee, ApproverEmployeeDto>()
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<ApproverEmployeeDto, ApproverEmployee>().ReverseMap();
    }
}
