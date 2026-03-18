using ApprovalService.Domain.Entities;
using ApprovalService.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ApprovalService.Infrastructure.Database
{
    /// <summary>
    /// Database seeding utility for ApprovalServiceDbContext
    /// Provides sample data for development and testing
    /// </summary>
    public static class DbSeed
    {
        /// <summary>
        /// Seeds the database with initial data if tables are empty
        /// Called during application startup
        /// </summary>
        /// <param name="context">The ApprovalServiceDbContext instance</param>
        /// <returns>Task for async operation</returns>
        public static async Task SeedDatabaseAsync(ApprovalServiceDbContext context)
        {
            try
            {
                // Check if approval masters already exist (avoid duplicate seeding)
                if (await context.ApprovalMasters.AnyAsync())
                {
                    return; // Database already seeded
                }

                // Seed Approval Masters
                var approvalMasters = new List<ApprovalMaster>
                {
                    // Travel Request Approval (3 levels)
                    ApprovalMaster.Create(
                        approvalCode: "TRV_REQ",
                        description: "Travel Request Approval",
                        module: "Travel",
                        level: 3,
                        createdBy: "SYSTEM"
                    ),

                    // Leave Request Approval (2 levels)
                    ApprovalMaster.Create(
                        approvalCode: "LEV_REQ",
                        description: "Leave Request Approval",
                        module: "Leave",
                        level: 2,
                        createdBy: "SYSTEM"
                    ),

                    // Expense Report Approval (2 levels)
                    ApprovalMaster.Create(
                        approvalCode: "EXP_RPT",
                        description: "Expense Report Approval",
                        module: "Finance",
                        level: 2,
                        createdBy: "SYSTEM"
                    ),

                    // Document Approval (4 levels)
                    ApprovalMaster.Create(
                        approvalCode: "DOC_APR",
                        description: "Document Approval",
                        module: "Admin",
                        level: 4,
                        createdBy: "SYSTEM"
                    )
                };

                // Add approval masters to database
                await context.ApprovalMasters.AddRangeAsync(approvalMasters);
                await context.SaveChangesAsync();

                // Retrieve the saved records with their IDs
                var travelApproval = approvalMasters.First(a => a.ApprovalCode == "TRV_REQ");
                var leaveApproval = approvalMasters.First(a => a.ApprovalCode == "LEV_REQ");
                var expenseApproval = approvalMasters.First(a => a.ApprovalCode == "EXP_RPT");
                var documentApproval = approvalMasters.First(a => a.ApprovalCode == "DOC_APR");

                // Seed Approver Employees
                var today = DateOnly.FromDateTime(DateTime.Now);

                var approverEmployees = new List<ApproverEmployee>
                {
                    // Travel Request Approvers
                    ApproverEmployee.Create(
                        approvalMasterId: travelApproval.Id,
                        employeeId: 1001,
                        level: 1,
                        effectiveFromDate: today,
                        createdBy: "SYSTEM"
                    ),
                    ApproverEmployee.Create(
                        approvalMasterId: travelApproval.Id,
                        employeeId: 1002,
                        level: 2,
                        effectiveFromDate: today,
                        createdBy: "SYSTEM"
                    ),
                    ApproverEmployee.Create(
                        approvalMasterId: travelApproval.Id,
                        employeeId: 1003,
                        level: 3,
                        effectiveFromDate: today,
                        createdBy: "SYSTEM"
                    ),

                    // Leave Request Approvers
                    ApproverEmployee.Create(
                        approvalMasterId: leaveApproval.Id,
                        employeeId: 1004,
                        level: 1,
                        effectiveFromDate: today,
                        createdBy: "SYSTEM"
                    ),
                    ApproverEmployee.Create(
                        approvalMasterId: leaveApproval.Id,
                        employeeId: 1005,
                        level: 2,
                        effectiveFromDate: today,
                        createdBy: "SYSTEM"
                    ),

                    // Expense Report Approvers
                    ApproverEmployee.Create(
                        approvalMasterId: expenseApproval.Id,
                        employeeId: 1006,
                        level: 1,
                        effectiveFromDate: today,
                        createdBy: "SYSTEM"
                    ),
                    ApproverEmployee.Create(
                        approvalMasterId: expenseApproval.Id,
                        employeeId: 1007,
                        level: 2,
                        effectiveFromDate: today,
                        createdBy: "SYSTEM"
                    ),

                    // Document Approval Approvers
                    ApproverEmployee.Create(
                        approvalMasterId: documentApproval.Id,
                        employeeId: 1001,
                        level: 1,
                        effectiveFromDate: today,
                        createdBy: "SYSTEM"
                    ),
                    ApproverEmployee.Create(
                        approvalMasterId: documentApproval.Id,
                        employeeId: 1002,
                        level: 2,
                        effectiveFromDate: today,
                        createdBy: "SYSTEM"
                    ),
                    ApproverEmployee.Create(
                        approvalMasterId: documentApproval.Id,
                        employeeId: 1003,
                        level: 3,
                        effectiveFromDate: today,
                        createdBy: "SYSTEM"
                    ),
                    ApproverEmployee.Create(
                        approvalMasterId: documentApproval.Id,
                        employeeId: 1004,
                        level: 4,
                        effectiveFromDate: today,
                        createdBy: "SYSTEM"
                    )
                };

                // Add approver employees to database
                await context.ApproverEmployees.AddRangeAsync(approverEmployees);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log or handle seeding errors
                throw new InvalidOperationException("Database seeding failed.", ex);
            }
        }

        /// <summary>
        /// Extension method for WebApplication to ease seeding integration into Program.cs
        /// </summary>
        /// <param name="app">The WebApplication instance</param>
        /// <returns>Task for async operation</returns>
        public static async Task MigrateAndSeedDatabaseAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApprovalServiceDbContext>();

                try
                {
                    // Apply any pending migrations to the database
                    await context.Database.MigrateAsync();

                    // Seed initial data if database is empty
                    await SeedDatabaseAsync(context);
                }
                catch (Exception ex)
                {
                    // Log migration/seeding errors
                    // In production, implement proper logging here
                    throw new InvalidOperationException(
                        "An error occurred while migrating or seeding the database.",
                        ex
                    );
                }
            }
        }
    }
}
