using AutoMapper;
using CSA.Service.Application.DTOs;
using CSA.Service.Domain.Entities;

namespace CSA.Service.Application.Mappings;

public class CsaMappingProfile : Profile
{
    public CsaMappingProfile()
    {
        CreateMap<Control, ControlDto>();
        CreateMap<CreateControlDto, Control>();
        CreateMap<UpdateControlDto, Control>();
        CreateMap<Evidence, EvidenceDto>();
        CreateMap<Survey, SurveyDto>();
        CreateMap<CreateSurveyDto, Survey>();
        CreateMap<SurveyQuestion, SurveyQuestionDto>();
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
