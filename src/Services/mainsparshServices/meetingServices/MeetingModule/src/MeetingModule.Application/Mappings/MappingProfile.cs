using AutoMapper;
using MeetingModule.Application.DTOs;
using MeetingModule.Domain.Entities;

namespace MeetingModule.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<MeetingType, MeetingTypeDto>();

        CreateMap<PollDetail, PollDetailDto>()
            .ConstructUsing((s, _) => new PollDetailDto(
                s.PollId, s.MeetingId, s.PollQuestion, s.PollType, s.PollStatus,
                s.CreatedBy, s.CreatedOn));

        CreateMap<MeetingSchedule, MeetingScheduleDto>()
            .ConstructUsing((s, ctx) => new MeetingScheduleDto(
                s.MeetingId,
                s.MeetTypeId,
                s.MeetingType != null ? s.MeetingType.MeetTypeName : null,
                s.MeetingTitle,
                s.MeetingDate,
                s.MeetingLocation,
                s.MeetingDuration,
                s.OrganizerId,
                s.MeetingStatus,
                s.Notes,
                s.CreatedBy,
                s.CreatedOn,
                s.Polls != null && s.Polls.Count > 0
                    ? ctx.Mapper.Map<List<PollDetailDto>>(s.Polls)
                    : null))
            .ForAllMembers(opt => opt.Ignore());
    }
}
