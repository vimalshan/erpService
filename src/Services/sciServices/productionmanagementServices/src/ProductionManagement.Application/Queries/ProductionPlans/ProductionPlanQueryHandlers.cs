using AutoMapper;
using MediatR;
using ProductionManagement.Application.DTOs;
using ProductionManagement.Domain.Interfaces;

namespace ProductionManagement.Application.Queries.ProductionPlans;

public class GetAllProductionPlansHandler : IRequestHandler<GetAllProductionPlansQuery, IReadOnlyList<ProductionPlanDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllProductionPlansHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ProductionPlanDto>> Handle(GetAllProductionPlansQuery request, CancellationToken cancellationToken)
    {
        var plans = await _unitOfWork.ProductionPlans.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<ProductionPlanDto>>(plans);
    }
}

public class GetProductionPlansByPlantIdHandler : IRequestHandler<GetProductionPlansByPlantIdQuery, IReadOnlyList<ProductionPlanDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductionPlansByPlantIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ProductionPlanDto>> Handle(GetProductionPlansByPlantIdQuery request, CancellationToken cancellationToken)
    {
        var plans = await _unitOfWork.ProductionPlans.GetByPlantIdAsync(request.ProductionPlantId, cancellationToken);
        return _mapper.Map<IReadOnlyList<ProductionPlanDto>>(plans);
    }
}

public class GetProductionPlanByIdHandler : IRequestHandler<GetProductionPlanByIdQuery, ProductionPlanDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductionPlanByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductionPlanDto?> Handle(GetProductionPlanByIdQuery request, CancellationToken cancellationToken)
    {
        var plan = await _unitOfWork.ProductionPlans.GetByIdAsync(request.ProductionPlantId, request.SciItemId, cancellationToken);
        return plan is null ? null : _mapper.Map<ProductionPlanDto>(plan);
    }
}
