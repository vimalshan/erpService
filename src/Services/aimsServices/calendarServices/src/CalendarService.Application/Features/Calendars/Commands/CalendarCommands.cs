using AutoMapper;
using CalendarService.Application.DTOs;
using CalendarService.Domain.Entities;
using CalendarService.Domain.Exceptions;
using CalendarService.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace CalendarService.Application.Features.Calendars.Commands;

// ─── Create ───────────────────────────────────────────────────────────────────
public record CreateCalendarCommand(string Name, int UnitId, DateTime EffDate, long CreatedBy) : IRequest<CalendarDto>;

public class CreateCalendarValidator : AbstractValidator<CreateCalendarCommand>
{
    public CreateCalendarValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.UnitId).GreaterThan(0);
        RuleFor(x => x.EffDate).NotEmpty();
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}

public class CreateCalendarHandler(ICalendarRepository repo, IMapper mapper)
    : IRequestHandler<CreateCalendarCommand, CalendarDto>
{
    public async Task<CalendarDto> Handle(CreateCalendarCommand cmd, CancellationToken ct)
    {
        if (await repo.ExistsByNameAsync(cmd.Name, ct))
            throw new DuplicateCalendarNameException(cmd.Name);

        var id = await repo.GetNextIdAsync(ct);
        var entity = CalendarMaster.Create(id, cmd.Name, cmd.UnitId, cmd.EffDate, cmd.CreatedBy);
        await repo.AddAsync(entity, ct);
        return mapper.Map<CalendarDto>(entity);
    }
}

// ─── Update ───────────────────────────────────────────────────────────────────
public record UpdateCalendarCommand(int Id, string Name, int UnitId, long ModifiedBy) : IRequest<CalendarDto>;

public class UpdateCalendarHandler(ICalendarRepository repo, IMapper mapper)
    : IRequestHandler<UpdateCalendarCommand, CalendarDto>
{
    public async Task<CalendarDto> Handle(UpdateCalendarCommand cmd, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(cmd.Id, ct)
            ?? throw new CalendarNotFoundException(cmd.Id);
        entity.Update(cmd.Name, cmd.UnitId, cmd.ModifiedBy);
        await repo.UpdateAsync(entity, ct);
        return mapper.Map<CalendarDto>(entity);
    }
}

// ─── Close ────────────────────────────────────────────────────────────────────
public record CloseCalendarCommand(int Id, DateTime CloseDate, long ModifiedBy) : IRequest<bool>;

public class CloseCalendarHandler(ICalendarRepository repo)
    : IRequestHandler<CloseCalendarCommand, bool>
{
    public async Task<bool> Handle(CloseCalendarCommand cmd, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(cmd.Id, ct)
            ?? throw new CalendarNotFoundException(cmd.Id);
        entity.Close(cmd.CloseDate, cmd.ModifiedBy);
        await repo.UpdateAsync(entity, ct);
        return true;
    }
}
