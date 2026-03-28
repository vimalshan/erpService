namespace CompensationService.Application.Commands.Handlers;

using MediatR;
using CompensationService.Application.DTOs;
using CompensationService.Domain.Repositories;
using CompensationService.Domain.Entities;
using CompensationService.Domain.ValueObjects;
using AutoMapper;

/// <summary>
/// Handler for creating a compensation period.
/// </summary>
public class CreateCompensationPeriodCommandHandler : IRequestHandler<CreateCompensationPeriodCommand, ApiResponse<CompensationPeriodDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCompensationPeriodCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<CompensationPeriodDto>> Handle(CreateCompensationPeriodCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var dto = request.Dto;
            var periodId = Convert.ToDecimal(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

            var period = CompensationPeriod.Create(
                periodId,
                dto.YearId,
                dto.QuarterNo,
                PeriodStatus.Open().StatusCode,
                dto.PeriodOpenDate,
                dto.PeriodCloseDate,
                dto.FormOpenDate
            );

            await _unitOfWork.CompensationPeriods.AddAsync(period, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var periodDto = _mapper.Map<CompensationPeriodDto>(period);
            return new ApiResponse<CompensationPeriodDto>
            {
                Success = true,
                Data = periodDto,
                Message = "Compensation period created successfully."
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<CompensationPeriodDto>
            {
                Success = false,
                Message = "Failed to create compensation period.",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}

/// <summary>
/// Handler for generating a circular for a period.
/// </summary>
public class GenerateCircularCommandHandler : IRequestHandler<GenerateCircularCommand, ApiResponse<CompensationPeriodDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GenerateCircularCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<CompensationPeriodDto>> Handle(GenerateCircularCommand request, CancellationToken cancellationToken)
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

            period.GenerateCircular(1, DateTime.UtcNow); // UserId = 1
            await _unitOfWork.CompensationPeriods.UpdateAsync(period, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var periodDto = _mapper.Map<CompensationPeriodDto>(period);
            return new ApiResponse<CompensationPeriodDto>
            {
                Success = true,
                Data = periodDto,
                Message = "Circular generated successfully."
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<CompensationPeriodDto>
            {
                Success = false,
                Message = "Failed to generate circular.",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}

/// <summary>
/// Handler for confirming a period to payroll.
/// </summary>
public class ConfirmPeriodToPayrollCommandHandler : IRequestHandler<ConfirmPeriodToPayrollCommand, ApiResponse<CompensationPeriodDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ConfirmPeriodToPayrollCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<CompensationPeriodDto>> Handle(ConfirmPeriodToPayrollCommand request, CancellationToken cancellationToken)
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

            period.ConfirmToPayroll();
            await _unitOfWork.CompensationPeriods.UpdateAsync(period, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var periodDto = _mapper.Map<CompensationPeriodDto>(period);
            return new ApiResponse<CompensationPeriodDto>
            {
                Success = true,
                Data = periodDto,
                Message = "Period confirmed to payroll successfully."
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<CompensationPeriodDto>
            {
                Success = false,
                Message = "Failed to confirm period to payroll.",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}

/// <summary>
/// Handler for creating a compensation recommendation.
/// </summary>
public class CreateCompensationRecommendationCommandHandler : IRequestHandler<CreateCompensationRecommendationCommand, ApiResponse<CompensationRecommendationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCompensationRecommendationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<CompensationRecommendationDto>> Handle(CreateCompensationRecommendationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var dto = request.Dto;
            var recommendationId = Convert.ToDecimal(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

            var recommendation = CompensationRecommendation.Create(
                recommendationId,
                dto.YearId,
                dto.PeriodId,
                dto.EmployeeSystemId,
                dto.LevelId,
                dto.CtcAmount,
                dto.MaximumCap,
                dto.EligibilityAmount,
                dto.InitiativeTaken,
                dto.Results,
                dto.RecommendedBy
            );

            await _unitOfWork.CompensationRecommendations.AddAsync(recommendation, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var recommendationDto = _mapper.Map<CompensationRecommendationDto>(recommendation);
            return new ApiResponse<CompensationRecommendationDto>
            {
                Success = true,
                Data = recommendationDto,
                Message = "Compensation recommendation created successfully."
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<CompensationRecommendationDto>
            {
                Success = false,
                Message = "Failed to create compensation recommendation.",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}

/// <summary>
/// Handler for submitting a recommendation.
/// </summary>
public class SubmitRecommendationCommandHandler : IRequestHandler<SubmitRecommendationCommand, ApiResponse<CompensationRecommendationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SubmitRecommendationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<CompensationRecommendationDto>> Handle(SubmitRecommendationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var dto = request.Dto;
            var recommendation = await _unitOfWork.CompensationRecommendations.GetByIdAsync(dto.RecommendationId, cancellationToken);
            
            if (recommendation == null)
            {
                return new ApiResponse<CompensationRecommendationDto>
                {
                    Success = false,
                    Message = "Recommendation not found."
                };
            }

            if (dto.RecommendedAmount.HasValue)
            {
                recommendation.SetRecommendedAmount(MoneyAmount.Create(dto.RecommendedAmount.Value));
            }

            recommendation.Submit(1, DateTime.UtcNow, dto.Role); // UserId = 1
            await _unitOfWork.CompensationRecommendations.UpdateAsync(recommendation, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var recommendationDto = _mapper.Map<CompensationRecommendationDto>(recommendation);
            return new ApiResponse<CompensationRecommendationDto>
            {
                Success = true,
                Data = recommendationDto,
                Message = "Recommendation submitted successfully."
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<CompensationRecommendationDto>
            {
                Success = false,
                Message = "Failed to submit recommendation.",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}

/// <summary>
/// Handler for approving a recommendation.
/// </summary>
public class ApproveRecommendationCommandHandler : IRequestHandler<ApproveRecommendationCommand, ApiResponse<CompensationRecommendationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ApproveRecommendationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<CompensationRecommendationDto>> Handle(ApproveRecommendationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var recommendation = await _unitOfWork.CompensationRecommendations.GetByIdAsync(request.Dto.RecommendationId, cancellationToken);
            
            if (recommendation == null)
            {
                return new ApiResponse<CompensationRecommendationDto>
                {
                    Success = false,
                    Message = "Recommendation not found."
                };
            }

            recommendation.Approve(request.Dto.FinalLevel, request.Dto.FinalAmount);
            await _unitOfWork.CompensationRecommendations.UpdateAsync(recommendation, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var recommendationDto = _mapper.Map<CompensationRecommendationDto>(recommendation);
            return new ApiResponse<CompensationRecommendationDto>
            {
                Success = true,
                Data = recommendationDto,
                Message = "Recommendation approved successfully."
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<CompensationRecommendationDto>
            {
                Success = false,
                Message = "Failed to approve recommendation.",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}

/// <summary>
/// Handler for rejecting a recommendation.
/// </summary>
public class RejectRecommendationCommandHandler : IRequestHandler<RejectRecommendationCommand, ApiResponse<CompensationRecommendationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RejectRecommendationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<CompensationRecommendationDto>> Handle(RejectRecommendationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var recommendation = await _unitOfWork.CompensationRecommendations.GetByIdAsync(request.Dto.RecommendationId, cancellationToken);
            
            if (recommendation == null)
            {
                return new ApiResponse<CompensationRecommendationDto>
                {
                    Success = false,
                    Message = "Recommendation not found."
                };
            }

            recommendation.Reject(1, DateTime.UtcNow, request.Dto.RejectionRemarks); // UserId = 1
            await _unitOfWork.CompensationRecommendations.UpdateAsync(recommendation, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var recommendationDto = _mapper.Map<CompensationRecommendationDto>(recommendation);
            return new ApiResponse<CompensationRecommendationDto>
            {
                Success = true,
                Data = recommendationDto,
                Message = "Recommendation rejected successfully."
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<CompensationRecommendationDto>
            {
                Success = false,
                Message = "Failed to reject recommendation.",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}
