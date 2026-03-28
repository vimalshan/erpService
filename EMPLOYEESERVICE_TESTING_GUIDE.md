# EmployeeService Testing Guide

## Service Status
- **API is Running**: ✅ Started via `dotnet run --project employeeServices/src/EmployeeService.API`
- **Database**: LocalDB (EmployeeServiceDB) - Seeded with 3 sample employees
- **Base URL**: `http://localhost:5049` (HTTP) or `https://localhost:7102` (HTTPS)

---

## 1. REST API Endpoints Testing

### Authentication (No Authorization Required)

#### 1.1 Generate JWT Token
**Endpoint**: `POST /api/auth/token`

**Query Parameters**:
- `role`: `Admin` | `HR` | `Manager` | `Employee` (Optional, defaults to Admin)

**Example Request** (PowerShell):
```powershell
$response = Invoke-WebRequest -Uri "http://localhost:5049/api/auth/token?role=Admin" `
  -Method POST
$token = ($response.Content | ConvertFrom-Json).token
Write-Host "Token: $token"
```

**Example Response**:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600,
  "tokenType": "Bearer",
  "role": "Admin"
}
```

#### 1.2 Validate JWT Token
**Endpoint**: `POST /api/auth/validate`

**Body** (JSON):
```json
{
  "token": "YOUR_JWT_TOKEN_HERE"
}
```

---

### Employee Operations (Requires JWT Token)

#### 1.3 Get Employee by ID
**Endpoint**: `GET /api/employees/{id}`

**Required Role**: Admin, HR, Manager

**Example Request**:
```powershell
$headers = @{
  "Authorization" = "Bearer $token"
  "Content-Type" = "application/json"
}

Invoke-RestMethod -Uri "http://localhost:5049/api/employees/1" `
  -Method GET `
  -Headers $headers
```

#### 1.4 Get Employee by Employee Number
**Endpoint**: `GET /api/employees/number/{employeeNumber}`

**Required Role**: Admin, HR

**Example**:
```powershell
Invoke-RestMethod -Uri "http://localhost:5049/api/employees/number/EMP001" `
  -Method GET `
  -Headers $headers
```

#### 1.5 Get All Active Employees
**Endpoint**: `GET /api/employees/active`

**Required Role**: Admin, HR, Manager

**Example**:
```powershell
$result = Invoke-RestMethod -Uri "http://localhost:5049/api/employees/active" `
  -Method GET `
  -Headers $headers

$result | ConvertTo-Json -Depth 10
```

#### 1.6 Get Employees by Unit
**Endpoint**: `GET /api/employees/unit/{unitId}`

**Required Role**: Admin, HR, Manager

**Example**:
```powershell
Invoke-RestMethod -Uri "http://localhost:5049/api/employees/unit/101" `
  -Method GET `
  -Headers $headers
```

#### 1.7 Get Employees by Grade
**Endpoint**: `GET /api/employees/grade/{gradeCode}`

**Required Role**: Admin, HR

**Example**:
```powershell
Invoke-RestMethod -Uri "http://localhost:5049/api/employees/grade/A-1" `
  -Method GET `
  -Headers $headers
```

#### 1.8 Search Employees
**Endpoint**: `GET /api/employees/search?searchTerm={term}`

**Required Role**: Admin, HR, Manager

**Example**:
```powershell
Invoke-RestMethod -Uri "http://localhost:5049/api/employees/search?searchTerm=John" `
  -Method GET `
  -Headers $headers
```

#### 1.9 Get Employee Statistics
**Endpoint**: `GET /api/employees/statistics`

**Required Role**: Admin, HR

**Example**:
```powershell
Invoke-RestMethod -Uri "http://localhost:5049/api/employees/statistics" `
  -Method GET `
  -Headers $headers
```

#### 1.10 Get Employee with All Details
**Endpoint**: `GET /api/employees/{id}/details`

**Required Role**: Admin, HR

**Example**:
```powershell
Invoke-RestMethod -Uri "http://localhost:5049/api/employees/1/details" `
  -Method GET `
  -Headers $headers
```

#### 1.11 Create Employee
**Endpoint**: `POST /api/employees`

**Required Role**: Admin, HR

**Body** (JSON):
```json
{
  "firstName": "John",
  "middleName": "Michael",
  "lastName": "Doe",
  "dateOfBirth": "1990-01-15",
  "gender": "M",
  "email": "john.doe@company.com",
  "phoneNumber": "555-0100",
  "alternatePhone": "555-0101",
  "employeeNumber": "EMP004",
  "userId": "jdoe",
  "nickName": "John D",
  "joiningDate": "2024-01-01",
  "effectiveDate": "2024-01-01",
  "confirmationDate": "2024-04-01",
  "gradeCode": "A-1",
  "gradeName": "Senior Manager",
  "gradeId": 1,
  "cadreName": "Management",
  "unitBusinessId": 1,
  "unitOrgId": 101,
  "unitCode": "UNIT01",
  "unit": "Operations",
  "designation": "Manager",
  "hrRoleId": 2,
  "basicSalary": 50000,
  "salaryType": "Monthly",
  "salutation": "Mr",
  "pinNumber": "123456"
}
```

**PowerShell Example**:
```powershell
$body = @{
  "firstName" = "John"
  "lastName" = "Doe"
  "email" = "john.doe@company.com"
  "phoneNumber" = "555-0100"
  "employeeNumber" = "EMP004"
  "joiningDate" = "2024-01-01"
  "gradeCode" = "A-1"
  "designation" = "Manager"
  "basicSalary" = 50000
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:5001/api/employees" `
  -Method POST `
  -Headers $headers `
  -Body $body `
  -SkipCertificateCheck
```

#### 1.12 Update Employee Personal Information
**Endpoint**: `PUT /api/employees/{id}/personal-info`

**Required Role**: Admin, HR

**Body** (JSON):
```json
{
  "firstName": "Jane",
  "middleName": "Ann",
  "lastName": "Smith",
  "dateOfBirth": "1992-05-20",
  "gender": "F",
  "nickName": "Jane S"
}
```

#### 1.13 Update Employee Contact Information
**Endpoint**: `PUT /api/employees/{id}/contact`

**Required Role**: Admin, HR, Employee

**Body** (JSON):
```json
{
  "email": "jane.smith@company.com",
  "phoneNumber": "555-0200",
  "alternatePhone": "555-0201"
}
```

#### 1.14 Update Employee Salary
**Endpoint**: `PUT /api/employees/{id}/salary`

**Required Role**: Admin, HR

**Body** (JSON):
```json
{
  "basicSalary": 55000,
  "salaryType": "Monthly",
  "effectiveDate": "2024-04-01"
}
```

#### 1.15 Promote Employee
**Endpoint**: `PUT /api/employees/{id}/promote`

**Required Role**: Admin, HR

**Body** (JSON):
```json
{
  "fromDesignation": "Manager",
  "toDesignation": "Senior Manager",
  "fromGrade": "A-1",
  "toGrade": "A",
  "newGradeId": 1,
  "newBasicSalary": 65000,
  "promotionDate": "2024-06-01"
}
```

#### 1.16 Terminate Employee
**Endpoint**: `PUT /api/employees/{id}/terminate`

**Required Role**: Admin, HR

**Body** (JSON):
```json
{
  "terminationFlag": true,
  "exitDate": "2024-12-31",
  "reason": "Resignation"
}
```

#### 1.17 Reactivate Employee
**Endpoint**: `PUT /api/employees/{id}/reactivate`

**Required Role**: Admin, HR

**Body** (JSON):
```json
{
  "status": "Active",
  "reactivationDate": "2024-07-01"
}
```

#### 1.18 Delete Employee (Soft Delete)
**Endpoint**: `DELETE /api/employees/{id}`

**Required Role**: Admin, HR

**Example**:
```powershell
Invoke-RestMethod -Uri "https://localhost:5001/api/employees/1" `
  -Method DELETE `
  -Headers $headers `
  -SkipCertificateCheck
```

---

## 2. GraphQL Testing

### Base URL: `http://localhost:5049/graphql`

### Using GraphQL Playground/Studio

1. Open your browser to: `http://localhost:5049/graphql`
2. The endpoint uses HotChocolate GraphQL Server

### GraphQL Queries

#### 2.1 Get Employee by ID
```graphql
query {
  getEmployee(id: 1) {
    id
    firstName
    lastName
    email
    phoneNumber
    employeeNumber
    designation
    gradeCode
  }
}
```

#### 2.2 Get Employee by Number
```graphql
query {
  getEmployeeByNumber(employeeNumber: "EMP001") {
    id
    firstName
    lastName
    email
    employeeNumber
  }
}
```

#### 2.3 Get All Active Employees
```graphql
query {
  getEmployees {
    id
    firstName
    lastName
    email
    designation
    gradeCode
  }
}
```

#### 2.4 Search Employees
```graphql
query {
  searchEmployees(searchTerm: "John") {
    id
    firstName
    lastName
    email
  }
}
```

#### 2.5 Get Employees by Unit
```graphql
query {
  getEmployeesByUnit(unitId: 101) {
    id
    firstName
    lastName
    designation
    unit
  }
}
```

#### 2.6 Get Employees by Grade
```graphql
query {
  getEmployeesByGrade(gradeCode: "A-1") {
    id
    firstName
    lastName
    gradeCode
    gradeName
  }
}
```

#### 2.7 Get Employee Count
```graphql
query {
  getEmployeeCount
}
```

#### 2.8 Get Employee Statistics
```graphql
query {
  getStatistics {
    totalEmployees
    activeEmployees
    terminatedEmployees
    gradeDistribution
    unitDistribution
  }
}
```

### GraphQL Mutations

#### 2.9 Create Employee
```graphql
mutation {
  createEmployee(input: {
    firstName: "Alice"
    lastName: "Johnson"
    email: "alice.johnson@company.com"
    phoneNumber: "555-0300"
    employeeNumber: "EMP005"
    joiningDate: "2024-02-01"
    designation: "Developer"
    gradeCode: "B-2"
    basicSalary: 45000
  }) {
    success
    message
    employeeId
  }
}
```

#### 2.10 Update Employee Salary
```graphql
mutation {
  updateEmployeeSalary(employeeId: 1, input: {
    basicSalary: 60000
    salaryType: "Monthly"
    effectiveDate: "2024-05-01"
  }) {
    success
    message
  }
}
```

#### 2.11 Promote Employee
```graphql
mutation {
  promoteEmployee(employeeId: 1, input: {
    fromDesignation: "Manager"
    toDesignation: "Senior Manager"
    fromGrade: "A-1"
    toGrade: "A"
    newGradeId: 1
    newBasicSalary: 70000
    promotionDate: "2024-06-01"
  }) {
    success
    message
  }
}
```

#### 2.12 Terminate Employee
```graphql
mutation {
  terminateEmployee(employeeId: 1, input: {
    terminationFlag: true
    exitDate: "2024-12-31"
    reason: "Resignation"
  }) {
    success
    message
  }
}
```

### PowerShell GraphQL Example
```powershell
$graphqlQuery = @"
{
  getEmployees {
    id
    firstName
    lastName
    email
  }
}
"@

$body = @{
  query = $graphqlQuery
} | ConvertTo-Json

$response = Invoke-RestMethod -Uri "http://localhost:5049/graphql" `
  -Method POST `
  -Headers @{"Content-Type" = "application/json"} `
  -Body $body `
  -SkipCertificateCheck

$response | ConvertTo-Json -Depth 10
```

---

## 3. RabbitMQ Configuration & Testing

### RabbitMQ Settings (from appsettings.json)
- **Host**: localhost
- **Port**: 5672
- **Username**: guest
- **Password**: guest
- **Queue Name**: employee.events
- **Virtual Host**: /
- **Retry Count**: 2
- **Retry Delay**: 2 seconds
- **Circuit Breaker Duration**: 30 seconds

### Prerequisites
1. RabbitMQ Server must be running on localhost:5672
2. Management UI available at: `http://localhost:15672` (guest/guest)

### RabbitMQ Queue Monitoring

#### 3.1 Access RabbitMQ Management
1. Open browser: `http://localhost:15672`
2. Login: guest / guest
3. Check tabs:
   - **Queues**: View employee.events queue
   - **Connections**: View API connection status
   - **Channels**: View message channels

#### 3.2 View Publish Events
When you create, update, or delete employees via REST/GraphQL:
1. Go to RabbitMQ Management UI
2. Navigate to **Queues** tab
3. Click **employee.events** queue
4. View message count and details
5. Use **Get messages** button to inspect published events

### Event Messages Published

The following actions trigger RabbitMQ events:

1. **Employee Created**
   - Trigger: `POST /api/employees` or GraphQL create mutation
   - Message: Employee details with timestamp

2. **Employee Updated**
   - Trigger: `PUT /api/employees/{id}/personal-info`, contact, salary updates
   - Message: Updated employee details

3. **Employee Promoted**
   - Trigger: `PUT /api/employees/{id}/promote`
   - Message: Promotion details

4. **Employee Terminated**
   - Trigger: `PUT /api/employees/{id}/terminate`
   - Message: Termination details

5. **Employee Deleted**
   - Trigger: `DELETE /api/employees/{id}`
   - Message: Deletion confirmation

### Testing RabbitMQ Publishing

#### Method 1: Via REST API
```powershell
# Create employee (triggers RabbitMQ publish)
$body = @{
  "firstName" = "TestUser"
  "lastName" = "Testing"
  "email" = "test@test.com"
  "employeeNumber" = "EMP999"
  "designation" = "Tester"
  "basicSalary" = 40000
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:5001/api/employees" `
  -Method POST `
  -Headers @{
    "Authorization" = "Bearer $token"
    "Content-Type" = "application/json"
  } `
  -Body $body `
  -SkipCertificateCheck

# Check RabbitMQ Management UI -> Queues -> employee.events
```

#### Method 2: Via GraphQL
```powershell
$query = @"
mutation {
  createEmployee(input: {
    firstName: "GraphQLTest"
    lastName: "User"
    email: "graphql@test.com"
    employeeNumber: "EMP998"
    designation: "Tester"
    basicSalary: 35000
  }) {
    success
    message
    employeeId
  }
}
"@

$body = @{ query = $query } | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:5001/graphql" `
  -Method POST `
  -Headers @{"Content-Type" = "application/json"} `
  -Body $body `
  -SkipCertificateCheck
```

### Troubleshooting RabbitMQ

**Issue**: No messages in queue  
**Solution**: 
- Ensure RabbitMQ server is running: `rabbitmq-server`
- Check connection logs in API output
- Verify credentials in appsettings.json

**Issue**: RabbitMQ Connection Timeout  
**Solution**:
- Install RabbitMQ: `choco install rabbitmq` (Windows) or use Docker
- Start RabbitMQ service
- Check port 5672 is open

**Issue**: Circuit Breaker Active  
**Solution**:
- Wait 30 seconds (circuit break duration)
- Check RabbitMQ server health
- Review API logs for detailed errors

---

## 4. Swagger/OpenAPI Documentation

**URL**: `http://localhost:5049/swagger`

Access interactive API documentation:
1. Open: `http://localhost:5049/swagger`
2. All endpoints are documented with:
   - Request/response examples
   - Authorization requirements
   - Parameter descriptions
   - Status code meanings

---

## 5. Health Checks

#### 5.1 API Health
**Endpoint**: `GET /health`

**URL**: `http://localhost:5049/health`

**Response**:
```json
{
  "status": "Healthy",
  "checks": {
    "Database": "Healthy"
  }
}
```

#### 5.2 Readiness Check
**Endpoint**: `GET /health/ready`

---

## 6. Quick Test Checklist

- [ ] REST endpoint: Get active employees
- [ ] REST endpoint: Create new employee
- [ ] REST endpoint: Update employee
- [ ] REST endpoint: Delete employee
- [ ] GraphQL: Query employees
- [ ] GraphQL: Mutation to create employee
- [ ] RabbitMQ: Verify employees.events queue receives messages
- [ ] JWT: Generate and validate token
- [ ] Swagger: Access API documentation

---

## 7. Common Issues & Solutions

| Issue | Solution |
|-------|----------|
| 401 Unauthorized | Generate JWT token via `/api/auth/token` and include in Authorization header |
| 403 Forbidden | Verify user role matches endpoint requirements |
| Connection refused | Verify API is running and SSL certificate is trusted |
| RabbitMQ errors | Check RabbitMQ server is running and credentials are correct |
| GraphQL errors | Check query syntax and included fields are valid |

---

## 8. Test Data Sample

Pre-seeded employees (from database initialization):
- **EMP001**: Sample Employee 1
- **EMP002**: Sample Employee 2
- **EMP003**: Sample Employee 3

---

Generated: March 28, 2026
Service: EmployeeService.API v1
Database: EmployeeServiceDB (LocalDB)
