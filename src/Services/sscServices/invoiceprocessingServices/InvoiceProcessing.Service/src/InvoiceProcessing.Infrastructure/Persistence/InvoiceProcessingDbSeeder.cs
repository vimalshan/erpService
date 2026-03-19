using InvoiceProcessing.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvoiceProcessing.Infrastructure.Persistence;

public static class InvoiceProcessingDbSeeder
{
    public static async Task SeedAsync(InvoiceProcessingDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.DocumentStatuses.AnyAsync()) return;

        var statuses = new List<DocumentStatus>();
        // Seed statuses via raw SQL since the entity uses private setters
        await context.Database.ExecuteSqlRawAsync(@"
            INSERT INTO DOC_STATUS (DOC_FLAG, DOC_TYPE, DOC_COMPLETEDREM, DOC_PENDINGREM, DOC_STAGEORDER, DOC_CATGROUP, DOC_STAGENO)
            VALUES
            ('DR', 'D', 'Draft Created', 'Pending Draft', 1, 'Documentation', 1),
            ('SB', 'D', 'Submitted', 'Pending Submission', 2, 'Documentation', 2),
            ('RC', 'D', 'Received at SSC', 'Pending Receipt', 3, 'Documentation', 3),
            ('IP', 'P', 'Invoice Processed', 'Pending Processing', 4, 'Processing', 4),
            ('IV', 'P', 'Invoice Validated', 'Pending Validation', 5, 'Processing', 5),
            ('AP', 'A', 'Approved', 'Pending Approval', 6, 'Approval', 6),
            ('RJ', 'A', 'Rejected', 'Pending Review', 7, 'Approval', 7),
            ('CN', 'C', 'Cancelled', 'Active', 8, 'Cancellation', 8),
            ('HD', 'H', 'On Hold', 'Active', 9, 'Hold', 9),
            ('CM', 'F', 'Completed', 'In Progress', 10, 'Final', 10)
        ");

        await context.Database.ExecuteSqlRawAsync(@"
            INSERT INTO DOC_COUNTER (DOC_BUID, DOC_NO)
            VALUES ('BU001', 1000), ('BU002', 2000), ('BU003', 3000)
        ");

        await context.Database.ExecuteSqlRawAsync(@"
            SET IDENTITY_INSERT DOC_REPORTFIELDS ON;
            INSERT INTO DOC_REPORTFIELDS (RPT_FIELDID, RPT_COLFIELD, RPT_COLDISPFIELD)
            VALUES
            (1, 'DOC_ID', 'Document ID'),
            (2, 'DOC_NO', 'Document No'),
            (3, 'DOC_INVOICENO', 'Invoice No'),
            (4, 'DOC_INVAMT', 'Invoice Amount'),
            (5, 'DOC_INVDATE', 'Invoice Date'),
            (6, 'DOC_DOCSTATUS', 'Status'),
            (7, 'DOC_ORGID', 'Organization'),
            (8, 'DOC_PONO', 'PO Number');
            SET IDENTITY_INSERT DOC_REPORTFIELDS OFF;
        ");
    }
}
