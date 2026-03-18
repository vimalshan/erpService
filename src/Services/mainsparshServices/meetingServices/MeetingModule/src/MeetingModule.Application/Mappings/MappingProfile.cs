using AutoMapper;
using MeetingModule.Application.DTOs;
using MeetingModule.Domain.Entities;

namespace MeetingModule.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<MeetingType, MeetingTypeDto>();

        CreateMap<MeetingSchedule, MeetingScheduleDto>()
            .ForMember(d => d.MeetTypeName, opt => opt.MapFrom(s => s.MeetingType != null ? s.MeetingType.MeetTypeName : null))
            .ForMember(d => d.Polls, opt => opt.MapFrom(s => s.Polls));

        CreateMap<PollDetail, PollDetailDto>();
    }
}
