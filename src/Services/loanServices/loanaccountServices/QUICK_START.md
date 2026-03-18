# Loan Account Microservice - Quick Start Guide

## 📦 Project Overview

This is a **production-ready microservice** for managing employee loan accounts with:
- ✅ RESTful API + GraphQL
- ✅ JWT Authentication
- ✅ Event-driven architecture with RabbitMQ
- ✅ Azure cloud integration
- ✅ Enterprise resilience patterns
- ✅ Complete DDD and CQRS implementation

---

## 🚀 Quick Start (5 minutes)

### 1. **Clone/Open Solution**
```bash
# Navigate to workspace
cd e:\ERPMicroservice\src\Services\loanServices\loanaccountServices

# Open in VS Code (if available)
code .
```

### 2. **Build the Solution**
```bash
dotnet build LoanAccountService.sln -c Release
```

### 3. **Create Database**
```bash
cd src\LoanAccount.API
dotnet ef database update
```

### 4. **Run the API**
```bash
dotnet run
```

### 5. **Access Swagger**
```
http://localhost:5000/swagger
```

---

## 🔐 Authentication

### Get JWT Token
```bash
curl -X POST "http://localhost:5000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"username":"demo","password":"demo"}'
```

**Response:**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "tokenType": "Bearer",
  "expiresIn": 3600
}
```

### Use Token in Requests
```bash
curl -X GET "http://localhost:5000/api/loans/1001" \
  -H "Authorization: Bearer {accessToken}"
```

---

## 📊 API Quick Reference

### Create Loan
```bash
POST /api/loans
Content-Type: application/json

{
  "loanAppId": 1,
  "employeeId": 101,
  "loanId": "LN001",
  "gradeId": "G1",
  "principalAmount": 100000,
  "disbursementType": "NEW",
  "loanDate": "2024-01-15",
  "expectedClosureDate": "2025-01-15",
  "unitId": "U1",
  "subClassId": "SC1",
  "reason": "Emergency",
  "guarantorId": "G101",
  "createdBy": "system"
}
```

### Approve Loan
```bash
POST /api/loans/{loanNo}/approve
Content-Type: application/json

{
  "interestRate": 10.5,
  "approvedBy": "manager@company.com",
  "approvalRemarks": "Approved after verification"
}
```

### Disburse Loan
```bash
POST /api/loans/{loanNo}/disburse
Content-Type: application/json

{
  "disbursementAmount": 100000,
  "disbursedBy": "finance@company.com"
}
```

### Record EMI Payment
```bash
POST /api/loans/{loanNo}/payment
Content-Type: application/json

{
  "installmentId": 1,
  "principalPaid": 8333.33,
  "interestPaid": 666.67,
  "paymentDate": "2024-02-15",
  "paidBy": "employee@company.com"
}
```

### Get Loan Details
```bash
GET /api/loans/{loanNo}
Authorization: Bearer {token}
```

### Get All Active Loans
```bash
GET /api/loans/active
Authorization: Bearer {token}
```

---

## 📈 GraphQL Query Examples

### Get Loan Details
```graphql
{
  loanByNumber(loanNo: "1001") {
    loanNo
    loanAppId
    principalAmount
    status
    createdOn
    installments {
      id
      installmentNumber
      installmentAmount
      dueDate
      principalRecovered
      interestRecovered
    }
  }
}
```

### Get Employee Loans
```graphql
{
  employeeLoans(employeeId: 101) {
    loanNo
    principalAmount
    status
    createdOn
    ledgerEntries {
      transactionDate
      transactionType
      transactionAmount
    }
  }
}
```

### Create Loan (Mutation)
```graphql
mutation {
  createLoan(input: {
    loanAppId: 1
    employeeId: 101
    principalAmount: 100000
    disbursementType: "NEW"
    reason: "Emergency"
    createdBy: "system"
  }) {
    loanNo
    status
    createdOn
  }
}
```

---

## 📁 Project Structure

```
LoanAccountService.sln
├── LoanAccount.Domain/           # Business logic & entities
│   ├── Entities/
│   ├── ValueObjects/
│   ├── Events/
│   └── Interfaces/
├── LoanAccount.Application/      # CQRS implementation
│   ├── Commands/
│   ├── Queries/
│   ├── DTOs/
│   ├── Validators/
│   ├── Handlers/
│   └── Services/
├── LoanAccount.Infrastructure/   # Data access & services
│   ├── Persistence/
│   ├── Repositories/
│   ├── Services/
│   ├── Messaging/
│   └── HealthChecks/
├── LoanAccount.API/              # REST & GraphQL endpoints
│   ├── Controllers/
│   ├── GraphQL/
│   ├── Security/
│   ├── Middleware/
│   └── Program.cs
├── LoanAccount.Functions/        # Azure Functions
│   └── LoanFunctions.cs
└── LoanAccount.Tests/            # Unit tests
```

---

## 🔧 Configuration

### appsettings.json Sections

**Connection String:**
```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LoanAccountDb;..."
}
```

**JWT Settings:**
```json
"JwtSettings": {
  "SecretKey": "your-secret-key-minimum-32-characters",
  "Issuer": "LoanAccountService",
  "Audience": "LoanAccountServiceApi",
  "ExpirationMinutes": 60
}
```

**RabbitMQ:**
```json
"RabbitMQ": {
  "Host": "localhost",
  "Port": 5672,
  "Username": "guest",
  "Password": "guest"
}
```

**Azure Storage:**
```json
"AzureStorage": {
  "ContainerName": "loan-documents",
  "BlobPrefix": "loans"
}
```

---

## 🧪 Testing Workflow

### 1. **Verify API is Running**
```bash
GET http://localhost:5000/health
```

Expected: Status 200 with health information

### 2. **Login and Get Token**
```bash
POST http://localhost:5000/api/auth/login
Body: {"username":"demo","password":"demo"}
```

### 3. **Create a Test Loan**
```bash
POST http://localhost:5000/api/loans
Headers: Authorization: Bearer {token}
Body: {...loan creation details...}
```

### 4. **Verify Loan Created**
```bash
GET http://localhost:5000/api/loans/{loanNo}
Headers: Authorization: Bearer {token}
```

### 5. **Approve the Loan**
```bash
POST http://localhost:5000/api/loans/{loanNo}/approve
Headers: Authorization: Bearer {token}
Body: {...approval details...}
```

---

## 📊 Database Tables

| Table | Purpose |
|-------|---------|
| LOAN_MAINS | Master loan records |
| LOAN_INSTALLMENTS | EMI schedule tracking |
| LOAN_EMPLOYEE_INTEREST_RATES | Interest rate management |
| LOAN_LEDGERS | Financial transaction log |
| LOAN_SETTLEMENTS | Settlement records |

---

## 🔄 Event Flow

1. **Loan Created** → `LoanCreatedEvent`
2. **Loan Approved** → `LoanApprovedEvent`
3. **EMI Recorded** → `EMIPaymentRecordedEvent`
4. **Loan Settled** → `LoanSettledEvent`
5. **Events Published** → RabbitMQ Topic Exchange
6. **Consumers Process** → Update aggregates, send notifications

---

## 🛡️ Security Considerations

### JWT Token
- ✅ 32+ character secret key required
- ✅ Bearer token authentication
- ✅ Role-based authorization
- ✅ Token expiration (configurable)

### API Security
- ✅ HTTPS required in production
- ✅ CORS configured for allowed origins
- ✅ Input validation with FluentValidation
- ✅ Exception handling with proper error codes

### Database
- ✅ Connection string encryption
- ✅ SQL Server connection pooling
- ✅ Parameter validation (no SQL injection)
- ✅ Audit fields (CreatedBy, ModifiedBy)

---

## 📈 Performance Tips

1. **Use Pagination** - Add `skip` and `take` parameters
2. **Index Optimization** - Queries on LOAN_NO, EMPSYSID already indexed
3. **Async Operations** - All DB calls are async (non-blocking)
4. **Connection Pooling** - Enabled by default in connection string
5. **Caching Ready** - Redis integration can be added
6. **Resilience** - Polly circuit breaker for external services

---

## 🐛 Troubleshooting

### Database Connection Failed
```
Error: Cannot connect to (localdb)\MSSQLLocalDB

Solution:
1. Ensure SQL Server LocalDB is installed
2. Run: sqllocaldb start mssqllocaldb
3. Verify connection string in appsettings.json
```

### Authentication Failed
```
Error: Invalid token

Solution:
1. Verify JWT secret key (32+ chars)
2. Check token expiration time
3. Ensure Bearer scheme in Authorization header
```

### RabbitMQ Not Connected
```
Error: Cannot connect to localhost:5672

Solution:
1. Start RabbitMQ: docker run -d -p 5672:5672 rabbitmq
2. Or install RabbitMQ locally
3. Verify credentials in appsettings.json
```

---

## 📚 Documentation Files

- **README.md** - Complete architecture overview
- **BUILD_AND_VERIFY.md** - Detailed build & verification guide
- **ARCHITECTURE_SUMMARY.md** - Technical architecture deep-dive
- **QUICK_START.md** - This file

---

## 🎯 Next Steps

1. ✅ Build the solution
2. ✅ Create the database
3. ✅ Run the API
4. ✅ Test endpoints via Swagger
5. ✅ Integrate into your application
6. ✅ Deploy to Azure (optional)

---

## 📞 Support

For issues or questions:
1. Check BUILD_AND_VERIFY.md troubleshooting section
2. Review domain events in Domain/Events/
3. Check repository implementations in Infrastructure/Repositories/
4. Review CQRS handlers in Application/Handlers/

---

**Status**: ✅ **PRODUCTION READY**

All components implemented and ready for use. Happy lending! 🚀
