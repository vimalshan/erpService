# EmployeeService Quick Reference Card

## Service Access Points

| Component | URL | Notes |
|-----------|-----|-------|
| REST API | `http://localhost:5049/api/` | Requires JWT token |
| GraphQL | `http://localhost:5049/graphql` | POST queries/mutations |
| Swagger | `http://localhost:5049/swagger` | Interactive documentation |
| Health | `http://localhost:5049/health` | Public health endpoint |
| RabbitMQ | `http://localhost:15672` | Management UI (guest/guest) |

---

## One-Liner Tests

### 1. Get JWT Token
```powershell
Invoke-WebRequest -Uri "http://localhost:5049/api/auth/token?role=Admin" -Method POST | Select -ExpandProperty Content | ConvertFrom-Json | Select -ExpandProperty token
```

### 2. Get Active Employees
```powershell
$token = "YOUR_JWT_TOKEN"; Invoke-RestMethod -Uri "http://localhost:5049/api/employees/active" -Headers @{"Authorization"="Bearer $token"}
```

### 3. Test GraphQL
```powershell
Invoke-RestMethod -Uri "http://localhost:5049/graphql" -Method POST -Body (@{query='{ getEmployees { id firstName lastName } }'} | ConvertTo-Json) -Headers @{"Content-Type"="application/json"}
```

### 4. Check API Health
```powershell
Invoke-RestMethod http://localhost:5049/health
```

---

## Common Roles for Testing

| Role | Access Level | Use Case |
|------|--------------|----------|
| **Admin** | Full access | System administration |
| **HR** | Most operations | HR management |
| **Manager** | Read + limited write | Team management |
| **Employee** | Own data only | Personal information |

---

## Key Endpoints by Purpose

### Authentication
```
POST /api/auth/token?role=Admin           → Generate JWT
POST /api/auth/validate                   → Validate token
```

### Query Operations
```
GET /api/employees/active                 → All active employees
GET /api/employees/{id}                   → By ID
GET /api/employees/number/{number}        → By employee number
GET /api/employees/search?searchTerm=     → Search
GET /api/employees/statistics             → Statistics
```

### Write Operations  
```
POST /api/employees                       → Create
PUT /api/employees/{id}/personal-info     → Update personal
PUT /api/employees/{id}/contact           → Update contact
PUT /api/employees/{id}/salary            → Update salary
PUT /api/employees/{id}/promote           → Promote
PUT /api/employees/{id}/terminate         → Terminate
DELETE /api/employees/{id}                → Delete
```

### GraphQL Queries
```
query { getEmployees { id firstName lastName } }
query { getStatistics { totalEmployees activeEmployees } }
```

### GraphQL Mutations
```
mutation {
  createEmployee(input: {firstName: "John" lastName: "Doe" ...}) {
    success employeeId
  }
}
```

---

## RabbitMQ Monitoring

### Access Management UI
1. Open: `http://localhost:15672`
2. Login: `guest` / `guest`
3. Click: **Queues** tab
4. Select: **employee.events** queue
5. View: Message count and details

### Expected Queue Behavior
- Queue name: `employee.events`
- Messages increase when employees are created/updated/deleted
- Use **Get messages** button to inspect event content

---

## Test Data Available

| Employee ID | Employee Number | Name | Status |
|-------------|-----------------|------|--------|
| 1 | EMP001 | Sample Employee 1 | Active |
| 2 | EMP002 | Sample Employee 2 | Active |
| 3 | EMP003 | Sample Employee 3 | Active |

---

## Response Status Codes

| Code | Meaning | Example |
|------|---------|---------|
| 200 | Success | Employee retrieved |
| 201 | Created | New employee added |
| 400 | Bad Request | Invalid input |
| 401 | Unauthorized | Missing/invalid token |
| 403 | Forbidden | Insufficient role |
| 404 | Not Found | Employee doesn't exist |
| 500 | Server Error | Database error |

---

## JWT Token Example Response

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600,
  "tokenType": "Bearer",
  "role": "Admin"
}
```

**Usage**: Add to request header as:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

---

## Troubleshooting Checklist

- [ ] API is running on port 5049?
- [ ] RabbitMQ is running on port 5672?
- [ ] Database (LocalDB) is accessible?
- [ ] JWT token is valid and not expired?
- [ ] Authorization header is included?
- [ ] User role has permission for operation?
- [ ] GraphQL query syntax is correct?
- [ ] RabbitMQ Management UI shows messages?

---

## Quick Start Test Flow

1. **Start API**: `dotnet run --project employeeServices/src/EmployeeService.API`
2. **Get Token**: Access `/api/auth/token?role=Admin`
3. **Test GET**: Call `/api/employees/active` with token
4. **Test POST**: Create employee via `/api/employees`
5. **Test GraphQL**: POST to `/graphql` with query
6. **Monitor RabbitMQ**: Check `http://localhost:15672` for messages
7. **View Docs**: Open `http://localhost:5049/swagger`

---

## File Locations

| File | Purpose |
|------|---------|
| EMPLOYEESERVICE_TESTING_GUIDE.md | Detailed endpoint documentation |
| EMPLOYEESERVICE_TESTING_SUMMARY.md | Complete testing overview |
| Test-EmployeeService-Simple.ps1 | Automated test script |
| src/EmployeeService.API/ | API implementation |
| src/EmployeeService.Domain/ | Domain models |
| src/EmployeeService.Application/ | Business logic |
| src/EmployeeService.Infrastructure/ | Data access layer |

---

**Last Updated**: March 28, 2026

For detailed documentation, refer to:
- Comprehensive guide: `EMPLOYEESERVICE_TESTING_GUIDE.md`
- Complete summary: `EMPLOYEESERVICE_TESTING_SUMMARY.md`
