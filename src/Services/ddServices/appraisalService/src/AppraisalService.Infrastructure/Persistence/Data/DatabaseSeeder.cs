using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AppraisalService.Domain.Entities;
using AppraisalService.Domain;

namespace AppraisalService.Infrastructure.Persistence.Data;

/// <summary>
/// Seeds the database with test data for development and testing
/// </summary>
public class DatabaseSeeder
{
    private readonly AppraisalDbContext _context;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(AppraisalDbContext context, ILogger<DatabaseSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            // Check if data already exists
            if (await _context.AppraisalBands.AnyAsync())
            {
                _logger.LogInformation("Database already contains seed data. Skipping seeding.");
                return;
            }

            _logger.LogInformation("Starting database seeding...");

            await SeedAppraisalBands();
            await SeedAppraisalMainData();
            await SeedEmployeeGoals();
            await SeedCompetencyAssessments();

            _logger.LogInformation("Database seeding completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during database seeding");
            throw;
        }
    }

    private async Task SeedAppraisalBands()
    {
        var bands = new List<AppraisalBandEntity>
        {
            new AppraisalBandEntity(0, "Executive", "C-Level Executives", "CEO", "Chief Executive Officer", "EXE", 'Y', 1),
            new AppraisalBandEntity(0, "Senior Mgr", "Senior Management", "SVP", "Senior Vice President", "MGR", 'Y', 2),
            new AppraisalBandEntity(0, "Team Lead", "Team Leaders & Staff", "Director", "Director of Operations", "TL", 'Y', 3),
            new AppraisalBandEntity(0, "Individual IC", "Staff & Professionals", "Manager", "Department Manager", "IC", 'Y', 4)
        };

        await _context.AppraisalBands.AddRangeAsync(bands);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Seeded {Count} appraisal bands", bands.Count);
    }

    private async Task SeedAppraisalMainData()
    {
        // Appraisal 1
        var appraisal1 = new AppraisalMainEntity(1001, "EMP001", new DateTime(2026, 01, 15), 3, 10, 2026);
        appraisal1.SetEmployeeDetails("Mr.", "John", "Michael", "Smith", "Senior Developer", 101, 2001);
        appraisal1.SetAppraisalPeriod(new DateTime(2026, 01, 01), new DateTime(2026, 03, 31));

        // Appraisal 2
        var appraisal2 = new AppraisalMainEntity(1002, "EMP002", new DateTime(2026, 01, 16), 2, 10, 2026);
        appraisal2.SetEmployeeDetails("Ms.", "Sarah", "Ann", "Johnson", "Product Manager", 102, 2002);
        appraisal2.SetAppraisalPeriod(new DateTime(2026, 01, 01), new DateTime(2026, 03, 31));

        // Appraisal 3
        var appraisal3 = new AppraisalMainEntity(1003, "EMP003", new DateTime(2026, 01, 17), 4, 10, 2026);
        appraisal3.SetEmployeeDetails("Mr.", "Robert", "James", "Williams", "QA Engineer", 103, 2003);
        appraisal3.SetAppraisalPeriod(new DateTime(2026, 01, 01), new DateTime(2026, 03, 31));

        await _context.AppraisalMains.AddRangeAsync(appraisal1, appraisal2, appraisal3);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Seeded {Count} appraisal main records", 3);
    }

    private async Task SeedEmployeeGoals()
    {
        var goal1 = new EmployeeGoalEntity(1001, 1, "EMP001", 2001);
        goal1.PersonDesignation = "Senior Developer";
        goal1.UnitFrom = "Engineering";
        goal1.UnitTo = "Engineering";
        goal1.Weightage = 100m;
        goal1.Category = "Technical";
        goal1.UnitOfMeasure = "Completion %";
        goal1.FinancialStartDate = new DateTime(2026, 01, 01);
        goal1.FinancialEndDate = new DateTime(2026, 03, 31);
        goal1.ModifiedSerialNumber = 1;
        goal1.ExperienceCode = "EXP";
        goal1.GoalFlag = "Y";
        goal1.AddAchievements("Completed major components on schedule", "None");

        var goal2 = new EmployeeGoalEntity(1002, 1, "EMP002", 2002);
        goal2.PersonDesignation = "Product Manager";
        goal2.UnitFrom = "Product";
        goal2.UnitTo = "Product";
        goal2.Weightage = 100m;
        goal2.Category = "Business";
        goal2.UnitOfMeasure = "Goals Met %";
        goal2.FinancialStartDate = new DateTime(2026, 01, 01);
        goal2.FinancialEndDate = new DateTime(2026, 03, 31);
        goal2.ModifiedSerialNumber = 1;
        goal2.ExperienceCode = "EXP";
        goal2.GoalFlag = "Y";
        goal2.AddAchievements("Launched new feature ahead of schedule", "Minor resource constraints");

        await _context.EmployeeGoals.AddRangeAsync(goal1, goal2);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Seeded {Count} employee goals", 2);
    }

    private async Task SeedCompetencyAssessments()
    {
        var assessment1 = new CompetencyAssessmentEntity(1001, 1, 1, "MGR001");
        assessment1.SetAssessmentDetails(4m, 4m, "Excellent technical skills and problem-solving abilities",
            "Completed AWS certification", "Led architecture reviews for team", "Attended advanced C# workshop");
        assessment1.AppraiserUserNumber = 201;
        assessment1.PinNumber = 2001;
        assessment1.Role = "Manager";

        var assessment2 = new CompetencyAssessmentEntity(1001, 2, 2, "MGR001");
        assessment2.SetAssessmentDetails(3m, 3m, "Good communication and team collaboration",
            "Improved presentation skills", "Mentored 2 junior developers", "Leadership training");
        assessment2.AppraiserUserNumber = 201;
        assessment2.PinNumber = 2001;
        assessment2.Role = "Manager";

        var assessment3 = new CompetencyAssessmentEntity(1002, 1, 1, "SVP001");
        assessment3.SetAssessmentDetails(4m, 4m, "Strong strategic thinking and business acumen",
            "Completed MBA course", "Developed new product strategy", "Executive leadership program");
        assessment3.AppraiserUserNumber = 202;
        assessment3.PinNumber = 2002;
        assessment3.Role = "Senior Manager";

        await _context.CompetencyAssessments.AddRangeAsync(assessment1, assessment2, assessment3);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Seeded {Count} competency assessments", 3);
    }
}
