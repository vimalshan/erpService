using AutoMapper;
using Recruitment.Application.DTOs;
using Recruitment.Domain.Entities;

namespace Recruitment.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Job mappings
        CreateMap<Job, JobDto>().ReverseMap();
        CreateMap<CreateJobDto, Job>()
            .ConstructUsing(src => new Job(
                src.JobId,
                src.RecruitmentCycleNo,
                src.JobDescription,
                src.RoleDetails,
                src.CadreCode,
                src.EffectiveDate,
                src.PrincipalAccount,
                src.JobType,
                src.BusinessCode,
                src.UnitCode));

        // Application mappings
        CreateMap<Domain.Entities.Application, ApplicationDto>()
            .ForMember(dest => dest.SparshId, opt => opt.MapFrom(src => src.ContactInfo.SparshId))
            .ForMember(dest => dest.SparshPin, opt => opt.MapFrom(src => src.ContactInfo.SparshPin))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.CurrentJobDescription, opt => opt.MapFrom(src => src.CurrentJobDesciption));

        CreateMap<ApplicationDto, Domain.Entities.Application>().ReverseMap();

        // CourseDetail mappings
        CreateMap<CourseDetail, CourseDetailDto>().ReverseMap();

        // ApplicationStatusHistory mappings
        CreateMap<ApplicationStatusHistory, ApplicationStatusHistoryDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}
