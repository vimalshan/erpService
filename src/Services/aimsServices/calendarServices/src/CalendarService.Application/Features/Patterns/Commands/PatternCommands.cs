using AutoMapper;
using CalendarService.Application.DTOs;
using CalendarService.Domain.Entities;
using CalendarService.Domain.Exceptions;
using CalendarService.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace CalendarService.Application.Features.Patterns.Commands;

public record CreatePatternCommand(string Name, int CycleId, long CreatedBy, string? Description = null) : IRequest<PatternDto>;

public class CreatePatternValidator : AbstractValidator<CreatePatternCommand>
{
    public CreatePatternValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CycleId).GreaterThan(0);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}

public class CreatePatternHandler(IPatternRepository repo, IMapper mapper)
    : IRequestHandler<CreatePatternCommand, PatternDto>
{
    public async Task<PatternDto> Handle(CreatePatternCommand cmd, CancellationToken ct)
    {
        var id = await repo.GetNextIdAsync(ct);
        var entity = PatternMaster.Create(id, cmd.Name, cmd.CycleId, cmd.CreatedBy, cmd.Description);
        await repo.AddAsync(entity, ct);
        return mapper.Map<PatternDto>(entity);
    }
}

public record UpdatePatternCommand(int Id, string Name, string? Description, long ModifiedBy) : IRequest<PatternDto>;

public class UpdatePatternHandler(IPatternRepository repo, IMapper mapper)
    : IRequestHandler<UpdatePatternCommand, PatternDto>
{
    public async Task<PatternDto> Handle(UpdatePatternCommand cmd, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(cmd.Id, ct)
            ?? throw new PatternNotFoundException(cmd.Id);
        entity.Update(cmd.Name, cmd.Description, cmd.ModifiedBy);
        await repo.UpdateAsync(entity, ct);
        return mapper.Map<PatternDto>(entity);
    }
}
