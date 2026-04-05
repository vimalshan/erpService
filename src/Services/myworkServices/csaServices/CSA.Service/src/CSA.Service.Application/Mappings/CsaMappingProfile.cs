using AutoMapper;
using CSA.Service.Application.DTOs;
using CSA.Service.Domain.Entities;

namespace CSA.Service.Application.Mappings;

public class CsaMappingProfile : Profile
{
    public CsaMappingProfile()
    {
        CreateMap<Control, ControlDto>()
            .ForMember(d => d.ControlType, o => o.MapFrom(s => s.ControlType.HasValue ? s.ControlType.Value.ToString() : null))
            .ForMember(d => d.ControlMethod, o => o.MapFrom(s => s.ControlMethod.HasValue ? s.ControlMethod.Value.ToString() : null))
            .ForMember(d => d.Priority, o => o.MapFrom(s => s.Priority.HasValue ? s.Priority.Value.ToString() : null))
            .ForMember(d => d.Periodicity, o => o.MapFrom(s => s.Periodicity.HasValue ? s.Periodicity.Value.ToString() : null))
            .ForMember(d => d.EvidenceFlag, o => o.MapFrom(s => s.EvidenceFlag.HasValue ? s.EvidenceFlag.Value.ToString() : null))
            .ForMember(d => d.ApproverFlag, o => o.MapFrom(s => s.ApproverFlag.HasValue ? s.ApproverFlag.Value.ToString() : null));
        CreateMap<CreateControlDto, Control>()
            .ForMember(d => d.ControlType, o => o.MapFrom(s => string.IsNullOrEmpty(s.ControlType) ? (char?)null : s.ControlType[0]))
            .ForMember(d => d.ControlMethod, o => o.MapFrom(s => string.IsNullOrEmpty(s.ControlMethod) ? (char?)null : s.ControlMethod[0]))
            .ForMember(d => d.Priority, o => o.MapFrom(s => string.IsNullOrEmpty(s.Priority) ? (char?)null : s.Priority[0]))
            .ForMember(d => d.Periodicity, o => o.MapFrom(s => string.IsNullOrEmpty(s.Periodicity) ? (char?)null : s.Periodicity[0]))
            .ForMember(d => d.EvidenceFlag, o => o.MapFrom(s => string.IsNullOrEmpty(s.EvidenceFlag) ? (char?)null : s.EvidenceFlag[0]))
            .ForMember(d => d.ApproverFlag, o => o.MapFrom(s => string.IsNullOrEmpty(s.ApproverFlag) ? (char?)null : s.ApproverFlag[0]));
        CreateMap<UpdateControlDto, Control>()
            .ForMember(d => d.ControlType, o => o.MapFrom(s => string.IsNullOrEmpty(s.ControlType) ? (char?)null : s.ControlType[0]))
            .ForMember(d => d.ControlMethod, o => o.MapFrom(s => string.IsNullOrEmpty(s.ControlMethod) ? (char?)null : s.ControlMethod[0]))
            .ForMember(d => d.Priority, o => o.MapFrom(s => string.IsNullOrEmpty(s.Priority) ? (char?)null : s.Priority[0]))
            .ForMember(d => d.Periodicity, o => o.MapFrom(s => string.IsNullOrEmpty(s.Periodicity) ? (char?)null : s.Periodicity[0]))
            .ForMember(d => d.EvidenceFlag, o => o.MapFrom(s => string.IsNullOrEmpty(s.EvidenceFlag) ? (char?)null : s.EvidenceFlag[0]))
            .ForMember(d => d.ApproverFlag, o => o.MapFrom(s => string.IsNullOrEmpty(s.ApproverFlag) ? (char?)null : s.ApproverFlag[0]));
        CreateMap<Evidence, EvidenceDto>();
        CreateMap<Survey, SurveyDto>();
        CreateMap<CreateSurveyDto, Survey>();
        CreateMap<SurveyQuestion, SurveyQuestionDto>()
            .ForMember(d => d.AssessmentFlag, o => o.MapFrom(s => s.AssessmentFlag.HasValue ? s.AssessmentFlag.Value.ToString() : null))
            .ForMember(d => d.ApprovalFlag, o => o.MapFrom(s => s.ApprovalFlag.HasValue ? s.ApprovalFlag.Value.ToString() : null))
            .ForMember(d => d.RemedialFlag, o => o.MapFrom(s => s.RemedialFlag.HasValue ? s.RemedialFlag.Value.ToString() : null));
        CreateMap<SurveyFeedback, SurveyFeedbackDto>();
        CreateMap<Process, ProcessDto>();
        CreateMap<CreateProcessDto, Process>()
            .ForMember(d => d.ProcessId, opt => opt.Ignore());
        CreateMap<SubProcess, SubProcessDto>();
        CreateMap<CreateSubProcessDto, SubProcess>()
            .ForMember(d => d.SubProcessId, opt => opt.Ignore());
        CreateMap<Unit, UnitDto>();
        CreateMap<UnitMapDetail, UnitMapDetailDto>();
    }
}
