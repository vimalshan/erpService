using InsuranceManagement.Domain.Common;

namespace InsuranceManagement.Domain.Entities;

/// <summary>
/// Insurance Plan master entity representing different insurance plans available
/// </summary>
public class InsurancePlan : AggregateRoot
{
    public long InsurancePlanId { get; private set; }
    public string PlanName { get; private set; } = string.Empty;
    public string PlanDescription { get; private set; } = string.Empty;
    public decimal PremiumRate { get; private set; } // Percentage
    public decimal MinPremium { get; private set; }
    public decimal MaxPremium { get; private set; }
    public string CoverageDetails { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime? ModifiedOn { get; private set; }
    public long? ModifiedBy { get; private set; }

    // EF constructor
    private InsurancePlan() { }

    public InsurancePlan(
        string planName,
        string planDescription,
        decimal premiumRate,
        decimal minPremium,
        decimal maxPremium,
        string coverageDetails,
        long createdBy)
    {
        if (string.IsNullOrWhiteSpace(planName))
            throw new ArgumentException("Plan name cannot be empty", nameof(planName));

        if (premiumRate < 0 || premiumRate > 100)
            throw new ArgumentException("Premium rate must be between 0 and 100", nameof(premiumRate));

        if (minPremium < 0)
            throw new ArgumentException("Minimum premium cannot be negative", nameof(minPremium));

        if (maxPremium < minPremium)
            throw new ArgumentException("Maximum premium must be greater than or equal to minimum", nameof(maxPremium));

        PlanName = planName;
        PlanDescription = planDescription;
        PremiumRate = premiumRate;
        MinPremium = minPremium;
        MaxPremium = maxPremium;
        CoverageDetails = coverageDetails;
        IsActive = true;
        CreatedOn = DateTime.UtcNow;
        CreatedBy = createdBy;
    }

    public void Update(string planName, string description, decimal premiumRate, 
        decimal minPremium, decimal maxPremium, string coverageDetails, long modifiedBy)
    {
        if (string.IsNullOrWhiteSpace(planName))
            throw new ArgumentException("Plan name cannot be empty", nameof(planName));

        PlanName = planName;
        PlanDescription = description;
        PremiumRate = premiumRate;
        MinPremium = minPremium;
        MaxPremium = maxPremium;
        CoverageDetails = coverageDetails;
        ModifiedOn = DateTime.UtcNow;
        ModifiedBy = modifiedBy;
    }

    public void Deactivate(long modifiedBy)
    {
        IsActive = false;
        ModifiedOn = DateTime.UtcNow;
        ModifiedBy = modifiedBy;
    }

    public void Activate(long modifiedBy)
    {
        IsActive = true;
        ModifiedOn = DateTime.UtcNow;
        ModifiedBy = modifiedBy;
    }
}
