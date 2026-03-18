using AutoMapper;
using CalendarService.Application.DTOs;
using CalendarService.Domain.Entities;
using CalendarService.Domain.Exceptions;
using CalendarService.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace CalendarService.Application.Features.Shifts.Commands;

// ─── Create ───────────────────────────────────────────────────────────────────
public record CreateShiftCommand(string Code, string Name, string InTime, string OutTime, long CreatedBy) : IRequest<ShiftDto>;

public class CreateShiftValidator : AbstractValidator<CreateShiftCommand>
{
    public CreateShiftValidator()
    {
        RuleFor(x => x.Code).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.InTime).NotEmpty().Must(t => TimeOnly.TryParse(t, out _)).WithMessage("InTime must be HH:mm");
        RuleFor(x => x.OutTime).NotEmpty().Must(t => TimeOnly.TryParse(t, out _)).WithMessage("OutTime must be HH:mm");
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}

public class CreateShiftHandler(IShiftRepository repo, IMapper mapper)
    : IRequestHandler<CreateShiftCommand, ShiftDto>
{
    public async Task<ShiftDto> Handle(CreateShiftCommand cmd, CancellationToken ct)
    {
        if (await repo.ExistsByCodeAsync(cmd.Code, ct))
            throw new DuplicateShiftCodeException(cmd.Code);

        var id = await repo.GetNextIdAsync(ct);
        var entity = ShiftMaster.Create(id, cmd.Code, cmd.Name,
            TimeOnly.Parse(cmd.InTime), TimeOnly.Parse(cmd.OutTime), cmd.CreatedBy);
        await repo.AddAsync(entity, ct);
        return mapper.Map<ShiftDto>(entity);
    }
}

// ─── Update ───────────────────────────────────────────────────────────────────
public record UpdateShiftCommand(int Id, string Name, string InTime, string OutTime, long ModifiedBy) : IRequest<ShiftDto>;

public class UpdateShiftHandler(IShiftRepository repo, IMapper mapper)
    : IRequestHandler<UpdateShiftCommand, ShiftDto>
{
    public async Task<ShiftDto> Handle(UpdateShiftCommand cmd, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(cmd.Id, ct)
            ?? throw new ShiftNotFoundException(cmd.Id);
        entity.Update(cmd.Name, TimeOnly.Parse(cmd.InTime), TimeOnly.Parse(cmd.OutTime), cmd.ModifiedBy);
        await repo.UpdateAsync(entity, ct);
        return mapper.Map<ShiftDto>(entity);
    }
}
