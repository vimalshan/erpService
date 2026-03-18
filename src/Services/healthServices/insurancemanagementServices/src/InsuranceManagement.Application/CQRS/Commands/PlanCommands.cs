using MediatR;
using InsuranceManagement.Application.DTOs;

namespace InsuranceManagement.Application.CQRS.Commands;

/// <summary>
/// Command to create insurance plan
/// </summary>
public class CreateInsurancePlanCommand : IRequest<ApiResponse<InsurancePlanDto>>
{
    public string PlanName { get; set; } = string.Empty;
    public string PlanDescription { get; set; } = string.Empty;
    public decimal PremiumRate { get; set; }
    public decimal MinPremium { get; set; }
    public decimal MaxPremium { get; set; }
    public string CoverageDetails { get; set; } = string.Empty;
    public long CreatedBy { get; set; }
}

/// <summary>
/// Command to update insurance plan
/// </summary>
public class UpdateInsurancePlanCommand : IRequest<ApiResponse<InsurancePlanDto>>
{
    public long PlanId { get; set; }
    public string PlanName { get; set; } = string.Empty;
    public string PlanDescription { get; set; } = string.Empty;
    public decimal PremiumRate { get; set; }
    public decimal MinPremium { get; set; }
    public decimal MaxPremium { get; set; }
    public string CoverageDetails { get; set; } = string.Empty;
    public long ModifiedBy { get; set; }
}

/// <summary>
/// Command to deactivate insurance plan
/// </summary>
public class DeactivateInsurancePlanCommand : IRequest<ApiResponse<bool>>
{
    public long PlanId { get; set; }
    public long ModifiedBy { get; set; }
}

/// <summary>
/// Command to activate insurance plan
/// </summary>
public class ActivateInsurancePlanCommand : IRequest<ApiResponse<bool>>
{
    public long PlanId { get; set; }
    public long ModifiedBy { get; set; }
}
