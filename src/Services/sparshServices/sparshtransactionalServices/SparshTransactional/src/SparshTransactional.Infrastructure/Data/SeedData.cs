using Microsoft.EntityFrameworkCore;
using SparshTransactional.Domain.Entities;

namespace SparshTransactional.Infrastructure.Data;

public static class SeedData
{
    public static async Task SeedAsync(SparshTransactionalDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!await context.Scholarships.AnyAsync())
        {
            context.Scholarships.AddRange(
                new ScholarshipMaster
                {
                    ScholarshipName = "Merit Excellence Scholarship",
                    ScholarshipDescription = "Full scholarship for top-performing students",
                    ScholarshipType = "M",
                    CoveragePercent = 100,
                    MaxAmount = 500000,
                    Status = "A",
                    CreatedBy = 1,
                    CreatedOn = DateTime.UtcNow
                },
                new ScholarshipMaster
                {
                    ScholarshipName = "Need-Based Financial Aid",
                    ScholarshipDescription = "Scholarship for students from economically weaker sections",
                    ScholarshipType = "N",
                    CoveragePercent = 75,
                    MaxAmount = 300000,
                    Status = "A",
                    CreatedBy = 1,
                    CreatedOn = DateTime.UtcNow
                },
                new ScholarshipMaster
                {
                    ScholarshipName = "Employee Dependent Scholarship",
                    ScholarshipDescription = "Scholarship for dependents of company employees",
                    ScholarshipType = "E",
                    CoveragePercent = 50,
                    MaxAmount = 200000,
                    Status = "A",
                    CreatedBy = 1,
                    CreatedOn = DateTime.UtcNow
                }
            );
            await context.SaveChangesAsync();
        }

        if (!await context.EligibilityCriteria.AnyAsync())
        {
            context.EligibilityCriteria.AddRange(
                new EligibilityCriteria
                {
                    ScholarshipId = 1,
                    CriteriaName = "Minimum GPA",
                    CriteriaDescription = "Student must maintain minimum 3.5 GPA",
                    MinScore = 3.5m,
                    EligibilityStatus = "A",
                    CreatedBy = 1,
                    CreatedOn = DateTime.UtcNow
                },
                new EligibilityCriteria
                {
                    ScholarshipId = 2,
                    CriteriaName = "Family Income Threshold",
                    CriteriaDescription = "Annual family income must be below threshold",
                    MaxFamilyIncome = 500000,
                    EligibilityStatus = "A",
                    CreatedBy = 1,
                    CreatedOn = DateTime.UtcNow
                },
                new EligibilityCriteria
                {
                    ScholarshipId = 3,
                    CriteriaName = "Employment Verification",
                    CriteriaDescription = "Parent/guardian must be active employee",
                    EligibilityStatus = "A",
                    CreatedBy = 1,
                    CreatedOn = DateTime.UtcNow
                }
            );
            await context.SaveChangesAsync();
        }

        if (!await context.Applications.AnyAsync())
        {
            context.Applications.Add(new ScholarshipApplication
            {
                StudentId = 1001,
                ScholarshipId = 1,
                ApplicationDate = DateTime.UtcNow.AddDays(-30),
                FamilyIncome = 300000,
                ApplicationStatus = "S",
                CreatedBy = 1001,
                CreatedOn = DateTime.UtcNow.AddDays(-30)
            });
            context.Applications.Add(new ScholarshipApplication
            {
                StudentId = 1002,
                ScholarshipId = 2,
                ApplicationDate = DateTime.UtcNow.AddDays(-15),
                FamilyIncome = 200000,
                ApplicationStatus = "A",
                ApprovedAmount = 225000,
                ApprovedBy = 1,
                CreatedBy = 1002,
                CreatedOn = DateTime.UtcNow.AddDays(-15),
                UpdatedOn = DateTime.UtcNow.AddDays(-10)
            });
            await context.SaveChangesAsync();
        }

        if (!await context.Disbursements.AnyAsync())
        {
            context.Disbursements.Add(new ScholarshipDisbursement
            {
                ApplicationId = 2,
                StudentId = 1002,
                ScholarshipId = 2,
                DisbursementAmount = 225000,
                DisbursementStatus = "P",
                CreatedBy = 1,
                CreatedOn = DateTime.UtcNow.AddDays(-10)
            });
            await context.SaveChangesAsync();
        }
    }
}
