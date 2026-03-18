# EF Core Migrations & Database Setup

## Overview
This document describes the Entity Framework Core migrations for the GST Compliance module and instructions for applying them to the database.

## Migration Files

### 1. **20260317000000_InitialCreate.cs**
The initial migration that creates the entire database schema with 5 tables and their relationships:
- `GST_MAIN` - Root aggregate for GST registrations (PK: GST_ID, Unique: GST_PANNO)
- `GST_SUPPLIER` - Supplier reference data (PK: SUPPLIER_NUMBER)
- `GST_HSNDET` - HSN (Harmonized System of Nomenclature) product codes (FK → GST_MAIN, CASCADE DELETE)
- `GST_SERVDET` - SAC (Services Accounting Code) service codes (FK → GST_MAIN, CASCADE DELETE)
- `GST_STATEREGDET` - State-wise GST registration details (FK → GST_MAIN, CASCADE DELETE)

### 2. **20260317000000_InitialCreate.Designer.cs**
EF Core internal metadata file - **DO NOT EDIT MANUALLY**

### 3. **GstDbContextModelSnapshot.cs**
Current model snapshot used for future migrations - **DO NOT EDIT MANUALLY**

---

## Database Schema Overview

```
GST_SUPPLIER (Reference)
├── SUPPLIER_NUMBER (PK, BIGINT IDENTITY)
├── SUPPLIER_NAME (NVARCHAR(200), NOT NULL)
├── EMAIL_ADDRESS (NVARCHAR(50))
├── OU (NVARCHAR(200))
└── PAN_NO (NVARCHAR(MAX))

GST_MAIN (Root Aggregate)
├── GST_ID (PK, BIGINT IDENTITY)
├── GST_PANNO (NVARCHAR(20), UNIQUE, NOT NULL)
├── GST_TYPE (NVARCHAR(1)) - R/C/U/N
├── GST_STATUS (NVARCHAR(1)) - P/A/I/S
├── GST_EMAILID (NVARCHAR(200))
├── GST_MOBILENO (NVARCHAR(MAX))
├── GST_CREATEDON (DATETIME2, DEFAULT: GETUTCDATE())
├── GST_MODIFIEDON (DATETIME2)
├── GST_REGISTRATIONTYPE (INT) - 1-9 enum values
├── GST_VENDORNAMEFLAG through GST_SCREENTYPE (vendor & contact info)
└─── Navigation: HsnDetails (→ GST_HSNDET), ServiceDetails (→ GST_SERVDET), StateRegDetails (→ GST_STATEREGDET)

GST_HSNDET (HSN Product Codes)
├── GSTHSN_ID (PK, BIGINT IDENTITY)
├── GSTHSN_GSTID (FK → GST_MAIN.GST_ID, CASCADE DELETE)
├── GSTHSN_PRODUCTNAME (NVARCHAR(100))
├── GSTHSN_HSNCODE (NVARCHAR(50))
└── GSTHSN_REMARKS (NVARCHAR(200))

GST_SERVDET (SAC Service Codes)
├── GSTSAC_ID (PK, BIGINT IDENTITY)
├── GSTSAC_GSTID (FK → GST_MAIN.GST_ID, CASCADE DELETE)
├── GSTSAC_SERVICENAME (NVARCHAR(100))
├── GSTSAC_SACCODE (NVARCHAR(50))
└── GSTSAC_REMARKS (NVARCHAR(200))

GST_STATEREGDET (State Registrations)
├── GST_TINID (PK, BIGINT IDENTITY)
├── GST_ID (FK → GST_MAIN.GST_ID, CASCADE DELETE)
├── GST_STATE (NVARCHAR(20))
├── GST_GSTINNO (NVARCHAR(50)) - GSTIN per state
├── GST_ARNNO (NVARCHAR(50)) - ARN number
├── GST_CONTACTPERSON through GST_MOBILENO (contact & registration details)
└── GST_REMARKS (NVARCHAR(200))
```

---

## Application Methods

### Option 1: Automatic Migration (Recommended for Development)
The application automatically applies migrations on startup via `DatabaseSeeder.MigrateAsync()` in `Program.cs`:

```csharp
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GstDbContext>();
    await db.Database.MigrateAsync();
}
```

**Steps:**
1. Ensure database exists: `CREATE DATABASE SCIDB;`
2. Set connection string in `appsettings.json` (default: `(localdb)\MSSQLLocalDB`)
3. Run the API: `dotnet run --project src/GSTComplianceService.API/GSTComplianceService.API.csproj`
4. The migration applies automatically, seed data is inserted by `DatabaseSeeder.SeedAsync()`

### Option 2: Manual SQL Script Application
Apply the idempotent SQL migration script:

**Steps:**
1. Create database:
   ```sql
   CREATE DATABASE SCIDB;
   GO
   USE SCIDB;
   ```

2. Execute migration script:
   ```powershell
   sqlcmd -S (localdb)\MSSQLLocalDB -d SCIDB -i migrations.sql
   ```
   
   Or in SQL Server Management Studio:
   - Connect to `(localdb)\MSSQLLocalDB`
   - Open `migrations.sql`
   - Press F5 to execute

3. Apply seed data (optional):
   ```powershell
   sqlcmd -S (localdb)\MSSQLLocalDB -d SCIDB -i Seed.sql
   ```

### Option 3: Entity Framework CLI (Development/Ci)
Update database via EF CLI:

```powershell
# From solution root
dotnet ef database update --project src/GSTComplianceService.Infrastructure --startup-project src/GSTComplianceService.API
```

---

## Migration Features

### Idempotency
Both the manual SQL script (`migrations.sql`) and EF migrations use idempotent checks:
- `IF NOT EXISTS` clauses prevent errors if tables already exist
- Safe to run multiple times
- Migration history tracked in `__EFMigrationsHistory` table

### Foreign Key Constraints
All child tables use **CASCADE DELETE**:
- Deleting a GST registration automatically deletes all HSN, SAC, and State registration details
- Maintains referential integrity

### Default Values
- `GST_CREATEDON`: Defaults to current UTC time (`GETUTCDATE()`)
- `GST_MODIFIEDON`: Set to NULL, updated by application logic

### Unique Constraints
- `GST_PANNO` has a unique constraint to prevent duplicate PAN entries
- Enforced at database level: `CONSTRAINT [AK_GST_MAIN_GST_PANNO] UNIQUE ([GST_PANNO])`

### Indexes
Indexes created on foreign keys for performance:
- `IX_GST_HSNDET_GSTHSN_GSTID` on GST_HSNDET
- `IX_GST_SERVDET_GSTSAC_GSTID` on GST_SERVDET
- `IX_GST_STATEREGDET_GST_ID` on GST_STATEREGDET

---

## Seed Data

### Supplied in Seed.sql
The `Seed.sql` script includes:

1. **3 Supplier Records**
   - TCS, Infosys, Wipro with email and PAN information

2. **3 Sample GST Registrations**
   - **Pending (P)**: AAACG5055K - Initial submission state
   - **Active (A)**: ACACD5055K - Fully approved and operational
   - **Inactive (I)**: AXCDE5055K - Deactivated/closed

3. **HSN Details** (Product Categories)
   - Electronic Data Processing (HSN 8471)
   - Electrical Machinery (HSN 8504)
   - Optical Instruments (HSN 9015)

4. **SAC Details** (Service Categories)
   - Hostel management (SAC 998311)
   - Event management (SAC 998361)
   - Other professional services (SAC 998369)

5. **State Registration Details**
   - Maharashtra: Head office with GSTIN 27ACACD5055K1ZA
   - Karnataka: Branch office with GSTIN 29ACACD5055K2ZA

### Applying Seed Data
Option 1 - Automatic application:
- Seed data runs automatically via `DatabaseSeeder.SeedAsync()` when database is first created

Option 2 - Manual SQL:
```powershell
sqlcmd -S (localdb)\MSSQLLocalDB -d SCIDB -i Seed.sql
```

---

## Connection String Configuration

### Development (Default - Uses LocalDB)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SCIDB;Integrated Security=True;TrustServerCertificate=True"
  }
}
```

### Production (SQL Server)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server.database.windows.net;Database=SCIDB;User Id=sa;Password=YourPassword;Encrypt=True;TrustServerCertificate=False"
  }
}
```

### Docker/Containers
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=mssql-container;Database=SCIDB;User Id=sa;Password=YourPassword;Encrypt=False"
  }
}
```

---

## Troubleshooting

### Migration Not Applied
**Symptom:** `DbUpdateException: The database does not exist` or table not found
**Fix:**
```powershell
# Ensure database exists
sqlcmd -S (localdb)\MSSQLLocalDB -Q "CREATE DATABASE SCIDB;"

# Then apply migration via any method above
```

### Duplicate Key Error During Seed
**Symptom:** `Violation of PRIMARY KEY constraint` or `Violation of UNIQUE constraint`
**Fix:**
- Clear existing data: `TRUNCATE TABLE GST_HSNDET; DELETE FROM GST_MAIN; DELETE FROM GST_SUPPLIER;`
- Re-run seed script

### Foreign Key Constraint Violation
**Symptom:** Cannot delete GST registration due to child records
**Reason:** Cascade delete may not be working if manually inserted data
**Fix:** Drop and recreate tables or use:
```sql
ALTER TABLE GST_HSNDET DROP CONSTRAINT FK_GST_HSNDET_GST_MAIN_GSTHSN_GSTID;
ALTER TABLE GST_HSNDET ADD CONSTRAINT FK_GST_HSNDET_GST_MAIN_GSTHSN_GSTID 
    FOREIGN KEY (GSTHSN_GSTID) REFERENCES GST_MAIN(GST_ID) ON DELETE CASCADE;
```

### EF Tool Version Mismatch
**Symptom:** `Method 'Identifier' not found` or version errors
**Fix:**
```powershell
dotnet tool uninstall -g dotnet-ef
dotnet tool install -g dotnet-ef --version 10.0.5
```

---

## Creating New Migrations

After modifying entity models:

```powershell
cd src/GSTComplianceService

# Create new migration (only for development, use manual method for schema changes)
dotnet ef migrations add AddNewFeature `
  --project ../GSTComplianceService.Infrastructure `
  --startup-project ../GSTComplianceService.API

# Generate idempotent SQL (if EF CLI works)
dotnet ef migrations script --idempotent `
  --project ../GSTComplianceService.Infrastructure `
  --startup-project ../GSTComplianceService.API `
  --output new-migration.sql
```

For production, always generate and review the SQL script before applying.

---

## Verification Checklist

After migration:
- [ ] Database `SCIDB` exists
- [ ] 5 tables created: GST_MAIN, GST_SUPPLIER, GST_HSNDET, GST_SERVDET, GST_STATEREGDET
- [ ] Foreign key relationships established with CASCADE DELETE
- [ ] Unique constraint on GST_PANNO exists
- [ ] `__EFMigrationsHistory` contains migration record
- [ ] Seed data (3 suppliers, 3 GST registrations) imported if using Seed.sql
- [ ] API can connect and perform CRUD operations

---

## Related Files

- **Migration Files:** `/Migrations/*.*`
- **DbContext:** `Persistence/GstDbContext.cs`
- **Entity Configurations:** `Persistence/Configurations/EntityConfigurations.cs`
- **Database Seeder:** `Persistence/Seed/DatabaseSeeder.cs`
- **Migration Script:** `migrations.sql` (root of solution)
- **Seed Script:** `Seed.sql` (root of solution)

---

## References

- [Entity Framework Core Migrations Documentation](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [Code-First Approach](https://learn.microsoft.com/en-us/ef/ef6/modeling/code-first/)
- [SQL Server Constraints](https://learn.microsoft.com/en-us/sql/relational-databases/tables/primary-and-foreign-key-constraints)
