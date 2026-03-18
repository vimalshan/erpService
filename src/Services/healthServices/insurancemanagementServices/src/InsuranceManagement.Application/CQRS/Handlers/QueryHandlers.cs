namespace InsuranceManagement.Application.CQRS.Handlers;

using MediatR;
using AutoMapper;
using InsuranceManagement.Application.DTOs;
using InsuranceManagement.Application.CQRS.Queries;
using InsuranceManagement.Infrastructure.Repositories;
using InsuranceManagement.Infrastructure.Repositories;

/// <summary>
/// Handler for GetInsurancePlanByIdQuery
/// </summary>
public class GetInsurancePlanByIdQueryHandler : IRequestHandler<GetInsurancePlanByIdQuery, ApiResponse<InsurancePlanDto>>
{
    private readonly IInsuranceManagementUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetInsurancePlanByIdQueryHandler(IInsuranceManagementUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ApiResponse<InsurancePlanDto>> Handle(GetInsurancePlanByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var plan = await _unitOfWork.PlanRepository.GetByIdAsync(request.PlanId, cancellationToken);
            if (plan == null)
                return ApiResponse<InsurancePlanDto>.FailureResponse("Insurance plan not found");

            var planDto = _mapper.Map<InsurancePlanDto>(plan);
            return ApiResponse<InsurancePlanDto>.SuccessResponse(planDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<InsurancePlanDto>.FailureResponse($"Failed to retrieve insurance plan: {ex.Message}");
        }
    }
}

/// <summary>
/// Handler for GetAllActiveInsurancePlansQuery
/// </summary>
public class GetAllActiveInsurancePlansQueryHandler : IRequestHandler<GetAllActiveInsurancePlansQuery, ApiResponse<List<InsurancePlanDto>>>
{
    private readonly IInsuranceManagementUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllActiveInsurancePlansQueryHandler(IInsuranceManagementUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ApiResponse<List<InsurancePlanDto>>> Handle(GetAllActiveInsurancePlansQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var plans = await _unitOfWork.PlanRepository.GetAllActiveAsync();
            var planDtos = _mapper.Map<List<InsurancePlanDto>>(plans);
            return ApiResponse<List<InsurancePlanDto>>.SuccessResponse(planDtos);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<InsurancePlanDto>>.FailureResponse($"Failed to retrieve active insurance plans: {ex.Message}");
        }
    }
}

/// <summary>
/// Handler for GetAllInsurancePlansQuery
/// </summary>
public class GetAllInsurancePlansQueryHandler : IRequestHandler<GetAllInsurancePlansQuery, ApiResponse<PaginatedResponse<InsurancePlanDto>>>
{
    private readonly IInsuranceManagementUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllInsurancePlansQueryHandler(IInsuranceManagementUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ApiResponse<PaginatedResponse<InsurancePlanDto>>> Handle(GetAllInsurancePlansQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var plans = await _unitOfWork.PlanRepository.GetPagedAsync(request.PageNumber, request.PageSize, cancellationToken);
            var totalCount = await _unitOfWork.PlanRepository.GetCountAsync(cancellationToken);
            var planDtos = _mapper.Map<List<InsurancePlanDto>>(plans);
            
            var paginatedResponse = new PaginatedResponse<InsurancePlanDto>
            {
                Items = planDtos,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };

            return ApiResponse<PaginatedResponse<InsurancePlanDto>>.SuccessResponse(paginatedResponse);
        }
        catch (Exception ex)
        {
            return ApiResponse<PaginatedResponse<InsurancePlanDto>>.FailureResponse($"Failed to retrieve insurance plans: {ex.Message}");
        }
    }
}

/// <summary>
/// Handler for GetInsuranceEnrollmentByIdQuery
/// </summary>
public class GetInsuranceEnrollmentByIdQueryHandler : IRequestHandler<GetInsuranceEnrollmentByIdQuery, ApiResponse<InsuranceEnrollmentDetailDto>>
{
    private readonly IInsuranceManagementUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetInsuranceEnrollmentByIdQueryHandler(IInsuranceManagementUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ApiResponse<InsuranceEnrollmentDetailDto>> Handle(GetInsuranceEnrollmentByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var enrollment = await _unitOfWork.EnrollmentRepository.GetByIdAsync(request.EnrollmentId, cancellationToken);
            if (enrollment == null)
                return ApiResponse<InsuranceEnrollmentDetailDto>.FailureResponse("Enrollment not found");

            var plan = await _unitOfWork.PlanRepository.GetByIdAsync(enrollment.InsurancePlanId, cancellationToken);
            
            var enrollmentDetailDto = _mapper.Map<InsuranceEnrollmentDetailDto>(enrollment);
            if (plan != null)
                enrollmentDetailDto.Plan = _mapper.Map<InsurancePlanDto>(plan);
            
            return ApiResponse<InsuranceEnrollmentDetailDto>.SuccessResponse(enrollmentDetailDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<InsuranceEnrollmentDetailDto>.FailureResponse($"Failed to retrieve enrollment: {ex.Message}");
        }
    }
}

/// <summary>
/// Handler for GetEmployeeActiveEnrollmentsQuery
/// </summary>
public class GetEmployeeActiveEnrollmentsQueryHandler : IRequestHandler<GetEmployeeActiveEnrollmentsQuery, ApiResponse<List<InsuranceEnrollmentDto>>>
{
    private readonly IInsuranceManagementUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetEmployeeActiveEnrollmentsQueryHandler(IInsuranceManagementUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ApiResponse<List<InsuranceEnrollmentDto>>> Handle(GetEmployeeActiveEnrollmentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var enrollments = await _unitOfWork.EnrollmentRepository.GetActiveByEmployeeAsync(request.EmpSysId);
            var enrollmentDtos = _mapper.Map<List<InsuranceEnrollmentDto>>(enrollments);
            return ApiResponse<List<InsuranceEnrollmentDto>>.SuccessResponse(enrollmentDtos);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<InsuranceEnrollmentDto>>.FailureResponse($"Failed to retrieve active enrollments: {ex.Message}");
        }
    }
}

/// <summary>
/// Handler for GetEmployeeAllEnrollmentsQuery
/// </summary>
public class GetEmployeeAllEnrollmentsQueryHandler : IRequestHandler<GetEmployeeAllEnrollmentsQuery, ApiResponse<List<InsuranceEnrollmentDto>>>
{
    private readonly IInsuranceManagementUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetEmployeeAllEnrollmentsQueryHandler(IInsuranceManagementUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ApiResponse<List<InsuranceEnrollmentDto>>> Handle(GetEmployeeAllEnrollmentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var enrollments = await _unitOfWork.EnrollmentRepository.GetByEmployeeAsync(request.EmpSysId);
            var enrollmentDtos = _mapper.Map<List<InsuranceEnrollmentDto>>(enrollments);
            return ApiResponse<List<InsuranceEnrollmentDto>>.SuccessResponse(enrollmentDtos);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<InsuranceEnrollmentDto>>.FailureResponse($"Failed to retrieve all enrollments: {ex.Message}");
        }
    }
}

/// <summary>
/// Handler for GetInsuranceClaimByIdQuery
/// </summary>
public class GetInsuranceClaimByIdQueryHandler : IRequestHandler<GetInsuranceClaimByIdQuery, ApiResponse<InsuranceClaimDto>>
{
    private readonly IInsuranceManagementUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetInsuranceClaimByIdQueryHandler(IInsuranceManagementUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ApiResponse<InsuranceClaimDto>> Handle(GetInsuranceClaimByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var claim = await _unitOfWork.ClaimRepository.GetByIdAsync(request.ClaimId, cancellationToken);
            if (claim == null)
                return ApiResponse<InsuranceClaimDto>.FailureResponse("Claim not found");

            var claimDto = _mapper.Map<InsuranceClaimDto>(claim);
            return ApiResponse<InsuranceClaimDto>.SuccessResponse(claimDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<InsuranceClaimDto>.FailureResponse($"Failed to retrieve claim: {ex.Message}");
        }
    }
}

/// <summary>
/// Handler for GetEmployeeClaimsQuery
/// </summary>
public class GetEmployeeClaimsQueryHandler : IRequestHandler<GetEmployeeClaimsQuery, ApiResponse<PaginatedResponse<InsuranceClaimDto>>>
{
    private readonly IInsuranceManagementUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetEmployeeClaimsQueryHandler(IInsuranceManagementUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ApiResponse<PaginatedResponse<InsuranceClaimDto>>> Handle(GetEmployeeClaimsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var skip = (request.PageNumber - 1) * request.PageSize;
            var claims = await _unitOfWork.ClaimRepository.GetByEmployeeAsync(request.EmpSysId, skip, request.PageSize);
            // Optionally filter claims by Status if requested, currently handled manually:
            if (!string.IsNullOrEmpty(request.Status)) {
                claims = claims.Where(c => c.Status.Value == request.Status).ToList();
            }
            var totalCount = await _unitOfWork.ClaimRepository.GetCountByEmployeeAsync(request.EmpSysId);

            var claimDtos = _mapper.Map<List<InsuranceClaimDto>>(claims);
            
            var paginatedResponse = new PaginatedResponse<InsuranceClaimDto>
            {
                Items = claimDtos,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };

            return ApiResponse<PaginatedResponse<InsuranceClaimDto>>.SuccessResponse(paginatedResponse);
        }
        catch (Exception ex)
        {
            return ApiResponse<PaginatedResponse<InsuranceClaimDto>>.FailureResponse($"Failed to retrieve employee claims: {ex.Message}");
        }
    }
}

/// <summary>
/// Handler for GetEnrollmentClaimsQuery
/// </summary>
public class GetEnrollmentClaimsQueryHandler : IRequestHandler<GetEnrollmentClaimsQuery, ApiResponse<List<InsuranceClaimDto>>>
{
    private readonly IInsuranceManagementUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetEnrollmentClaimsQueryHandler(IInsuranceManagementUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ApiResponse<List<InsuranceClaimDto>>> Handle(GetEnrollmentClaimsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var claims = await _unitOfWork.ClaimRepository.GetByEnrollmentAsync(request.EnrollmentId);
            var claimDtos = _mapper.Map<List<InsuranceClaimDto>>(claims);
            return ApiResponse<List<InsuranceClaimDto>>.SuccessResponse(claimDtos);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<InsuranceClaimDto>>.FailureResponse($"Failed to retrieve enrollment claims: {ex.Message}");
        }
    }
}

/// <summary>
/// Handler for GetClaimsForApprovalQuery
/// </summary>
public class GetClaimsForApprovalQueryHandler : IRequestHandler<GetClaimsForApprovalQuery, ApiResponse<List<InsuranceClaimDto>>>
{
    private readonly IInsuranceManagementUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetClaimsForApprovalQueryHandler(IInsuranceManagementUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ApiResponse<List<InsuranceClaimDto>>> Handle(GetClaimsForApprovalQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var claims = await _unitOfWork.ClaimRepository.GetByStatusAsync("PENDING", 0, 1000);
            var claimDtos = _mapper.Map<List<InsuranceClaimDto>>(claims);
            return ApiResponse<List<InsuranceClaimDto>>.SuccessResponse(claimDtos);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<InsuranceClaimDto>>.FailureResponse($"Failed to retrieve claims for approval: {ex.Message}");
        }
    }
}

/// <summary>
/// Handler for CalculateClaimReimbursementQuery
/// </summary>
public class CalculateClaimReimbursementQueryHandler : IRequestHandler<CalculateClaimReimbursementQuery, ApiResponse<decimal>>
{
    public Task<ApiResponse<decimal>> Handle(CalculateClaimReimbursementQuery request, CancellationToken cancellationToken)
    {
        try
        {
            decimal coveragePercentage = request.ClaimType.ToUpper() switch
            {
                "DENTAL" => 50.0m,
                "OPTICAL" => 75.0m,
                "IN_PATIENT" => 100.0m,
                "OUT_PATIENT" => 80.0m,
                _ => 100.0m
            };

            var copayAmount = request.ClaimAmount * (request.CopayPercentage / 100);
            var afterCopay = request.ClaimAmount - copayAmount;
            var reimbursementAmount = afterCopay * (coveragePercentage / 100);

            return Task.FromResult(ApiResponse<decimal>.SuccessResponse(reimbursementAmount));
        }
        catch (Exception ex)
        {
            return Task.FromResult(ApiResponse<decimal>.FailureResponse($"Failed to calculate reimbursement: {ex.Message}"));
        }
    }
}

/// <summary>
/// Handler for CheckEmployeeEligibilityQuery
/// </summary>
public class CheckEmployeeEligibilityQueryHandler : IRequestHandler<CheckEmployeeEligibilityQuery, ApiResponse<string>>
{
    private readonly IInsuranceManagementUnitOfWork _unitOfWork;

    public CheckEmployeeEligibilityQueryHandler(IInsuranceManagementUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<ApiResponse<string>> Handle(CheckEmployeeEligibilityQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var enrollments = await _unitOfWork.EnrollmentRepository.GetActiveByEmployeeAsync(request.EmpSysId);
            
            if (enrollments == null || enrollments.Count == 0)
                return ApiResponse<string>.SuccessResponse("NOT_ELIGIBLE", "Employee has no active enrollments");

            return ApiResponse<string>.SuccessResponse("ELIGIBLE", "Employee is eligible for insurance benefits");
        }
        catch (Exception ex)
        {
            return ApiResponse<string>.FailureResponse($"Failed to check employee eligibility: {ex.Message}");
        }
    }
}
