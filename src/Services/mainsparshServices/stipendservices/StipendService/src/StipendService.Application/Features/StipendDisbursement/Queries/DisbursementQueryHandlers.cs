using AutoMapper;
using MediatR;
using StipendService.Application.DTOs;
using StipendService.Domain.Interfaces;

namespace StipendService.Application.Features.StipendDisbursement.Queries;

public class GetDisbursementByIdQueryHandler : IRequestHandler<GetDisbursementByIdQuery, StipendDisbursementDto?>
{
    private readonly IStipendDisbursementRepository _repository;
    private readonly IMapper _mapper;

    public GetDisbursementByIdQueryHandler(IStipendDisbursementRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<StipendDisbursementDto?> Handle(GetDisbursementByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.DisbursementId, cancellationToken);
        return entity is null ? null : _mapper.Map<StipendDisbursementDto>(entity);
    }
}

public class GetDisbursementsByMonthQueryHandler : IRequestHandler<GetDisbursementsByMonthQuery, IEnumerable<StipendDisbursementDto>>
{
    private readonly IStipendDisbursementRepository _repository;
    private readonly IMapper _mapper;

    public GetDisbursementsByMonthQueryHandler(IStipendDisbursementRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StipendDisbursementDto>> Handle(GetDisbursementsByMonthQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetByMonthYearAsync(request.MonthYear, cancellationToken);
        return _mapper.Map<IEnumerable<StipendDisbursementDto>>(entities);
    }
}

public class GetDisbursementsBySrfQueryHandler : IRequestHandler<GetDisbursementsBySrfQuery, IEnumerable<StipendDisbursementDto>>
{
    private readonly IStipendDisbursementRepository _repository;
    private readonly IMapper _mapper;

    public GetDisbursementsBySrfQueryHandler(IStipendDisbursementRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StipendDisbursementDto>> Handle(GetDisbursementsBySrfQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetBySrfIdAsync(request.SrfId, cancellationToken);
        return _mapper.Map<IEnumerable<StipendDisbursementDto>>(entities);
    }
}
