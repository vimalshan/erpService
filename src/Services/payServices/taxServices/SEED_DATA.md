# Seed Data & Database Management

## Overview

The Tax Service includes both programmatic and SQL-based seed data options to initialize your database with sample records.

## Automatic Seeding (On Startup)

When you run the API, it automatically seeds the database with sample data on first run:

### What Gets Seeded

**3 Sample Payees (ConditionalMasters)**:
- PAY001: ABC Corporation Ltd. (Old Tax Regime)
- PAY002: XYZ Industries Pvt. Ltd. (New Tax Regime)  
- PAY003: Global Tech Solutions Pvt. Ltd. (Old Tax Regime)

**4 Sample Employee Tax Records (TaxMarginalDetails)**:
- EMP001: Gross Income ₹900,000
- EMP002: Gross Income ₹1,500,000
- EMP003: Gross Income ₹500,000
- EMP004: Gross Income ₹2,000,000

All records are auto-calculated with Indian tax slabs and include a 4% cess.

### How It Works

The seeding runs in [Program.cs](../src/TaxService.API/Program.cs) before the app starts:

```csharp
// Seed database with initial data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TaxServiceDbContext>();
    await TaxServiceDbContextSeed.SeedAsync(dbContext);
}
```

The seeding logic is in [TaxServiceDbContextSeed.cs](../src/TaxService.Infrastructure/Data/TaxServiceDbContextSeed.cs).

### Important Notes

- **Idempotent**: Seeding only runs if the database is empty (checks `ConditionalMasters` table)
- **Safe**: Won't overwrite existing data
- **Non-blocking**: Runs once at startup, then never again for that database

## Manual SQL Seeding

For production deployments or manual database initialization, use the SQL scripts:

### Seed Sample Data

```bash
sqlcmd -S (localdb)\MSSQLLocalDB -d TaxService -i SQL\Seed_Data.sql
```

Or in SQL Management Studio:
```sql
USE TaxService;
GO
-- Copy contents of SQL\Seed_Data.sql and execute
```

The SQL script includes:
- 3 payees with complete details
- 4 employee tax records with calculated taxes
- All inserts are wrapped in `IF NOT EXISTS` checks to prevent duplicates

### Reset Database

To delete all data and start fresh:

```bash
sqlcmd -S (localdb)\MSSQLLocalDB -d TaxService -i SQL\Reset_Database.sql
```

Or in SQL Management Studio:
```sql
DELETE FROM [dbo].[TaxMarginalDetails];
DELETE FROM [dbo].[ConditionalMasters];
DBCC CHECKIDENT ('dbo.TaxMarginalDetails', RESEED, 0);
DBCC CHECKIDENT ('dbo.ConditionalMasters', RESEED, 0);
```

## Tax Calculation Formula

The seeded records use this tax calculation:

### Old Regime (India FY 2024-25)

| Income Slab | Tax Rate |
|-----|------|
| Up to ₹2,50,000 | 0% |
| ₹2,50,000 - ₹5,00,000 | 5% |
| ₹5,00,000 - ₹10,00,000 | 20% |
| Above ₹10,00,000 | 30% |

**Formula:**
```
Taxable Income = Gross Income - Standard Deduction (₹50,000)
Tax = Slab Tax Calculation
Gross Tax = Tax + (Tax × 4%) [Health and Education Cess]
```

### Example

Employee with ₹900,000 gross income:
- Taxable Income = 900,000 - 50,000 = ₹850,000
- Tax = (500,000 × 5%) + (350,000 × 20%) = 25,000 + 70,000 = ₹95,000  
- Total = 95,000 + (95,000 × 4%) = ₹98,800 (before rounding)

## Customizing Seed Data

### Add More Employees

Edit [TaxServiceDbContextSeed.cs](../src/TaxService.Infrastructure/Data/TaxServiceDbContextSeed.cs):

```csharp
private static List<TaxMarginalDetail> CreateSampleTaxDetails()
{
    var taxDetails = new List<TaxMarginalDetail>();
    var taxRates = GetSampleTaxRates();
    
    // Add new employee
    var employee = TaxMarginalDetail.Create(
        employeeSystemId: 1005L,  // Change ID
        financialYear: 2025,
        grossIncome: new Money(1200000, "INR"),  // Change income
        standardDeduction: new Money(50000, "INR"),
        createdBy: "admin"
    );
    employee.CalculateTax(taxRates);
    taxDetails.Add(employee);
    
    return taxDetails;
}
```

### Add Exemptions/Deductions

After seeding payees, you can add exemptions/deductions via the API:

```bash
# Add exemption to payee
curl -X POST "https://localhost:5001/api/conditionalmasters/exemption" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "conditionalMasterId": 1,
    "code": "EX_CUSTOM",
    "description": "Custom Exemption",
    "amount": 100000
  }'
```

## Verify Seeding

### Check via API

```bash
# Get all payees
curl -X GET "https://localhost:5001/api/conditionalmasters/active" \
  -H "Authorization: Bearer YOUR_TOKEN"

# Get all employee records
curl -X GET "https://localhost:5001/api/taxmarginaldetails/active" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### Check via SQL

```sql
USE TaxService;

-- Count payees
SELECT COUNT(*) as PayeeCount FROM ConditionalMasters;

-- Count employee tax records
SELECT COUNT(*) as EmployeeCount FROM TaxMarginalDetails;

-- View data
SELECT * FROM ConditionalMasters;
SELECT * FROM TaxMarginalDetails;
```

## Troubleshooting

### "Database already seeded. Skipping seed data..."

This message means the database already has data. This is normal and safe. The seeding is idempotent - it only runs on empty databases.

**To reseed:**
```bash
cd src/TaxService.Infrastructure
dotnet ef database drop --force -s ../TaxService.API/TaxService.API.csproj
dotnet ef database update -s ../TaxService.API/T axService.API.csproj
# Database recreated - run app to seed
```

### Seed data not appearing

1. Check connection string in [appsettings.json](../src/TaxService.API/appsettings.json)
2. Ensure Initial Catalog includes database name: `Initial Catalog=TaxService`
3. Verify database was created: `SELECT * FROM sys.databases WHERE name = 'TaxService'`
4. Check application logs for seed errors

### Identity seed errors

If you get "Cannot insert explicit value for identity column", ensure your database is empty or drop and recreate it.

## Best Practices

✅ **DO**:
- Use automatic seeding for development
- Use SQL scripts for production/documented migrations
- Keep seed data in version control
- Document any custom tax calculations

❌ **DON'T**:
- Modify seed data without updating documentation
- Use hardcoded IDs for new records
- Mix manual and automatic seeding without testing
- Skip database backups before seeding production

## Next Steps

1. Run the API to auto-seed: `dotnet run`
2. Test endpoints with seeded data
3. Add more employees/payees as needed via:
   - API endpoints
   - Direct SQL inserts
   - Seed data updates
4. Implement event handlers for seed state changes
