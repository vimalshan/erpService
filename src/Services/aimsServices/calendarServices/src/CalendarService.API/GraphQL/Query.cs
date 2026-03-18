using CalendarService.Application.DTOs;
using CalendarService.Application.Features.Calendars.Queries;
using CalendarService.Application.Features.Holidays.Queries;
using CalendarService.Application.Features.Patterns.Queries;
using CalendarService.Application.Features.Shifts.Queries;
using MediatR;

namespace CalendarService.API.GraphQL;

public class Query
{
    public async Task<IEnumerable<CalendarDto>> GetCalendars([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllCalendarsQuery(), ct);

    public async Task<CalendarDto?> GetCalendar(int id, [Service] IMediator mediator, CancellationToken ct)
    {
        try { return await mediator.Send(new GetCalendarByIdQuery(id), ct); }
        catch { return null; }
    }

    public async Task<IEnumerable<HolidayDto>> GetHolidays([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllHolidaysQuery(), ct);

    public async Task<IEnumerable<ShiftDto>> GetShifts([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllShiftsQuery(), ct);

    public async Task<IEnumerable<PatternDto>> GetPatterns([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllPatternsQuery(), ct);
}
