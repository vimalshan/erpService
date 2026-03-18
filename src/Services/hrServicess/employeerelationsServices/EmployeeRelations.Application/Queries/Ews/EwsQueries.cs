using MediatR;
using EmployeeRelations.Application.DTOs;
using EmployeeRelations.Domain.Interfaces;
using EmployeeRelations.Domain.Exceptions;
using AutoMapper;

namespace EmployeeRelations.Application.Queries.Ews;

public record GetEwsByIdQuery(long Id) : IRequest<EwsMainDto>;
public record GetEwsByEmpQuery(long EmpSysId) : IRequest<IEnumerable<EwsMainDto>>;
public record GetEwsByPeriodQuery(int PeriodNo) : IRequest<IEnumerable<EwsMainDto>>;

public class GetEwsByIdHandler : IRequestHandler<GetEwsByIdQuery, EwsMainDto>
{
    private readonly IEwsRepository _repo;
    private readonly IMapper _mapper;

    public GetEwsByIdHandler(IEwsRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<EwsMainDto> Handle(GetEwsByIdQuery req, CancellationToken ct)
    {
        var ews = await _repo.GetByIdAsync(req.Id, ct)
            ?? throw new EntityNotFoundException("EwsMain", req.Id);
        return _mapper.Map<EwsMainDto>(ews);
    }
}

public class GetEwsByEmpHandler : IRequestHandler<GetEwsByEmpQuery, IEnumerable<EwsMainDto>>
{
    private readonly IEwsRepository _repo;
    private readonly IMapper _mapper;

    public GetEwsByEmpHandler(IEwsRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IEnumerable<EwsMainDto>> Handle(GetEwsByEmpQuery req, CancellationToken ct)
    {
        var list = await _repo.GetByEmpAsync(req.EmpSysId, ct);
        return list.Select(_mapper.Map<EwsMainDto>);
    }
}

public class GetEwsByPeriodHandler : IRequestHandler<GetEwsByPeriodQuery, IEnumerable<EwsMainDto>>
{
    private readonly IEwsRepository _repo;
    private readonly IMapper _mapper;

    public GetEwsByPeriodHandler(IEwsRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IEnumerable<EwsMainDto>> Handle(GetEwsByPeriodQuery req, CancellationToken ct)
    {
        var list = await _repo.GetByPeriodAsync(req.PeriodNo, ct);
        return list.Select(_mapper.Map<EwsMainDto>);
    }
}
