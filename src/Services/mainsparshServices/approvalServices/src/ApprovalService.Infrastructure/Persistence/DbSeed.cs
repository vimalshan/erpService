namespace ApprovalService.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using ApprovalService.Domain.Entities;

/// <summary>
/// Seed data for the database
/// </summary>
public static class DbSeed
{
    public static async Task SeedDatabaseAsync(ApprovalServiceDbContext context)
    {
        if (await context.ApprovalMasters.AnyAsync())
        {
            return; // Database has been seeded
        }

        try
        {
            // Seed Approval Masters
            var travelApproval = ApprovalMaster.Create(
                "TRAVEL_APR",
                "Travel Request Approval",
                "PER",
                3,
                1);

            var leaveApproval = ApprovalMaster.Create(
                "LEAVE_APR",
                "Leave Request Approval",
                "PER",
                2,
                1);

            var expenseApproval = ApprovalMaster.Create(
                "EXPENSE_APR",
                "Expense Report Approval",
                "PER",
                2,
                1);

            var documentApproval = ApprovalMaster.Create(
                "DOC_APR",
                "Document Approval Process",
                "DDP",
                4,
                1);

            await context.ApprovalMasters.AddAsync(travelApproval);
            await context.ApprovalMasters.AddAsync(leaveApproval);
            await context.ApprovalMasters.AddAsync(expenseApproval);
            await context.ApprovalMasters.AddAsync(documentApproval);

            await context.SaveChangesAsync();

            // Seed Approver Employees
            var approversForTravel = new[]
            {
                ApproverEmployee.Create(travelApproval.Id, 1001, 1, DateTime.UtcNow.AddDays(-365), null, 1),
                ApproverEmployee.Create(travelApproval.Id, 1002, 2, DateTime.UtcNow.AddDays(-365), null, 1),
                ApproverEmployee.Create(travelApproval.Id, 1003, 3, DateTime.UtcNow.AddDays(-365), null, 1),
            };

            var approversForLeave = new[]
            {
                ApproverEmployee.Create(leaveApproval.Id, 1001, 1, DateTime.UtcNow.AddDays(-365), null, 1),
                ApproverEmployee.Create(leaveApproval.Id, 1004, 2, DateTime.UtcNow.AddDays(-365), null, 1),
            };

            var approversForExpense = new[]
            {
                ApproverEmployee.Create(expenseApproval.Id, 1002, 1, DateTime.UtcNow.AddDays(-365), null, 1),
                ApproverEmployee.Create(expenseApproval.Id, 1005, 2, DateTime.UtcNow.AddDays(-365), null, 1),
            };

            var approversForDocument = new[]
            {
                ApproverEmployee.Create(documentApproval.Id, 1001, 1, DateTime.UtcNow.AddDays(-365), null, 1),
                ApproverEmployee.Create(documentApproval.Id, 1002, 2, DateTime.UtcNow.AddDays(-365), null, 1),
                ApproverEmployee.Create(documentApproval.Id, 1006, 3, DateTime.UtcNow.AddDays(-365), null, 1),
                ApproverEmployee.Create(documentApproval.Id, 1007, 4, DateTime.UtcNow.AddDays(-365), null, 1),
            };

            foreach (var approver in approversForTravel.Concat(approversForLeave).Concat(approversForExpense).Concat(approversForDocument))
            {
                await context.ApproverEmployees.AddAsync(approver);
            }

            await context.SaveChangesAsync();

            Console.WriteLine("Database seeded successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding database: {ex.Message}");
            throw;
        }
    }
}
