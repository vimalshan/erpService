# EmployeeService Testing Summary

## Overview
EmployeeService.API has been successfully started and configured with three main integration points:

1. **REST API** - CRUD operations via HTTP/HTTPS
2. **GraphQL** - Query and mutation operations
3. **RabbitMQ** - Asynchronous event messaging

---

## API Service Details

| Component | Value |
|-----------|-------|
| **Service Name** | EmployeeService.API |
| **HTTP Port** | 5049 |
| **HTTPS Port** | 7102 |
| **Database** | LocalDB - EmployeeServiceDB |
| **GraphQL Endpoint** | /graphql |
| **Swagger Docs** | /swagger |
| **Launch Profile** | Development |

---

## 1. REST API Endpoints

### Base URLs
- **HTTP**: `http://localhost:5049/api/`
- **HTTPS**: `https://localhost:7102/api/`

### Authentication Controller (`/api/auth`)
- `POST /token` - Generate JWT token (public, no auth required)
- `POST /validate` - Validate JWT token (public, no auth required)

### Employees Controller (`/api/employees`)

#### Queries (GET)
- `GET /` - Get employee by ID (`/api/employees/{id}`)
- `GET /number/{employeeNumber}` - Get by employee number
- `GET /active` - Get all active employees
- `GET /unit/{unitId}` - Get employees by unit
- `GET /grade/{gradeCode}` - Get employees by grade
- `GET /search?searchTerm={term}` - Search employees
- `GET /statistics` - Get employee statistics
- `GET /{id}/details` - Get employee with full details

#### Commands (POST/PUT/DELETE)
- `POST /` - Create new employee
- `PUT /{id}/personal-info` - Update personal information
- `PUT /{id}/contact` - Update contact details
- `PUT /{id}/salary` - Update salary
- `PUT /{id}/promote` - Promote employee
- `PUT /{id}/terminate` - Terminate employee
- `PUT /{id}/reactivate` - Reactivate employee
- `DELETE /{id}` - Delete employee (soft delete)

### Authorization Requirements
- **Admin** role: Full access to all endpoints
- **HR** role: Access to most operations except sensitive finance
- **Manager** role: Read-only access to team members
- **Employee** role: Limited access (own data only)

---

## 2. GraphQL Endpoints

### Base URL: `http://localhost:5049/graphql`

### Available Queries
```graphql
{
  # Get employee by ID
  getEmployee(id: Long): EmployeeDto
  
  # Get employee by number
  getEmployeeByNumber(employeeNumber: String): EmployeeDto
  
  # Get all active employees
  getEmployees: [EmployeeDto]
  
  # Search employees
  searchEmployees(searchTerm: String): [EmployeeDto]
  
  # Get employees by unit
  getEmployeesByUnit(unitId: Long): [EmployeeDto]
  
  # Get employees by grade
  getEmployeesByGrade(gradeCode: String): [EmployeeDto]
  
  # Get total employee count
  getEmployeeCount: Int
  
  # Get statistics
  getStatistics: EmployeeStatisticsDto
}
```

### Available Mutations
```graphql
{
  # Create employee
  createEmployee(input: CreateEmployeeInput): CreateEmployeeResponse
  
  # Update salary
  updateEmployeeSalary(employeeId: Long, input: UpdateEmployeeSalaryInput): BaseResponse
  
  # Promote employee
  promoteEmployee(employeeId: Long, input: PromoteEmployeeInput): BaseResponse
  
  # Terminate employee
  terminateEmployee(employeeId: Long, input: TerminateEmployeeInput): BaseResponse
}
```

---

## 3. RabbitMQ Configuration

### Connection Details
| Parameter | Value |
|-----------|-------|
| **Host** | localhost |
| **Port** | 5672 |
| **Username** | guest |
| **Password** | guest |
| **Virtual Host** | / |
| **Queue Name** | employee.events |

### Management UI
- **URL**: `http://localhost:15672`
- **Login**: guest / guest
- **Location**: Queues tab → employee.events

### Events Published
The following employee operations trigger RabbitMQ messages:

1. **EmployeeCreated** - When a new employee is added
2. **EmployeeUpdated** - When employee details are modified
3. **EmployeePromoted** - When promotion occurs
4. **EmployeeTerminated** - When employment ends
5. **EmployeeReactivated** - When terminated employee is reactivated
6. **EmployeeDeleted** - When employee is deleted

### Circuit Breaker Configuration
| Setting | Value |
|---------|-------|
| **Retry Count** | 2 |
| **Retry Delay** | 2 seconds |
| **Circuit Break Duration** | 30 seconds |
| **Min Throughput** | 2 requests |
| **Failure Ratio** | 0.5 (50%) |

---

## 4. Quick Test Examples

### Generate JWT Token (PowerShell)
```powershell
$response = Invoke-WebRequest -Uri "http://localhost:5049/api/auth/token?role=Admin" -Method POST
$token = ($response.Content | ConvertFrom-Json).token
$token
```

### Get Active Employees (PowerShell)
```powershell
$headers = @{
  "Authorization" = "Bearer $token"
  "Content-Type" = "application/json"
}

Invoke-RestMethod -Uri "http://localhost:5049/api/employees/active" `
  -Method GET `
  -Headers $headers
```

### Create Employee (PowerShell)
```powershell
$body = @{
  firstName = "John"
  lastName = "Doe"
  email = "john.doe@company.com"
  employeeNumber = "EMP004"
  designation = "Manager"
  basicSalary = 50000
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5049/api/employees" `
  -Method POST `
  -Body $body `
  -Headers $headers
```

### GraphQL Query Example (PowerShell)
```powershell
$query = '{ getEmployees { id firstName lastName email } }'
$body = @{ query = $query } | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5049/graphql" `
  -Method POST `
  -Body $body `
  -Headers @{"Content-Type" = "application/json"}
```

---

## 5. Database Information

### Connection String
```
Data Source=(localdb)\MSSQLLocalDB
Initial Catalog=EmployeeServiceDB
Integrated Security=True
```

### Pre-seeded Employees
The database includes 3 sample employees:
- **EMP001**: Sample Employee 1
- **EMP002**: Sample Employee 2
- **EMP003**: Sample Employee 3

### Tables
- Employees
- ContactInfo
- EmploymentDetails
- GradeInfo
- OrganizationalAssignment
- PersonalInfo
- SalaryInfo

---

## 6. Testing Resources

### Automated Test Scripts
- **Full Test Suite**: `Test-EmployeeService.ps1` (comprehensive with all tests)
- **Simple Test Suite**: `Test-EmployeeService-Simple.ps1` (quick validation)

### Documentation
- **Testing Guide**: `EMPLOYEESERVICE_TESTING_GUIDE.md` (detailed endpoint documentation)
- **This Document**: EmployeeService Testing Summary

### Online Tools
- **Swagger UI**: http://localhost:5049/swagger
- **GraphQL Playground**: http://localhost:5049/graphql
- **RabbitMQ Management**: http://localhost:15672

---

## 7. Common Testing Scenarios

### Scenario 1: Create and Retrieve Employee
1. Generate JWT token
2. Create new employee via REST POST
3. Retrieve employee via GraphQL query
4. Verify message in RabbitMQ queue

### Scenario 2: Update Employee
1. Generate token with HR role
2. Update employee salary via REST PUT
3. Verify update via REST GET
4. Check RabbitMQ for "EmployeeUpdated" message

### Scenario 3: Search and Filter
1. Create multiple test employees
2. Test search functionality
3. Test unit and grade filters
4. Verify results consistency between REST and GraphQL

### Scenario 4: Authorization Testing
1. Generate tokens with different roles (Admin, HR, Manager, Employee)
2. Attempt restricted operations
3. Verify 403 Forbidden for unauthorized access
4. Verify 401 Unauthorized for missing tokens

---

## 8. Troubleshooting

### API Not Responding
**Problem**: Connection refused on port 5049
**Solution**:
- Verify API is running: `ps | Where { $_.ProcessName -eq 'dotnet' }`
- Check logs for startup errors
- Ensure port 5049 is not blocked by firewall

### RabbitMQ Connection Failed
**Problem**: Circuit breaker activated or messages not publishing
**Solution**:
- Verify RabbitMQ service is running
- Check connection settings in appsettings.json
- Access management UI to verify queue exists
- Check API logs for detailed error messages

### GraphQL Query Errors
**Problem**: "Field not found" or "Invalid input" errors
**Solution**:
- Verify query syntax matches schema
- Check field names are case-sensitive
- Use Swagger docs to verify available fields
- Test with simple query first

### Authorization Failures
**Problem**: 401 Unauthorized or 403 Forbidden
**Solution**:
- Regenerate JWT token: use `/api/auth/token`
- Verify token is included in Authorization header
- Check token hasn't expired (60 minute default)
- Verify user role matches endpoint requirements

---

## 9. Performance Notes

### Database Optimization
- Entity Framework Core with locally optimized queries
- Connection pooling configured (MaxPoolSize: varies by system)

### Caching
- No explicit caching configured (ready for addition)
- Database queries are optimized for performance

### RabbitMQ
- Circuit breaker prevents cascading failures
- Automatic retry with exponential backoff
- Messages are persistent in queue

---

## 10. Security Considerations

### JWT Configuration
- **Key**: Should be changed in production (minimum 32 characters)
- **Expiry**: 60 minutes (configurable)
- **Issuer**: EmployeeService
- **Audience**: EmployeeServiceAPI

### HTTPS
- Required for production deployment
- Certificate configuration in launchSettings.json
- Current: Development certificates

### CORS
- AllowAll policy for development
- Specific origins can be restricted in production
- Allowed origins: localhost:3000, localhost:5000

---

## 11. Next Steps

### For Testing
1. Run automated test script: `Test-EmployeeService-Simple.ps1`
2. Access Swagger documentation for interactive testing
3. Monitor RabbitMQ queue for event messaging
4. Test with various user roles

### For Production
1. Update JWT secret key
2. Configure HTTPS with proper certificates
3. Set up proper CORS policies
4. Configure external RabbitMQ instance
5. Set up database backups
6. Enable comprehensive logging
7. Configure health checks and monitoring

---

## 12. Support Commands

```powershell
# Start API (from project directory)
dotnet run --project employeeServices/src/EmployeeService.API

# Run specific test script
.\Test-EmployeeService-Simple.ps1

# Check RabbitMQ messages (via browser)
http://localhost:15672

# Access API documentation
http://localhost:5049/swagger

# Test API health
Invoke-RestMethod http://localhost:5049/health
```

---

**Document Generated**: March 28, 2026  
**Service Version**: 1.0  
**Status**: Operational and Tested

For detailed endpoint documentation, see [EMPLOYEESERVICE_TESTING_GUIDE.md](EMPLOYEESERVICE_TESTING_GUIDE.md)
