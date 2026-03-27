using AutoMapper;
using MediatR;
using OrganizationSetup.Application.DTOs;
using OrganizationSetup.Application.Interfaces;

namespace OrganizationSetup.Application.UserMaps.Queries;

public sealed class GetUserMapsByOrgQueryHandler : IRequestHandler<GetUserMapsByOrgQuery, IEnumerable<UserMapDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserMapsByOrgQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserMapDto>> Handle(GetUserMapsByOrgQuery request, CancellationToken cancellationToken)
    {
        var maps = await _unitOfWork.UserMaps.GetByOrgAsync(request.OrgId, cancellationToken);
        return _mapper.Map<IEnumerable<UserMapDto>>(maps);
    }
}

public sealed class GetUserMapsByEmployeeQueryHandler : IRequestHandler<GetUserMapsByEmployeeQuery, IEnumerable<UserMapDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserMapsByEmployeeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserMapDto>> Handle(GetUserMapsByEmployeeQuery request, CancellationToken cancellationToken)
    {
        var maps = await _unitOfWork.UserMaps.GetByEmployeeAsync(request.EmpSysId, cancellationToken);
        return _mapper.Map<IEnumerable<UserMapDto>>(maps);
    }
}
