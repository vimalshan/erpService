using AutoMapper;
using Document.Application.DTOs;
using Document.Domain.Entities;

namespace Document.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Signatory, SignatoryDto>().ReverseMap();
        CreateMap<AppraisalLetter, AppraisalLetterDto>().ReverseMap();
        CreateMap<GeneratedLetter, GeneratedLetterDto>().ReverseMap();
        CreateMap<LetterLogHistory, LetterLogHistoryDto>().ReverseMap();
    }
}
