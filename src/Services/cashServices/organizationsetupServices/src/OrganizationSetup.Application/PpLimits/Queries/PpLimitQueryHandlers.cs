using AutoMapper;
using MediatR;
using OrganizationSetup.Application.DTOs;
using OrganizationSetup.Application.Interfaces;

namespace OrganizationSetup.Application.PpLimits.Queries;

public sealed class GetPpLimitsByOrgAndYearQueryHandler : IRequestHandler<GetPpLimitsByOrgAndYearQuery, IEnumerable<PpLimitDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPpLimitsByOrgAndYearQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PpLimitDto>> Handle(GetPpLimitsByOrgAndYearQuery request, CancellationToken cancellationToken)
    {
        var limits = await _unitOfWork.PpLimits.GetByOrgAndYearAsync(request.OrgId, request.FinYear, cancellationToken);
        return _mapper.Map<IEnumerable<PpLimitDto>>(limits);
    }
}

public sealed class GetPpLimitByIdQueryHandler : IRequestHandler<GetPpLimitByIdQuery, PpLimitDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPpLimitByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PpLimitDto?> Handle(GetPpLimitByIdQuery request, CancellationToken cancellationToken)
    {
        var limit = await _unitOfWork.PpLimits.GetByIdAsync(request.LimitId, cancellationToken);
        return limit is null ? null : _mapper.Map<PpLimitDto>(limit);
    }
}
