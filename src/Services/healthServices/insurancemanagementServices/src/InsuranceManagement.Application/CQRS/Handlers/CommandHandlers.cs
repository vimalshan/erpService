namespace InsuranceManagement.Application.CQRS.Handlers;

using MediatR;
using AutoMapper;
using InsuranceManagement.Application.DTOs;
using InsuranceManagement.Application.CQRS.Commands;
using InsuranceManagement.Domain.Entities;
using InsuranceManagement.Domain.ValueObjects;
using InsuranceManagement.Infrastructure.Repositories;
using InsuranceManagement.Infrastructure.Repositories;

/// <summary>
/// Handler for CreateInsurancePlanCommand
/// </summary>
public class CreateInsurancePlanCommandHandler : IRequestHandler<CreateInsurancePlanCommand, ApiResponse<InsurancePlanDto>>
{
    private readonly IInsuranceManagementUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateInsurancePlanCommandHandler(IInsuranceManagementUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ApiResponse<InsurancePlanDto>> Handle(CreateInsurancePlanCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var plan = new InsurancePlan(
                request.PlanName,
                request.PlanDescription,
                request.PremiumRate,
                request.MinPremium,
                request.MaxPremium,
                request.CoverageDetails,
                request.CreatedBy);

            await _unitOfWork.PlanRepository.AddAsync(plan, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var planDto = _mapper.Map<InsurancePlanDto>(plan);
            return ApiResponse<InsurancePlanDto>.SuccessResponse(planDto, "Insurance plan created successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<InsurancePlanDto>.FailureResponse($"Failed to create insurance plan: {ex.Message}");
        }
    }
}

/// <summary>
/// Handler for EnrollInsuranceCommand
/// </summary>
public class EnrollInsuranceCommandHandler : IRequestHandler<EnrollInsuranceCommand, ApiResponse<InsuranceEnrollmentDto>>
{
    private readonly IInsuranceManagementUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EnrollInsuranceCommandHandler(IInsuranceManagementUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ApiResponse<InsuranceEnrollmentDto>> Handle(EnrollInsuranceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if plan exists
            var plan = await _unitOfWork.PlanRepository.GetByIdAsync(request.InsurancePlanId, cancellationToken);
            if (plan == null)
                return ApiResponse<InsuranceEnrollmentDto>.FailureResponse("Insurance plan not found");

            // Check for existing active enrollment
            var existingEnrollment = await _unitOfWork.EnrollmentRepository
                .GetActiveEnrollmentAsync(request.EmpSysId, request.InsurancePlanId);
            
            if (existingEnrollment != null)
                return ApiResponse<InsuranceEnrollmentDto>.FailureResponse("Employee already enrolled in this plan");

            // Create enrollment
            var coverageType = CoverageType.Create(request.CoverageType);
            
            var enrollment = new InsuranceEnrollment(
                request.EmpSysId,
                request.InsurancePlanId,
                coverageType,
                request.EnrollmentDate,
                request.EffectiveDate,
                plan.PremiumRate, // Will be calculated properly
                request.CreatedBy);

            await _unitOfWork.EnrollmentRepository.AddAsync(enrollment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var enrollmentDto = _mapper.Map<InsuranceEnrollmentDto>(enrollment);
            return ApiResponse<InsuranceEnrollmentDto>.SuccessResponse(enrollmentDto, "Enrollment created successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<InsuranceEnrollmentDto>.FailureResponse($"Failed to enroll: {ex.Message}");
        }
    }
}

/// <summary>
/// Handler for SubmitClaimCommand
/// </summary>
public class SubmitClaimCommandHandler : IRequestHandler<SubmitClaimCommand, ApiResponse<InsuranceClaimDto>>
{
    private readonly IInsuranceManagementUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SubmitClaimCommandHandler(IInsuranceManagementUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ApiResponse<InsuranceClaimDto>> Handle(SubmitClaimCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate enrollment exists and is active
            var enrollment = await _unitOfWork.EnrollmentRepository.GetByIdAsync(request.EnrollmentId, cancellationToken);
            if (enrollment == null || !enrollment.Status.IsActive)
                return ApiResponse<InsuranceClaimDto>.FailureResponse("Active enrollment not found");

            var claimType = ClaimType.Create(request.ClaimType);
            var claimAmount = Money.From(request.ClaimAmount);
            
            // Calculate reimbursable amount
            var reimbursableAmount = CalculateReimbursement(claimAmount, claimType);

            var claim = new InsuranceClaim(
                request.EmpSysId,
                request.EnrollmentId,
                enrollment.InsurancePlanId,
                claimType,
                claimAmount,
                reimbursableAmount,
                request.ServiceDate,
                request.HospitalName,
                request.Remarks,
                request.CreatedBy);

            await _unitOfWork.ClaimRepository.AddAsync(claim, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var claimDto = _mapper.Map<InsuranceClaimDto>(claim);
            return ApiResponse<InsuranceClaimDto>.SuccessResponse(claimDto, "Claim submitted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<InsuranceClaimDto>.FailureResponse($"Failed to submit claim: {ex.Message}");
        }
    }

    private Money CalculateReimbursement(Money claimAmount, ClaimType claimType)
    {
        decimal coveragePercentage = claimType.Value switch
        {
            "DENTAL" => 50.0m,
            "OPTICAL" => 75.0m,
            _ => 100.0m
        };

        const decimal copayPercentage = 20.0m;
        var copayAmount = claimAmount.Multiply(copayPercentage / 100);
        var afterCopay = claimAmount.Subtract(copayAmount);
        
        return afterCopay.Multiply(coveragePercentage / 100);
    }
}

/// <summary>
/// Handler for UpdateInsurancePlanCommand
/// </summary>
public class UpdateInsurancePlanCommandHandler : IRequestHandler<UpdateInsurancePlanCommand, ApiResponse<InsurancePlanDto>>
{
    private readonly IInsuranceManagementUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateInsurancePlanCommandHandler(IInsuranceManagementUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ApiResponse<InsurancePlanDto>> Handle(UpdateInsurancePlanCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var plan = await _unitOfWork.PlanRepository.GetByIdAsync(request.PlanId, cancellationToken);
            if (plan == null)
                return ApiResponse<InsurancePlanDto>.FailureResponse("Insurance plan not found");

            plan.Update(
                request.PlanName,
                request.PlanDescription,
                request.PremiumRate,
                request.MinPremium,
                request.MaxPremium,
                request.CoverageDetails,
                request.ModifiedBy);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var planDto = _mapper.Map<InsurancePlanDto>(plan);
            return ApiResponse<InsurancePlanDto>.SuccessResponse(planDto, "Insurance plan updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<InsurancePlanDto>.FailureResponse($"Failed to update insurance plan: {ex.Message}");
        }
    }
}

/// <summary>
/// Handler for ActivateInsurancePlanCommand
/// </summary>
public class ActivateInsurancePlanCommandHandler : IRequestHandler<ActivateInsurancePlanCommand, ApiResponse<bool>>
{
    private readonly IInsuranceManagementUnitOfWork _unitOfWork;

    public ActivateInsurancePlanCommandHandler(IInsuranceManagementUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<ApiResponse<bool>> Handle(ActivateInsurancePlanCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var plan = await _unitOfWork.PlanRepository.GetByIdAsync(request.PlanId, cancellationToken);
            if (plan == null)
                return ApiResponse<bool>.FailureResponse("Insurance plan not found");

            plan.Activate(request.ModifiedBy);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiResponse<bool>.SuccessResponse(true, "Insurance plan activated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.FailureResponse($"Failed to activate insurance plan: {ex.Message}");
        }
    }
}

/// <summary>
/// Handler for DeactivateInsurancePlanCommand
/// </summary>
public class DeactivateInsurancePlanCommandHandler : IRequestHandler<DeactivateInsurancePlanCommand, ApiResponse<bool>>
{
    private readonly IInsuranceManagementUnitOfWork _unitOfWork;

    public DeactivateInsurancePlanCommandHandler(IInsuranceManagementUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<ApiResponse<bool>> Handle(DeactivateInsurancePlanCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var plan = await _unitOfWork.PlanRepository.GetByIdAsync(request.PlanId, cancellationToken);
            if (plan == null)
                return ApiResponse<bool>.FailureResponse("Insurance plan not found");

            plan.Deactivate(request.ModifiedBy);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiResponse<bool>.SuccessResponse(true, "Insurance plan deactivated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.FailureResponse($"Failed to deactivate insurance plan: {ex.Message}");
        }
    }
}

/// <summary>
/// Handler for ApproveClaimCommand
/// </summary>
public class ApproveClaimCommandHandler : IRequestHandler<ApproveClaimCommand, ApiResponse<InsuranceClaimDto>>
{
    private readonly IInsuranceManagementUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ApproveClaimCommandHandler(IInsuranceManagementUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ApiResponse<InsuranceClaimDto>> Handle(ApproveClaimCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var claim = await _unitOfWork.ClaimRepository.GetByIdAsync(request.ClaimId, cancellationToken);
            if (claim == null)
                return ApiResponse<InsuranceClaimDto>.FailureResponse("Insurance claim not found");

            var approvedAmount = Money.From(request.ApprovedAmount);
            claim.Approve(approvedAmount, request.ApprovedBy);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var claimDto = _mapper.Map<InsuranceClaimDto>(claim);
            return ApiResponse<InsuranceClaimDto>.SuccessResponse(claimDto, "Claim approved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<InsuranceClaimDto>.FailureResponse($"Failed to approve claim: {ex.Message}");
        }
    }
}

/// <summary>
/// Handler for RejectClaimCommand
/// </summary>
public class RejectClaimCommandHandler : IRequestHandler<RejectClaimCommand, ApiResponse<InsuranceClaimDto>>
{
    private readonly IInsuranceManagementUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RejectClaimCommandHandler(IInsuranceManagementUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ApiResponse<InsuranceClaimDto>> Handle(RejectClaimCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var claim = await _unitOfWork.ClaimRepository.GetByIdAsync(request.ClaimId, cancellationToken);
            if (claim == null)
                return ApiResponse<InsuranceClaimDto>.FailureResponse("Insurance claim not found");

            claim.Reject(request.RejectionReason, request.RejectedBy);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var claimDto = _mapper.Map<InsuranceClaimDto>(claim);
            return ApiResponse<InsuranceClaimDto>.SuccessResponse(claimDto, "Claim rejected successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<InsuranceClaimDto>.FailureResponse($"Failed to reject claim: {ex.Message}");
        }
    }
}

/// <summary>
/// Handler for MarkClaimAsPaidCommand
/// </summary>
public class MarkClaimAsPaidCommandHandler : IRequestHandler<MarkClaimAsPaidCommand, ApiResponse<InsuranceClaimDto>>
{
    private readonly IInsuranceManagementUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MarkClaimAsPaidCommandHandler(IInsuranceManagementUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ApiResponse<InsuranceClaimDto>> Handle(MarkClaimAsPaidCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var claim = await _unitOfWork.ClaimRepository.GetByIdAsync(request.ClaimId, cancellationToken);
            if (claim == null)
                return ApiResponse<InsuranceClaimDto>.FailureResponse("Insurance claim not found");

            claim.MarkAsPaid(request.PaidBy);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var claimDto = _mapper.Map<InsuranceClaimDto>(claim);
            return ApiResponse<InsuranceClaimDto>.SuccessResponse(claimDto, "Claim marked as paid successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<InsuranceClaimDto>.FailureResponse($"Failed to mark claim as paid: {ex.Message}");
        }
    }
}

/// <summary>
/// Handler for TerminateEnrollmentCommand
/// </summary>
public class TerminateEnrollmentCommandHandler : IRequestHandler<TerminateEnrollmentCommand, ApiResponse<bool>>
{
    private readonly IInsuranceManagementUnitOfWork _unitOfWork;

    public TerminateEnrollmentCommandHandler(IInsuranceManagementUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<ApiResponse<bool>> Handle(TerminateEnrollmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var enrollment = await _unitOfWork.EnrollmentRepository.GetByIdAsync(request.EnrollmentId, cancellationToken);
            if (enrollment == null)
                return ApiResponse<bool>.FailureResponse("Enrollment not found");

            enrollment.Terminate(request.Reason, request.ModifiedBy);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiResponse<bool>.SuccessResponse(true, "Enrollment terminated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.FailureResponse($"Failed to terminate enrollment: {ex.Message}");
        }
    }
}

/// <summary>
/// Handler for SuspendEnrollmentCommand
/// </summary>
public class SuspendEnrollmentCommandHandler : IRequestHandler<SuspendEnrollmentCommand, ApiResponse<bool>>
{
    private readonly IInsuranceManagementUnitOfWork _unitOfWork;

    public SuspendEnrollmentCommandHandler(IInsuranceManagementUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<ApiResponse<bool>> Handle(SuspendEnrollmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var enrollment = await _unitOfWork.EnrollmentRepository.GetByIdAsync(request.EnrollmentId, cancellationToken);
            if (enrollment == null)
                return ApiResponse<bool>.FailureResponse("Enrollment not found");

            enrollment.Suspend(request.ModifiedBy);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiResponse<bool>.SuccessResponse(true, "Enrollment suspended successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.FailureResponse($"Failed to suspend enrollment: {ex.Message}");
        }
    }
}

/// <summary>
/// Handler for ReactivateEnrollmentCommand
/// </summary>
public class ReactivateEnrollmentCommandHandler : IRequestHandler<ReactivateEnrollmentCommand, ApiResponse<bool>>
{
    private readonly IInsuranceManagementUnitOfWork _unitOfWork;

    public ReactivateEnrollmentCommandHandler(IInsuranceManagementUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<ApiResponse<bool>> Handle(ReactivateEnrollmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var enrollment = await _unitOfWork.EnrollmentRepository.GetByIdAsync(request.EnrollmentId, cancellationToken);
            if (enrollment == null)
                return ApiResponse<bool>.FailureResponse("Enrollment not found");

            enrollment.Reactivate(request.ModifiedBy);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiResponse<bool>.SuccessResponse(true, "Enrollment reactivated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.FailureResponse($"Failed to reactivate enrollment: {ex.Message}");
        }
    }
}
