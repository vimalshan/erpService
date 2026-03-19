using AutoMapper;
using MediatR;
using ProductionManagement.Application.DTOs;
using ProductionManagement.Domain.Entities;
using ProductionManagement.Domain.Interfaces;

namespace ProductionManagement.Application.Commands.ProductionPlants;

public class CreateProductionPlantHandler : IRequestHandler<CreateProductionPlantCommand, ProductionPlantDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateProductionPlantHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductionPlantDto> Handle(CreateProductionPlantCommand request, CancellationToken cancellationToken)
    {
        var plant = new ProductionPlant(
            request.Dto.CompanyUnitId,
            request.Dto.PlantName,
            request.Dto.Location,
            request.Dto.CreatedBy);

        var result = await _unitOfWork.ProductionPlants.AddAsync(plant, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProductionPlantDto>(result);
    }
}

public class UpdateProductionPlantHandler : IRequestHandler<UpdateProductionPlantCommand, ProductionPlantDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateProductionPlantHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductionPlantDto> Handle(UpdateProductionPlantCommand request, CancellationToken cancellationToken)
    {
        var plant = await _unitOfWork.ProductionPlants.GetByIdAsync(request.Dto.ProductionPlantId, cancellationToken)
            ?? throw new KeyNotFoundException($"Production plant {request.Dto.ProductionPlantId} not found.");

        plant.Update(request.Dto.PlantName, request.Dto.Location, request.Dto.ModifiedBy);
        await _unitOfWork.ProductionPlants.UpdateAsync(plant, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProductionPlantDto>(plant);
    }
}

public class DeleteProductionPlantHandler : IRequestHandler<DeleteProductionPlantCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductionPlantHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> Handle(DeleteProductionPlantCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.ProductionPlants.DeleteAsync(request.ProductionPlantId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class MapProductToPlantHandler : IRequestHandler<MapProductToPlantCommand, ProductionPlantProductMapDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MapProductToPlantHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductionPlantProductMapDto> Handle(MapProductToPlantCommand request, CancellationToken cancellationToken)
    {
        var plant = await _unitOfWork.ProductionPlants.GetByIdAsync(request.Dto.ProductionPlantId, cancellationToken)
            ?? throw new KeyNotFoundException($"Production plant {request.Dto.ProductionPlantId} not found.");

        plant.AddProductMap(request.Dto.ProductId, request.Dto.CreatedBy);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var map = plant.ProductMaps.First(pm => pm.ProductId == request.Dto.ProductId);
        return _mapper.Map<ProductionPlantProductMapDto>(map);
    }
}
