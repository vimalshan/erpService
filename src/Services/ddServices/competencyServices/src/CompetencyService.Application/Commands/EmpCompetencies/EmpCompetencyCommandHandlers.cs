using MediatR;
using AutoMapper;
using CompetencyService.Application.DTOs;
using CompetencyService.Domain.Entities;
using CompetencyService.Domain.Interfaces;

namespace CompetencyService.Application.Commands.EmpCompetencies;

public class AssignEmpCompetencyCommandHandler(
    IEmpSpecificCompetencyRepository repo, IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<AssignEmpCompetencyCommand, EmpSpecificCompetencyDto>
{
    public async Task<EmpSpecificCompetencyDto> Handle(AssignEmpCompetencyCommand cmd, CancellationToken ct)
    {
        var entity = EmpSpecificCompetency.Assign(
            cmd.EmpSysId, cmd.CompetencyId, cmd.CompetencyType, cmd.YearId, cmd.ModifiedBy);
        await repo.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<EmpSpecificCompetencyDto>(entity);
    }
}
