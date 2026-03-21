using AutoMapper;
using MasterDataService.Application.DTOs;
using MasterDataService.Domain.Exceptions;
using MasterDataService.Domain.Interfaces;
using MediatR;

namespace MasterDataService.Application.Commands.Route;

public class CreateRouteCommandHandler : IRequestHandler<CreateRouteCommand, RouteDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateRouteCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<RouteDto> Handle(CreateRouteCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Route(request.RouteId, request.RouteName);
        await _unitOfWork.Routes.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<RouteDto>(entity);
    }
}

public class UpdateRouteCommandHandler : IRequestHandler<UpdateRouteCommand, RouteDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateRouteCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<RouteDto> Handle(UpdateRouteCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Routes.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Domain.Entities.Route), request.Id);

        entity.UpdateName(request.RouteName);
        await _unitOfWork.Routes.UpdateAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<RouteDto>(entity);
    }
}

public class DeleteRouteCommandHandler : IRequestHandler<DeleteRouteCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRouteCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> Handle(DeleteRouteCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Routes.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Domain.Entities.Route), request.Id);

        await _unitOfWork.Routes.DeleteAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
