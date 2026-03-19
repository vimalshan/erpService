-- ============================================================================
-- EF Migrations Setup Script
-- Purpose: Create initial migration for Mobile Expense Management
-- ============================================================================

/*
This script documents the process to create EF migrations.
Run these commands in the Package Manager Console:

1. Add initial migration:
   Add-Migration InitialCreate -Project MobileExpenseManagement.Infrastructure -StartupProject MobileExpenseManagement.API

2. Update database:
   Update-Database -Project MobileExpenseManagement.Infrastructure -StartupProject MobileExpenseManagement.API

3. Add new migration after model changes:
   Add-Migration [MigrationName] -Project MobileExpenseManagement.Infrastructure -StartupProject MobileExpenseManagement.API

4. Remove last migration (if not yet applied to database):
   Remove-Migration -Project MobileExpenseManagement.Infrastructure -StartupProject MobileExpenseManagement.API

5. View pending migrations:
   Get-Migration -Project MobileExpenseManagement.Infrastructure

Requirements:
- Visual Studio Package Manager Console OR
- Command line: dotnet ef (requires "dotnet tool install --global dotnet-ef")

Useful commands:
- Generate SQL script: Script-Migration -From [PreviousMigration] -To [TargetMigration]
- Drop database: Drop-Database -Project MobileExpenseManagement.Infrastructure
*/
