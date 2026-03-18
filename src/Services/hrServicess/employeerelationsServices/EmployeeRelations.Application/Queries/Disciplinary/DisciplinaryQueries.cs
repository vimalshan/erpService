using MediatR;
using EmployeeRelations.Application.DTOs;
using EmployeeRelations.Domain.Interfaces;
using EmployeeRelations.Domain.Exceptions;
using AutoMapper;

namespace EmployeeRelations.Application.Queries.Disciplinary;

public record GetDisciplinaryCaseQuery(long Id) : IRequest<DisciplinaryMainDto>;
public record GetAllDisciplinaryCasesQuery : IRequest<IEnumerable<DisciplinaryMainDto>>;

public class GetDisciplinaryCaseHandler : IRequestHandler<GetDisciplinaryCaseQuery, DisciplinaryMainDto>
{
    private readonly IDisciplinaryRepository _repo;
    private readonly IMapper _mapper;

    public GetDisciplinaryCaseHandler(IDisciplinaryRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<DisciplinaryMainDto> Handle(GetDisciplinaryCaseQuery req, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(req.Id, ct)
            ?? throw new EntityNotFoundException("DisciplinaryMain", req.Id);
        return _mapper.Map<DisciplinaryMainDto>(entity);
    }
}

public class GetAllDisciplinaryCasesHandler : IRequestHandler<GetAllDisciplinaryCasesQuery, IEnumerable<DisciplinaryMainDto>>
{
    private readonly IDisciplinaryRepository _repo;
    private readonly IMapper _mapper;

    public GetAllDisciplinaryCasesHandler(IDisciplinaryRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IEnumerable<DisciplinaryMainDto>> Handle(GetAllDisciplinaryCasesQuery req, CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync(ct);
        return entities.Select(_mapper.Map<DisciplinaryMainDto>);
    }
}
