using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Entities;

namespace TransactionService.Infrastructure.Persistence.Seeds;

public static class TransactionDbSeed
{
    public static async Task SeedAsync(TransactionDbContext context)
    {
        if (!await context.ApprovalWorkflows.AnyAsync())
        {
            var workflows = new[]
            {
                ApprovalWorkflow.Create("BOOKING", 1, 1001, 2001, 2, "Room booking approval", 1),
                ApprovalWorkflow.Create("REIMBURSEMENT", 1, 1002, 2002, 3, "Travel reimbursement approval", 1),
                ApprovalWorkflow.Create("STIPEND", 1, 1003, 2003, 1, "Stipend disbursement approval", 1),
                ApprovalWorkflow.Create("TIMESHEET", 1, 1004, 2001, 2, "Weekly timesheet approval", 1),
                ApprovalWorkflow.Create("MEETING", 1, 1005, 2004, 1, "Meeting schedule approval", 1),
            };

            foreach (var wf in workflows)
                wf.ClearDomainEvents();

            await context.ApprovalWorkflows.AddRangeAsync(workflows);
            await context.SaveChangesAsync();
        }

        if (!await context.TransactionLogs.AnyAsync())
        {
            var logs = new[]
            {
                TransactionLog.Create("BOOKING", 1, "CREATE", 1001, "{\"room\":\"Conference A\"}", null, "SUBMITTED", "127.0.0.1"),
                TransactionLog.Create("REIMBURSEMENT", 1, "SUBMIT", 1002, "{\"amount\":5000}", null, "SUBMITTED", "127.0.0.1"),
                TransactionLog.Create("STIPEND", 1, "PROCESS", 1003, "{\"month\":\"2026-01\"}", null, "PROCESSED", "127.0.0.1"),
                TransactionLog.Create("TIMESHEET", 1, "APPROVE", 2001, null, "SUBMITTED", "APPROVED", "127.0.0.1"),
                TransactionLog.Create("MEETING", 1, "CREATE", 1005, "{\"type\":\"Review\"}", null, "SCHEDULED", "127.0.0.1"),
            };

            foreach (var log in logs)
                log.ClearDomainEvents();

            await context.TransactionLogs.AddRangeAsync(logs);
            await context.SaveChangesAsync();
        }
    }
}
