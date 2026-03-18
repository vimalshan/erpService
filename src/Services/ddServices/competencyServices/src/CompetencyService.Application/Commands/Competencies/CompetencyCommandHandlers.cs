using MediatR;
using AutoMapper;
using CompetencyService.Application.DTOs;
using CompetencyService.Domain.Entities;
using CompetencyService.Domain.Interfaces;

namespace CompetencyService.Application.Commands.Competencies;

public class CreateCompetencyCommandHandler(ICompetencyRepository repo, IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<CreateCompetencyCommand, CompetencyDto>
{
    public async Task<CompetencyDto> Handle(CreateCompetencyCommand cmd, CancellationToken ct)
    {
        var entity = CompetencyMaster.Create(
            cmd.Id, cmd.Name, cmd.EffectiveDate, cmd.CompetencyType, cmd.ParentId,
            cmd.Remarks, cmd.JobCode, cmd.PositiveIndicator, cmd.NegativeIndicator, cmd.SelfDescription);
        await repo.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<CompetencyDto>(entity);
    }
}

public class UpdateCompetencyCommandHandler(ICompetencyRepository repo, IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<UpdateCompetencyCommand, CompetencyDto>
{
    public async Task<CompetencyDto> Handle(UpdateCompetencyCommand cmd, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(cmd.Id, ct)
            ?? throw new KeyNotFoundException($"Competency {cmd.Id} not found.");
        entity.Update(cmd.Name, cmd.EffectiveDate, cmd.ClosureDate, cmd.Remarks, cmd.CompetencyType, cmd.ModifiedBy);
        repo.Update(entity);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<CompetencyDto>(entity);
    }
}

public class CloseCompetencyCommandHandler(ICompetencyRepository repo, IUnitOfWork uow)
    : IRequestHandler<CloseCompetencyCommand, bool>
{
    public async Task<bool> Handle(CloseCompetencyCommand cmd, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(cmd.Id, ct)
            ?? throw new KeyNotFoundException($"Competency {cmd.Id} not found.");
        entity.Close(cmd.ClosureDate, cmd.ModifiedBy);
        repo.Update(entity);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}

public class DeleteCompetencyCommandHandler(ICompetencyRepository repo, IUnitOfWork uow)
    : IRequestHandler<DeleteCompetencyCommand, bool>
{
    public async Task<bool> Handle(DeleteCompetencyCommand cmd, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(cmd.Id, ct)
            ?? throw new KeyNotFoundException($"Competency {cmd.Id} not found.");
        repo.Delete(entity);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}
