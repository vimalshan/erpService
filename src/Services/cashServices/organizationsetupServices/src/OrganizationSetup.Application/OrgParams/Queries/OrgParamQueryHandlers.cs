using AutoMapper;
using MediatR;
using OrganizationSetup.Application.DTOs;
using OrganizationSetup.Application.Interfaces;

namespace OrganizationSetup.Application.OrgParams.Queries;

public sealed class GetOrgParamsByOrgQueryHandler : IRequestHandler<GetOrgParamsByOrgQuery, IEnumerable<OrgParamsDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetOrgParamsByOrgQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrgParamsDto>> Handle(GetOrgParamsByOrgQuery request, CancellationToken cancellationToken)
    {
        var parameters = await _unitOfWork.OrgParams.GetByOrgAsync(request.OrgId, cancellationToken);
        return _mapper.Map<IEnumerable<OrgParamsDto>>(parameters);
    }
}

public sealed class GetOrgParamByTypeQueryHandler : IRequestHandler<GetOrgParamByTypeQuery, OrgParamsDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetOrgParamByTypeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OrgParamsDto?> Handle(GetOrgParamByTypeQuery request, CancellationToken cancellationToken)
    {
        var param = await _unitOfWork.OrgParams.GetByTypeAsync(request.OrgId, request.ParamType, cancellationToken);
        return param is null ? null : _mapper.Map<OrgParamsDto>(param);
    }
}
