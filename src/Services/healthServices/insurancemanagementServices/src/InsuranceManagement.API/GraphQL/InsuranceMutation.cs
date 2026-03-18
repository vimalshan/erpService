using InsuranceManagement.Application.DTOs;
using InsuranceManagement.Application.CQRS.Commands;
using MediatR;

namespace InsuranceManagement.API.GraphQL;

/// <summary>
/// GraphQL Mutation type for Insurance Management
/// </summary>
public class InsuranceMutation
{
    private readonly IMediator _mediator;

    public InsuranceMutation(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Plan Mutations
    public async Task<ApiResponse<InsurancePlanDto>> CreateInsurancePlanAsync(
        string planName, string planDescription, decimal premiumRate, decimal minPremium, 
        decimal maxPremium, string coverageDetails, long createdBy)
    {
        var command = new CreateInsurancePlanCommand
        {
            PlanName = planName,
            PlanDescription = planDescription,
            PremiumRate = premiumRate,
            MinPremium = minPremium,
            MaxPremium = maxPremium,
            CoverageDetails = coverageDetails,
            CreatedBy = createdBy
        };
        return await _mediator.Send(command);
    }

    public async Task<ApiResponse<InsurancePlanDto>> UpdateInsurancePlanAsync(
        long planId, string planName, string planDescription, decimal premiumRate, 
        decimal minPremium, decimal maxPremium, string coverageDetails, long modifiedBy)
    {
        var command = new UpdateInsurancePlanCommand
        {
            PlanId = planId,
            PlanName = planName,
            PlanDescription = planDescription,
            PremiumRate = premiumRate,
            MinPremium = minPremium,
            MaxPremium = maxPremium,
            CoverageDetails = coverageDetails,
            ModifiedBy = modifiedBy
        };
        return await _mediator.Send(command);
    }

    public async Task<ApiResponse<bool>> DeactivateInsurancePlanAsync(long planId, long modifiedBy)
    {
        var command = new DeactivateInsurancePlanCommand { PlanId = planId, ModifiedBy = modifiedBy };
        return await _mediator.Send(command);
    }

    public async Task<ApiResponse<bool>> ActivateInsurancePlanAsync(long planId, long modifiedBy)
    {
        var command = new ActivateInsurancePlanCommand { PlanId = planId, ModifiedBy = modifiedBy };
        return await _mediator.Send(command);
    }

    // Enrollment Mutations
    public async Task<ApiResponse<InsuranceEnrollmentDto>> EnrollEmployeeAsync(
        long empSysId, long insurancePlanId, string coverageType, DateTime enrollmentDate, 
        DateTime effectiveDate, long createdBy)
    {
        var command = new EnrollInsuranceCommand
        {
            EmpSysId = empSysId,
            InsurancePlanId = insurancePlanId,
            CoverageType = coverageType,
            EnrollmentDate = enrollmentDate,
            EffectiveDate = effectiveDate,
            CreatedBy = createdBy
        };
        return await _mediator.Send(command);
    }

    public async Task<ApiResponse<bool>> TerminateEnrollmentAsync(long enrollmentId, string reason, long modifiedBy)
    {
        var command = new TerminateEnrollmentCommand
        {
            EnrollmentId = enrollmentId,
            Reason = reason,
            ModifiedBy = modifiedBy
        };
        return await _mediator.Send(command);
    }

    public async Task<ApiResponse<bool>> SuspendEnrollmentAsync(long enrollmentId, long modifiedBy)
    {
        var command = new SuspendEnrollmentCommand { EnrollmentId = enrollmentId, ModifiedBy = modifiedBy };
        return await _mediator.Send(command);
    }

    public async Task<ApiResponse<bool>> ReactivateEnrollmentAsync(long enrollmentId, long modifiedBy)
    {
        var command = new ReactivateEnrollmentCommand { EnrollmentId = enrollmentId, ModifiedBy = modifiedBy };
        return await _mediator.Send(command);
    }

    // Claim Mutations
    public async Task<ApiResponse<InsuranceClaimDto>> SubmitClaimAsync(
        long empSysId, long enrollmentId, string claimType, decimal claimAmount, 
        DateTime serviceDate, string hospitalName, string remarks, long createdBy)
    {
        var command = new SubmitClaimCommand
        {
            EmpSysId = empSysId,
            EnrollmentId = enrollmentId,
            ClaimType = claimType,
            ClaimAmount = claimAmount,
            ServiceDate = serviceDate,
            HospitalName = hospitalName,
            Remarks = remarks,
            CreatedBy = createdBy
        };
        return await _mediator.Send(command);
    }

    public async Task<ApiResponse<InsuranceClaimDto>> ApproveClaimAsync(long claimId, decimal approvedAmount, long approvedBy)
    {
        var command = new ApproveClaimCommand { ClaimId = claimId, ApprovedAmount = approvedAmount, ApprovedBy = approvedBy };
        return await _mediator.Send(command);
    }

    public async Task<ApiResponse<InsuranceClaimDto>> RejectClaimAsync(long claimId, string rejectionReason, long rejectedBy)
    {
        var command = new RejectClaimCommand { ClaimId = claimId, RejectionReason = rejectionReason, RejectedBy = rejectedBy };
        return await _mediator.Send(command);
    }

    public async Task<ApiResponse<InsuranceClaimDto>> MarkClaimAsPaidAsync(long claimId, long paidBy)
    {
        var command = new MarkClaimAsPaidCommand { ClaimId = claimId, PaidBy = paidBy };
        return await _mediator.Send(command);
    }
}
