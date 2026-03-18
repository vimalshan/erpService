# EF Migrations & Seed Data Guide

## Overview

This guide provides step-by-step instructions for creating and applying Entity Framework Core migrations and seeding initial data for the HR Microservice.

## Prerequisites

- Visual Studio 2022 or Visual Studio Code with .NET SDK 8.0+
- SQL Server LocalDB installed and running
- Package Manager Console access (Visual Studio)
- Connection string configured in `appsettings.json`

## Database Connection String

The default connection string in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PAYDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=False;"
}
```

**Key Components:**
- **Data Source**: `(localdb)\MSSQLLocalDB` - SQL Server LocalDB instance
- **Initial Catalog**: `PAYDB` - Database name
- **Integrated Security**: True - Uses Windows authentication
- **Encrypt**: True - Enables encryption

## Step 1: Setup Entity Framework Tools

### Install EF CLI (if not installed):

```bash
dotnet tool install --global dotnet-ef
```

### Verify installation:

```bash
dotnet ef --version
```

Should output version 8.0.2 or higher.

## Step 2: Apply Initial Migration

### Option A: Using Package Manager Console (Visual Studio)

1. Open **Package Manager Console** in Visual Studio
   - Menu: `Tools` → `NuGet Package Manager` → `Package Manager Console`

2. Set the default project to `HRService.Infrastructure`

3. Run the initial migration:

```powershell
Add-Migration InitialCreate -Project HRService.Infrastructure
```

This will create a migration file in the `Infrastructure\Migrations` folder.

### Option B: Using .NET CLI

1. Open terminal/PowerShell in the repository root

2. Navigate to the Infrastructure project:

```bash
cd HRService.Infrastructure
```

3. Add migration:

```bash
dotnet ef migrations add InitialCreate
```

## Step 3: Update Database with Migration

### Option A: Using Package Manager Console

```powershell
Update-Database -Project HRService.Infrastructure
```

### Option B: Using .NET CLI

```bash
dotnet ef database update --project HRService.Infrastructure
```

### Expected Output:

```
Build started...
Build succeeded.
Done. Building model took 2,345 ms.
Applying migration '20260317000000_InitialCreate'.
Done
```

This creates all tables, indexes, foreign keys, and constraints defined in the migration.

## Step 4: Verify Database Schema

### Using SQL Server Management Studio (SSMS):

1. Open SQL Server Management Studio
2. Connect to: `(localdb)\MSSQLLocalDB`
3. Expand `Databases` node
4. Find `PAYDB` database
5. Verify these tables in `Tables`:
   - HR_Department
   - HR_Shift
   - HR_Position
   - HR_LeaveType
   - HR_SalaryComponent
   - HR_Employee
   - HR_EmployeeLeave
   - HR_Attendance
   - HR_EmployeeSalary
   - HR_PerformanceReview
   - HR_AuditLog

### Using Query:

```sql
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'dbo' 
  AND TABLE_NAME LIKE 'HR_%'
ORDER BY TABLE_NAME;
```

## Step 5: Seed Initial Data

The seed data is configured in `SeedDataConfiguration.cs` and automatically applied during migration through `OnModelCreating()` in `HRServiceDbContext`.

### Verify Seed Data:

```sql
SELECT 'Departments' AS TableName, COUNT(*) AS RecordCount FROM HR_Department
UNION ALL
SELECT 'Employees', COUNT(*) FROM HR_Employee
UNION ALL
SELECT 'Leave Types', COUNT(*) FROM HR_LeaveType
UNION ALL
SELECT 'Positions', COUNT(*) FROM HR_Position
UNION ALL
SELECT 'Shifts', COUNT(*) FROM HR_Shift
UNION ALL
SELECT 'Salary Components', COUNT(*) FROM HR_SalaryComponent;
```

**Expected Results:**
- Departments: 4 records
- Employees: 5 records
- Leave Types: 4 records
- Positions: 5 records
- Shifts: 3 records
- Salary Components: 5 records

### Alternative: Run SQL Seed Script

If using manual seed script instead of EF seed data:

```sql
-- Execute in SQL Server Management Studio
:setvar DbName "PAYDB"

USE $(DbName);
GO

-- Run seed script
:r "C:\Path\To\DB_Scripts\seed-data.sql"
```

## Migration Management

### List All Migrations:

```powershell
# Using Package Manager Console
Get-Migration -Project HRService.Infrastructure

# Using CLI
dotnet ef migrations list --project HRService.Infrastructure
```

### Remove Last Migration (Before Updating Database):

```powershell
# Using Package Manager Console
Remove-Migration -Project HRService.Infrastructure

# Using CLI
dotnet ef migrations remove --project HRService.Infrastructure
```

**Note:** Only removes the migration files. If database was already updated, use `Update-Database` to revert.

### Revert to Previous Migration:

```powershell
# Revert to specific migration
Update-Database -Migration <MigrationName> -Project HRService.Infrastructure

# Revert to baseline (removes entire schema)
Update-Database -Migration 0 -Project HRService.Infrastructure
```

### Reset Database Completely:

```powershell
# Remove all migrations
Update-Database -Migration 0 -Project HRService.Infrastructure

# Delete migration files manually from Infrastructure\Migrations folder

# Create fresh migration
Add-Migration InitialCreate -Project HRService.Infrastructure

# Apply fresh migration
Update-Database -Project HRService.Infrastructure
```

## Troubleshooting

### Issue: "The migration 'InitialCreate' has already been applied to the database"

**Solution:** 
- If migration wasn't actually applied, remove it and reapply
- If it was applied, the database is already up to date

### Issue: "Cannot find the migrations assembly"

**Solution:**
```powershell
# Ensure correct project is set as startup/default
Set-StartupProject HRService.API
# Or in Package Manager Console dropdown
```

### Issue: "Connection timeout"

**Verify SQL Server LocalDB is running:**
```bash
sqllocaldb info
sqllocaldb start MSSQLLocalDB
```

### Issue: "Database 'PAYDB' does not exist"

**Create it manually:**
```sql
CREATE DATABASE PAYDB;
```

Or the migration will create it automatically if the connection string allows.

### Issue: Foreign key constraint violations during seed

**Solution:**
- Ensure parent records are inserted before child records
- Check GUIDs match between seed data entities
- Run `dotnet ef database drop` and recreate if needed

## Adding New Migrations

When you modify domain entities:

1. Update your entity class in `Domain/Entities/`

2. Create migration:

```powershell
Add-Migration DescriptiveNameOfChange -Project HRService.Infrastructure
```

3. Review generated migration file in `Infrastructure/Migrations/`

4. Apply migration:

```powershell
Update-Database -Project HRService.Infrastructure
```

### Example: Adding Performance Bonus Field

```csharp
// In migration file
migrationBuilder.AddColumn<decimal>(
    name: "PerformanceBonus",
    table: "HR_EmployeeSalary",
    type: "numeric(18,2)",
    nullable: true);
```

## Production Deployment

For production databases:

### 1. Generate SQL Script:

```powershell
Script-Migration -From <PreviousMigration> -To <NewMigration> -Project HRService.Infrastructure
```

### 2. Review Script:

- Check SQL syntax
- Verify column types and constraints
- Test on staging environment first

### 3. Apply with Caution:

```powershell
# Enable transaction
Script-Migration -Idempotent -Project HRService.Infrastructure | Invoke-SqlCmd
```

### 4. Backup Database Before Migration:

```sql
BACKUP DATABASE [PAYDB] 
TO DISK = 'C:\Backups\PAYDB_20260317.bak'
```

## Seed Data Structure

### Departments (4 records)
- HR (Department Code: HR)
- IT (Department Code: IT)
- Finance (Department Code: FIN)
- Operations (Department Code: OPS)

### Employees (5 records)
- John Smith (HR Manager, ID: 60000000-0000-0000-0000-000000000001)
- William Johnson (IT Director, ID: 60000000-0000-0000-0000-000000000002)
- Mary Williams (Finance Manager, ID: 60000000-0000-0000-0000-000000000003)
- James Brown (Senior Developer, Reports to William)
- Patricia Davis (HR Specialist, Reports to John, Contract)

### Shifts (3 records)
- Morning Shift: 8 AM - 4 PM
- Afternoon Shift: 2 PM - 10 PM
- Night Shift: 10 PM - 6 AM

### Leave Types (4 records)
- Annual Leave (20 days, Paid)
- Sick Leave (12 days, Paid)
- Maternity Leave (90 days, Paid)
- Unpaid Leave (Unlimited, Unpaid)

### Salary Components (5 records)
**Earnings:**
- Basic Salary
- HRA (House Rent Allowance)
- Dearness Allowance

**Deductions:**
- Income Tax
- PF Contribution

## Checking Migration Status

```sql
-- View migration history
SELECT MigrationId, ProductVersion
FROM __EFMigrationsHistory
ORDER BY MigrationId DESC;
```

Expected output:
```
20260317000000_InitialCreate | 8.0.2
```

## References

- [EF Core Migrations Documentation](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [Package Manager Console Guide](https://docs.microsoft.com/en-us/nuget/tools/package-manager-console)
- [SQL Server LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)

## Quick Reference Commands

```powershell
# Package Manager Console (PMC)
Add-Migration <Name> -Project HRService.Infrastructure
Update-Database -Project HRService.Infrastructure
Remove-Migration -Project HRService.Infrastructure
Get-Migration -Project HRService.Infrastructure
Update-Database -Migration 0 -Project HRService.Infrastructure  # Reset

# .NET CLI
dotnet ef migrations add <Name> --project HRService.Infrastructure
dotnet ef database update --project HRService.Infrastructure
dotnet ef migrations remove --project HRService.Infrastructure
dotnet ef migrations list --project HRService.Infrastructure
```

## Support

For issues or questions:
1. Check `appsettings.json` connection string
2. Verify SQL Server LocalDB is running
3. Review migration files in `Infrastructure/Migrations/`
4. Check EF Core version: `dotnet ef --version`
5. Review application logs in `logs/` folder
