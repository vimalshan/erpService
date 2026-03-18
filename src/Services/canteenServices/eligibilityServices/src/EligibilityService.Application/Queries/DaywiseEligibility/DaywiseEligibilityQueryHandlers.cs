using AutoMapper;
using MediatR;
using EligibilityService.Application.DTOs;
using EligibilityService.Application.Queries.DaywiseEligibility;
using EligibilityService.Domain.Interfaces;

namespace EligibilityService.Application.Queries.DaywiseEligibility;

public class GetDaywiseEligibilityHandler : IRequestHandler<GetDaywiseEligibilityQuery, DaywiseEligibilityDto?>
{
    private readonly IDaywiseEligibilityRepository _repo;
    private readonly IMapper _mapper;

    public GetDaywiseEligibilityHandler(IDaywiseEligibilityRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<DaywiseEligibilityDto?> Handle(GetDaywiseEligibilityQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetBySerialNumberAsync(request.SerialNumber, cancellationToken);
        return entity is null ? null : _mapper.Map<DaywiseEligibilityDto>(entity);
    }
}

public class GetDaywiseEligibilityByEmployeeHandler : IRequestHandler<GetDaywiseEligibilityByEmployeeQuery, IEnumerable<DaywiseEligibilityDto>>
{
    private readonly IDaywiseEligibilityRepository _repo;
    private readonly IMapper _mapper;

    public GetDaywiseEligibilityByEmployeeHandler(IDaywiseEligibilityRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DaywiseEligibilityDto>> Handle(GetDaywiseEligibilityByEmployeeQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repo.GetByEmployeeAsync(request.CompanyCode, request.EmployeeSysId, cancellationToken);
        return _mapper.Map<IEnumerable<DaywiseEligibilityDto>>(entities);
    }
}

public class GetDaywiseEligibilityByDateHandler : IRequestHandler<GetDaywiseEligibilityByDateQuery, IEnumerable<DaywiseEligibilityDto>>
{
    private readonly IDaywiseEligibilityRepository _repo;
    private readonly IMapper _mapper;

    public GetDaywiseEligibilityByDateHandler(IDaywiseEligibilityRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DaywiseEligibilityDto>> Handle(GetDaywiseEligibilityByDateQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repo.GetByDateAsync(request.CompanyCode, request.Date, cancellationToken);
        return _mapper.Map<IEnumerable<DaywiseEligibilityDto>>(entities);
    }
}
