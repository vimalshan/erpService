using AutoMapper;
using MasterDataService.Application.DTOs;
using MasterDataService.Domain.Exceptions;
using MasterDataService.Domain.Interfaces;
using MediatR;

namespace MasterDataService.Application.Commands.Area;

public class CreateAreaCommandHandler : IRequestHandler<CreateAreaCommand, AreaDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateAreaCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AreaDto> Handle(CreateAreaCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Area(request.AreaId, request.AreaName);
        await _unitOfWork.Areas.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<AreaDto>(entity);
    }
}

public class UpdateAreaCommandHandler : IRequestHandler<UpdateAreaCommand, AreaDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateAreaCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AreaDto> Handle(UpdateAreaCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Areas.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Domain.Entities.Area), request.Id);

        entity.UpdateName(request.AreaName);
        await _unitOfWork.Areas.UpdateAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<AreaDto>(entity);
    }
}

public class DeleteAreaCommandHandler : IRequestHandler<DeleteAreaCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAreaCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> Handle(DeleteAreaCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Areas.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Domain.Entities.Area), request.Id);

        await _unitOfWork.Areas.DeleteAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
