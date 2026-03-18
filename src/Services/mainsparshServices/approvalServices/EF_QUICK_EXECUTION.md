# 🚀 Quick Migration & Seed Execution Guide

> **TL;DR** - Run these 3 commands:
> ```powershell
> cd src\ApprovalService.API
> dotnet ef database update
> dotnet run
> ```

---

## ✅ Pre-Flight Checklist

Before running migrations, verify:

- [ ] .NET 8.0 SDK installed: `dotnet --version`
- [ ] SQL Server or LocalDB running
- [ ] Connection string configured in `appsettings.json`
- [ ] EF Core tools installed (if not auto-installed)
- [ ] All NuGet packages restored

---

## 📍 Step-by-Step Execution

### Step 1: Navigate to API Project

```powershell
# Change to the API project directory
cd src\ApprovalService.API
```

**Why?** EF Core CLI runs migration commands from the API project context where both the DbContext and migrations are accessible.

---

### Step 2: Restore NuGet Packages (First Time Only)

```powershell
# Restore all NuGet packages for the solution
dotnet restore
```

**Output:**
```
Restore completed in 45.23 sec for e:\...\ApprovalService.API.csproj.
```

---

### Step 3: Verify Database Connection

```powershell
# Test if connection string is valid
sqlcmd -S (localdb)\MSSQLLocalDB
GO
SELECT @@VERSION
GO
EXIT
```

**Expected Output:** SQL Server version information

If error: "Named instance not found" → Start SQL Server LocalDB:
```powershell
sqllocaldb start mssqllocaldb
```

---

### Step 4: Apply Migrations & Create Database

```powershell
# This single command:
# 1. Creates the database if it doesn't exist
# 2. Applies all pending migrations (currently: InitialCreate)
# 3. Creates all tables, indexes, constraints
dotnet ef database update
```

**Expected Output:**
```
Build started...
Build succeeded.
Applying migration '20260315000000_InitialCreate'.
Done. To undo this action, use 'ef database drop'.
```

**What was created:**
- ✅ Database: `ApprovalServiceDb`
- ✅ Table: `APPR_MAST` (4 seed records)
- ✅ Table: `APPROVER_EMP` (11 seed records)
- ✅ Indexes: 4 performance indexes
- ✅ Constraints: Foreign keys, unique, check constraints
- ✅ Table: `__EFMigrationsHistory` (tracks migrations)

---

### Step 5: Verify Database Creation

```powershell
# Query the database to verify
sqlcmd -S (localdb)\MSSQLLocalDB -d ApprovalServiceDb

# Inside sqlcmd:
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo';
GO

# You should see:
# APPR_MAST
# APPROVER_EMP

# Check seed data
SELECT COUNT(*) as ApprovalMasterCount FROM APPR_MAST;
GO
# Output: 4

SELECT COUNT(*) as ApproverEmployeeCount FROM APPROVER_EMP;
GO
# Output: 11

# Exit
EXIT
```

---

### Step 6: Start the Application

```powershell
# This will:
# 1. Automatically call MigrateAndSeedDatabaseAsync()
# 2. Start the API on https://localhost:5001
dotnet run
```

**Expected Output:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[0]
      Application started.
```

---

### Step 7: Verify API is Working

**Open in Browser or REST Client:**

```
https://localhost:5001/swagger
```

**Test an endpoint:**

```powershell
# Get all approvals (should return 4 seed records)
curl -X GET "https://localhost:5001/api/approvals" `
  -H "Content-Type: application/json" `
  -k  # Ignore SSL certificate for localhost

# OR use PowerShell:
Invoke-WebRequest -Uri "https://localhost:5001/api/approvals" -SkipCertificateCheck
```

**Expected Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "approvalCode": "TRV_REQ",
      "approvalDescription": "Travel Request Approval",
      "module": "Travel",
      "level": 3,
      "status": "A"
    },
    {
      "id": 2,
      "approvalCode": "LEV_REQ",
      "approvalDescription": "Leave Request Approval",
      "module": "Leave",
      "level": 2,
      "status": "A"
    },
    // ... more records
  ],
  "message": "Approval masters retrieved successfully."
}
```

✅ **SUCCESS!** Database is ready with seed data.

---

## 📋 Seed Data Overview

### Approval Masters Seeded (4 records)

| Code | Description | Module | Levels | Status |
|------|-------------|--------|--------|--------|
| TRV_REQ | Travel Request Approval | Travel | 3 | Active |
| LEV_REQ | Leave Request Approval | Leave | 2 | Active |
| EXP_RPT | Expense Report Approval | Finance | 2 | Active |
| DOC_APR | Document Approval | Admin | 4 | Active |

### Approver Employee Assignments (11 records)

```
TRV_REQ:
  ├─ Level 1: Employee 1001
  ├─ Level 2: Employee 1002
  └─ Level 3: Employee 1003

LEV_REQ:
  ├─ Level 1: Employee 1004
  └─ Level 2: Employee 1005

EXP_RPT:
  ├─ Level 1: Employee 1006
  └─ Level 2: Employee 1007

DOC_APR:
  ├─ Level 1: Employee 1001
  ├─ Level 2: Employee 1002
  ├─ Level 3: Employee 1003
  └─ Level 4: Employee 1004
```

---

## 🔧 Common Operations

### View Applied Migrations

```powershell
dotnet ef migrations list
```

**Output:**
```
20260315000000_InitialCreate (Applied)
```

### View Pending Migrations

```powershell
# If you made model changes but haven't created a migration
dotnet ef migrations list
# Shows pending (not applied) migrations with "Pending" status
```

### Create a New Migration (After Making Schema Changes)

```powershell
# 1. Edit your domain entity
# 2. Create a migration
dotnet ef migrations add AddNewFieldToApprovalMaster

# Output:
# Adding migration '20260315000001_AddNewFieldToApprovalMaster'.
# Generating migration file '20260315000001_AddNewFieldToApprovalMaster.cs'.
# Done.

# 3. Review the generated migration file
# 4. Apply it
dotnet ef database update
```

### Undo/Revert Last Migration

```powershell
# Option 1: Remove the migration file (if not applied to production)
dotnet ef migrations remove

# Option 2: Revert database to previous migration
dotnet ef database update PreviousMigrationName

# Option 3: Drop entire database and start fresh
dotnet ef database drop --force
dotnet ef database update
```

### Generate SQL Script (View What Will Execute)

```powershell
# Generate SQL for all pending migrations
dotnet ef database script

# Generate SQL between two migrations
dotnet ef database script --from 20260315000000_InitialCreate --to 20260315000001_AddNewField

# Save to file
dotnet ef database script -o migration.sql
```

---

## 🐛 Troubleshooting

### Error: "No database provider has been configured"

**Cause:** DbContext registration issue in Program.cs

**Solution:**
```csharp
// In Program.cs, verify:
builder.Services.AddDbContext<ApprovalServiceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

---

### Error: "Could not find the project or its dependencies"

**Cause:** Running from wrong directory

**Solution:**
```powershell
# Must run from API project folder
cd src\ApprovalService.API

# Then run migrations
dotnet ef database update
```

---

### Error: "Named Pipes Provider error: 40"

**Cause:** SQL Server not running or wrong connection string

**Solution:**
```powershell
# Start SQL Server LocalDB
sqllocaldb start mssqllocaldb

# Verify running
sqllocaldb info mssqllocaldb

# Check connection string in appsettings.json:
# Should be: Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ApprovalServiceDb;...
```

---

### Error: "Migrations history table not created"

**Cause:** Database access issue or permissions

**Solution:**
```powershell
# Drop and recreate database
dotnet ef database drop --force
dotnet ef database update
```

---

### Error: "EF Core tools not installed"

**Cause:** Outdated or missing dotnet ef CLI tools

**Solution:**
```powershell
# Install globally
dotnet tool install --global dotnet-ef

# Or update if already installed
dotnet tool update --global dotnet-ef

# Verify installation
dotnet ef --version
```

---

## 📊 Database Verification Queries

### Get Database Info

```sql
-- Connect to: (localdb)\MSSQLLocalDB, Database: ApprovalServiceDb

-- List all tables
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo';

-- List all columns in APPR_MAST
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'APPR_MAST' AND TABLE_SCHEMA = 'dbo';

-- Count seed data
SELECT 'APPR_MAST' as TableName, COUNT(*) as RowCount FROM APPR_MAST
UNION ALL
SELECT 'APPROVER_EMP', COUNT(*) FROM APPROVER_EMP;

-- View migrations applied
SELECT MigrationId, ProductVersion FROM __EFMigrationsHistory ORDER BY MigrationId;

-- Check indexes
SELECT name, type_desc FROM sys.indexes WHERE object_id = OBJECT_ID('APPR_MAST');

-- Check constraints
SELECT CONSTRAINT_NAME, CONSTRAINT_TYPE 
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS 
WHERE TABLE_NAME = 'APPR_MAST';
```

---

## 🎯 Next Steps

After successful migration and seeding:

1. ✅ **Database Ready** - Schema created, seed data loaded
2. 🧪 **Test API** - Run the 25 test cases from API_TESTING_GUIDE.md
3. 📝 **Review Logs** - Check `logs/nlog-*.log` for any warnings
4. 🔧 **Configure** - Update connection strings for your environment
5. 🚀 **Deploy** - Follow deployment instructions in README_COMPREHENSIVE.md

---

## 💡 Tips & Best Practices

**Tip 1:** Always test migrations on staging before production
```powershell
# Create backup before applying migrations to production database
BACKUP DATABASE ApprovalServiceDb TO DISK = 'backup.bak'
```

**Tip 2:** Include migration scripts in version control
```bash
git add src/Infrastructure/Database/Migrations/
git commit -m "Add migration: AddNewFieldToApprovalMaster"
```

**Tip 3:** Document migration purposes
```powershell
# In the generated migration file, add comment explaining the change
/// <summary>
/// Adds ApprovalPriority field to track critical approvals
/// Required for feature: PriorityBasedRouting
/// </summary>
```

**Tip 4:** Test rollback procedures
```powershell
# Verify you can rollback before production deployment
dotnet ef database update PreviousMigrationName
# Then reapply
dotnet ef database update
```

---

## 📚 Reference

- [Complete EF Migrations Guide](EF_MIGRATIONS_GUIDE.md)
- [Troubleshooting Guide](README_COMPREHENSIVE.md#troubleshooting)
- [API Testing Examples](API_TESTING_GUIDE.md)
- [Quick Start Setup](QUICK_START.md)

---

**Ready?** Start with Step 1 above! ✅

