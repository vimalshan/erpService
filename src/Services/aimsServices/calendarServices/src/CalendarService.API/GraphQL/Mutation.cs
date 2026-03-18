using CalendarService.Application.DTOs;
using CalendarService.Application.Features.Calendars.Commands;
using CalendarService.Application.Features.Holidays.Commands;
using CalendarService.Application.Features.Patterns.Commands;
using CalendarService.Application.Features.Shifts.Commands;
using MediatR;

namespace CalendarService.API.GraphQL;

public class Mutation
{
    public async Task<CalendarDto> CreateCalendar(CreateCalendarCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<HolidayDto> CreateHoliday(CreateHolidayCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<ShiftDto> CreateShift(CreateShiftCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<PatternDto> CreatePattern(CreatePatternCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);
}
