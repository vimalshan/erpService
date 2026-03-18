using Recruitment.Domain.Entities;
using Recruitment.Domain.Enums;
using Recruitment.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using AppApplication = Recruitment.Domain.Entities.Application;

namespace Recruitment.Infrastructure.Persistence;

/// <summary>
/// Service to seed initial data into the database
/// </summary>
public static class SeedDataService
{
    public static async Task SeedAsync(RecruitmentDbContext context)
    {
        try
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Check if we already have data - use a try-catch in case tables don't exist yet
            bool hasData = false;
            try
            {
                hasData = await context.RecruitmentCycles.AnyAsync();
            }
            catch
            {
                // Table might not exist yet, which is fine
                hasData = false;
            }

            // Only seed if tables are empty
            if (hasData)
                return;

            // Create recruitment cycles
            var cycle2024 = new RecruitmentCycle(
                recruitmentCycleNo: 1,
                effectiveFromDate: new DateTime(2024, 1, 1),
                endDate: new DateTime(2024, 12, 31)
            );
            cycle2024.CreatedDate = DateTime.UtcNow;
            cycle2024.CreatedBy = "System";
            cycle2024.ModifiedBy = "System";

            var cycle2025 = new RecruitmentCycle(
                recruitmentCycleNo: 2,
                effectiveFromDate: new DateTime(2025, 1, 1),
                endDate: new DateTime(2025, 12, 31)
            );
            cycle2025.CreatedDate = DateTime.UtcNow;
            cycle2025.CreatedBy = "System";
            cycle2025.ModifiedBy = "System";

            await context.RecruitmentCycles.AddRangeAsync(cycle2024, cycle2025);
            await context.SaveChangesAsync();

            // Create assessment parameters
            var param1 = new AssessmentParameter(
                recruitmentCycleNo: 1,
                parameterNo: 1,
                parameterName: "Written Test"
            );
            param1.CreatedDate = DateTime.UtcNow;
            param1.CreatedBy = "System";
            param1.ModifiedBy = "System";

            var param2 = new AssessmentParameter(
                recruitmentCycleNo: 1,
                parameterNo: 2,
                parameterName: "Technical Interview"
            );
            param2.CreatedDate = DateTime.UtcNow;
            param2.CreatedBy = "System";
            param2.ModifiedBy = "System";

            var param3 = new AssessmentParameter(
                recruitmentCycleNo: 1,
                parameterNo: 3,
                parameterName: "HR Interview"
            );
            param3.CreatedDate = DateTime.UtcNow;
            param3.CreatedBy = "System";
            param3.ModifiedBy = "System";

            await context.AssessmentParameters.AddRangeAsync(param1, param2, param3);
            await context.SaveChangesAsync();

            // Create jobs
            var job1 = new Job(
                jobId: 1,
                recruitmentCycleNo: 1,
                jobDescription: "Senior Software Engineer",
                roleDetails: "Design and develop scalable software solutions",
                cadreCode: "SE",
                effectiveDate: new DateTime(2024, 1, 1),
                principalAccount: "IT Department",
                jobType: "Full-Time",
                businessCode: "IT",
                unitCode: "ENG"
            );
            job1.CreatedDate = DateTime.UtcNow;
            job1.CreatedBy = "System";
            job1.ModifiedBy = "System";

            var job2 = new Job(
                jobId: 2,
                recruitmentCycleNo: 1,
                jobDescription: "Business Analyst",
                roleDetails: "Analyze business requirements and provide technical solutions",
                cadreCode: "BA",
                effectiveDate: new DateTime(2024, 1, 1),
                principalAccount: "Business Division",
                jobType: "Full-Time",
                businessCode: "BIZ",
                unitCode: "ANA"
            );
            job2.CreatedDate = DateTime.UtcNow;
            job2.CreatedBy = "System";
            job2.ModifiedBy = "System";

            var job3 = new Job(
                jobId: 3,
                recruitmentCycleNo: 2,
                jobDescription: "DevOps Engineer",
                roleDetails: "Manage infrastructure and CI/CD pipelines",
                cadreCode: "DO",
                effectiveDate: new DateTime(2025, 1, 1),
                principalAccount: "IT Department",
                jobType: "Full-Time",
                businessCode: "IT",
                unitCode: "DEV"
            );
            job3.CreatedDate = DateTime.UtcNow;
            job3.CreatedBy = "System";
            job3.ModifiedBy = "System";

            await context.Jobs.AddRangeAsync(job1, job2, job3);
            await context.SaveChangesAsync();

            // Create sample applications
            var app1 = new AppApplication(
                applicationNumber: 1001,
                jobId: 1,
                contactInfo: new ContactInfo(sparshId: "EMP001", sparshPin: 12345)
            );
            app1.CreatedDate = DateTime.UtcNow;
            app1.CreatedBy = "SPARSH_EMP001";
            app1.ModifiedBy = "System";
            app1.UpdateApplicationDetails(
                currentJobDescription: "Senior Developer",
                achievements: "Implemented microservices architecture",
                reasonForJoining: "Career growth opportunity",
                strength: "Strong problem-solving skills",
                awards: "Employee of the Year 2023"
            );

            var app2 = new AppApplication(
                applicationNumber: 1002,
                jobId: 2,
                contactInfo: new ContactInfo(sparshId: "EMP002", sparshPin: 12346)
            );
            app2.CreatedDate = DateTime.UtcNow;
            app2.CreatedBy = "SPARSH_EMP002";
            app2.ModifiedBy = "System";
            app2.UpdateApplicationDetails(
                currentJobDescription: "Project Manager",
                achievements: "Led cross-functional teams",
                reasonForJoining: "Want to transition to technical analysis",
                strength: "Excellent communication skills",
                awards: "Best Manager Award 2022"
            );

            var app3 = new AppApplication(
                applicationNumber: 1003,
                jobId: 1,
                contactInfo: new ContactInfo(sparshId: "EMP003", sparshPin: 12347)
            );
            app3.CreatedDate = DateTime.UtcNow;
            app3.CreatedBy = "SPARSH_EMP003";
            app3.ModifiedBy = "System";
            app3.UpdateApplicationDetails(
                currentJobDescription: "Software Engineer",
                achievements: "Developed RESTful APIs",
                reasonForJoining: "Senior role opportunity",
                strength: "Full-stack development expertise",
                awards: "Innovation Award 2023"
            );

            await context.Applications.AddRangeAsync(app1, app2, app3);
            await context.SaveChangesAsync();

            System.Console.WriteLine("Database seeded successfully");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error seeding database: {ex.Message}");
            throw;
        }
    }
}
