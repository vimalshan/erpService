using AutoMapper;
using MasterDataService.Application.DTOs;
using MasterDataService.Domain.Interfaces;
using MediatR;

namespace MasterDataService.Application.Queries.Area;

public class GetAllAreasQueryHandler : IRequestHandler<GetAllAreasQuery, IReadOnlyList<AreaDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllAreasQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<AreaDto>> Handle(GetAllAreasQuery request, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.Areas.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<AreaDto>>(entities);
    }
}

public class GetAreaByIdQueryHandler : IRequestHandler<GetAreaByIdQuery, AreaDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAreaByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AreaDto?> Handle(GetAreaByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Areas.GetByIdAsync(request.Id, cancellationToken);
        return entity is null ? null : _mapper.Map<AreaDto>(entity);
    }
}
