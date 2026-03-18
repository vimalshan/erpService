using AutoMapper;
using CalendarService.Application.DTOs;
using CalendarService.Domain.Entities;
using CalendarService.Domain.Exceptions;
using CalendarService.Domain.Interfaces;
using CalendarService.Domain.ValueObjects;
using FluentValidation;
using MediatR;

namespace CalendarService.Application.Features.Holidays.Commands;

// ─── Create ───────────────────────────────────────────────────────────────────
public record CreateHolidayCommand(DateTime Date, string Description, string Type, long CreatedBy, int? UnitId = null) : IRequest<HolidayDto>;

public class CreateHolidayValidator : AbstractValidator<CreateHolidayCommand>
{
    public CreateHolidayValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Type).Must(t => t == "N" || t == "O").WithMessage("Type must be N or O");
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}

public class CreateHolidayHandler(IHolidayRepository repo, IMapper mapper)
    : IRequestHandler<CreateHolidayCommand, HolidayDto>
{
    public async Task<HolidayDto> Handle(CreateHolidayCommand cmd, CancellationToken ct)
    {
        var type = cmd.Type == "N" ? HolidayType.National : HolidayType.Optional;
        var id = await repo.GetNextIdAsync(ct);
        var entity = HolidayMaster.Create(id, cmd.Date, cmd.Description, type, cmd.CreatedBy, cmd.UnitId);
        await repo.AddAsync(entity, ct);
        return mapper.Map<HolidayDto>(entity);
    }
}

// ─── Update ───────────────────────────────────────────────────────────────────
public record UpdateHolidayCommand(int Id, string Description, string Type, long ModifiedBy) : IRequest<HolidayDto>;

public class UpdateHolidayHandler(IHolidayRepository repo, IMapper mapper)
    : IRequestHandler<UpdateHolidayCommand, HolidayDto>
{
    public async Task<HolidayDto> Handle(UpdateHolidayCommand cmd, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(cmd.Id, ct)
            ?? throw new HolidayNotFoundException(cmd.Id);
        var type = cmd.Type == "N" ? HolidayType.National : HolidayType.Optional;
        entity.Update(cmd.Description, type, cmd.ModifiedBy);
        await repo.UpdateAsync(entity, ct);
        return mapper.Map<HolidayDto>(entity);
    }
}
