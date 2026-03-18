namespace CompensationService.Application.Commands.Handlers;

using MediatR;
using CompensationService.Application.DTOs;
using CompensationService.Domain.Repositories;
using CompensationService.Domain.Entities;
using AutoMapper;

/// <summary>
/// Handler for creating a new budget.
/// </summary>
public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, ApiResponse<BudgetDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateBudgetCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<BudgetDto>> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var dto = request.Dto;
            var budgetId = decimal.Parse(Guid.NewGuid().ToString("N").Substring(0, 10));
            
            var budget = Budget.Create(
                budgetId,
                dto.BusinessId,
                dto.YearId,
                dto.BudgetAmount,
                1, // UserId - would come from current user in real app
                DateTime.UtcNow
            );

            await _unitOfWork.Budgets.AddAsync(budget, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var budgetDto = _mapper.Map<BudgetDto>(budget);
            return new ApiResponse<BudgetDto>
            {
                Success = true,
                Data = budgetDto,
                Message = "Budget created successfully."
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<BudgetDto>
            {
                Success = false,
                Message = "Failed to create budget.",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}

/// <summary>
/// Handler for updating a budget.
/// </summary>
public class UpdateBudgetCommandHandler : IRequestHandler<UpdateBudgetCommand, ApiResponse<BudgetDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateBudgetCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<BudgetDto>> Handle(UpdateBudgetCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var budget = await _unitOfWork.Budgets.GetByIdAsync(request.BudgetId, cancellationToken);
            if (budget == null)
            {
                return new ApiResponse<BudgetDto>
                {
                    Success = false,
                    Message = "Budget not found."
                };
            }

            budget.UpdateAmount(
                Domain.ValueObjects.MoneyAmount.Create(request.Dto.BudgetAmount),
                1, // UserId
                DateTime.UtcNow
            );

            await _unitOfWork.Budgets.UpdateAsync(budget, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var budgetDto = _mapper.Map<BudgetDto>(budget);
            return new ApiResponse<BudgetDto>
            {
                Success = true,
                Data = budgetDto,
                Message = "Budget updated successfully."
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<BudgetDto>
            {
                Success = false,
                Message = "Failed to update budget.",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}

/// <summary>
/// Handler for creating a compensation level.
/// </summary>
public class CreateCompensationLevelCommandHandler : IRequestHandler<CreateCompensationLevelCommand, ApiResponse<CompensationLevelDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCompensationLevelCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<CompensationLevelDto>> Handle(CreateCompensationLevelCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var dto = request.Dto;
            var levelId = decimal.Parse(Guid.NewGuid().ToString("N").Substring(0, 10));

            var level = CompensationLevel.Create(
                levelId,
                dto.LevelDesc,
                dto.LevelAmount,
                dto.LevelReason,
                dto.MinAmount,
                dto.MaxAmount,
                dto.EffectiveDate,
                1, // UserId
                DateTime.UtcNow
            );

            await _unitOfWork.CompensationLevels.AddAsync(level, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var levelDto = _mapper.Map<CompensationLevelDto>(level);
            return new ApiResponse<CompensationLevelDto>
            {
                Success = true,
                Data = levelDto,
                Message = "Compensation level created successfully."
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<CompensationLevelDto>
            {
                Success = false,
                Message = "Failed to create compensation level.",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}

/// <summary>
/// Handler for closing a compensation level.
/// </summary>
public class CloseCompensationLevelCommandHandler : IRequestHandler<CloseCompensationLevelCommand, ApiResponse<CompensationLevelDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CloseCompensationLevelCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<CompensationLevelDto>> Handle(CloseCompensationLevelCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var level = await _unitOfWork.CompensationLevels.GetByIdAsync(request.LevelId, cancellationToken);
            if (level == null)
            {
                return new ApiResponse<CompensationLevelDto>
                {
                    Success = false,
                    Message = "Compensation level not found."
                };
            }

            level.Close(DateTime.UtcNow);
            await _unitOfWork.CompensationLevels.UpdateAsync(level, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var levelDto = _mapper.Map<CompensationLevelDto>(level);
            return new ApiResponse<CompensationLevelDto>
            {
                Success = true,
                Data = levelDto,
                Message = "Compensation level closed successfully."
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<CompensationLevelDto>
            {
                Success = false,
                Message = "Failed to close compensation level.",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}
