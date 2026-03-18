# 🗄️ Entity Framework Migrations & Database Seeding Guide

## Quick Commands

```powershell
# Navigate to API project
cd src\ApprovalService.API

# Add a new migration (if you made model changes)
dotnet ef migrations add YourMigrationName

# Update database with migrations
dotnet ef database update

# Undo last migration
dotnet ef migrations remove

# View applied migrations
dotnet ef migrations list

# Drop all tables and reapply migrations
dotnet ef database drop --force
dotnet ef database update
```

---

## 📋 Step-by-Step Setup Guide

### Prerequisites
- SQL Server or SQL Server LocalDB installed
- .NET 8.0 SDK installed
- Visual Studio or VS Code with C# extension

### Step 1: Restore NuGet Packages
```powershell
cd src\ApprovalService.API
dotnet restore
```

### Step 2: Create/Update Database
```powershell
# Create database and apply all migrations
dotnet ef database update

# Output should show:
# Done. To undo this action, use 'ef database drop'
```

### Step 3: Verify Database Creation
```powershell
# Check if ApprovalServiceDb database exists in SQL Server
# Tables should appear: APPR_MAST, APPROVER_EMP
```

### Step 4: Seed Sample Data
```powershell
# The seed data is automatically applied when:
# 1. Application starts and calls Program.cs
# 2. DbSeed.SeedDatabaseAsync() is invoked

# Or manually seed:
dotnet run --project src/ApprovalService.API
# This executes: app.MigrateAndSeedDatabase(); in Program.cs
```

---

## 🔍 Migration Details

### Project Structure for Migrations

```
src/ApprovalService.Infrastructure/
└── Database/
    ├── ApprovalServiceDbContext.cs (DbContext definition)
    ├── DbSeed.cs (Sample data seeding)
    └── Migrations/
        ├── 20260315000000_InitialCreate.cs (Migration file)
        ├── 20260315000001_InitialCreate.Designer.cs (Designer metadata)
        └── ApprovalServiceDbContextModelSnapshot.cs (Current model snapshot)
```

### Understanding InitialCreate Migration

The `InitialCreate.cs` migration creates two tables:

#### APPR_MAST Table
```sql
CREATE TABLE APPR_MAST (
    APPR_ID INT PRIMARY KEY IDENTITY(1,1),
    APPR_CODE VARCHAR(20) NOT NULL UNIQUE,
    APPR_DESC VARCHAR(100) NOT NULL,
    APPR_MODULE VARCHAR(50) NOT NULL,
    APPR_LEVEL INT NOT NULL CHECK (APPR_LEVEL > 0),
    APPR_STATUS CHAR(1) NOT NULL DEFAULT 'A', -- A=Active, I=Inactive
    CREATED_BY VARCHAR(100) NOT NULL,
    CREATED_DATE DATETIME2 NOT NULL DEFAULT GETDATE(),
    UPDATED_BY VARCHAR(100),
    UPDATED_DATE DATETIME2
)
```

#### APPROVER_EMP Table
```sql
CREATE TABLE APPROVER_EMP (
    APPROVER_ID INT PRIMARY KEY IDENTITY(1,1),
    APPR_ID INT NOT NULL FOREIGN KEY REFERENCES APPR_MAST(APPR_ID),
    EMP_ID INT NOT NULL,
    APPR_LEVEL INT NOT NULL CHECK (APPR_LEVEL > 0),
    APPR_STATUS CHAR(1) NOT NULL DEFAULT 'A', -- A=Active, I=Inactive
    EFF_FROM_DATE DATE NOT NULL,
    EFF_TO_DATE DATE,
    CREATED_BY VARCHAR(100) NOT NULL,
    CREATED_DATE DATETIME2 NOT NULL DEFAULT GETDATE(),
    UPDATED_BY VARCHAR(100),
    UPDATED_DATE DATETIME2
)
```

### Indexes Created

```sql
-- APPR_MAST Indexes
CREATE INDEX IX_APPR_MAST_CODE ON APPR_MAST(APPR_CODE);
CREATE INDEX IX_APPR_MAST_MODULE ON APPR_MAST(APPR_MODULE);

-- APPROVER_EMP Indexes
CREATE INDEX IX_APPROVER_EMP_APPR_ID ON APPROVER_EMP(APPR_ID);
CREATE INDEX IX_APPROVER_EMP_EMP_ID ON APPROVER_EMP(EMP_ID);
```

---

## 🌱 Seed Data Details

### Sample Data Included

The `DbSeed.cs` automatically seeds:

#### 4 Approval Masters
1. **Travel Request** (TRV_REQ) - 3 levels
2. **Leave Request** (LEV_REQ) - 2 levels
3. **Expense Report** (EXP_RPT) - 2 levels
4. **Document Approval** (DOC_APR) - 4 levels

#### 10+ Approver Assignments
Each approval type has employees assigned as approvers at different levels:

| Module | Level | Employee ID | Effective From |
|--------|-------|------------|-----------------|
| TRV_REQ | 1 | 1001 | Current Date |
| TRV_REQ | 2 | 1002 | Current Date |
| TRV_REQ | 3 | 1003 | Current Date |
| LEV_REQ | 1 | 1004 | Current Date |
| LEV_REQ | 2 | 1005 | Current Date |
| EXP_RPT | 1 | 1006 | Current Date |
| EXP_RPT | 2 | 1007 | Current Date |

### Accessing Seed Code

**File**: `src/Infrastructure/Database/DbSeed.cs`

```csharp
public static class DbSeed
{
    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApprovalServiceDbContext>();
            await context.Database.MigrateAsync();
            
            // Seed data if tables are empty
            if (!context.ApprovalMasters.Any())
            {
                // Adds sample approval masters and approver employees
                // Called automatically on startup
            }
        }
    }
}
```

### Called From Program.cs

**File**: `src/API/Program.cs`

```csharp
// After building app
var app = builder.Build();

// Apply migrations and seed data
await app.MigrateAndSeedDatabase();

app.Run();
```

---

## 🔄 Common Migration Scenarios

### Scenario 1: Adding a New Approval Field

**Step 1**: Update the entity in `ApprovalMaster.cs`
```csharp
public class ApprovalMaster : Entity
{
    public string ApprovalCode { get; set; }
    public string ApprovalDescription { get; set; }
    public string Module { get; set; }
    public int Level { get; set; }
    public string NewField { get; set; } // ← NEW
    // ... rest of properties
}
```

**Step 2**: Create migration
```powershell
cd src\ApprovalService.API
dotnet ef migrations add AddNewFieldToApprovalMaster --project ..\ApprovalService.Infrastructure
```

**Step 3**: Review generated migration file

**Step 4**: Apply migration
```powershell
dotnet ef database update
```

### Scenario 2: Adding a New Table

**Step 1**: Create entity in Domain project

**Step 2**: Add DbSet to DbContext
```csharp
public DbSet<NewEntity> NewEntities { get; set; }
```

**Step 3**: Add fluent configuration in DbContext.OnModelCreating()

**Step 4**: Create migration
```powershell
dotnet ef migrations add AddNewEntityTable
```

**Step 5**: Apply migration
```powershell
dotnet ef database update
```

### Scenario 3: Reverting to Previous Migration

```powershell
# Revert to previous migration
dotnet ef database update PreviousMigrationName

# Or remove latest migration (if not applied to production)
dotnet ef migrations remove
```

---

## ⚙️ Configuration Required

### Connection String

**File**: `src/API/appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ApprovalServiceDb;Integrated Security=True;Persist Security Info=False;..."
  }
}
```

### Environment-Specific Settings

**Development** (`appsettings.Development.json`):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ApprovalServiceDb_Dev;Integrated Security=True;..."
  }
}
```

**Production** (`appsettings.Production.json`):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server;Initial Catalog=ApprovalServiceDb;User Id=sa;Password=***;..."
  }
}
```

---

## ✅ Verification Steps

### After Running Migrations

1. **Check Database Creation**
   ```powershell
   # Open SQL Server Management Studio or use sqlcmd
   sqlcmd -S (localdb)\MSSQLLocalDB
   SELECT name FROM sys.databases WHERE name = 'ApprovalServiceDb'
   ```

2. **Check Tables**
   ```sql
   SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES 
   WHERE TABLE_SCHEMA = 'dbo';
   -- Should show: APPR_MAST, APPROVER_EMP
   ```

3. **Check Seed Data**
   ```sql
   SELECT * FROM APPR_MAST;
   -- Should show 4 approval types
   
   SELECT * FROM APPROVER_EMP;
   -- Should show 10+ approver assignments
   ```

4. **Check Migrations Table**
   ```sql
   SELECT * FROM __EFMigrationsHistory;
   -- Should show: 20260315000000_InitialCreate
   ```

5. **Check Schema Version**
   ```sql
   SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('APPR_MAST');
   -- Verify all columns exist
   ```

---

## 🚑 Troubleshooting

### Issue: "No database provider has been configured"

**Solution**: Ensure Program.cs has database configuration
```csharp
builder.Services.AddDbContext<ApprovalServiceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

### Issue: "Migration not found"

**Solution**: Run from correct directory
```powershell
cd src\ApprovalService.API
dotnet ef migrations list
```

### Issue: "Connection string error"

**Solution**: Verify connection string in appsettings.json
```powershell
# Test connection
sqlcmd -S (localdb)\MSSQLLocalDB

# If SQL Server not running, start it
```

### Issue: "Database already exists"

**Solution**: Drop and recreate
```powershell
dotnet ef database drop --force
dotnet ef database update
```

### Issue: "EF Tools not installed"

**Solution**: Install EF Core CLI tools
```powershell
dotnet tool install --global dotnet-ef
# or update
dotnet tool update --global dotnet-ef
```

---

## 📊 Migration Commands Reference Table

| Command | Purpose | Example |
|---------|---------|---------|
| `dotnet ef migrations add` | Create new migration | `dotnet ef migrations add AddNewField` |
| `dotnet ef database update` | Apply migrations | `dotnet ef database update` |
| `dotnet ef database update {name}` | Update to specific | `dotnet ef database update InitialCreate` |
| `dotnet ef migrations remove` | Remove last migration | `dotnet ef migrations remove` |
| `dotnet ef migrations list` | Show all migrations | `dotnet ef migrations list` |
| `dotnet ef database drop` | Delete database | `dotnet ef database drop --force` |
| `dotnet ef dbcontext info` | Show DbContext info | `dotnet ef dbcontext info` |
| `dotnet ef model script` | Generate SQL script | `dotnet ef database script` |

---

## 🔐 Production Migration Strategy

### Pre-Deployment Checklist
- [ ] Test migrations on staging environment
- [ ] Backup production database
- [ ] Create rollback plan
- [ ] Review generated SQL scripts
- [ ] Test data preservation
- [ ] Verify indexes creation
- [ ] Check foreign key constraints
- [ ] Validate seed data completeness

### Deployment Steps
1. Backup production database
2. Generate SQL script: `dotnet ef database script`
3. Review SQL script for breaking changes
4. Test on staging first
5. Apply to production during maintenance window
6. Verify data integrity
7. Monitor application logs

### Rollback Plan
```powershell
# If migration fails, rollback to previous version
dotnet ef database update PreviousMigrationName

# Or restore from backup
# Restore-SqlDatabase -ServerInstance YourServer -Database ApprovalServiceDb -BackupFile backup.bak
```

---

## 📚 Additional Resources

- [EF Core Migrations Documentation](https://docs.microsoft.com/ef/core/managing-schemas/migrations/)
- [EF Core Data Seeding](https://docs.microsoft.com/ef/core/modeling/data-seeding)
- [Connection Strings](https://www.connectionstrings.com/sql-server/)
- [SQL Server LocalDB](https://docs.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb)

---

## 🎯 Next Steps

1. **Run Initial Setup**
   ```powershell
   cd src\ApprovalService.API
   dotnet ef database update
   ```

2. **Verify Database**
   - Check tables in SQL Server
   - Verify seed data loaded

3. **Start Application**
   ```powershell
   dotnet run
   ```

4. **Test API**
   - Navigate to https://localhost:5001/swagger
   - Execute GET /api/approvals
   - Should return seed data

---

**Quick Reference**: Use this file as a reference guide for all migration and seeding tasks.

**Last Updated**: March 15, 2026
**Status**: ✅ Complete Documentation

