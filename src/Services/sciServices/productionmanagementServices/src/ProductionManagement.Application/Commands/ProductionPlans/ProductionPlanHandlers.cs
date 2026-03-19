using AutoMapper;
using MediatR;
using ProductionManagement.Application.DTOs;
using ProductionManagement.Domain.Entities;
using ProductionManagement.Domain.Interfaces;

namespace ProductionManagement.Application.Commands.ProductionPlans;

public class CreateProductionPlanHandler : IRequestHandler<CreateProductionPlanCommand, ProductionPlanDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateProductionPlanHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductionPlanDto> Handle(CreateProductionPlanCommand request, CancellationToken cancellationToken)
    {
        var plan = new ProductionPlan(
            request.Dto.ProductionPlantId,
            request.Dto.SciItemId,
            request.Dto.QtyPerDay,
            request.Dto.PlanStartDate,
            request.Dto.ModifiedBy);

        var result = await _unitOfWork.ProductionPlans.AddAsync(plan, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProductionPlanDto>(result);
    }
}

public class UpdateProductionPlanHandler : IRequestHandler<UpdateProductionPlanCommand, ProductionPlanDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateProductionPlanHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductionPlanDto> Handle(UpdateProductionPlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await _unitOfWork.ProductionPlans.GetByIdAsync(request.Dto.ProductionPlantId, request.Dto.SciItemId, cancellationToken)
            ?? throw new KeyNotFoundException($"Production plan not found.");

        plan.UpdateQuantity(request.Dto.QtyPerDay, request.Dto.ModifiedBy);
        await _unitOfWork.ProductionPlans.UpdateAsync(plan, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProductionPlanDto>(plan);
    }
}

public class CloseProductionPlanHandler : IRequestHandler<CloseProductionPlanCommand, ProductionPlanDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CloseProductionPlanHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductionPlanDto> Handle(CloseProductionPlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await _unitOfWork.ProductionPlans.GetByIdAsync(request.ProductionPlantId, request.SciItemId, cancellationToken)
            ?? throw new KeyNotFoundException("Production plan not found.");

        plan.ClosePlan(request.ModifiedBy);
        await _unitOfWork.ProductionPlans.UpdateAsync(plan, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProductionPlanDto>(plan);
    }
}

public class DeleteProductionPlanHandler : IRequestHandler<DeleteProductionPlanCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductionPlanHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> Handle(DeleteProductionPlanCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.ProductionPlans.DeleteAsync(request.ProductionPlantId, request.SciItemId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
