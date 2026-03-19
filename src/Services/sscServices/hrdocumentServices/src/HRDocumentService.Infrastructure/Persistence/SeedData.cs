using HRDocumentService.Domain.Entities;
using HRDocumentService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HRDocumentService.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HRDocumentDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<HRDocumentDbContext>>();

        try
        {
            await context.Database.MigrateAsync();
            await SeedHRDocumentsAsync(context);
            logger.LogInformation("Database seeded successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }

    private static async Task SeedHRDocumentsAsync(HRDocumentDbContext context)
    {
        if (await context.HRDocuments.AnyAsync())
            return;

        var doc1 = HRDocument.Create(
            1, 1001, DocumentType.Create("PAY"), 5001, 1, 1,
            "Monthly payroll document Jan 2026", 100, DocumentSource.Create("SSC"),
            "REF-001", "John Smith");

        var doc2 = HRDocument.Create(
            2, 1002, DocumentType.Create("TAX"), 5002, 1, 1,
            "Tax filing document Q1 2026", 101, DocumentSource.Create("HRD"),
            "REF-002", "Jane Doe");

        var doc3 = HRDocument.Create(
            3, 1003, DocumentType.Create("INS"), 5003, 2, 1,
            "Insurance enrollment form", 102, DocumentSource.Create("EMP"),
            "REF-003", "Bob Wilson");

        // Clear domain events from seed data
        doc1.ClearDomainEvents();
        doc2.ClearDomainEvents();
        doc3.ClearDomainEvents();

        context.HRDocuments.AddRange(doc1, doc2, doc3);

        var file1 = HRDocumentFile.Create(1, 1, "/docs", "payroll_jan2026.pdf");
        var file2 = HRDocumentFile.Create(2, 2, "/docs", "tax_q1_2026.pdf");

        context.HRDocumentFiles.AddRange(file1, file2);

        await context.SaveChangesAsync();
    }
}
