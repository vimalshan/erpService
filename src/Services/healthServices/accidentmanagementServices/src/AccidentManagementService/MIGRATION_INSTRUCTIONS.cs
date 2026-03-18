/// <summary>
/// MIGRATION INSTRUCTIONS FOR ACCIDENT MANAGEMENT SERVICE
/// 
/// This file contains instructions for creating and applying EF Core migrations.
/// Execute these commands in Package Manager Console or terminal.
/// </summary>

// STEP 1: Create Initial Migration
// ================================
// Run this command in Package Manager Console:
// Add-Migration InitialCreate -Project AccidentManagementService -StartupProject AccidentManagementService
// 
// Or via CLI:
// dotnet ef migrations add InitialCreate --project AccidentManagementService

// STEP 2: Review Migration File
// ==============================
// A file will be created:
// Migrations/yyyyMMddHHmmss_InitialCreate.cs
// 
// Review the migration to ensure all tables and relationships are correct.
// The migration should create:
// - ACCIDENT_SEVERITY
// - ACCIDENT_STATUS
// - CATEGORY_INJURY
// - NATURE_INJURY
// - ACC_CONTRCT_LST
// - ACC_PERS_INJ
// - DAILY_ACC_FIR (main accident table)
// - AUDIT_LOG (if applicable)

// STEP 3: Apply Migration
// =======================
// Run this command in Package Manager Console:
// Update-Database -Project AccidentManagementService
// 
// Or via CLI:
// dotnet ef database update --project AccidentManagementService

// STEP 4: Seed Master Data (Reference Tables)
// =============================================
// Add this to DbContext.OnModelCreating() method:

/*
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    
    // Seed ACCIDENT_SEVERITY
    modelBuilder.Entity<ACCIDENT_SEVERITY>().HasData(
        new ACCIDENT_SEVERITY { SEVERITY_ID = 1, SEVERITY_NAME = "Critical", SEVERITY_DESC = "Life-threatening injury requiring immediate hospitalization" },
        new ACCIDENT_SEVERITY { SEVERITY_ID = 2, SEVERITY_NAME = "High", SEVERITY_DESC = "Serious injury requiring hospitalization" },
        new ACCIDENT_SEVERITY { SEVERITY_ID = 3, SEVERITY_NAME = "Medium", SEVERITY_DESC = "Injury requiring first aid and medical attention" },
        new ACCIDENT_SEVERITY { SEVERITY_ID = 4, SEVERITY_NAME = "Low", SEVERITY_DESC = "Minor injury requiring basic first aid" }
    );
    
    // Seed ACCIDENT_STATUS
    modelBuilder.Entity<ACCIDENT_STATUS>().HasData(
        new ACCIDENT_STATUS { STATUS_ID = 1, STATUS_NAME = "New", STATUS_DESC = "Newly reported accident" },
        new ACCIDENT_STATUS { STATUS_ID = 2, STATUS_NAME = "InProgress", STATUS_DESC = "Under investigation" },
        new ACCIDENT_STATUS { STATUS_ID = 3, STATUS_NAME = "Resolved", STATUS_DESC = "Investigation completed" },
        new ACCIDENT_STATUS { STATUS_ID = 4, STATUS_NAME = "Closed", STATUS_DESC = "Final disposition made" }
    );
    
    // Seed CATEGORY_INJURY - Injury Classification
    modelBuilder.Entity<CATEGORY_INJURY>().HasData(
        new CATEGORY_INJURY { CAT_ID = 1, CAT_NAME = "Chemical Burn" },
        new CATEGORY_INJURY { CAT_ID = 2, CAT_NAME = "Electrical Burn" },
        new CATEGORY_INJURY { CAT_ID = 3, CAT_NAME = "Fracture" },
        new CATEGORY_INJURY { CAT_ID = 4, CAT_NAME = "Cut" },
        new CATEGORY_INJURY { CAT_ID = 5, CAT_NAME = "Crush Injury" },
        new CATEGORY_INJURY { CAT_ID = 6, CAT_NAME = "Eye Injury" },
        new CATEGORY_INJURY { CAT_ID = 7, CAT_NAME = "Puncture" },
        new CATEGORY_INJURY { CAT_ID = 8, CAT_NAME = "Contusion" },
        new CATEGORY_INJURY { CAT_ID = 9, CAT_NAME = "Sprain" },
        new CATEGORY_INJURY { CAT_ID = 10, CAT_NAME = "Amputation" },
        new CATEGORY_INJURY { CAT_ID = 11, CAT_NAME = "Head Injury" },
        new CATEGORY_INJURY { CAT_ID = 12, CAT_NAME = "Inhalation" },
        new CATEGORY_INJURY { CAT_ID = 13, CAT_NAME = "Other" }
    );
    
    // Seed NATURE_INJURY - Injury Nature
    modelBuilder.Entity<NATURE_INJURY>().HasData(
        new NATURE_INJURY { NATURE_ID = 1, NATURE_NAME = "Deep" },
        new NATURE_INJURY { NATURE_ID = 2, NATURE_NAME = "Superficial" },
        new NATURE_INJURY { NATURE_ID = 3, NATURE_NAME = "Severe" },
        new NATURE_INJURY { NATURE_ID = 4, NATURE_NAME = "Penetrating" },
        new NATURE_INJURY { NATURE_ID = 5, NATURE_NAME = "Blunt Force" },
        new NATURE_INJURY { NATURE_ID = 6, NATURE_NAME = "Avulsion" },
        new NATURE_INJURY { NATURE_ID = 7, NATURE_NAME = "Abrasion" },
        new NATURE_INJURY { NATURE_ID = 8, NATURE_NAME = "Other" }
    );
}
*/

// STEP 5: Create New Migration After Seeding
// ===========================================
// After adding seed data, create another migration:
// Add-Migration SeedMasterData -Project AccidentManagementService
//
// Then apply it:
// Update-Database -Project AccidentManagementService

// STEP 6: Verify Database
// ========================
// Connect to SQL Server and verify:
// 
// SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES 
// WHERE TABLE_SCHEMA = 'dbo' 
// ORDER BY TABLE_NAME;
// 
// Should return:
// - ACCIDENT_SEVERITY
// - ACCIDENT_STATUS
// - CATEGORY_INJURY
// - DAILY_ACC_FIR
// - NATURE_INJURY
// + Any other tables in your schema

// STEP 7: Backup Current Migration State
// =======================================
// To see list of migrations:
// Get-Migration
// 
// To create a backup of current state (optional):
// Create a snapshot of your DbContext by saving the ModelSnapshot.cs file

// MIGRATION ROLLBACK (If Needed)
// ==============================
// To rollback to a previous migration:
// Update-Database -TargetMigration "PreviousMigrationName"
// 
// To remove last migration (before applying):
// Remove-Migration
// 
// To remove migration after applying:
// Update-Database -TargetMigration "PreviousMigrationName"
// Then: Remove-Migration

// COMMON ISSUES
// =============
// Issue: "Package 'EntityFrameworkCore.Design' not found"
// Solution: Install-Package Microsoft.EntityFrameworkCore.Design
//
// Issue: "DbContext type not specified"
// Solution: Make sure Program.cs is the startup project, or explicitly specify:
// Add-Migration InitialCreate -Project AccidentManagementService -StartupProject AccidentManagementService
//
// Issue: "Connection string not found"
// Solution: Ensure appsettings.json has "HealthDb" connection string configured
//
// Issue: Permission denied
// Solution: SQL Server login must have CREATE TABLE permissions

// ADDITIONAL SCHEMA MANAGEMENT
// =============================

/*
// Drop entire database (CAUTION!)
// Update-Database -TargetMigration 0

// Script migration to SQL file (for version control):
// Script-Migration -From InitialCreate -To SeedMasterData | Out-File migration.sql

// Get migration history:
// Get-DbContext -Project AccidentManagementService
*/
