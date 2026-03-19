using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Infrastructure.Persistence;

namespace VehicleTracking.Infrastructure.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<VehicleTrackingDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<VehicleTrackingDbContext>>();

        try
        {
            await context.Database.MigrateAsync();

            await SeedStageMastersAsync(context);
            await SeedPurposeMastersAsync(context);
            await context.SaveChangesAsync();

            await SeedPurposeStagesAsync(context);
            await SeedStageDecisionsAsync(context);
            await SeedStageFlexAsync(context);
            await SeedPurposeProductsAsync(context);
            await SeedSparshNavigationAsync(context);
            await context.SaveChangesAsync();

            logger.LogInformation("Database seeded successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database");
        }
    }

    private static async Task SeedStageMastersAsync(VehicleTrackingDbContext context)
    {
        if (await context.StageMasters.AnyAsync()) return;

        context.StageMasters.AddRange(
            new StageMaster { StageCode = 1, OptionName = "GATE_ENTRY", UpdatedBy = "SYSTEM", UpdateNumber = 0, UpdatedDate = DateTime.UtcNow },
            new StageMaster { StageCode = 2, OptionName = "WEIGHBRIDGE_IN", UpdatedBy = "SYSTEM", UpdateNumber = 0, UpdatedDate = DateTime.UtcNow },
            new StageMaster { StageCode = 3, OptionName = "QUALITY_CHECK", UpdatedBy = "SYSTEM", UpdateNumber = 0, UpdatedDate = DateTime.UtcNow },
            new StageMaster { StageCode = 4, OptionName = "LOADING_UNLOAD", UpdatedBy = "SYSTEM", UpdateNumber = 0, UpdatedDate = DateTime.UtcNow },
            new StageMaster { StageCode = 5, OptionName = "WEIGHBRIDGE_OUT", UpdatedBy = "SYSTEM", UpdateNumber = 0, UpdatedDate = DateTime.UtcNow },
            new StageMaster { StageCode = 6, OptionName = "GATE_EXIT", UpdatedBy = "SYSTEM", UpdateNumber = 0, UpdatedDate = DateTime.UtcNow }
        );
    }

    private static async Task SeedPurposeMastersAsync(VehicleTrackingDbContext context)
    {
        if (await context.PurposeMasters.AnyAsync()) return;

        context.PurposeMasters.AddRange(
            new PurposeMaster { PurposeCode = 1, PurposeName = "RAW_MATERIAL_INBOUND", TransactionType = 'I', PurposeCategory = "INBOUND", LastStage = 6 },
            new PurposeMaster { PurposeCode = 2, PurposeName = "FINISHED_GOODS_OUTBOUND", TransactionType = 'O', PurposeCategory = "OUTBOUND", LastStage = 6 },
            new PurposeMaster { PurposeCode = 3, PurposeName = "MISCELLANEOUS_VISIT", TransactionType = 'M', PurposeCategory = "MISC", LastStage = 6 },
            new PurposeMaster { PurposeCode = 4, PurposeName = "RETURNABLE_INBOUND", TransactionType = 'I', PurposeCategory = "INBOUND", LastStage = 6, ParentPurpose = 1 },
            new PurposeMaster { PurposeCode = 5, PurposeName = "SAMPLE_DISPATCH", TransactionType = 'O', PurposeCategory = "OUTBOUND", LastStage = 6, ParentPurpose = 2 }
        );
    }

    private static async Task SeedPurposeStagesAsync(VehicleTrackingDbContext context)
    {
        if (await context.PurposeStages.AnyAsync()) return;

        // Inbound flow: Gate Entry -> Weighbridge In -> Quality Check -> Loading/Unload -> Weighbridge Out -> Gate Exit
        context.PurposeStages.AddRange(
            new PurposeStage { PurposeCode = 1, StageCode = 1, StageSerial = 1, FlexField = 'N', ParallelFlag = 'N', RoleCode = 1, BooleanFlag = 'N', TargetTime = 10 },
            new PurposeStage { PurposeCode = 1, StageCode = 2, StageSerial = 2, FlexField = 'N', ParallelFlag = 'N', RoleCode = 2, BooleanFlag = 'N', TargetTime = 15 },
            new PurposeStage { PurposeCode = 1, StageCode = 3, StageSerial = 3, FlexField = 'Y', ParallelFlag = 'N', RoleCode = 3, BooleanFlag = 'Y', BooleanDescription = "Quality Approved?", TrueStage = 4, FalseStage = 6, TargetTime = 30 },
            new PurposeStage { PurposeCode = 1, StageCode = 4, StageSerial = 4, FlexField = 'N', ParallelFlag = 'N', RoleCode = 4, BooleanFlag = 'N', TargetTime = 60 },
            new PurposeStage { PurposeCode = 1, StageCode = 5, StageSerial = 5, FlexField = 'N', ParallelFlag = 'N', RoleCode = 2, BooleanFlag = 'N', TargetTime = 15 },
            new PurposeStage { PurposeCode = 1, StageCode = 6, StageSerial = 6, FlexField = 'N', ParallelFlag = 'N', RoleCode = 1, BooleanFlag = 'N', TargetTime = 10 },
            // Outbound flow
            new PurposeStage { PurposeCode = 2, StageCode = 1, StageSerial = 1, FlexField = 'N', ParallelFlag = 'N', RoleCode = 1, BooleanFlag = 'N', TargetTime = 10 },
            new PurposeStage { PurposeCode = 2, StageCode = 2, StageSerial = 2, FlexField = 'N', ParallelFlag = 'N', RoleCode = 2, BooleanFlag = 'N', TargetTime = 15 },
            new PurposeStage { PurposeCode = 2, StageCode = 4, StageSerial = 3, FlexField = 'N', ParallelFlag = 'N', RoleCode = 4, BooleanFlag = 'N', TargetTime = 45 },
            new PurposeStage { PurposeCode = 2, StageCode = 5, StageSerial = 4, FlexField = 'N', ParallelFlag = 'N', RoleCode = 2, BooleanFlag = 'N', TargetTime = 15 },
            new PurposeStage { PurposeCode = 2, StageCode = 6, StageSerial = 5, FlexField = 'N', ParallelFlag = 'N', RoleCode = 1, BooleanFlag = 'N', TargetTime = 10 },
            // Miscellaneous flow (Gate Entry -> Gate Exit only)
            new PurposeStage { PurposeCode = 3, StageCode = 1, StageSerial = 1, FlexField = 'N', ParallelFlag = 'N', RoleCode = 1, BooleanFlag = 'N', TargetTime = 10 },
            new PurposeStage { PurposeCode = 3, StageCode = 6, StageSerial = 2, FlexField = 'N', ParallelFlag = 'N', RoleCode = 1, BooleanFlag = 'N', TargetTime = 10 }
        );
    }

    private static async Task SeedStageDecisionsAsync(VehicleTrackingDbContext context)
    {
        if (await context.StageDecisions.AnyAsync()) return;

        context.StageDecisions.AddRange(
            // Inbound sequential flow decisions
            new StageDecision { PurposeCode = 1, StageCode = 1, OptionName = "PROCEED", OptionId = 1, NextStage = 2 },
            new StageDecision { PurposeCode = 1, StageCode = 2, OptionName = "PROCEED", OptionId = 1, NextStage = 3 },
            new StageDecision { PurposeCode = 1, StageCode = 3, OptionName = "APPROVED", OptionId = 1, NextStage = 4 },
            new StageDecision { PurposeCode = 1, StageCode = 3, OptionName = "REJECTED", OptionId = 2, NextStage = 6 },
            new StageDecision { PurposeCode = 1, StageCode = 4, OptionName = "PROCEED", OptionId = 1, NextStage = 5 },
            new StageDecision { PurposeCode = 1, StageCode = 5, OptionName = "PROCEED", OptionId = 1, NextStage = 6 },
            // Outbound flow decisions
            new StageDecision { PurposeCode = 2, StageCode = 1, OptionName = "PROCEED", OptionId = 1, NextStage = 2 },
            new StageDecision { PurposeCode = 2, StageCode = 2, OptionName = "PROCEED", OptionId = 1, NextStage = 4 },
            new StageDecision { PurposeCode = 2, StageCode = 4, OptionName = "PROCEED", OptionId = 1, NextStage = 5 },
            new StageDecision { PurposeCode = 2, StageCode = 5, OptionName = "PROCEED", OptionId = 1, NextStage = 6 },
            // Miscellaneous flow decisions
            new StageDecision { PurposeCode = 3, StageCode = 1, OptionName = "PROCEED", OptionId = 1, NextStage = 6 }
        );
    }

    private static async Task SeedStageFlexAsync(VehicleTrackingDbContext context)
    {
        if (await context.StageFlexes.AnyAsync()) return;

        context.StageFlexes.AddRange(
            new StageFlex { PurposeCode = 1, StageSerial = 3, FlexNumber = 1, FlexDescription = "Quality Grade", LovFlag = 'Y', LovType = "QGR", FlexType = 'S' },
            new StageFlex { PurposeCode = 2, StageSerial = 4, FlexNumber = 1, FlexDescription = "Dispatch Note", LovFlag = 'N', FlexType = 'T' }
        );
    }

    private static async Task SeedPurposeProductsAsync(VehicleTrackingDbContext context)
    {
        if (await context.PurposeProducts.AnyAsync()) return;

        context.PurposeProducts.AddRange(
            new PurposeProduct { ProductCode = "RM-COAL", PurposeCode = 1 },
            new PurposeProduct { ProductCode = "RM-IRON-ORE", PurposeCode = 1 },
            new PurposeProduct { ProductCode = "RM-LIMESTONE", PurposeCode = 1 },
            new PurposeProduct { ProductCode = "FG-STEEL-BAR", PurposeCode = 2 },
            new PurposeProduct { ProductCode = "FG-STEEL-COIL", PurposeCode = 2 },
            new PurposeProduct { ProductCode = "FG-STEEL-PLATE", PurposeCode = 2 }
        );
    }

    private static async Task SeedSparshNavigationAsync(VehicleTrackingDbContext context)
    {
        if (await context.SparshNavigations.AnyAsync()) return;

        context.SparshNavigations.AddRange(
            new SparshNavigation { RequestNumber = 1, UserId = "ADMIN", UserNumber = 1, RandomNumber = "SYS001", UpdateDate = DateTime.UtcNow, SciId = 'Y', StatusFlag = 'A' },
            new SparshNavigation { RequestNumber = 2, UserId = "GATE_USER", UserNumber = 2, RandomNumber = "SYS002", UpdateDate = DateTime.UtcNow, SciId = 'Y', StatusFlag = 'A' },
            new SparshNavigation { RequestNumber = 3, UserId = "WB_USER", UserNumber = 3, RandomNumber = "SYS003", UpdateDate = DateTime.UtcNow, SciId = 'Y', StatusFlag = 'A' }
        );
    }
}
