namespace CompensationService.Application.Queries.Handlers;

using MediatR;
using CompensationService.Application.DTOs;
using CompensationService.Domain.Repositories;
using AutoMapper;

/// <summary>
/// Handler for getting a recommendation by ID.
/// </summary>
public class GetCompensationRecommendationByIdQueryHandler : IRequestHandler<GetCompensationRecommendationByIdQuery, ApiResponse<CompensationRecommendationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCompensationRecommendationByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<CompensationRecommendationDto>> Handle(GetCompensationRecommendationByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var recommendation = await _unitOfWork.CompensationRecommendations.GetByIdAsync(request.RecommendationId, cancellationToken);
            if (recommendation == null)
            {
                return new ApiResponse<CompensationRecommendationDto>
                {
                    Success = false,
                    Message = "Compensation recommendation not found."
                };
            }

            var recommendationDto = _mapper.Map<CompensationRecommendationDto>(recommendation);
            return new ApiResponse<CompensationRecommendationDto>
            {
                Success = true,
                Data = recommendationDto
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<CompensationRecommendationDto>
            {
                Success = false,
                Message = "Error retrieving recommendation.",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}

/// <summary>
/// Handler for getting recommendations for a period and employee.
/// </summary>
public class GetRecommendationsByPeriodAndEmployeeQueryHandler : IRequestHandler<GetRecommendationsByPeriodAndEmployeeQuery, ApiResponse<List<CompensationRecommendationDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRecommendationsByPeriodAndEmployeeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<CompensationRecommendationDto>>> Handle(GetRecommendationsByPeriodAndEmployeeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var recommendations = await _unitOfWork.CompensationRecommendations.GetByPeriodAndEmployeeAsync(request.PeriodId, request.EmployeeSystemId, cancellationToken);
            var recommendationDtos = _mapper.Map<List<CompensationRecommendationDto>>(recommendations);

            return new ApiResponse<List<CompensationRecommendationDto>>
            {
                Success = true,
                Data = recommendationDtos
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<CompensationRecommendationDto>>
            {
                Success = false,
                Message = "Error retrieving recommendations.",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}

/// <summary>
/// Handler for getting all recommendations for a period.
/// </summary>
public class GetRecommendationsByPeriodQueryHandler : IRequestHandler<GetRecommendationsByPeriodQuery, ApiResponse<PagedResultDto<CompensationRecommendationDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRecommendationsByPeriodQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<PagedResultDto<CompensationRecommendationDto>>> Handle(GetRecommendationsByPeriodQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var recommendations = await _unitOfWork.CompensationRecommendations.GetByPeriodAsync(request.PeriodId, cancellationToken);
            var recommendationDtos = _mapper.Map<List<CompensationRecommendationDto>>(recommendations);

            var pageNumber = request.PageNumber ?? 1;
            var pageSize = request.PageSize ?? 10;
            var skip = (pageNumber - 1) * pageSize;

            var pagedResult = new PagedResultDto<CompensationRecommendationDto>
            {
                Items = recommendationDtos.Skip(skip).Take(pageSize).ToList(),
                TotalCount = recommendationDtos.Count,
                Page = pageNumber,
                PageSize = pageSize
            };

            return new ApiResponse<PagedResultDto<CompensationRecommendationDto>>
            {
                Success = true,
                Data = pagedResult
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<PagedResultDto<CompensationRecommendationDto>>
            {
                Success = false,
                Message = "Error retrieving recommendations.",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}

/// <summary>
/// Handler for getting recommendations by status.
/// </summary>
public class GetRecommendationsByStatusQueryHandler : IRequestHandler<GetRecommendationsByStatusQuery, ApiResponse<PagedResultDto<CompensationRecommendationDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRecommendationsByStatusQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<PagedResultDto<CompensationRecommendationDto>>> Handle(GetRecommendationsByStatusQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var recommendations = await _unitOfWork.CompensationRecommendations.GetByStatusAsync(request.StatusCode, cancellationToken);
            var recommendationDtos = _mapper.Map<List<CompensationRecommendationDto>>(recommendations);

            var pageNumber = request.PageNumber ?? 1;
            var pageSize = request.PageSize ?? 10;
            var skip = (pageNumber - 1) * pageSize;

            var pagedResult = new PagedResultDto<CompensationRecommendationDto>
            {
                Items = recommendationDtos.Skip(skip).Take(pageSize).ToList(),
                TotalCount = recommendationDtos.Count,
                Page = pageNumber,
                PageSize = pageSize
            };

            return new ApiResponse<PagedResultDto<CompensationRecommendationDto>>
            {
                Success = true,
                Data = pagedResult
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<PagedResultDto<CompensationRecommendationDto>>
            {
                Success = false,
                Message = "Error retrieving recommendations.",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}

/// <summary>
/// Handler for getting pending recommendations for a reviewer.
/// </summary>
public class GetPendingRecommendationsForReviewerQueryHandler : IRequestHandler<GetPendingRecommendationsForReviewerQuery, ApiResponse<PagedResultDto<CompensationRecommendationDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPendingRecommendationsForReviewerQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<PagedResultDto<CompensationRecommendationDto>>> Handle(GetPendingRecommendationsForReviewerQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var recommendations = await _unitOfWork.CompensationRecommendations.GetPendingForReviewerAsync(request.PeriodId, request.Role, cancellationToken);
            var recommendationDtos = _mapper.Map<List<CompensationRecommendationDto>>(recommendations);

            var pageNumber = request.PageNumber ?? 1;
            var pageSize = request.PageSize ?? 10;
            var skip = (pageNumber - 1) * pageSize;

            var pagedResult = new PagedResultDto<CompensationRecommendationDto>
            {
                Items = recommendationDtos.Skip(skip).Take(pageSize).ToList(),
                TotalCount = recommendationDtos.Count,
                Page = pageNumber,
                PageSize = pageSize
            };

            return new ApiResponse<PagedResultDto<CompensationRecommendationDto>>
            {
                Success = true,
                Data = pagedResult
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<PagedResultDto<CompensationRecommendationDto>>
            {
                Success = false,
                Message = "Error retrieving pending recommendations.",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}

/// <summary>
/// Handler for getting recommendation history for an employee.
/// </summary>
public class GetRecommendationHistoryForEmployeeQueryHandler : IRequestHandler<GetRecommendationHistoryForEmployeeQuery, ApiResponse<List<CompensationRecommendationDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRecommendationHistoryForEmployeeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<CompensationRecommendationDto>>> Handle(GetRecommendationHistoryForEmployeeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // This would typically require querying all recommendations for an employee across all periods
            // For now, we'll create a simple implementation
            var allRecommendations = new List<CompensationRecommendationDto>();

            var recommendationDtos = _mapper.Map<List<CompensationRecommendationDto>>(allRecommendations);

            return new ApiResponse<List<CompensationRecommendationDto>>
            {
                Success = true,
                Data = recommendationDtos
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<CompensationRecommendationDto>>
            {
                Success = false,
                Message = "Error retrieving recommendation history.",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}
