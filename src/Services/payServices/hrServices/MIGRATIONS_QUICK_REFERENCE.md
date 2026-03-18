# EF Migrations & Seed Data - Quick Reference

## 📋 Overview

The HR Microservice uses **Entity Framework Core 8.0** with **SQL Server LocalDB** for data persistence. The migration system creates 11 tables with proper relationships, indexes, and constraints. Seed data automatically populates reference tables with 4 departments, 5 employees, 3 shifts, 4 leave types, and 5 salary components.

---

## 🚀 Quick Start (3 Steps)

### 1. Run Automated Setup (Recommended)

**Windows - Batch File:**
```bash
setup-migrations.bat
```

**Windows - PowerShell:**
```powershell
.\setup-migrations.ps1
```

**macOS/Linux/Manual:**
```bash
dotnet build HRService.sln
dotnet ef migrations add InitialCreate --project HRService.Infrastructure --startup-project HRService.API
dotnet ef database update --project HRService.Infrastructure --startup-project HRService.API
```

### 2. Verify Database

```sql
-- In SQL Server Management Studio
USE PAYDB;
SELECT COUNT(*) AS TableCount FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='dbo' AND TABLE_NAME LIKE 'HR_%';
```

Expected: **11 tables**

### 3. Start API

```bash
cd HRService.API
dotnet run
```

Access: https://localhost:7001/swagger

---

## 📊 Database Schema

### Tables Created (11 total)

| Table | Purpose | Records |
|-------|---------|---------|
| HR_Department | Organizational divisions | 4 |
| HR_Employee | Employee master data | 5 |
| HR_Position | Job positions | 5 |
| HR_Shift | Work shifts | 3 |
| HR_LeaveType | Leave categories | 4 |
| HR_EmployeeLeave | Leave requests | 0 |
| HR_Attendance | Daily attendance | 0 |
| HR_EmployeeSalary | Salary records | 0 |
| HR_SalaryComponent | Salary components | 5 |
| HR_PerformanceReview | Performance reviews | 0 |
| HR_AuditLog | Audit trail | 0 |

### Key Relationships

```
Department (1) ───── (M) Employee
Department (1) ───── (M) Position
Position (1) ───── (M) Employee
Employee (1) ───── (M) EmployeeLeave
LeaveType (1) ───── (M) EmployeeLeave
Employee (1) ───── (M) Attendance
Shift (1) ───── (M) Attendance
Employee (1) ───── (M) EmployeeSalary
Employee (1) ───── (M) PerformanceReview
```

---

## 🔧 Migration Commands (NPM-Style)

### Create Migrations

```bash
# Initial migration (already created)
dotnet ef migrations add InitialCreate --project HRService.Infrastructure

# Future migrations
dotnet ef migrations add AddNewFeature --project HRService.Infrastructure
```

### Apply Migrations

```bash
# Apply to latest
dotnet ef database update --project HRService.Infrastructure

# Apply to specific migration
dotnet ef database update NameOfMigration --project HRService.Infrastructure

# Revert to baseline (removes all)
dotnet ef database update --migration 0 --project HRService.Infrastructure
```

### View Migrations

```bash
# Package Manager Console (Visual Studio)
Get-Migration -Project HRService.Infrastructure

# CLI
dotnet ef migrations list --project HRService.Infrastructure
```

### Remove/Delete Migrations

```bash
# Remove last migration (before applying to DB)
Remove-Migration -Project HRService.Infrastructure

# Or using CLI
dotnet ef migrations remove --project HRService.Infrastructure
```

---

## 📁 Seed Data

### Seeding Strategy

**Automatic:** Seed data configured in `SeedDataConfiguration.cs` via `HasData()` in `OnModelCreating()`.

**Trigger:** Automatically applied when migration is first applied.

**Location:** `Infrastructure/Data/SeedDataConfiguration.cs`

### Seed Data Content

**Departments (4):**
- HR - Human Resources
- IT - Information Technology
- FIN - Finance
- OPS - Operations

**Employees (5):**
- John Smith (HR Manager) - EMP001
- William Johnson (IT Director) - EMP002
- Mary Williams (Finance Manager) - EMP003
- James Brown (Senior Developer, Reports to William) - EMP004
- Patricia Davis (HR Specialist, Reports to John) - EMP005

**Shifts (3):**
- SHIFT_A: 8:00 AM - 4:00 PM
- SHIFT_B: 2:00 PM - 10:00 PM
- SHIFT_C: 10:00 PM - 6:00 AM

**Leave Types (4):**
- Annual Leave (20 days, Paid)
- Sick Leave (12 days, Paid)
- Maternity Leave (90 days, Paid)
- Unpaid Leave (Unlimited, Unpaid)

**Salary Components (5):**
- Basic Salary (Earning)
- HRA (Earning)
- Dearness Allowance (Earning)
- Income Tax (Deduction)
- PF Contribution (Deduction)

### Verify Seed Data

```sql
-- Count records by table
SELECT 'Departments' AS [Table], COUNT(*) AS [Records] FROM HR_Department
UNION ALL
SELECT 'Employees', COUNT(*) FROM HR_Employee
UNION ALL
SELECT 'Positions', COUNT(*) FROM HR_Position
UNION ALL
SELECT 'Shifts', COUNT(*) FROM HR_Shift
UNION ALL
SELECT 'Leave Types', COUNT(*) FROM HR_LeaveType
UNION ALL
SELECT 'Salary Components', COUNT(*) FROM HR_SalaryComponent;
```

**Expected Output:**
```
Table                Records
Departments          4
Employees            5
Positions            5
Shifts               3
Leave Types          4
Salary Components    5
```

---

## 🔐 Connection String

**Default (LocalDB):**
```
Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PAYDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=False;
```

**Parts:**
- Host: `(localdb)\MSSQLLocalDB`
- Database: `PAYDB`
- Auth: Windows Integrated Security
- SSL: Enabled

**Connection String Location:**
- Development: `appsettings.Development.json`
- Production: `appsettings.json` or environment variables

---

## ⚙️ Configuration Files

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PAYDB;..."
  }
}
```

### appsettings.Development.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PAYDB;..."
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

---

## 🐛 Troubleshooting

### Issue: "Cannot find the migrations assembly"

**Solution:**
```bash
# Specify correct projects
dotnet ef migrations add InitialCreate `
  --project HRService.Infrastructure `
  --startup-project HRService.API
```

### Issue: "Connection timeout"

**Check LocalDB:**
```bash
sqllocaldb info
sqllocaldb start MSSQLLocalDB
```

### Issue: "Foreign key constraint violation"

**Reset and try again:**
```bash
dotnet ef database update --migration 0 --project HRService.Infrastructure
dotnet ef database update --project HRService.Infrastructure
```

### Issue: Migration files don't exist

**Files should be here:**
```
HRService.Infrastructure/
├── Migrations/
│   ├── 20260317000000_InitialCreate.cs
│   ├── HRServiceDbContextModelSnapshot.cs
│   └── ...
```

If missing, recreate:
```bash
dotnet ef migrations add InitialCreate --force --project HRService.Infrastructure
```

### Issue: "Unable to resolve service"

**Update `Program.cs`:**
- Ensure DbContext is registered: `services.AddDbContext<HRServiceDbContext>`
- Verify connection string is available to startup project

---

## 📝 Manual Seed Data (SQL Script)

Alternative to EF seeding - directly execute SQL:

```bash
sqlcmd -S "(localdb)\MSSQLLocalDB" -d PAYDB -i DB_Scripts\seed-data.sql
```

Script location: `DB_Scripts/seed-data.sql`

---

## 🚢 Production Deployment

### Generate SQL Script (Preview Changes)

```bash
# View SQL without applying
dotnet ef migrations script --project HRService.Infrastructure --output migration.sql
```

### Backup Before Migration

```sql
BACKUP DATABASE [PAYDB] 
TO DISK = 'C:\Backups\PAYDB_backup.bak'
WITH INIT, NAME = 'PAYDB Backup';
```

### Apply to Production

```bash
# Via CLI
dotnet ef database update --project HRService.Infrastructure

# Via SQL script (reviewed first)
sqlcmd -S server_name -d PAYDB -i migration.sql
```

---

## 📚 Common Patterns

### Add New Entity Property

1. Update entity class:
```csharp
public string NewProperty { get; set; }
```

2. Create migration:
```bash
dotnet ef migrations add AddNewProperty --project HRService.Infrastructure
```

3. Apply:
```bash
dotnet ef database update --project HRService.Infrastructure
```

### Add Seed Data

1. Update `SeedDataConfiguration.cs`:
```csharp
private static void SeedNewData(ModelBuilder modelBuilder)
{
    var data = new[] { /*...*/ };
    modelBuilder.Entity("HRService.Domain.Entities.EntityName").HasData(data);
}
```

2. Call in `SeedData()`:
```csharp
SeedNewData(modelBuilder);
```

3. Create and apply migration

### Drop and Recreate Database

```bash
# Full reset
dotnet ef database drop --force --project HRService.Infrastructure
dotnet ef database update --project HRService.Infrastructure
```

---

## 🔗 Documentation

- **Detailed Guide:** See `MIGRATIONS_AND_SEED_GUIDE.md`
- **EF Core Docs:** https://docs.microsoft.com/en-us/ef/core/
- **SQL Server LocalDB:** https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb

---

## ✅ Pre-Deployment Checklist

- [ ] Connection string configured correctly
- [ ] SQL Server LocalDB installed and running
- [ ] Database PAYDB created or auto-created
- [ ] Migration applied successfully
- [ ] Seed data verified (correct record counts)
- [ ] All 11 tables present in database
- [ ] Foreign key relationships established
- [ ] Indexes created (11 indexes total)
- [ ] `__EFMigrationsHistory` table shows migration
- [ ] API can connect and query database

---

## 📞 Quick Commands Reference

```bash
# Build
dotnet build HRService.sln

# Create migration
dotnet ef migrations add InitialCreate --project HRService.Infrastructure --startup-project HRService.API

# Update database
dotnet ef database update --project HRService.Infrastructure --startup-project HRService.API

# List migrations
dotnet ef migrations list --project HRService.Infrastructure

# Revert to baseline
dotnet ef database update --migration 0 --project HRService.Infrastructure

# Run API
dotnet run --project HRService.API

# View Swagger
https://localhost:7001/swagger
```

---

**Last Updated:** March 17, 2026  
**EF Core Version:** 8.0.2  
**SQL Server:** LocalDB / 2019+
