using AutoMapper;
using MediatR;
using OrganizationSetup.Application.DTOs;
using OrganizationSetup.Application.Interfaces;
using OrganizationSetup.Domain.Entities;

namespace OrganizationSetup.Application.UserMaps.Commands;

public sealed class CreateUserMapCommandHandler : IRequestHandler<CreateUserMapCommand, UserMapDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateUserMapCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserMapDto> Handle(CreateUserMapCommand request, CancellationToken cancellationToken)
    {
        var map = DealUserMap.Create(request.MapId, request.RoleId, request.EmpSysId, request.OrgId, request.Business);
        await _unitOfWork.UserMaps.AddAsync(map, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<UserMapDto>(map);
    }
}

public sealed class DeleteUserMapCommandHandler : IRequestHandler<DeleteUserMapCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserMapCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteUserMapCommand request, CancellationToken cancellationToken)
    {
        var map = await _unitOfWork.UserMaps.GetByIdAsync(request.MapId, cancellationToken);
        if (map is null) return false;
        await _unitOfWork.UserMaps.DeleteAsync(request.MapId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
