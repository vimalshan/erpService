namespace CompensationService.Application.Queries.Handlers;

using MediatR;
using CompensationService.Application.DTOs;
using CompensationService.Domain.Repositories;
using AutoMapper;

/// <summary>
/// Handler for getting a budget by ID.
/// </summary>
public class GetBudgetByIdQueryHandler : IRequestHandler<GetBudgetByIdQuery, ApiResponse<BudgetDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBudgetByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<BudgetDto>> Handle(GetBudgetByIdQuery request, CancellationToken cancellationToken)
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

            var budgetDto = _mapper.Map<BudgetDto>(budget);
            return new ApiResponse<BudgetDto>
            {
                Success = true,
                Data = budgetDto
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<BudgetDto>
            {
                Success = false,
                Message = "Error retrieving budget.",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}

/// <summary>
/// Handler for getting budgets by year and business.
/// </summary>
public class GetBudgetsByYearAndBusinessQueryHandler : IRequestHandler<GetBudgetsByYearAndBusinessQuery, ApiResponse<List<BudgetDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBudgetsByYearAndBusinessQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<BudgetDto>>> Handle(GetBudgetsByYearAndBusinessQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var budgets = await _unitOfWork.Budgets.GetByYearAndBusinessAsync(request.YearId, request.BusinessId, cancellationToken);
            var budgetDtos = _mapper.Map<List<BudgetDto>>(budgets);

            return new ApiResponse<List<BudgetDto>>
            {
                Success = true,
                Data = budgetDtos
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<BudgetDto>>
            {
                Success = false,
                Message = "Error retrieving budgets.",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}

/// <summary>
/// Handler for getting a compensation level by ID.
/// </summary>
public class GetCompensationLevelByIdQueryHandler : IRequestHandler<GetCompensationLevelByIdQuery, ApiResponse<CompensationLevelDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCompensationLevelByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<CompensationLevelDto>> Handle(GetCompensationLevelByIdQuery request, CancellationToken cancellationToken)
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

            var levelDto = _mapper.Map<CompensationLevelDto>(level);
            return new ApiResponse<CompensationLevelDto>
            {
                Success = true,
                Data = levelDto
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<CompensationLevelDto>
            {
                Success = false,
                Message = "Error retrieving compensation level.",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}

/// <summary>
/// Handler for getting all active levels.
/// </summary>
public class GetActiveLevelsQueryHandler : IRequestHandler<GetActiveLevelsQuery, ApiResponse<List<CompensationLevelDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetActiveLevelsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<CompensationLevelDto>>> Handle(GetActiveLevelsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var levels = await _unitOfWork.CompensationLevels.GetActiveLevelsAsync(cancellationToken);
            var levelDtos = _mapper.Map<List<CompensationLevelDto>>(levels);

            return new ApiResponse<List<CompensationLevelDto>>
            {
                Success = true,
                Data = levelDtos
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<CompensationLevelDto>>
            {
                Success = false,
                Message = "Error retrieving compensation levels.",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}

/// <summary>
/// Handler for getting all levels.
/// </summary>
public class GetAllLevelsQueryHandler : IRequestHandler<GetAllLevelsQuery, ApiResponse<List<CompensationLevelDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllLevelsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<CompensationLevelDto>>> Handle(GetAllLevelsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var levels = await _unitOfWork.CompensationLevels.GetAllLevelsAsync(cancellationToken);
            var levelDtos = _mapper.Map<List<CompensationLevelDto>>(levels);

            return new ApiResponse<List<CompensationLevelDto>>
            {
                Success = true,
                Data = levelDtos
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<CompensationLevelDto>>
            {
                Success = false,
                Message = "Error retrieving compensation levels.",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}

/// <summary>
/// Handler for getting a period by ID.
/// </summary>
public class GetCompensationPeriodByIdQueryHandler : IRequestHandler<GetCompensationPeriodByIdQuery, ApiResponse<CompensationPeriodDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCompensationPeriodByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<CompensationPeriodDto>> Handle(GetCompensationPeriodByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var period = await _unitOfWork.CompensationPeriods.GetByIdAsync(request.PeriodId, cancellationToken);
            if (period == null)
            {
                return new ApiResponse<CompensationPeriodDto>
                {
                    Success = false,
                    Message = "Compensation period not found."
                };
            }

            var periodDto = _mapper.Map<CompensationPeriodDto>(period);
            return new ApiResponse<CompensationPeriodDto>
            {
                Success = true,
                Data = periodDto
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<CompensationPeriodDto>
            {
                Success = false,
                Message = "Error retrieving compensation period.",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}

/// <summary>
/// Handler for getting periods by year.
/// </summary>
public class GetPeriodsByYearQueryHandler : IRequestHandler<GetPeriodsByYearQuery, ApiResponse<List<CompensationPeriodDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPeriodsByYearQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<CompensationPeriodDto>>> Handle(GetPeriodsByYearQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var periods = await _unitOfWork.CompensationPeriods.GetByYearAsync(request.YearId, cancellationToken);
            var periodDtos = _mapper.Map<List<CompensationPeriodDto>>(periods);

            return new ApiResponse<List<CompensationPeriodDto>>
            {
                Success = true,
                Data = periodDtos
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<CompensationPeriodDto>>
            {
                Success = false,
                Message = "Error retrieving compensation periods.",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}

/// <summary>
/// Handler for getting a period by year and quarter.
/// </summary>
public class GetPeriodByYearAndQuarterQueryHandler : IRequestHandler<GetPeriodByYearAndQuarterQuery, ApiResponse<CompensationPeriodDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPeriodByYearAndQuarterQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<CompensationPeriodDto>> Handle(GetPeriodByYearAndQuarterQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var period = await _unitOfWork.CompensationPeriods.GetByYearAndQuarterAsync(request.YearId, request.QuarterNo, cancellationToken);
            if (period == null)
            {
                return new ApiResponse<CompensationPeriodDto>
                {
                    Success = false,
                    Message = "Compensation period not found."
                };
            }

            var periodDto = _mapper.Map<CompensationPeriodDto>(period);
            return new ApiResponse<CompensationPeriodDto>
            {
                Success = true,
                Data = periodDto
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<CompensationPeriodDto>
            {
                Success = false,
                Message = "Error retrieving compensation period.",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}

/// <summary>
/// Handler for getting open periods.
/// </summary>
public class GetOpenPeriodsQueryHandler : IRequestHandler<GetOpenPeriodsQuery, ApiResponse<List<CompensationPeriodDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetOpenPeriodsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<CompensationPeriodDto>>> Handle(GetOpenPeriodsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var periods = await _unitOfWork.CompensationPeriods.GetOpenPeriodsAsync(cancellationToken);
            var periodDtos = _mapper.Map<List<CompensationPeriodDto>>(periods);

            return new ApiResponse<List<CompensationPeriodDto>>
            {
                Success = true,
                Data = periodDtos
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<CompensationPeriodDto>>
            {
                Success = false,
                Message = "Error retrieving open periods.",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}
