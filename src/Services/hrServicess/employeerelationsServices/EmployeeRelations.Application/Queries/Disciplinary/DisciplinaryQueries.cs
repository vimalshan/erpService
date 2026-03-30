using MediatR;
using EmployeeRelations.Application.DTOs;
using EmployeeRelations.Application.Mappings;
using EmployeeRelations.Domain.Interfaces;
using EmployeeRelations.Domain.Exceptions;

namespace EmployeeRelations.Application.Queries.Disciplinary;

public record GetDisciplinaryCaseQuery(long Id) : IRequest<DisciplinaryMainDto>;
public record GetAllDisciplinaryCasesQuery : IRequest<IEnumerable<DisciplinaryMainDto>>;

public class GetDisciplinaryCaseHandler : IRequestHandler<GetDisciplinaryCaseQuery, DisciplinaryMainDto>
{
    private readonly IDisciplinaryRepository _repo;

    public GetDisciplinaryCaseHandler(IDisciplinaryRepository repo) { _repo = repo; }

    public async Task<DisciplinaryMainDto> Handle(GetDisciplinaryCaseQuery req, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(req.Id, ct)
            ?? throw new EntityNotFoundException("DisciplinaryMain", req.Id);
        return entity.ToDto();
    }
}

public class GetAllDisciplinaryCasesHandler : IRequestHandler<GetAllDisciplinaryCasesQuery, IEnumerable<DisciplinaryMainDto>>
{
    private readonly IDisciplinaryRepository _repo;

    public GetAllDisciplinaryCasesHandler(IDisciplinaryRepository repo) { _repo = repo; }

    public async Task<IEnumerable<DisciplinaryMainDto>> Handle(GetAllDisciplinaryCasesQuery req, CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return entities.Select(e => e.ToDto());
    }
}
