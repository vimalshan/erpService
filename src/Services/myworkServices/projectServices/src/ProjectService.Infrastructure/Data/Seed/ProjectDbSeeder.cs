using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProjectService.Domain.Entities;
using ProjectService.Infrastructure.Data;

namespace ProjectService.Infrastructure.Data.Seed;

public static class ProjectDbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ProjectDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ProjectDbContext>>();

        logger.LogInformation("Applying database migrations...");
        await context.Database.MigrateAsync();
        logger.LogInformation("Migrations applied successfully.");

        if (await context.ProjectDepartments.AnyAsync())
        {
            logger.LogInformation("Database already seeded. Skipping.");
            return;
        }

        logger.LogInformation("Seeding database...");

        // ── Supporting Masters ──────────────────────────────────────

        // Departments
        context.ProjectDepartments.AddRange(
            new ProjectDepartment { ProjDepId = 1, ProjDepName = "Engineering", ProjDepCode = "ENG", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectDepartment { ProjDepId = 2, ProjDepName = "Manufacturing", ProjDepCode = "MFG", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectDepartment { ProjDepId = 3, ProjDepName = "Quality", ProjDepCode = "QA", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectDepartment { ProjDepId = 4, ProjDepName = "Supply Chain", ProjDepCode = "SCM", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow }
        );

        // Locations
        context.ProjectLocations.AddRange(
            new ProjectLocation { LocName = "Plant A - Main Factory", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectLocation { LocName = "Plant B - Assembly Unit", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectLocation { LocName = "Head Office", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectLocation { LocName = "R&D Center", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow }
        );

        // Processes
        context.ProjectProcesses.AddRange(
            new ProjectProcess { ProcName = "Design", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectProcess { ProcName = "Development", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectProcess { ProcName = "Testing", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectProcess { ProcName = "Deployment", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectProcess { ProcName = "Maintenance", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow }
        );

        // Functions
        context.ProjectFunctions.AddRange(
            new ProjectFunction { ProjFuncName = "Project Manager", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectFunction { ProjFuncName = "Team Lead", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectFunction { ProjFuncName = "Developer", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectFunction { ProjFuncName = "QA Engineer", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectFunction { ProjFuncName = "Business Analyst", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectFunction { ProjFuncName = "Scrum Master", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow }
        );

        // ── Type Categories ────────────────────────────────────────

        context.ProjectTypeCategoryMasters.AddRange(
            new ProjectTypeCategoryMaster { ProjCatName = "Product Development", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectTypeCategoryMaster { ProjCatName = "Process Improvement", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectTypeCategoryMaster { ProjCatName = "Infrastructure", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow }
        );

        // Project Category Masters
        context.ProjectCategoryMasters.AddRange(
            new ProjectCategoryMaster { CategoryName = "Internal Project", CategoryTeamId = 1, LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectCategoryMaster { CategoryName = "Client Project", CategoryTeamId = 2, LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectCategoryMaster { CategoryName = "R&D Project", CategoryTeamId = 1, LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow }
        );

        // Save first batch so identity columns get generated
        await context.SaveChangesAsync();

        // ── Project Type Masters ───────────────────────────────────

        var typeCats = await context.ProjectTypeCategoryMasters.ToListAsync();
        var typeCat1 = typeCats[0].ProjCatId;
        var typeCat2 = typeCats[1].ProjCatId;
        var typeCat3 = typeCats[2].ProjCatId;

        context.ProjectTypeMasters.AddRange(
            new ProjectTypeMaster { ProjTypeName = "New Product Launch", ProjTypeCode = "NPL", ProjTypeDepId = 1, ProjTypeCatId = typeCat1, ProjTypeModifiedBy = 1, ProjTypeModifiedOn = DateTime.UtcNow },
            new ProjectTypeMaster { ProjTypeName = "Continuous Improvement", ProjTypeCode = "CI", ProjTypeDepId = 2, ProjTypeCatId = typeCat2, ProjTypeModifiedBy = 1, ProjTypeModifiedOn = DateTime.UtcNow },
            new ProjectTypeMaster { ProjTypeName = "Plant Expansion", ProjTypeCode = "PE", ProjTypeDepId = 1, ProjTypeCatId = typeCat3, ProjTypeModifiedBy = 1, ProjTypeModifiedOn = DateTime.UtcNow }
        );

        await context.SaveChangesAsync();

        // ── Project Type Mappings (Deliverables, Objectives, Scopes) ──

        var projTypes = await context.ProjectTypeMasters.ToListAsync();
        var type1Id = projTypes[0].ProjTypeId;
        var type2Id = projTypes[1].ProjTypeId;

        // Deliverable Maps
        context.Set<ProjectTypeDeliverableMap>().AddRange(
            new ProjectTypeDeliverableMap { DelProjTypeId = type1Id, DelDesc = "Product Design Document", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectTypeDeliverableMap { DelProjTypeId = type1Id, DelDesc = "Prototype Approval", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectTypeDeliverableMap { DelProjTypeId = type1Id, DelDesc = "Production Readiness Review", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectTypeDeliverableMap { DelProjTypeId = type2Id, DelDesc = "Process Analysis Report", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectTypeDeliverableMap { DelProjTypeId = type2Id, DelDesc = "Implementation Plan", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow }
        );

        // Objective Maps
        context.Set<ProjectTypeObjectiveMap>().AddRange(
            new ProjectTypeObjectiveMap { ObjProjTypeId = type1Id, ObjDesc = "Launch product within budget and timeline", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectTypeObjectiveMap { ObjProjTypeId = type1Id, ObjDesc = "Meet quality standards per specification", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectTypeObjectiveMap { ObjProjTypeId = type2Id, ObjDesc = "Reduce cycle time by 15%", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectTypeObjectiveMap { ObjProjTypeId = type2Id, ObjDesc = "Reduce waste by 10%", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow }
        );

        // Scope Maps
        context.Set<ProjectTypeScopeMap>().AddRange(
            new ProjectTypeScopeMap { ScopeProjTypeId = type1Id, ScopeDesc = "Design and prototyping phase", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectTypeScopeMap { ScopeProjTypeId = type1Id, ScopeDesc = "Testing and validation phase", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectTypeScopeMap { ScopeProjTypeId = type2Id, ScopeDesc = "Process mapping and analysis", LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow }
        );

        // Financial Year Sequence
        context.Set<ProjectTypeFinYearSeq>().AddRange(
            new ProjectTypeFinYearSeq { ProjTypeId = type1Id, ProjTypeYear = 2026, ProjTypeSeq = 1 },
            new ProjectTypeFinYearSeq { ProjTypeId = type2Id, ProjTypeYear = 2026, ProjTypeSeq = 1 }
        );

        // Function-Type Maps
        var functions = await context.ProjectFunctions.ToListAsync();
        context.Set<ProjectTypeFunctionMap>().AddRange(
            new ProjectTypeFunctionMap { ProjTypeFuncTypeId = type1Id, ProjTypeFuncFuncId = functions[0].ProjFuncId, ProjTypeFuncAddlNo = 1, LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectTypeFunctionMap { ProjTypeFuncTypeId = type1Id, ProjTypeFuncFuncId = functions[1].ProjFuncId, ProjTypeFuncAddlNo = 2, LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectTypeFunctionMap { ProjTypeFuncTypeId = type1Id, ProjTypeFuncFuncId = functions[2].ProjFuncId, ProjTypeFuncAddlNo = 4, LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectTypeFunctionMap { ProjTypeFuncTypeId = type2Id, ProjTypeFuncFuncId = functions[0].ProjFuncId, ProjTypeFuncAddlNo = 1, LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectTypeFunctionMap { ProjTypeFuncTypeId = type2Id, ProjTypeFuncFuncId = functions[3].ProjFuncId, ProjTypeFuncAddlNo = 2, LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow }
        );

        // Function-Employee Maps
        context.Set<ProjectFunctionEmployeeMap>().AddRange(
            new ProjectFunctionEmployeeMap { ProjFuncEmpMapFuncId = functions[0].ProjFuncId, ProjFuncEmpMapEmpSysId = 1001, ProjFuncEmpLiveFlag = 'Y', LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectFunctionEmployeeMap { ProjFuncEmpMapFuncId = functions[1].ProjFuncId, ProjFuncEmpMapEmpSysId = 1002, ProjFuncEmpLiveFlag = 'Y', LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectFunctionEmployeeMap { ProjFuncEmpMapFuncId = functions[2].ProjFuncId, ProjFuncEmpMapEmpSysId = 1003, ProjFuncEmpLiveFlag = 'Y', LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectFunctionEmployeeMap { ProjFuncEmpMapFuncId = functions[2].ProjFuncId, ProjFuncEmpMapEmpSysId = 1004, ProjFuncEmpLiveFlag = 'Y', LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectFunctionEmployeeMap { ProjFuncEmpMapFuncId = functions[3].ProjFuncId, ProjFuncEmpMapEmpSysId = 1005, ProjFuncEmpLiveFlag = 'Y', LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow }
        );

        await context.SaveChangesAsync();

        // ── Project Master & Employee Maps ─────────────────────────

        var categories = await context.ProjectCategoryMasters.ToListAsync();

        context.ProjectMasters.AddRange(
            new ProjectMaster { ProjectName = "ERP Modernization", ProjectCategoryId = categories[0].CategoryId, ProjectEffDate = new DateTime(2026, 1, 1), ProjectTeamId = 1, ProjectListAll = 'Y', LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectMaster { ProjectName = "Client Portal v2", ProjectCategoryId = categories[1].CategoryId, ProjectEffDate = new DateTime(2026, 3, 1), ProjectTeamId = 2, ProjectListAll = 'Y', LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectMaster { ProjectName = "R&D Pilot Program", ProjectCategoryId = categories[2].CategoryId, ProjectEffDate = new DateTime(2026, 4, 1), ProjectTeamId = 1, ProjectListAll = 'N', LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow }
        );

        await context.SaveChangesAsync();

        var masters = await context.ProjectMasters.ToListAsync();

        context.Set<ProjectEmployeeMap>().AddRange(
            new ProjectEmployeeMap { ProjEmpProjectId = masters[0].ProjectId, ProjEmpEmpSysId = 1001, ProjEmpEffDate = new DateTime(2026, 1, 1), ProjEmpCloseDate = new DateTime(2026, 12, 31), LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectEmployeeMap { ProjEmpProjectId = masters[0].ProjectId, ProjEmpEmpSysId = 1002, ProjEmpEffDate = new DateTime(2026, 1, 1), ProjEmpCloseDate = new DateTime(2026, 12, 31), LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow },
            new ProjectEmployeeMap { ProjEmpProjectId = masters[1].ProjectId, ProjEmpEmpSysId = 1003, ProjEmpEffDate = new DateTime(2026, 3, 1), ProjEmpCloseDate = new DateTime(2026, 12, 31), LastModifiedBy = 1, LastModifiedOn = DateTime.UtcNow }
        );

        await context.SaveChangesAsync();

        // ── Project Main (actual projects) ─────────────────────────

        var locations = await context.ProjectLocations.ToListAsync();
        var processes = await context.ProjectProcesses.ToListAsync();
        var deliverables = await context.Set<ProjectTypeDeliverableMap>().ToListAsync();
        var scopeMaps = await context.Set<ProjectTypeScopeMap>().ToListAsync();

        context.ProjectMains.AddRange(
            new ProjectMain
            {
                ProjName = "NPL-2026-001",
                ProjCharterNo = 20260001,
                ProjLeaderId = 1001,
                ProjTypeId = type1Id,
                ProjLocId = locations[0].LocId,
                ProjProcessId = processes[0].ProcId,
                ProjStartDate = new DateTime(2026, 1, 15),
                ProjEndDate = new DateTime(2026, 6, 30),
                ProjEstEndDate = new DateTime(2026, 6, 30),
                ProjStatus = 'O',
                ProjRevNo = 1,
                ProjVerNo = 1,
                ProjObjId = 1,
                ProjObjDesc = "Launch Widget X into production",
                ProjTargetProd = "1000 units/month",
                ProjNotes = "Phase 1 of the new product launch program.",
                ProjLastModifiedOn = DateTime.UtcNow
            },
            new ProjectMain
            {
                ProjName = "CI-2026-001",
                ProjCharterNo = 20260002,
                ProjLeaderId = 1002,
                ProjTypeId = type2Id,
                ProjLocId = locations[1].LocId,
                ProjProcessId = processes[1].ProcId,
                ProjStartDate = new DateTime(2026, 2, 1),
                ProjEndDate = new DateTime(2026, 9, 30),
                ProjEstEndDate = new DateTime(2026, 8, 31),
                ProjStatus = 'O',
                ProjRevNo = 1,
                ProjVerNo = 1,
                ProjObjDesc = "Reduce assembly line cycle time",
                ProjTargetProd = "15% reduction target",
                ProjNotes = "Continuous improvement initiative for Plant B.",
                ProjLastModifiedOn = DateTime.UtcNow
            },
            new ProjectMain
            {
                ProjName = "NPL-2025-010",
                ProjCharterNo = 20250010,
                ProjLeaderId = 1001,
                ProjTypeId = type1Id,
                ProjLocId = locations[0].LocId,
                ProjProcessId = processes[0].ProcId,
                ProjStartDate = new DateTime(2025, 3, 1),
                ProjEndDate = new DateTime(2025, 12, 31),
                ProjEstEndDate = new DateTime(2025, 11, 30),
                ProjStatus = 'C',
                ProjRevNo = 3,
                ProjVerNo = 2,
                ProjObjDesc = "Completed product launch from 2025",
                ProjActualProd = "1200 units/month achieved",
                ProjClsDate = new DateTime(2025, 12, 15),
                ProjNotes = "Successfully closed. All deliverables met.",
                ProjLastModifiedOn = DateTime.UtcNow
            }
        );

        await context.SaveChangesAsync();

        // ── Child Records for Project Main ─────────────────────────

        var projects = await context.ProjectMains.ToListAsync();
        var proj1 = projects[0].ProjId;
        var proj2 = projects[1].ProjId;
        var proj3 = projects[2].ProjId;

        // Members
        context.ProjectMembers.AddRange(
            new ProjectMember { ProjMemProjId = proj1, ProjMemFuncId = functions[0].ProjFuncId, ProjMemEmpSysId = 1001 },
            new ProjectMember { ProjMemProjId = proj1, ProjMemFuncId = functions[1].ProjFuncId, ProjMemEmpSysId = 1002 },
            new ProjectMember { ProjMemProjId = proj1, ProjMemFuncId = functions[2].ProjFuncId, ProjMemEmpSysId = 1003 },
            new ProjectMember { ProjMemProjId = proj1, ProjMemFuncId = functions[3].ProjFuncId, ProjMemEmpSysId = 1005 },
            new ProjectMember { ProjMemProjId = proj2, ProjMemFuncId = functions[0].ProjFuncId, ProjMemEmpSysId = 1002 },
            new ProjectMember { ProjMemProjId = proj2, ProjMemFuncId = functions[2].ProjFuncId, ProjMemEmpSysId = 1004 }
        );

        // Scopes
        context.ProjectScopes.AddRange(
            new ProjectScope { ProjScopeProjId = proj1, ProjScopeScopeId = scopeMaps[0].ScopeId },
            new ProjectScope { ProjScopeProjId = proj1, ProjScopeScopeId = scopeMaps[1].ScopeId },
            new ProjectScope { ProjScopeProjId = proj2, ProjScopeScopeId = scopeMaps[2].ScopeId }
        );

        // Deliverables
        context.ProjectDeliverables.AddRange(
            new ProjectDeliverable { ProjDelProjId = proj1, ProjDelDelId = deliverables[0].DelId },
            new ProjectDeliverable { ProjDelProjId = proj1, ProjDelDelId = deliverables[1].DelId },
            new ProjectDeliverable { ProjDelProjId = proj1, ProjDelDelId = deliverables[2].DelId },
            new ProjectDeliverable { ProjDelProjId = proj2, ProjDelDelId = deliverables[3].DelId },
            new ProjectDeliverable { ProjDelProjId = proj2, ProjDelDelId = deliverables[4].DelId }
        );

        // Additional Deliverables
        context.Set<ProjectAdditionalDeliverable>().AddRange(
            new ProjectAdditionalDeliverable { ProjAdlDelProjId = proj1, ProjAdlDelDesc = "Risk Assessment Report" },
            new ProjectAdditionalDeliverable { ProjAdlDelProjId = proj1, ProjAdlDelDesc = "Training Manual" },
            new ProjectAdditionalDeliverable { ProjAdlDelProjId = proj2, ProjAdlDelDesc = "Before/After Analysis" }
        );

        // Additional Scopes
        context.Set<ProjectAdditionalScope>().AddRange(
            new ProjectAdditionalScope { ProjAdScopeProjId = proj1, ProjAdScopeDesc = "Vendor qualification" },
            new ProjectAdditionalScope { ProjAdScopeProjId = proj2, ProjAdScopeDesc = "Operator retraining" }
        );

        // Status History
        context.ProjectStatusHistories.AddRange(
            new ProjectStatusHistory { ProjStatusProjId = proj1, ProjStatusDate = new DateTime(2026, 1, 15), ProjStatusRem = "Project kicked off", ProjStatusRevNo = 1, ProjStatusVerNo = 1 },
            new ProjectStatusHistory { ProjStatusProjId = proj1, ProjStatusDate = new DateTime(2026, 2, 15), ProjStatusRem = "Design phase completed", ProjStatusRevNo = 1, ProjStatusVerNo = 1 },
            new ProjectStatusHistory { ProjStatusProjId = proj1, ProjStatusDate = new DateTime(2026, 3, 15), ProjStatusRem = "Prototype under review", ProjStatusRevNo = 1, ProjStatusVerNo = 1 },
            new ProjectStatusHistory { ProjStatusProjId = proj2, ProjStatusDate = new DateTime(2026, 2, 1), ProjStatusRem = "CI project initiated", ProjStatusRevNo = 1, ProjStatusVerNo = 1 },
            new ProjectStatusHistory { ProjStatusProjId = proj2, ProjStatusDate = new DateTime(2026, 3, 1), ProjStatusRem = "Data collection in progress", ProjStatusRevNo = 1, ProjStatusVerNo = 1 },
            new ProjectStatusHistory { ProjStatusProjId = proj3, ProjStatusDate = new DateTime(2025, 12, 15), ProjStatusRem = "Project closed - all targets met", ProjStatusRevNo = 3, ProjStatusVerNo = 2 }
        );

        // Approval Details
        context.Set<ProjectApprovalDetail>().AddRange(
            new ProjectApprovalDetail { ProjApprProjId = proj1, ProjApprType = 'A', ProjApprSentOn = new DateTime(2026, 1, 10), ProjAppEmpSysId = 2001, ProjApprAppDate = new DateTime(2026, 1, 12), ProjApprStatus = 'A', ProjApprRemarks = "Charter approved", ProjApprDropRemarks = "-" },
            new ProjectApprovalDetail { ProjApprProjId = proj2, ProjApprType = 'A', ProjApprSentOn = new DateTime(2026, 1, 25), ProjAppEmpSysId = 2001, ProjApprAppDate = new DateTime(2026, 1, 28), ProjApprStatus = 'A', ProjApprRemarks = "Approved for execution", ProjApprDropRemarks = "-" },
            new ProjectApprovalDetail { ProjApprProjId = proj3, ProjApprType = 'C', ProjApprSentOn = new DateTime(2025, 12, 10), ProjAppEmpSysId = 2001, ProjApprAppDate = new DateTime(2025, 12, 12), ProjApprStatus = 'A', ProjApprRemarks = "Closure approved", ProjApprDropRemarks = "-" }
        );

        // Project Holds (proj2 has a hold and unhold history)
        context.Set<ProjectHold>().AddRange(
            new ProjectHold { ProjHoldProjId = proj2, ProjHoldType = 'H', ProjHoldDate = new DateTime(2026, 2, 15), ProjHoldReason = "Pending raw material delivery", ProjHoldUpdatedBy = 1002, ProjHoldUpdatedOn = new DateTime(2026, 2, 15) },
            new ProjectHold { ProjHoldProjId = proj2, ProjHoldType = 'U', ProjHoldDate = new DateTime(2026, 2, 25), ProjHoldReason = "Material received - resuming", ProjHoldUpdatedBy = 1002, ProjHoldUpdatedOn = new DateTime(2026, 2, 25) }
        );

        // Project Access
        context.Set<ProjectAccess>().AddRange(
            new ProjectAccess { ProjAccEmpSysId = 1001, ProjAccType = 'A', ProjAccDepId = 1 },
            new ProjectAccess { ProjAccEmpSysId = 1002, ProjAccType = 'A', ProjAccDepId = 1 },
            new ProjectAccess { ProjAccEmpSysId = 2001, ProjAccType = 'A', ProjAccDepId = 1 },
            new ProjectAccess { ProjAccEmpSysId = 1003, ProjAccType = 'V', ProjAccDepId = 2 },
            new ProjectAccess { ProjAccEmpSysId = 1005, ProjAccType = 'V', ProjAccDepId = 3 }
        );

        await context.SaveChangesAsync();
        logger.LogInformation("Database seeded successfully with {ProjectCount} projects.", projects.Count);
    }
}
