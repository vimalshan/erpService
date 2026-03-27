using AutoMapper;
using MediatR;
using OrganizationSetup.Application.DTOs;
using OrganizationSetup.Application.Interfaces;
using OrganizationSetup.Domain.Entities;

namespace OrganizationSetup.Application.OrgParams.Commands;

public sealed class CreateOrgParamCommandHandler : IRequestHandler<CreateOrgParamCommand, OrgParamsDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public CreateOrgParamCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<OrgParamsDto> Handle(CreateOrgParamCommand request, CancellationToken cancellationToken)
    {
        var param = DealOrgParams.Create(request.ParamId, request.ParamType, request.ParamValue, request.OrgId, _currentUserService.UserId ?? 0);
        await _unitOfWork.OrgParams.AddAsync(param, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<OrgParamsDto>(param);
    }
}

public sealed class UpdateOrgParamCommandHandler : IRequestHandler<UpdateOrgParamCommand, OrgParamsDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public UpdateOrgParamCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<OrgParamsDto> Handle(UpdateOrgParamCommand request, CancellationToken cancellationToken)
    {
        var param = await _unitOfWork.OrgParams.GetByIdAsync(request.ParamId, cancellationToken)
            ?? throw new KeyNotFoundException($"OrgParam with ID {request.ParamId} not found.");
        param.UpdateValue(request.NewValue, _currentUserService.UserId ?? 0);
        await _unitOfWork.OrgParams.UpdateAsync(param, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<OrgParamsDto>(param);
    }
}
