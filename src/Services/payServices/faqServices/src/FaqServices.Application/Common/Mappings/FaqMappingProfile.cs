using AutoMapper;
using FaqServices.Domain.Entities;
using FaqServices.Application.Common.DTOs;

namespace FaqServices.Application.Common.Mappings;

public class FaqMappingProfile : Profile
{
    public FaqMappingProfile()
    {
        CreateMap<FaqGrade, FaqGradeDto>()
            .ForMember(d => d.QuestionCount, o => o.MapFrom(s => s.Questions.Count));

        CreateMap<FaqQuestion, FaqQuestionDto>()
            .ForMember(d => d.GradeName, o => o.MapFrom(s => s.Grade != null ? s.Grade.GradeName : string.Empty));

        CreateMap<FaqAnswer, FaqAnswerDto>();
    }
}
