namespace InsuranceManagement.Application.DTOs;

/// <summary>
/// DTO for Insurance Plan
/// </summary>
public class InsurancePlanDto
{
    public long InsurancePlanId { get; set; }
    public string PlanName { get; set; } = string.Empty;
    public string PlanDescription { get; set; } = string.Empty;
    public decimal PremiumRate { get; set; }
    public decimal MinPremium { get; set; }
    public decimal MaxPremium { get; set; }
    public string CoverageDetails { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedOn { get; set; }
    public long CreatedBy { get; set; }
}

/// <summary>
/// DTO for creating Insurance Plan
/// </summary>
public class CreateInsurancePlanDto
{
    public string PlanName { get; set; } = string.Empty;
    public string PlanDescription { get; set; } = string.Empty;
    public decimal PremiumRate { get; set; }
    public decimal MinPremium { get; set; }
    public decimal MaxPremium { get; set; }
    public string CoverageDetails { get; set; } = string.Empty;
}

/// <summary>
/// DTO for updating Insurance Plan
/// </summary>
public class UpdateInsurancePlanDto
{
    public string PlanName { get; set; } = string.Empty;
    public string PlanDescription { get; set; } = string.Empty;
    public decimal PremiumRate { get; set; }
    public decimal MinPremium { get; set; }
    public decimal MaxPremium { get; set; }
    public string CoverageDetails { get; set; } = string.Empty;
}
