using AutoMapper;
using MediatR;
using MeetingModule.Application.DTOs;
using MeetingModule.Domain.Interfaces;

namespace MeetingModule.Application.Queries.Meetings;

public class GetAllMeetingsHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetAllMeetingsQuery, IReadOnlyList<MeetingScheduleDto>>
{
    public async Task<IReadOnlyList<MeetingScheduleDto>> Handle(GetAllMeetingsQuery request, CancellationToken ct)
    {
        var entities = await uow.MeetingSchedules.GetAllAsync(ct);
        return mapper.Map<IReadOnlyList<MeetingScheduleDto>>(entities);
    }
}

public class GetMeetingByIdHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetMeetingByIdQuery, MeetingScheduleDto?>
{
    public async Task<MeetingScheduleDto?> Handle(GetMeetingByIdQuery request, CancellationToken ct)
    {
        var entity = await uow.MeetingSchedules.GetByIdWithPollsAsync(request.Id, ct);
        return entity is null ? null : mapper.Map<MeetingScheduleDto>(entity);
    }
}

public class GetMeetingsByDateRangeHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetMeetingsByDateRangeQuery, IReadOnlyList<MeetingScheduleDto>>
{
    public async Task<IReadOnlyList<MeetingScheduleDto>> Handle(GetMeetingsByDateRangeQuery request, CancellationToken ct)
    {
        var entities = await uow.MeetingSchedules.GetByDateRangeAsync(request.From, request.To, ct);
        return mapper.Map<IReadOnlyList<MeetingScheduleDto>>(entities);
    }
}

public class GetMeetingsByStatusHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetMeetingsByStatusQuery, IReadOnlyList<MeetingScheduleDto>>
{
    public async Task<IReadOnlyList<MeetingScheduleDto>> Handle(GetMeetingsByStatusQuery request, CancellationToken ct)
    {
        var entities = await uow.MeetingSchedules.GetByStatusAsync(request.Status, ct);
        return mapper.Map<IReadOnlyList<MeetingScheduleDto>>(entities);
    }
}

public class GetMeetingsByOrganizerHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetMeetingsByOrganizerQuery, IReadOnlyList<MeetingScheduleDto>>
{
    public async Task<IReadOnlyList<MeetingScheduleDto>> Handle(GetMeetingsByOrganizerQuery request, CancellationToken ct)
    {
        var entities = await uow.MeetingSchedules.GetByOrganizerAsync(request.OrganizerId, ct);
        return mapper.Map<IReadOnlyList<MeetingScheduleDto>>(entities);
    }
}
