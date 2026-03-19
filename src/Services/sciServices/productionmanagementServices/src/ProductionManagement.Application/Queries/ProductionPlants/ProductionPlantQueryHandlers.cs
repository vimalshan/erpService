using AutoMapper;
using MediatR;
using ProductionManagement.Application.DTOs;
using ProductionManagement.Domain.Interfaces;

namespace ProductionManagement.Application.Queries.ProductionPlants;

public class GetAllProductionPlantsHandler : IRequestHandler<GetAllProductionPlantsQuery, IReadOnlyList<ProductionPlantDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllProductionPlantsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ProductionPlantDto>> Handle(GetAllProductionPlantsQuery request, CancellationToken cancellationToken)
    {
        var plants = await _unitOfWork.ProductionPlants.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<ProductionPlantDto>>(plants);
    }
}

public class GetProductionPlantByIdHandler : IRequestHandler<GetProductionPlantByIdQuery, ProductionPlantDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductionPlantByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductionPlantDto?> Handle(GetProductionPlantByIdQuery request, CancellationToken cancellationToken)
    {
        var plant = await _unitOfWork.ProductionPlants.GetByIdAsync(request.ProductionPlantId, cancellationToken);
        return plant is null ? null : _mapper.Map<ProductionPlantDto>(plant);
    }
}
