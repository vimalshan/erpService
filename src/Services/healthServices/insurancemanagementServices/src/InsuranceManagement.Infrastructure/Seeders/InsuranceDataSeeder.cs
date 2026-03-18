using InsuranceManagement.Domain.Entities;
using InsuranceManagement.Domain.ValueObjects;
using InsuranceManagement.Infrastructure.Data;

namespace InsuranceManagement.Infrastructure.Seeders;

/// <summary>
/// Seeder for initial insurance data
/// </summary>
public static class InsuranceDataSeeder
{
    /// <summary>
    /// Seed initial insurance plans
    /// </summary>
    public static async Task SeedInsurancePlansAsync(InsuranceManagementDbContext context)
    {
        // Check if data already exists
        if (context.InsurancePlans.Any())
            return;

        var plans = new List<InsurancePlan>
        {
            new InsurancePlan(
                planName: "Basic Health Coverage",
                planDescription: "Comprehensive health coverage plan with basic benefits",
                premiumRate: 2.5m,
                minPremium: 2000.00m,
                maxPremium: 3500.00m,
                coverageDetails: "In-patient and out-patient coverage with 100% reimbursement",
                createdBy: 1),

            new InsurancePlan(
                planName: "Premium Family Coverage",
                planDescription: "Premium family insurance plan with extended benefits",
                premiumRate: 5.5m,
                minPremium: 5000.00m,
                maxPremium: 7000.00m,
                coverageDetails: "Full family coverage including dental and optical benefits",
                createdBy: 1),

            new InsurancePlan(
                planName: "Dental and Optical Plan",
                planDescription: "Specialized plan for dental and optical coverage",
                premiumRate: 0.8m,
                minPremium: 600.00m,
                maxPremium: 1200.00m,
                coverageDetails: "50% reimbursement for dental and 75% for optical services",
                createdBy: 1),

            new InsurancePlan(
                planName: "Senior Citizen Plan",
                planDescription: "Special health insurance plan for senior citizens",
                premiumRate: 3.5m,
                minPremium: 3000.00m,
                maxPremium: 4500.00m,
                coverageDetails: "Comprehensive coverage with enhanced benefits for senior citizens",
                createdBy: 1),

            new InsurancePlan(
                planName: "Maternity and Child Care",
                planDescription: "Specialized plan for maternity and child care coverage",
                premiumRate: 3.0m,
                minPremium: 2500.00m,
                maxPremium: 4000.00m,
                coverageDetails: "Full coverage for maternity, delivery, and child health care",
                createdBy: 1)
        };

        context.InsurancePlans.AddRange(plans);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Seed sample enrollments (optional)
    /// </summary>
    public static async Task SeedSampleEnrollmentsAsync(InsuranceManagementDbContext context)
    {
        // Check if data already exists
        if (context.InsuranceEnrollments.Any())
            return;

        var plans = context.InsurancePlans.Take(2).ToList();
        if (plans.Count < 2)
            return;

        var enrollments = new List<InsuranceEnrollment>
        {
            new InsuranceEnrollment(
                empSysId: 1001,
                insurancePlanId: plans[0].InsurancePlanId,
                coverageType: CoverageType.Employee_Coverage,
                enrollmentDate: DateTime.UtcNow.AddMonths(-2),
                effectiveDate: DateTime.UtcNow.AddMonths(-2),
                monthlyPremium: plans[0].PremiumRate * 100,
                createdBy: 1),

            new InsuranceEnrollment(
                empSysId: 1002,
                insurancePlanId: plans[1].InsurancePlanId,
                coverageType: CoverageType.Family_Coverage,
                enrollmentDate: DateTime.UtcNow.AddMonths(-1),
                effectiveDate: DateTime.UtcNow.AddMonths(-1),
                monthlyPremium: plans[1].PremiumRate * 100,
                createdBy: 1)
        };

        context.InsuranceEnrollments.AddRange(enrollments);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Clear all insurance data (for testing/reset purposes)
    /// </summary>
    public static async Task ClearAllDataAsync(InsuranceManagementDbContext context)
    {
        // Note: Be cautious with this method - it deletes all data
        context.InsuranceClaims.RemoveRange(context.InsuranceClaims);
        context.InsuranceEnrollments.RemoveRange(context.InsuranceEnrollments);
        context.InsurancePlans.RemoveRange(context.InsurancePlans);
        
        await context.SaveChangesAsync();
    }
}
