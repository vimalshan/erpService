using AutoMapper;
using MediatR;
using MeetingModule.Application.DTOs;
using MeetingModule.Domain.Interfaces;

namespace MeetingModule.Application.Queries.Polls;

public class GetPollsByMeetingIdHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetPollsByMeetingIdQuery, IReadOnlyList<PollDetailDto>>
{
    public async Task<IReadOnlyList<PollDetailDto>> Handle(GetPollsByMeetingIdQuery request, CancellationToken ct)
    {
        var entities = await uow.PollDetails.GetByMeetingIdAsync(request.MeetingId, ct);
        return mapper.Map<IReadOnlyList<PollDetailDto>>(entities);
    }
}

public class GetPollByIdHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetPollByIdQuery, PollDetailDto?>
{
    public async Task<PollDetailDto?> Handle(GetPollByIdQuery request, CancellationToken ct)
    {
        var entity = await uow.PollDetails.GetByIdAsync(request.Id, ct);
        return entity is null ? null : mapper.Map<PollDetailDto>(entity);
    }
}
