# HR Service - Migration Instructions

## Database Setup & Migrations Guide

This document provides step-by-step instructions for setting up and managing the database migrations for the HR Microservice.

## Prerequisites

- SQL Server LocalDB or Express Edition
- .NET 8 SDK
- Visual Studio 2022 or VS Code
- Package Manager Console access in Visual Studio

## Initial Setup

### Step 1: Create Database (Optional)

The database will be created automatically during migration, but you can create it manually:

```sql
CREATE DATABASE PAYDB;
GO
```

### Step 2: Run Migrations

#### Using Package Manager Console (Recommended)

1. Open Visual Studio
2. Tools → NuGet Package Manager → Package Manager Console
3. Ensure the default project is `HRService.Infrastructure`
4. Run the following commands:

```powershell
# Create initial migration from existing schema
Add-Migration InitialCreate -StartupProject HRService.API -Project HRService.Infrastructure

# Apply migration to database
Update-Database -StartupProject HRService.API -Project HRService.Infrastructure
```

#### Using .NET CLI

```bash
# From solution root directory
cd HRService.Infrastructure

# Create migration
dotnet ef migrations add InitialCreate --startup-project ../HRService.API

# Update database
dotnet ef database update --startup-project ../HRService.API
```

### Step 3: Verify Migration

1. Open SQL Server Management Studio
2. Connect to `(localdb)\MSSQLLocalDB`
3. Verify database `PAYDB` exists
4. Check tables:
   - HR_Department
   - HR_Employee
   - HR_EmployeeLeave
   - HR_Attendance
   - HR_EmployeeSalary
   - HR_PerformanceReview

## Seed Data

### Using SQL Script

Execute [HR-Module.sql](./HR/HR-Module.sql) to populate initial data:

```sql
USE [PAYDB]
GO
-- Execute seed data statements
```

### Using Entity Framework Seeding

The seed data can be configured in `HRServiceDbContext.OnModelCreating()`:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Seed Departments
    modelBuilder.Entity<Department>().HasData(
        Department.Create("IT", "Information Technology", "IT Department")
    );
    
    // Seed other entities...
}
```

## Common Migration Tasks

### Add New Migration

```powershell
# Add migration for new features
Add-Migration AddEmployeePhoneNumberField
Update-Database
```

### Rollback to Previous Migration

```powershell
# List all migrations
Get-Migration

# Rollback to specific migration
Update-Database -Migration <previous-migration-name>
```

### Remove Last Migration

```powershell
# Remove migration that hasn't been applied
Remove-Migration
```

### Generate SQL from Migration

```powershell
# Generate SQL script without applying
Script-Migration -From <source-migration> -To <target-migration>
```

## Troubleshooting

### Issue: "The model backing the 'HRServiceDbContext' context has changed..."

**Solution:**
```powershell
Add-Migration AutomaticMigration -AutomaticMigrationDataLossAllowed
Update-Database
```

### Issue: "No database provider configured"

**Solution:**
Ensure `HRServiceDbContext` is properly configured in `Program.cs`:

```csharp
builder.Services.AddDbContext<HRServiceDbContext>(options =>
    options.UseSqlServer(connectionString));
```

### Issue: "Cannot drop database while it is in use"

**Solution:**
```sql
-- In SQL Server Management Studio
ALTER DATABASE PAYDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE PAYDB;
```

## Environment-Specific Configurations

### Development
- Connection String: `(localdb)\MSSQLLocalDB`
- Auto-migrations: Enabled
- Seed data: Automatic

### Staging
- Use backup of production database
- Review all pending migrations
- Validate seed data

### Production
- Use dedicated SQL Server
- Backup database before migration
- Test migration in staging first
- Monitor database size and performance

## Migration Best Practices

1. **Always in Separate Project**: Keep migrations in Infrastructure project
2. **Descriptive Names**: Use clear migration names (e.g., `AddEmployeePhoneNumber`)
3. **Test First**: Test migrations in development environment
4. **Document Changes**: Add comments to complex migrations
5. **Backup Before**: Always backup production database
6. **Zero-Downtime**: Use long-running transaction with caution
7. **Version Control**: Commit migrations with code

## Performance Considerations

### Large Tables
For tables with millions of rows:
```csharp
modelBuilder.Entity<Attendance>()
    .HasIndex(a => new { a.EmployeeId, a.AttendanceDate })
    .IsUnique()
    .HasDatabaseName("IX_Attendance_Employee_Date");
```

### Query Optimization
```csharp
// Use include for eager loading
var employees = await _context.Employees
    .Include(e => e.Department)
    .Include(e => e.Position)
    .ToListAsync();
```

## Monitoring Migrations

### Check Migration History
```sql
SELECT * FROM __EFMigrationsHistory ORDER BY MigrationId DESC;
```

### Monitor Large Schema Changes
```sql
-- Check table sizes
SELECT 
    t.name AS TableName,
    s.name AS SchemaName,
    p.rows AS RowCount,
    CAST(CAST(reserved_page_count * 8.0 / 1024 AS DECIMAL(15,2)) AS VARCHAR(15)) 
        AS ReservedSpaceMB
FROM sys.tables t
JOIN sys.schemas s ON t.schema_id = s.schema_id
JOIN sys.dm_db_partition_stats p 
    ON t.object_id = p.object_id 
    AND p.index_id < 2
ORDER BY reserved_page_count DESC;
```

## Deployment Checklist

- [ ] Test migrations locally
- [ ] Backup production database
- [ ] Review pending migrations
- [ ] Validate schema changes
- [ ] Check for breaking changes
- [ ] Verify seed data
- [ ] Monitor database performance
- [ ] Document migration details
- [ ] Prepare rollback plan

## Support

For migration issues:
1. Check migration history: `Get-Migration`
2. Review logs in `logs/` directory
3. Check SQL Server error logs
4. Consult EF Core documentation

---

**Last Updated**: March 2026
**EF Core Version**: 8.0.2
