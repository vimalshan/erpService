# Running the FinyearAPI

## Quick Start

### 1. Open Terminal/PowerShell
```powershell
cd e:\ERPMicroservice\src\Services\adminServices\finyearServices\src\FinyearAPI
```

### 2. Build the Project
```bash
dotnet build
```

### 3. Setup Database
```bash
dotnet ef database update
```

### 4. Run the Application
```bash
dotnet run
```

### 5. Access API
Open browser: **https://localhost:5001/swagger**

## Entity Framework Core CLI Commands

### View migrations
```bash
dotnet ef migrations list
```

### Create new migration
```bash
dotnet ef migrations add MigrationName
```

### Apply migrations
```bash
dotnet ef database update
```

### Drop database
```bash
dotnet ef database drop --force
```

## SQL Deployment (Alternative)

Run SQL scripts directly in SQL Server Management Studio:

1. **FINYEAR-Migration.sql** - Creates database and tables
2. **FINYEAR-Procedures.sql** - Creates stored procedures
3. **FINYEAR-SampleData.sql** - Inserts sample data

## Debugging

### Enable SQL Logging
In `Program.cs`, adjust logging level:
```csharp
.LogTo(Console.WriteLine, LogLevel.Information)
```

### View Generated SQL
Use SQL Profiler or check console output:
```
SELECT [f].[FY_ID], [f].[FY_CLOSEDATE], ...
FROM [FINYEAR_MASTER] AS [f]
```

## Common Issues

| Issue | Solution |
|-------|----------|
| Port 5001 in use | `dotnet run --urls https://localhost:5002` |
| Database not found | Run `dotnet ef database update` |
| DLL conflicts | `dotnet clean && dotnet build` |
| Migration errors | `dotnet ef migrations remove` then re-add |

## Understanding the Architecture

```
Request → Controller → Service → UnitOfWork → Repository → DbContext → Database
                                           ↓
                                    Dapper (Optional)
```

### Each Layer:
- **Controller**: Handles HTTP requests/responses
- **Service**: Business logic and validation
- **UnitOfWork**: Transaction management
- **Repository**: Data access abstraction
- **DbContext**: Entity Framework mapping
- **Dapper**: Direct SQL execution (performance)

## Connection String Explained

```
Data Source=(localdb)\MSSQLLocalDB          // LocalDB instance
Integrated Security=True                     // Windows authentication
Persist Security Info=False                  // Don't include password in connection
Pooling=False                               // Connection pooling disabled
MultipleActiveResultSets=False              // MARS disabled
Encrypt=True                                // Encrypt connection
TrustServerCertificate=False                // Validate certificate
Application Name="FinyearAPI"               // Application identifier
Command Timeout=0                           // No timeout
```

## Testing the API

### Using Curl
```bash
# Get all financial years
curl https://localhost:5001/api/financialyear

# Get current financial year
curl https://localhost:5001/api/financialyear/current

# Create new financial year
curl -X POST https://localhost:5001/api/financialyear \
  -H "Content-Type: application/json" \
  -d '{
    "financialYearId": 1,
    "financialYearName": "2024-2025",
    "startDate": "2024-04-01T00:00:00Z",
    "closeDate": "2025-03-31T23:59:59Z",
    "updatedBy": 1
  }'
```

### Using Postman
1. Import collection from Swagger: `https://localhost:5001/swagger/v1/swagger.json`
2. Set base URL: `https://localhost:5001`
3. Test each endpoint

## Next Steps

1. **Add Authentication**: Implement JWT or Azure AD
2. **Add Logging**: ELK stack or Application Insights
3. **Add Validation**: FluentValidation for complex rules
4. **Add Caching**: Redis for frequently accessed data
5. **Add Testing**: Unit tests and integration tests
6. **Add Documentation**: XML comments and API docs

## Support
Refer to README.md for detailed documentation.
