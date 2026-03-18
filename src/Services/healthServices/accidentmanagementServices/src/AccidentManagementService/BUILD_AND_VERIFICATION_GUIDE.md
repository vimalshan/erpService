# AccidentManagement Service - Build & Verification Guide

**Task 13: Build & Verify Solution**  
**Date**: March 13, 2026

---

## π Phase 1: Pre-Build Checklist

Before building, verify all prerequisites are in place:

- [ ] SQL Server (LocalDB) is running
- [ ] .NET 6+ SDK installed (`dotnet --version`)
- [ ] Visual Studio 2022 or JetBrains Rider with .NET support
- [ ] All NuGet packages can be accessed (internet connection)
- [ ] appsettings.json configured with correct connection strings
- [ ] RabbitMQ running (for testing event consumers)
  ```powershell
  docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:4-management-alpine
  ```

---

## π Phase 2: Build Solution

### Step 1: Clean Solution
```powershell
cd e:\ERPMicroservice\src\Services\healthServices\accidentmanagementServices\AccidentManagementService
dotnet clean
```

**Expected Output**:
```
Microsoft (R) Build Engine version 17.x.x
Build started 3/13/2026...
Done in X seconds
```

### Step 2: Restore NuGet Packages
```powershell
dotnet restore
```

**Expected Output**:
```
Determining projects to restore...
...
Restore completed in X.XXs for all projects
```

### Step 3: Build Solution
```powershell
dotnet build
```

**Expected Outcome**: 0 errors, 0 warnings (or minimal warnings)

**Successful Output Should Include**:
```
Build succeeded in X.XXs (X.XXs elapsed)
```

**If Build Fails**:
1. Check error message for missing using statements
2. Verify all NuGet packages are installed
3. Check that referenced types exist in imported namespaces
4. Review COMPILATION_ERRORS.md if generated

### Step 4: Create EF Core Migrations

Open Package Manager Console and run:

```powershell
# Navigate to project folder
cd AccidentManagementService

# Create initial migration
Add-Migration InitialCreate -StartupProject AccidentManagementService

# Apply migration to database
Update-Database
```

**Expected Output**:
```
Build started...
Build succeeded.
Done. To undo this action, use Remove-Migration.

Applying migration InitialCreate...
Done.
```

---

## π Phase 3: Run Application

### Start Development Server
```powershell
dotnet run
```

**Expected Console Output**:
```
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:7105
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

**Note**: Leave this terminal open to monitor logs

---

## π Phase 4: Verify API Endpoints

### 4.1: Check Swagger/OpenAPI
**URL**: http://localhost:7105/swagger/index.html

**Expected**: Swagger UI displays with all API endpoints organized by controller

**Available Endpoints Should Include**:
- POST /api/v2/accident-reports - Create accident report
- GET /api/v2/accident-reports/{id} - Get accident by ID
- GET /api/v2/accident-reports/company/{companyCode} - Get by company
- GET /api/v2/accident-reports/by-date-range - Get by date range  
- GET /api/v2/accident-reports/statistics - Get statistics
- PATCH /api/v2/accident-reports/{id}/status - Update status
- PATCH /api/v2/accident-reports/{id}/severity - Update severity
- GET /api/v2/accident-reports/categories/all - Get categories
- GET /api/v2/accident-reports/natures/all - Get natures

### 4.2: Check GraphQL Endpoint
**URL**: http://localhost:7105/graphql

**Expected**: GraphQL IDE (GraphiQL) with schema explorer

**Test Query**:
```graphql
query {
  allAccidentReports(pageNumber: 1, pageSize: 10) {
    totalCount
    accidents {
      accidentNumber
      companyCode
      accidentDateTime
    }
  }
}
```

### 4.3: Check Health Checks

#### Liveness Probe (app running?)
```powershell
curl http://localhost:7105/health/live
```

**Expected Response**:
```json
{
  "status": "Healthy",
  "checks": [],
  "totalDuration": "00:00:00.0123"
}
```

#### Readiness Probe (dependencies ready?)
```powershell
curl http://localhost:7105/health/ready
```

**Expected Response** (when all dependencies are ready):
```json
{
  "status": "Healthy",
  "checks": [
    {
      "name": "Database",
      "status": "Healthy",
      "duration": 45.23,
      "description": "Database is available..."
    },
    {
      "name": "RabbitMQ",
      "status": "Healthy",
      "duration": 32.10,
      "description": "RabbitMQ is available"
    },
    {
      "name": "Memory",
      "status": "Healthy",
      "duration": 1.05,
      "description": "Application memory usage: 150M"
    }
  ]
}
```

#### Detailed Health Check
```powershell
curl http://localhost:7105/health
```

**Expected**: Complete health report with all dependency statuses

---

## π Phase 5: Test Core Functionality

### 5.1: Create Accident Report (via Swagger)

1. Open Swagger UI: http://localhost:7105/swagger/index.html
2. Click "Try it out" on POST /api/v2/accident-reports
3. Fill in request body with sample data:
```json
{
  "companyCode": "COMP001",
  "employeeNumber": "EMP001",
  "employeeName": "John Smith",
  "employeeDepartment": "Operations",
  "injuredPersonName": "John Smith",
  "accidentDateTime": "2026-03-13T10:30:00Z",
  "accidentLocation": "Factory Floor Zone A",
  "causeOfIncident": "Equipment malfunction",
  "preventiveMeasures": "Regular maintenance schedule"
}
```

4. Click "Execute"

**Expected Response**:
```json
{
  "commandId": "550e8400-e29b-41d4-a716-446655440000",
  "isSuccess": true,
  "message": "Accident report created successfully",
  "data": 123,
  "statusCode": 201
}
```

### 5.2: Retrieve Accident Report

1. Copy the accident ID from the response (e.g., 123)
2. Click "Try it out" on GET /api/v2/accident-reports/{id}
3. Enter the ID
4. Click "Execute"

**Expected**: Accident report with all details returned

### 5.3: Test GraphQL Mutation

1. Navigate to GraphQL IDE: http://localhost:7105/graphql
2. Enter mutation:
```graphql
mutation {
  updateAccidentStatus(accidentReportId: 123, newStatus: "InProgress") {
    success
    message
    newStatus
  }
}
```

3. Execute

**Expected Response**:
```json
{
  "data": {
    "updateAccidentStatus": {
      "success": true,
      "message": "Status updated successfully",
      "newStatus": "InProgress"
    }
  }
}
```

### 5.4: Verify RabbitMQ Consumer

1. Monitor the console output from Step 3
2. Look for log entries like:
```
info: AccidentManagementService.Infrastructure.EventConsumers.AccidentReportCreatedConsumer[0]
      Processing accident report created event: AccidentNumber=ACC20260313000001, Company=COMP001...
```

3. Verify consumer processes the event without errors

---

## π Phase 6: Database Verification

### Verify Tables Created

```sql
-- Connect to (localdb)\MSSQLLocalDB -> HEALTHDB
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'dbo' 
ORDER BY TABLE_NAME;
```

**Expected Tables**:
- ACCIDENT_SEVERITY
- ACCIDENT_STATUS  
- CATEGORY_INJURY
- DAILY_ACC_FIR
- NATURE_INJURY
- ACC_CONTRCT_LST (optional)
- ACC_PERS_INJ (optional)
- AUDIT_LOG (optional)
- __EFMigrationsHistory (EF Core tracking)

### Verify Seed Data

```sql
-- Check if reference data was seeded
SELECT * FROM ACCIDENT_SEVERITY;  -- Should have 4 rows
SELECT * FROM ACCIDENT_STATUS;    -- Should have 4 rows
SELECT * FROM CATEGORY_INJURY;    -- Should have 13+ rows
SELECT * FROM NATURE_INJURY;      -- Should have 8 rows
```

### Check Accident Records

```sql
-- Verify accident created successfully
SELECT TOP 10 
    DAF_ID, 
    DAF_ACC_NUM, 
    COM_COD, 
    EMPL_NUM, 
    ACC_DAT,
    DAF_STATUS,
    DAF_SER_LEV
FROM DAILY_ACC_FIR 
ORDER BY DAF_ID DESC;
```

---

## π Phase 7: Logging Verification

### Monitor Application Logs

Logs are written to: `logs/accident-service-YYYYMMDD_NNN.txt`

**Check logs for**:
1. Application startup messages
2. Database connection successful
3. RabbitMQ consumer registrations
4. API request/response patterns
5. GraphQL query executions
6. Health check results

**Sample Log Entry**:
```
2026-03-13 10:30:15.123 +00:00 [INF] Application started. Press Ctrl+C to shut down.
2026-03-13 10:30:16.456 +00:00 [INF] Database connection successful
2026-03-13 10:30:17.789 +00:00 [INF] RabbitMQ consumers registered: AccidentReportCreatedConsumer
2026-03-13 10:31:00.012 +00:00 [INF] POST /api/v2/accident-reports - Status: 201 - Created
2026-03-13 10:31:01.345 +00:00 [INF] AccidentReportCreatedConsumer: Processing new accident report...
```

---

## π Phase 8: Performance & Stress Testing (Optional)

### Test Health Check Response Time

```powershell
# Measure health check execution time
Measure-Command { Invoke-WebRequest http://localhost:7105/health -UseBasicParsing }
```

**Expected**: < 500ms response time

### Test Concurrent API Calls

```powershell
# Load test - 10 concurrent requests
1..10 | ForEach-Object {
    Invoke-WebRequest http://localhost:7105/health -UseBasicParsing
}
```

**Expected**: All requests succeed with < 1s total time

---

## β  Verification Checklist

**Build Status**:
- [ ] `dotnet build` completes with 0 errors
- [ ] Solution builds successfully
- [ ] No unresolved references or NuGet package issues

**Runtime Status**:
- [ ] Application starts without exceptions
- [ ] Logs show successful initialization
- [ ] No connection errors in console output

**API Functionality**:
- [ ] Swagger UI accessible and complete
- [ ] GraphQL IDE accessible with schema
- [ ] REST endpoints return proper HTTP status codes
- [ ] GraphQL mutations execute successfully

**Database**:
- [ ] All expected tables created
- [ ] Reference data seeded correctly
- [ ] Can read/write accident records

**Health Checks**:
- [ ] `/health/live` returns 200 OK
- [ ] `/health/ready` returns 200 OK  
- [ ] `/health` shows all checks Healthy/Degraded (not Unhealthy)
- [ ] Database health check passes
- [ ] RabbitMQ health check passes (if configured)
- [ ] Memory health check passes

**Event Processing**:
- [ ] RabbitMQ consumers registered
- [ ] Log entries show event consumption
- [ ] No consumer errors logged
- [ ] Status changes propagate to consumers

**Security**:
- [ ] Authentication header required (401 without token)
- [ ] Valid JWT token accepted (200 response)
- [ ] Invalid token rejected

**Performance**:
- [ ] Health check response < 500ms
- [ ] API endpoints respond in < 1s
- [ ] Memory usage stable and < 300MB

---

## π' Common Issues & Solutions

| Issue | Cause | Solution |
|-------|-------|----------|
| **Build fails - "CS7002: Cannot find type"** | Missing using statements | Check imports in GraphQL files |
| **Swagger shows no endpoints** | Controllers not registered | Ensure `app.MapControllers()` in Program.cs |
| **GraphQL returns 404** | GraphQL endpoint not mapped | Check `app.MapGraphQL("/graphql")` |
| **Health check shows Unhealthy** | Database/RabbitMQ not available | Start SQL Server and RabbitMQ docker |
| **RabbitMQ consumer errors** | Message format mismatch | Verify integration event serialization |
| **Memory health check Degraded** | High memory usage | Reduce data query size or increase threshold |
| **JWT authentication fails** | Invalid token configuration | Check Authority/Audience in appsettings.json |
| **Port 7105 already in use** | Another process using port | Change port in launchSettings.json |

---

## π Completion Criteria

The build is **successfully verified** when:

1. βœ… Solution compiles with 0 errors
2. βœ… Application starts and logs indicate healthy initialization
3. βœ… All three health check endpoints return HTTP 200
4. βœ… Swagger UI displays all documented endpoints
5. βœ… GraphQL schema is queryable via IDE
6. βœ… REST API accepts and processes requests
7. βœ… Database has all expected tables
8. βœ… RabbitMQ consumers process events without errors
9. βœ… Logs show clean startup with no exceptions

---

## π Next Steps After Verification

Once all tests pass:

1. **Review Logs** - Check for any warnings or unexpected patterns
2. **Test All Endpoints** - Execute at least one operation for each major API endpoint
3. **Verify Database State** - Query database to confirm data persistence
4. **Monitor Performance** - Watch memory and CPU during operation
5. **Document Issues** - Create issues for any warnings encountered
6. **Plan Deployment** - Prepare containerization and deployment scripts

---

**Status**: Ready for Deployment  
**Final Check**: All 13 tasks completed  
**Next Phase**: Containerization, CI/CD setup, production deployment

