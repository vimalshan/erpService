using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Document.Domain.Entities;

namespace Document.Infrastructure.Persistence;

/// <summary>
/// Provides first-run seed data for the Document service database.
/// Invoke via <see cref="InitialiseAsync"/> during application startup (dev/staging only).
/// </summary>
public static class SeedData
{
    public static async Task InitialiseAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DocumentDbContext>();
        var logger  = scope.ServiceProvider.GetRequiredService<ILogger<DocumentDbContext>>();

        try
        {
            await context.Database.MigrateAsync();
            await SeedSignatoriesAsync(context);
            await SeedAppraisalLettersAsync(context);
            await context.SaveChangesAsync();
            logger.LogInformation("Database seed completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    // ──────────────────────────────────────────────────────────────────────
    // Signatories
    // ──────────────────────────────────────────────────────────────────────
    private static async Task SeedSignatoriesAsync(DocumentDbContext context)
    {
        if (await context.Signatories.AnyAsync()) return;

        var signatories = new[]
        {
            Signatory.Create(1m,  "John Smith",    "Chief Executive Officer",  employeeSysId: 1001m, imageFileName: "sig_001.png"),
            Signatory.Create(2m,  "Sarah Johnson", "Head of Human Resources",  employeeSysId: 1002m, imageFileName: "sig_002.png"),
            Signatory.Create(3m,  "Michael Chen",  "Director of Operations",   employeeSysId: 1003m)
        };

        // Deactivate the third signatory as sample inactive record
        signatories[2].Deactivate();

        await context.Signatories.AddRangeAsync(signatories);
    }

    // ──────────────────────────────────────────────────────────────────────
    // Appraisal Letters
    // ──────────────────────────────────────────────────────────────────────
    private static async Task SeedAppraisalLettersAsync(DocumentDbContext context)
    {
        if (await context.AppraisalLetters.AnyAsync()) return;

        var letters = new[]
        {
            AppraisalLetter.Create(
                serialNo:   1001m,
                letterType: "APR",
                fromDate:   new DateTime(2024, 1, 1),
                endDate:    new DateTime(2024, 12, 31),
                paragraph1: "Dear Employee, we are pleased to inform you of your annual appraisal outcome.",
                paragraph2: "Your performance has been assessed as Exceeds Expectations.",
                effectiveDate: new DateTime(2024, 4, 1)),

            AppraisalLetter.Create(
                serialNo:   1002m,
                letterType: "AN1",
                fromDate:   new DateTime(2024, 2, 1),
                endDate:    new DateTime(2024, 12, 31),
                paragraph1: "This letter confirms your employment with the company.",
                effectiveDate: new DateTime(2024, 2, 1)),

            AppraisalLetter.Create(
                serialNo:   1003m,
                letterType: "APR",
                fromDate:   new DateTime(2024, 3, 1),
                endDate:    new DateTime(2024, 12, 31),
                paragraph1: "We are delighted to confirm your promotion to Senior Analyst.",
                effectiveDate: new DateTime(2024, 3, 10))
        };

        await context.AppraisalLetters.AddRangeAsync(letters);
    }
}
