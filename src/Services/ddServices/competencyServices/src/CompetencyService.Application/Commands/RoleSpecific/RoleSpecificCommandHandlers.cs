using MediatR;
using AutoMapper;
using CompetencyService.Application.DTOs;
using CompetencyService.Domain.Entities;
using CompetencyService.Domain.Interfaces;

namespace CompetencyService.Application.Commands.RoleSpecific;

public class AssignRoleCompetencyCommandHandler(
    IRoleSpecificRepository repo, IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<AssignRoleCompetencyCommand, RoleSpecificDto>
{
    public async Task<RoleSpecificDto> Handle(AssignRoleCompetencyCommand cmd, CancellationToken ct)
    {
        var entity = Domain.Entities.RoleSpecific.Create(
            cmd.EmpSysId, cmd.CompetencyId, cmd.EffFrom, cmd.EffTo, cmd.ModifiedBy);
        await repo.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<RoleSpecificDto>(entity);
    }
}
