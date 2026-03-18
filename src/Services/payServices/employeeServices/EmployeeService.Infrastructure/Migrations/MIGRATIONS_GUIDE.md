# EF Core Migrations and Database Setup Guide

## Overview
This document provides comprehensive instructions for managing Entity Framework Core migrations and database setup for the Employee Service microservice.

## Migration Architecture

### Files Structure
```
EmployeeService.Infrastructure/
├── Migrations/
│   ├── 20260316000000_InitialCreate.cs          # Migration definition
│   └── EmployeeDbContextModelSnapshot.cs        # Current model state
├── Persistence/
│   └── EmployeeDbContext.cs                     # EF Core DbContext
├── Repositories/
│   ├── EmployeeRepository.cs                    # Employee data access
│   └── SalaryIncrementLogRepository.cs          # Increment history access
├── SeedData/
│   └── SeedData.sql                             # Manual seed script (optional)
└── DependencyInjection.cs                       # DI configuration with SeedInitialData()
```

## Initial Migration Details

### Migration: 20260316000000_InitialCreate

#### Tables Created

**1. Employees Table**
```sql
CREATE TABLE [Employees] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [EmployeeSystemId] INT UNIQUE NOT NULL,
    
    -- Personal Information
    [FirstName] NVARCHAR(100) NOT NULL,
    [LastName] NVARCHAR(100) NOT NULL,
    [MiddleName] NVARCHAR(100),
    [Email] NVARCHAR(256) NOT NULL UNIQUE,
    [PhoneNumber] NVARCHAR(20),
    
    -- Employment Data
    [EmployeeCode] NVARCHAR(20) NOT NULL UNIQUE,
    [CostCenterId] NVARCHAR(50),
    [EmploymentStatus] NVARCHAR(20) NOT NULL DEFAULT 'Active',
    [JoiningDate] DATETIME2 NOT NULL,
    [TerminationDate] DATETIME2,
    
    -- Salary Information (owned type Money)
    [GrossCTC] DECIMAL(19,2) NOT NULL,
    [GrossCTC_Currency] NVARCHAR(3) NOT NULL,
    [BasicSalary] DECIMAL(19,2) NOT NULL,
    [BasicSalary_Currency] NVARCHAR(3) NOT NULL,
    [CTCEffectiveDate] DATETIME2 NOT NULL,
    [LastCTCModificationDate] DATETIME2,
    
    -- Audit Information
    [CreatedAt] DATETIME2 NOT NULL,
    [UpdatedAt] DATETIME2,
    [CreatedBy] NVARCHAR(256),
    [UpdatedBy] NVARCHAR(256),
    [IsDeleted] BIT NOT NULL DEFAULT 0
);
```

**2. SalaryIncrementLogs Table**
```sql
CREATE TABLE [SalaryIncrementLogs] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [EmployeeSystemId] INT NOT NULL,
    
    -- CTC Information
    [OldCTC] DECIMAL(19,2) NOT NULL,
    [OldCTC_Currency] NVARCHAR(3) NOT NULL,
    [NewCTC] DECIMAL(19,2) NOT NULL,
    [NewCTC_Currency] NVARCHAR(3) NOT NULL,
    
    -- Increment Details (owned type Percentage)
    [IncrementPercentage] DECIMAL(8,2) NOT NULL,
    [EffectiveDate] DATETIME2 NOT NULL,
    
    -- Approval Information
    [ApprovedBy] INT,
    [ApprovedOn] DATETIME2,
    [ApprovalComments] NVARCHAR(500),
    [Status] NVARCHAR(20) NOT NULL DEFAULT 'Pending',
    
    -- Audit Information
    [CreatedAt] DATETIME2 NOT NULL,
    [UpdatedAt] DATETIME2,
    [CreatedBy] NVARCHAR(256),
    [UpdatedBy] NVARCHAR(256),
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    
    FOREIGN KEY ([EmployeeSystemId]) REFERENCES [Employees]([EmployeeSystemId])
);
```

#### Indices Created
```sql
-- Performance optimization indices
CREATE INDEX [IX_Employees_EmployeeSystemId] ON [Employees]([EmployeeSystemId]);
CREATE INDEX [IX_Employees_Email] ON [Employees]([Email]);
CREATE INDEX [IX_Employees_EmployeeCode] ON [Employees]([EmployeeCode]);
CREATE INDEX [IX_Employees_CostCenterId] ON [Employees]([CostCenterId]);
CREATE INDEX [IX_Employees_EmploymentStatus] ON [Employees]([EmploymentStatus]);
CREATE INDEX [IX_SalaryIncrementLogs_EmployeeSystemId] ON [SalaryIncrementLogs]([EmployeeSystemId]);
CREATE INDEX [IX_SalaryIncrementLogs_EffectiveDate] ON [SalaryIncrementLogs]([EffectiveDate]);
CREATE INDEX [IX_SalaryIncrementLogs_Status] ON [SalaryIncrementLogs]([Status]);
```

## Database Setup Instructions

### Prerequisites
- SQL Server 2019 or later (or SQL Server LocalDB)
- .NET 10 SDK
- Connection string: `Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;`

### Step 1: Verify Database Connection
```powershell
# Navigate to project root
cd e:\ERPMicroservice\src\Services\payServices\employeeServices

# Test connection string in appsettings.json
# Should target: (localdb)\MSSQLLocalDB
```

### Step 2: Apply Migration
```powershell
# Option A: Using EF Core CLI (recommended)
cd EmployeeService.Infrastructure
dotnet ef database update --startup-project ../EmployeeService.API

# Option B: Using Package Manager Console (Visual Studio)
Update-Database -StartupProject EmployeeService.API
```

**Expected Output:**
```
Build started...
Build succeeded.
Done. Creating database...
Applying migration '20260316000000_InitialCreate'.
Done.
```

### Step 3: Verify Database Creation
```sql
-- Using SQL Server Management Studio or Azure Data Studio
-- Connect to: (localdb)\MSSQLLocalDB
-- Database: PAYDB

SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME IN ('Employees', 'SalaryIncrementLogs');
```

### Step 4: Verify Seed Data
The database will automatically be seeded with 5 sample employees and 4 increment logs via the `SeedInitialData()` method called in `Program.cs`:

```powershell
# View seed data
cd EmployeeService.API
dotnet run
```

Then query via API:
```
GET https://localhost:7xxx/api/v1/employees
```

## Seed Data Details

### Sample Employees
| ID | EmployeeSystemId | Name | Email | CTC | Status |
|---|---|---|---|---|---|
| 1001 | Rajesh Kumar Singh | rajesh.kumar@example.com | 600,000 INR | Active |
| 1002 | Priya Sharma Tanvi | priya.sharma@example.com | 550,000 INR | Active |
| 1003 | Amit Patel Kumar | amit.patel@example.com | 750,000 INR | Active |
| 1004 | Neha Gupta Rani | neha.gupta@example.com | 500,000 INR | Active |
| 1005 | Vikram Singh Rajendra | vikram.singh@example.com | 650,000 INR | Active |

### Sample Increment Logs
- **2023-04-01**: Rajesh Kumar (600k → 660k, 10% increase)
- **2023-04-01**: Priya Sharma (550k → 605k, 10% increase)
- **2023-04-01**: Amit Patel (750k → 825k, 10% increase)
- **2024-04-01**: Rajesh Kumar (660k → 726k, 10% increase)
- **2024-04-01**: Vikram Singh (650k → 715k, 10% increase)

## Migration Management

### Creating New Migrations

When you modify the domain model or DbContext configuration, create a new migration:

```powershell
cd EmployeeService.Infrastructure

# Create a new migration
dotnet ef migrations add MigrationName --startup-project ../EmployeeService.API

# Example: Adding new columns
dotnet ef migrations add AddDepartmentField --startup-project ../EmployeeService.API
```

This creates:
```
20260316000001_AddDepartmentField.cs    # New migration file
```

### Reviewing Migrations

```powershell
# List all migrations
dotnet ef migrations list --startup-project ../EmployeeService.API

# Generate SQL script (for production deployment)
dotnet ef migrations script 20260316000000 20260316000001 --startup-project ../EmployeeService.API
```

### Rolling Back

```powershell
# Revert to specific migration
dotnet ef database update 20260316000000 --startup-project ../EmployeeService.API

# Remove last migration (if not applied to production)
dotnet ef migrations remove --startup-project ../EmployeeService.API
```

## Production Deployment

### Generate Script for Production

```powershell
# Generate idempotent SQL script for all pending migrations
dotnet ef migrations script --idempotent --output migrations_script.sql --startup-project ../EmployeeService.API
```

### Apply via SQL Script

```sql
-- In SQL Server Management Studio
-- Connect to production database
-- Execute the generated script
:r C:\path\to\migrations_script.sql
```

## Seed Data Management

### Programmatic Seeding
The `SeedInitialData()` method in `DependencyInjection.cs` automatically executes when:
1. Application starts for the first time
2. Database is created via migration
3. Called explicitly: `await dbContext.Database.MigrateAsync();`

### Manual SQL Seeding
If you prefer to seed data manually:

```powershell
# Execute the seed script
sqlcmd -S (localdb)\MSSQLLocalDB -d PAYDB -i SeedData.sql
```

### Clearing Seed Data

```sql
-- Delete all increment logs (referenced by FK)
DELETE FROM [dbo].[SalaryIncrementLogs];

-- Delete all employees
DELETE FROM [dbo].[Employees];

-- Reset identity seed
DBCC CHECKIDENT ('[Employees]', RESEED, 0);
DBCC CHECKIDENT ('[SalaryIncrementLogs]', RESEED, 0);
```

## Troubleshooting

### Issue: Migration Not Found
```powershell
# Rebuild solution first
dotnet build

# Check migration history
dotnet ef migrations list --startup-project ../EmployeeService.API
```

### Issue: Database Locked
```powershell
# Close all connections to database
# For LocalDB, you can restart the service:
sqllocaldb.exe stop MSSQLLocalDB
sqllocaldb.exe start MSSQLLocalDB
```

### Issue: Foreign Key Constraint Error
```sql
-- Check constraint names
SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS 
WHERE CONSTRAINT_TYPE = 'FOREIGN KEY';

-- Disable constraints temporarily
ALTER TABLE [SalaryIncrementLogs] NOCHECK CONSTRAINT ALL;
-- ... perform operations ...
ALTER TABLE [SalaryIncrementLogs] WITH CHECK CHECK CONSTRAINT ALL;
```

### Issue: Migration Validation Errors
```powershell
# Update database with verbose output
dotnet ef database update --startup-project ../EmployeeService.API --verbose

# Check DbContext configuration
dotnet ef dbcontext info --startup-project ../EmployeeService.API
```

## Key Concepts

### Owned Types
The migration uses owned types for value objects:
- `Money` (GrossCTC, BasicSalary, OldCTC, NewCTC)
- `Percentage` (IncrementPercentage)

These are stored as JSON columns or decomposed into multiple columns following the EF Core configuration.

### Audit Fields
All tables include:
- `CreatedAt`: When record was created
- `UpdatedAt`: When record was last modified
- `CreatedBy`: User who created the record
- `UpdatedBy`: User who last modified
- `IsDeleted`: Soft delete flag (for GDPR compliance)

### Indexes
Indices are created on frequently queried columns:
- Employee lookups: EmployeeSystemId, Email, EmployeeCode
- Cost Center queries: CostCenterId
- Status filtering: EmploymentStatus, Status
- Date range queries: EffectiveDate

## Related Documentation

- [DbContext Configuration](../Persistence/EmployeeDbContext.cs)
- [Domain Model](../../EmployeeService.Domain/)
- [Repository Pattern Implementation](./EmployeeRepository.cs)
- [Application Layer CQRS Handlers](../../EmployeeService.Application/)

## Support & Resources

- [EF Core Migrations Documentation](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [SQL Server LocalDB Guide](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)
- [Owned Entity Types in EF Core](https://learn.microsoft.com/en-us/ef/core/modeling/owned-entities)
