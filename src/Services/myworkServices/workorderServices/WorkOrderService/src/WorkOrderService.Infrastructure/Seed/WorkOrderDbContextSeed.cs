using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkOrderService.Domain.Entities;
using WorkOrderService.Infrastructure.Persistence;

namespace WorkOrderService.Infrastructure.Seed;

public static class WorkOrderDbContextSeed
{
    public static async Task SeedAsync(WorkOrderDbContext context, ILogger? logger = null)
    {
        if (await context.WorkOrders.AnyAsync())
        {
            logger?.LogInformation("Database already seeded — skipping");
            return;
        }

        logger?.LogInformation("Seeding WORK_ORDER and WORK_TASK tables...");

        // ── Work Orders ─────────────────────────────────────────────────
        var wo1 = new WorkOrder(
            "Server Room Maintenance",
            "Quarterly server room maintenance and inspection",
            DateTime.UtcNow.AddDays(30),
            assignedTo: 1001,
            createdBy: 1000);

        var wo2 = new WorkOrder(
            "Network Upgrade Project",
            "Upgrade office network infrastructure to 10Gbps",
            DateTime.UtcNow.AddDays(60),
            assignedTo: 1002,
            createdBy: 1000);

        var wo3 = new WorkOrder(
            "Workstation Deployment",
            "Deploy 50 new workstations to floor 3",
            DateTime.UtcNow.AddDays(14),
            assignedTo: 1003,
            createdBy: 1000);

        var wo4 = new WorkOrder(
            "Security Audit",
            "Annual security audit of all systems and access controls",
            DateTime.UtcNow.AddDays(45),
            assignedTo: 1004,
            createdBy: 1000);

        var wo5 = new WorkOrder(
            "Office Relocation IT Setup",
            "Set up IT infrastructure for new office on floor 5",
            DateTime.UtcNow.AddDays(90),
            assignedTo: 1005,
            createdBy: 1000);

        context.WorkOrders.AddRange(wo1, wo2, wo3, wo4, wo5);
        await context.SaveChangesAsync();

        // ── Tasks for WO1: Server Room Maintenance ──────────────────────
        var tasks = new[]
        {
            new WorkTask(wo1.WorkOrderId, "Inspect cooling systems", 1001, estimatedHours: 4, createdBy: 1000),
            new WorkTask(wo1.WorkOrderId, "Clean server racks", 1001, estimatedHours: 2, createdBy: 1000),
            new WorkTask(wo1.WorkOrderId, "Update firmware on all switches", 1004, estimatedHours: 6, createdBy: 1000),
            new WorkTask(wo1.WorkOrderId, "Test UPS failover", 1001, estimatedHours: 3, createdBy: 1000),

            // ── Tasks for WO2: Network Upgrade Project ──────────────────
            new WorkTask(wo2.WorkOrderId, "Survey current infrastructure", 1002, estimatedHours: 8, createdBy: 1000),
            new WorkTask(wo2.WorkOrderId, "Order network equipment", 1005, estimatedHours: 2, createdBy: 1000),
            new WorkTask(wo2.WorkOrderId, "Install new switches on floor 1", 1002, estimatedHours: 6, createdBy: 1000),
            new WorkTask(wo2.WorkOrderId, "Run cable certification tests", 1002, estimatedHours: 4, createdBy: 1000),

            // ── Tasks for WO3: Workstation Deployment ───────────────────
            new WorkTask(wo3.WorkOrderId, "Unbox and inventory workstations", 1003, estimatedHours: 4, createdBy: 1000),
            new WorkTask(wo3.WorkOrderId, "Image workstations with standard build", 1003, estimatedHours: 8, createdBy: 1000),
            new WorkTask(wo3.WorkOrderId, "Deploy to desks on floor 3", 1003, estimatedHours: 6, createdBy: 1000),

            // ── Tasks for WO4: Security Audit ───────────────────────────
            new WorkTask(wo4.WorkOrderId, "Review firewall rules", 1004, estimatedHours: 8, createdBy: 1000),
            new WorkTask(wo4.WorkOrderId, "Audit user access permissions", 1004, estimatedHours: 10, createdBy: 1000),
            new WorkTask(wo4.WorkOrderId, "Penetration testing report", 1004, estimatedHours: 16, createdBy: 1000),

            // ── Tasks for WO5: Office Relocation IT Setup ───────────────
            new WorkTask(wo5.WorkOrderId, "Design network layout for floor 5", 1005, estimatedHours: 8, createdBy: 1000),
            new WorkTask(wo5.WorkOrderId, "Order and rack new servers", 1005, estimatedHours: 12, createdBy: 1000),
            new WorkTask(wo5.WorkOrderId, "Configure VoIP phones", 1005, estimatedHours: 6, createdBy: 1000),
            new WorkTask(wo5.WorkOrderId, "Set up conference room AV equipment", 1005, estimatedHours: 4, createdBy: 1000),
        };

        context.WorkTasks.AddRange(tasks);
        await context.SaveChangesAsync();

        logger?.LogInformation("Seeded {OrderCount} work orders and {TaskCount} tasks",
            5, tasks.Length);
    }
}
