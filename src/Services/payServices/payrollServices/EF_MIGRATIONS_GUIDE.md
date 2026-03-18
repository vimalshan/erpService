# EF Core Migrations and Seed Data Guide

## Overview

This document explains the Entity Framework Core migrations and database seeding implementation for the Payroll microservice.

## Automatic Database Initialization

The solution implements automatic database initialization through **EF Core Migrations** that runs on application startup.

### How It Works

1. **Migration Files**: The `InitialCreate` migration creates the database schema with all three tables:
   - `PAYROLL_BATCH` - Payroll batches for monthly processing
   - `PAY_TRANDET` - Payroll transactions (employee salary records)
   - `PAY_ARR` - Payroll adjustments (allowances, deductions, arrears)

2. **DbContext Factory**: `PayrollDbContextFactory.cs` enables EF Core tools to discover the database context during migration generation.

3. **Automatic Migration**: On application startup, `Program.cs` automatically:
   - Applies pending migrations using `context.Database.MigrateAsync()`
   - Seeds initial data using `SeedDataBatch.SeedAsync(context)`

## File Structure

```
PayrollServices.Infrastructure/
├── Migrations/
│   ├── 20240101000000_InitialCreate.cs        # Migration definition
│   ├── PayrollDbContextModelSnapshot.cs       # EF Core state snapshot
│   └── SeedDataBatch.cs                       # Seed data functionality
├── Data/
│   ├── PayrollDbContext.cs                    # Main DbContext
│   └── PayrollDbContextFactory.cs             # Design-time factory
└── ...
```

## Seed Data

### SeedDataBatch.cs

The `SeedDataBatch` class provides seed data for development and testing:

#### Payroll Batches (3 records)
- **2024-01**: January batch (Completed)
- **2024-02**: February batch (Completed)
- **2024-03**: March batch (Processing)

#### Payroll Transactions (5 records)
Employees 101, 102, 103 with monthly salary records:
- Gross salary ranges: 50,000 - 60,000
- Deductions: 5-10% of gross salary
- Net salary calculated as: Gross - Deductions

#### Payroll Adjustments (5 records)
- 2 Allowances (Performance Bonus, HRA)
- 2 Deductions (Loan EMI, Canteen Charges)
- 1 Arrear (Previous month arrear)

All seed data is marked as created on January 2024 with approvals from system user (CreatedBy/ApprovedBy = 1).

## Database Initialization Methods

### Method 1: Automatic (Recommended for Development)

Simply start the API application:

```bash
cd PayrollServices.API
dotnet run
```

The application will:
1. Automatically apply any pending EF migrations
2. Create the database if it doesn't exist
3. Seed initial data
4. Display initialization status in console output

**Console Output Example:**
```
Database migrations applied successfully
Seeded 3 payroll batches
Seeded 5 payroll transactions
Seeded 5 payroll adjustments
Database seeding completed successfully
```

### Method 2: Manual EF CLI

To generate migrations or apply them manually:

#### Generate a New Migration (if schema changes)

```bash
cd PayrollServices.Infrastructure
dotnet ef migrations add YourMigrationName --startup-project ../PayrollServices.API
```

#### Update Database

```bash
cd PayrollServices.Infrastructure
dotnet ef database update --startup-project ../PayrollServices.API
```

#### Drop and Recreate (Development Only!)

```bash
cd PayrollServices.Infrastructure
dotnet ef database drop --startup-project ../PayrollServices.API -f
dotnet ef database update --startup-project ../PayrollServices.API
```

### Method 3: Manual SQL Script

For environments where EF CLI isn't available or preferred, use the SQL script:

#### On Windows (SQL Server Management Studio)

1. Open **SQL Server Management Studio**
2. Connect to: `(localdb)\MSSQLLocalDB`
3. Ctrl+O and open: `Database/InitializeDatabase.sql`
4. Execute (F5)

#### On Windows (PowerShell via sqlcmd)

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -i "Database\InitializeDatabase.sql"
```

#### On Windows (Visual Studio Code / Command Line)

```bash
# Assumes SQL Server installed locally
sqlcmd -S (localdb)\MSSQLLocalDB -i Database\InitializeDatabase.sql
```

## Database Connection

The connection string is configured in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PAYDB;Integrated Security=true;"
  }
}
```

- **Server**: `(localdb)\MSSQLLocalDB` (SQL Server LocalDB)
- **Database**: `PAYDB`
- **Authentication**: Windows Integrated Security

## Table Schema

### PAYROLL_BATCH
```
BATCH_ID (bigint, PK)
BATCH_MONTH (nvarchar(7), unique) - Format: YYYY-MM
BATCH_STATUS (nvarchar, max)
BATCH_CREATEDBY (bigint)
BATCH_CREATEDON (datetime2)
BATCH_UPDATEDON (datetime2, nullable)
BATCH_UPDATEDBY (bigint, nullable)

Indexes:
- IX_PAYROLL_BATCH_BATCH_MONTH (unique on BATCH_MONTH)
```

### PAY_TRANDET
```
TRN_ID (bigint, PK, Identity)
TRN_EMPSYSID (bigint) - Employee System ID
TRN_BATCHID (bigint, FK) → PAYROLL_BATCH.BATCH_ID
TRN_MONTH (nvarchar(7)) - Format: YYYY-MM
TRN_GROSS (decimal(19,0))
TRN_DEDUCTIONS (decimal(19,0))
TRN_NET (decimal(19,0))
TRN_STATUS (nvarchar, max)
TRN_CREATEDBY (bigint)
TRN_CREATEDON (datetime2)
TRN_UPDATEDON (datetime2, nullable)
TRN_UPDATEDBY (bigint, nullable)

Indexes:
- IX_PAY_TRANDET_TRN_BATCHID (on TRN_BATCHID)
- IX_PAY_TRANDET_TRN_EMPSYSID_TRN_MONTH (composite on TRN_EMPSYSID, TRN_MONTH)
```

### PAY_ARR
```
AR_ID (bigint, PK)
PAY_EMPSYSID (bigint) - Employee System ID
AR_AMOUNT (decimal(19,0))
AR_TYPE (nvarchar, max) - Allowance/Deduction/Arrear
AR_DATE (datetime2)
AR_DESCRIPTION (nvarchar(500), nullable)
AR_CREATEDBY (bigint)
AR_CREATEDON (datetime2)
AR_APPROVEDON (datetime2, nullable)
AR_APPROVEDBY (bigint, nullable)

Indexes:
- IX_PAY_ARR_PAY_EMPSYSID (on PAY_EMPSYSID)
- IX_PAY_ARR_PAY_EMPSYSID_AR_DATE (composite on PAY_EMPSYSID, AR_DATE)
```

## Verification

### Verify Database Created

```bash
cd PayrollServices.API
dotnet run
# Watch for success messages in console output
```

### Query Database After Initialization

Using **SQL Server Management Studio** or **sqlcmd**:

```sql
USE PAYDB;

-- Check tables
SELECT * FROM PAYROLL_BATCH;     -- Should have 3 rows
SELECT * FROM PAY_TRANDET;       -- Should have 5 rows
SELECT * FROM PAY_ARR;           -- Should have 5 rows

-- Verify foreign keys
SELECT t.TRN_ID, t.TRN_EMPSYSID, b.BATCH_MONTH 
FROM PAY_TRANDET t 
JOIN PAYROLL_BATCH b ON t.TRN_BATCHID = b.BATCH_ID;
```

### Verify Indexes

```sql
-- Check indexes on tables
EXEC sp_helpindex 'PAYROLL_BATCH';
EXEC sp_helpindex 'PAY_TRANDET';
EXEC sp_helpindex 'PAY_ARR';
```

## Clearing/Resetting Database

**For Development Only - Use with Caution!**

### Using EF CLI

```bash
cd PayrollServices.Infrastructure
dotnet ef database drop --startup-project ../PayrollServices.API -f
dotnet ef database update --startup-project ../PayrollServices.API
```

### Using SQL Script

```sql
USE master;
DROP DATABASE IF EXISTS PAYDB;
-- Then (re)run InitializeDatabase.sql
```

## Troubleshooting

### Issue: Connection String Not Found

**Error**: `No database provider has been configured`

**Solution**: Ensure `appsettings.json` exists in `PayrollServices.API` with correct connection string.

### Issue: Migration Files Not Found

**Error**: `Unable to create an object of type PayrollDbContext`

**Solution**: Verify `PayrollDbContextFactory` is located in Infrastructure project with correct namespaces and using statements.

### Issue: Seed Data Not Inserted

**Error**: Migrations applied but no data visible

**Cause**: Check appsettings.json exists and app started successfully

### Issue: Permission Denied on LocalDB

**Solution**: 
1. Verify SQL Server LocalDB instance is installed: `sqllocaldb info`
2. Run command prompt as Administrator if needed
3. Verify Windows authentication is enabled

## Adding New Seed Data

To extend seed data:

1. Edit `SeedDataBatch.cs`
2. Add new records to appropriate `GetSeed*` methods
3. Rebuild solution
4. Delete database and restart application, or:
   ```bash
   dotnet ef database drop -f && dotnet run
   ```

## Migration Best Practices

1. **Always test migrations locally first** before deploying to production
2. **Name migrations descriptively**: `AddPayrollBatchTable`, `AddIndexOnTransactionMonth`
3. **Review generated migration** before applying to ensure it matches intent
4. **Keep migrations small** - one logical change per migration
5. **Never edit previous migrations** - create new ones for changes
6. **Test rollback**: Verify Down() method works on test database

## Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PAYDB;Integrated Security=true;"
  }
}
```

### appsettings.Development.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug"
    }
  }
}
```

## References

- [Entity Framework Core Migrations Documentation](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [EF Core Database Providers](https://docs.microsoft.com/en-us/ef/core/providers/)
- [SQL Server LocalDB Documentation](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)
