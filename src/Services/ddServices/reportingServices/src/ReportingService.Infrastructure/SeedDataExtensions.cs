using Microsoft.EntityFrameworkCore;
using ReportingService.Domain.Entities;
using ReportingService.Infrastructure.Data;

namespace ReportingService.Infrastructure;

public static class SeedDataExtensions
{
    public static async Task SeedAppraisalDataAsync(this ReportingDbContext context)
    {
        // Seed Appraisals
        if (!context.Appraisals.Any())
        {
            var appraisals = new List<Appraisal>
            {
                new Appraisal(1, "John Doe", "JD001")
                {
                    StatusDescription = "In Progress",
                    FinancialPeriod = "2025-2026",
                    UnitCode = "001",
                    GradeCode = "G01",
                    AcademicYear = "2025-26",
                    DDType = "STANDARD",
                    CompletionFlag = '0'
                },
                new Appraisal(2, "Jane Smith", "JS002")
                {
                    StatusDescription = "Completed",
                    FinancialPeriod = "2025-2026",
                    UnitCode = "002",
                    GradeCode = "G02",
                    AcademicYear = "2025-26",
                    DDType = "STANDARD",
                    CompletionFlag = '1'
                },
                new Appraisal(3, "Robert Johnson", "RJ003")
                {
                    StatusDescription = "Pending",
                    FinancialPeriod = "2025-2026",
                    UnitCode = "001",
                    GradeCode = "G01",
                    AcademicYear = "2025-26",
                    DDType = "SPECIAL",
                    CompletionFlag = '0'
                }
            };

            await context.Appraisals.AddRangeAsync(appraisals);
            await context.SaveChangesAsync();
        }

        // Seed AppraisalGoals
        if (!context.AppraisalGoals.Any())
        {
            var goals = new List<AppraisalGoal>
            {
                new AppraisalGoal(1, "Improve customer satisfaction by 10%")
                {
                    Weightage = 25,
                    Category = "Performance",
                    AppraisalStatus = 'A'
                },
                new AppraisalGoal(1, "Complete training certification program")
                {
                    Weightage = 15,
                    Category = "Development",
                    AppraisalStatus = 'A'
                },
                new AppraisalGoal(2, "Lead cross-functional team project")
                {
                    Weightage = 30,
                    Category = "Leadership",
                    AppraisalStatus = 'C'
                },
                new AppraisalGoal(3, "Reduce operational costs by 5%")
                {
                    Weightage = 20,
                    Category = "Business",
                    AppraisalStatus = 'P'
                }
            };

            await context.AppraisalGoals.AddRangeAsync(goals);
            await context.SaveChangesAsync();
        }

        // Seed AppraiseePerformances
        if (!context.AppraiseePerformances.Any())
        {
            var performances = new List<AppraiseePerformance>
            {
                new AppraiseePerformance(1, 1)
                {
                    Description = "Customer Service Delivery",
                    PerformanceRatingValue = 4,
                    MeanRating = 4.0m,
                    PerformanceCategory = "Core Competencies"
                },
                new AppraiseePerformance(1, 2)
                {
                    Description = "Technical Skills",
                    PerformanceRatingValue = 3,
                    MeanRating = 3.5m,
                    PerformanceCategory = "Technical"
                },
                new AppraiseePerformance(2, 1)
                {
                    Description = "Leadership and Mentoring",
                    PerformanceRatingValue = 5,
                    MeanRating = 4.5m,
                    PerformanceCategory = "Leadership"
                },
                new AppraiseePerformance(3, 1)
                {
                    Description = "Project Management",
                    PerformanceRatingValue = 4,
                    MeanRating = 4.0m,
                    PerformanceCategory = "Project Management"
                }
            };

            await context.AppraiseePerformances.AddRangeAsync(performances);
            await context.SaveChangesAsync();
        }

        // Seed DDRatings
        if (!context.DDRatings.Any())
        {
            var ratings = new List<DDRating>
            {
                new DDRating("EMP001", "BUS001", "UNIT001")
                {
                    BusinessName = "Finance Department",
                    UnitName = "Accounting",
                    Rating1 = 5,
                    Rating2 = 4,
                    Rating3 = 3,
                    Rating4 = 4,
                    Rating5 = 5,
                    TotalRating = 21,
                    TotalPercentage = 84
                },
                new DDRating("EMP002", "BUS002", "UNIT002")
                {
                    BusinessName = "Operations Department",
                    UnitName = "Process Management",
                    Rating1 = 4,
                    Rating2 = 4,
                    Rating3 = 4,
                    Rating4 = 3,
                    Rating5 = 4,
                    TotalRating = 19,
                    TotalPercentage = 76
                },
                new DDRating("EMP003", "BUS001", "UNIT001")
                {
                    BusinessName = "Finance Department",
                    UnitName = "Planning",
                    Rating1 = 5,
                    Rating2 = 5,
                    Rating3 = 4,
                    Rating4 = 5,
                    Rating5 = 5,
                    TotalRating = 24,
                    TotalPercentage = 96
                }
            };

            await context.DDRatings.AddRangeAsync(ratings);
            await context.SaveChangesAsync();
        }
    }
}
