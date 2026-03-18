using AutoMapper;
using MediatR;
using EligibilityService.Application.DTOs;
using EligibilityService.Application.Queries.EligibilityMaster;
using EligibilityService.Domain.Aggregates;
using EligibilityService.Domain.Interfaces;

namespace EligibilityService.Application.Queries.EligibilityMaster;

public class GetEligibilityMasterHandler : IRequestHandler<GetEligibilityMasterQuery, EligibilityMasterDto?>
{
    private readonly IEligibilityMasterRepository _repo;
    private readonly IMapper _mapper;

    public GetEligibilityMasterHandler(IEligibilityMasterRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<EligibilityMasterDto?> Handle(GetEligibilityMasterQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetAsync(request.CanteenUnit, request.ShiftCode, request.ItemCode, cancellationToken);
        return entity is null ? null : _mapper.Map<EligibilityMasterDto>(entity);
    }
}

public class GetAllEligibilityMastersHandler : IRequestHandler<GetAllEligibilityMastersQuery, IEnumerable<EligibilityMasterDto>>
{
    private readonly IEligibilityMasterRepository _repo;
    private readonly IMapper _mapper;

    public GetAllEligibilityMastersHandler(IEligibilityMasterRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EligibilityMasterDto>> Handle(GetAllEligibilityMastersQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repo.GetAllAsync(request.CanteenUnit, cancellationToken);
        return _mapper.Map<IEnumerable<EligibilityMasterDto>>(entities);
    }
}

public class CheckEmployeeEligibilityHandler : IRequestHandler<CheckEmployeeEligibilityQuery, EligibilityCheckResultDto>
{
    private readonly IEligibilityMasterRepository _repo;

    public CheckEmployeeEligibilityHandler(IEligibilityMasterRepository repo) => _repo = repo;

    public async Task<EligibilityCheckResultDto> Handle(CheckEmployeeEligibilityQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetAsync(request.CanteenUnit, request.ShiftCode, request.ItemCode, cancellationToken);
        if (entity is null) return new EligibilityCheckResultDto(false, null);

        var aggregate = EligibilityAggregate.Load(entity);
        return new EligibilityCheckResultDto(aggregate.IsEligible(request.RequestedQty), entity.EligibleLimit);
    }
}

public class GetEligibilityHistoryHandler : IRequestHandler<GetEligibilityHistoryQuery, IEnumerable<EligibilityMasterHistoryDto>>
{
    private readonly IEligibilityMasterHistoryRepository _histRepo;
    private readonly IMapper _mapper;

    public GetEligibilityHistoryHandler(IEligibilityMasterHistoryRepository histRepo, IMapper mapper)
    {
        _histRepo = histRepo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EligibilityMasterHistoryDto>> Handle(GetEligibilityHistoryQuery request, CancellationToken cancellationToken)
    {
        var history = await _histRepo.GetHistoryAsync(request.CanteenUnit, request.ShiftCode, request.ItemCode, cancellationToken);
        return _mapper.Map<IEnumerable<EligibilityMasterHistoryDto>>(history);
    }
}
