using AutoMapper;
using CalendarService.Application.DTOs;
using CalendarService.Domain.Exceptions;
using CalendarService.Domain.Interfaces;
using MediatR;

namespace CalendarService.Application.Features.Calendars.Queries;

public record GetCalendarByIdQuery(int Id) : IRequest<CalendarDto>;
public record GetAllCalendarsQuery : IRequest<IEnumerable<CalendarDto>>;

public class GetCalendarByIdHandler(ICalendarRepository repo, IMapper mapper)
    : IRequestHandler<GetCalendarByIdQuery, CalendarDto>
{
    public async Task<CalendarDto> Handle(GetCalendarByIdQuery q, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(q.Id, ct)
            ?? throw new CalendarNotFoundException(q.Id);
        return mapper.Map<CalendarDto>(entity);
    }
}

public class GetAllCalendarsHandler(ICalendarRepository repo, IMapper mapper)
    : IRequestHandler<GetAllCalendarsQuery, IEnumerable<CalendarDto>>
{
    public async Task<IEnumerable<CalendarDto>> Handle(GetAllCalendarsQuery q, CancellationToken ct)
    {
        var entities = await repo.GetAllAsync(ct);
        return mapper.Map<IEnumerable<CalendarDto>>(entities);
    }
}
