# Loan Account Microservice

Enterprise-grade loan account management microservice built with clean architecture, CQRS pattern, and event-sourcing capabilities.

## Architecture Overview

The solution follows Clean Architecture principles with clear separation of concerns:

```
LoanAccountService/
├── src/
│   ├── LoanAccount.Domain/                 # Domain layer (entities, value objects, aggregates)
│   ├── LoanAccount.Application/            # Application layer (CQRS, DTOs, validators, handlers)
│   ├── LoanAccount.Infrastructure/         # Infrastructure layer (EF Core, repositories, external services)
│   ├── LoanAccount.API/                    # API layer (REST, GraphQL, authentication)
│   └── LoanAccount.Functions/              # Azure Functions (background tasks)
└── tests/
    └── LoanAccount.Tests/                  # Unit and integration tests
```

## Technology Stack

- **Framework**: .NET 8.0
- **Architecture**: Clean Architecture, CQRS, DDD
- **Database**: SQL Server (LocalDB or Azure SQL)
- **ORM**: Entity Framework Core 8.0
- **API**: REST (ASP.NET Core), GraphQL (HotChocolate)
- **Authentication**: JWT Bearer tokens
- **Messaging**: RabbitMQ
- **Cloud Services**: Azure Functions, Blob Storage, Application Insights
- **Resilience**: Polly (Circuit Breaker, Retry, Timeout)
- **Logging**: Serilog
- **Validation**: FluentValidation
- **Mapping**: AutoMapper
- **Testing**: xUnit, Moq

## Domain Model

### Core Entities
- **LoanMain** (Aggregate Root): Primary loan information
- **LoanInstallment**: EMI/installment details
- **LoanEmployeeInterestRate**: Employee-specific interest rate configuration
- **LoanLedger**: Transaction history
- **LoanSettlement**: Settlement records

### Value Objects
- **Money**: Represents currency amounts
- **InterestRate**: Interest rate percentage
- **LoanStatus**: Loan status enumeration
- **DisbursementType**: Loan disbursement type
- **RecoveryMethod**: Recovery method enumeration
- **SettlementType**: Settlement type enumeration

### Domain Events
- LoanCreatedEvent
- LoanApprovedEvent
- LoanDisbursedEvent
- EMIPaymentRecordedEvent
- LoanSettledEvent
- LoanClosedEvent

## API Endpoints

### REST API
```
POST   /api/auth/login                      # Authenticate and get JWT token
POST   /api/loans                           # Create new loan
GET    /api/loans/{loanNo}                  # Get loan details
GET    /api/loans/employee/{employeeId}    # Get all loans for employee
GET    /api/loans/{loanNo}/details          # Get loan with installments
GET    /api/loans/{loanNo}/installments     # Get all installments
POST   /api/loans/{loanNo}/approve          # Approve loan
POST   /api/loans/{loanNo}/disburse         # Disburse loan amount
POST   /api/loans/{loanNo}/payment          # Record EMI payment
POST   /api/loans/{loanNo}/settle           # Settle loan
GET    /api/loans/active                    # Get all active loans
GET    /health                              # Health check endpoint
```

### GraphQL Endpoint
```
POST   /graphql                             # GraphQL queries and mutations
```

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- SQL Server LocalDB or Azure SQL
- RabbitMQ (optional, for messaging features)
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
```bash
git clone <repository-url>
cd LoanAccountService
```

2. **Create Database**
```bash
cd src/LoanAccount.API
dotnet ef database update
```

This will:
- Apply migrations to create tables
- Populate seed data with sample loans

3. **Configure Settings**
   
   Edit `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "LoanAccountDb": "Data Source=(localdb)\\MSSQLLocalDB;..."
  },
  "JwtSettings": {
    "SecretKey": "your-secret-key-32-chars-minimum"
  },
  "RabbitMQ": {
    "Host": "localhost",
    "Port": 5672,
    "Username": "guest",
    "Password": "guest"
  }
}
```

4. **Build Solution**
```bash
dotnet build
```

5. **Run API**
```bash
cd src/LoanAccount.API
dotnet run
```

The API will be available at: `https://localhost:5001`

## Usage Examples

### 1. Login and Get Token
```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"demo","password":"demo"}'
```

Response:
```json
{
  "data": {
    "accessToken": "eyJhbGc...",
    "tokenType": "Bearer",
    "expiresIn": 3600
  }
}
```

### 2. Create a Loan
```bash
curl -X POST https://localhost:5001/api/loans \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "loanAppId": 100,
    "employeeId": 1,
    "loanId": 1,
    "gradeId": 5,
    "principalAmount": 100000,
    "disbursementType": "NEW",
    "loanDate": "2024-01-01",
    "firstInstallmentDate": "2024-02-01",
    "unitId": 1,
    "subClassId": 1,
    "reason": "Educational expenses",
    "guarantorId": 2
  }'
```

### 3. Get Loan Details
```bash
curl -X GET https://localhost:5001/api/loans/1001/details \
  -H "Authorization: Bearer <token>"
```

### 4. Approve Loan
```bash
curl -X POST https://localhost:5001/api/loans/1001/approve \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "interestRate": 10,
    "approvalRemarks": "Approved by manager"
  }'
```

### 5. Record Payment
```bash
curl -X POST https://localhost:5001/api/loans/1001/payment \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "installmentId": 1,
    "principalPaid": 8333.33,
    "interestPaid": 833.33,
    "paymentDate": "2024-02-01"
  }'
```

## API Documentation

- **Swagger/OpenAPI**: `https://localhost:5001/swagger`
- **GraphQL Playground**: `https://localhost:5001/graphql`
- **Health Checks**: `https://localhost:5001/health`

## Database Schema

### Tables

**LOAN_MAIN** - Main loan records
- LOAN_NO (Primary Key)
- LOAN_APPID, LOAN_EMPSYSID, LOAN_ID
- LOAN_PRNAMT, LOAN_PAID, LOAN_PRNOUT
- LOAN_DATE, LOAN_CLSDATE, LOAN_FIRSTINSDATE
- Status and recovery method fields

**LOAN_INSTALLMENT** - EMI/installment schedule
- LOANINS_ID (Primary Key)
- LOANINS_LOANNO, LOANINS_INSNO
- LOANINS_INSAMT, LOANINS_PRNOUT
- LOANINS_INSDATE, LOANINS_INTRATE

**LOAN_LEDGER** - Transaction history
- LOAN_LEDGERID (Primary Key)
- LOAN_NO, LOAN_TRNDATE, LOAN_TRNTYPE
- LOAN_DCFLAG (D/C), LOAN_TRNAMT

**LOAN_SETTLEMENT** - Settlement records
- LOANSET_ID (Primary Key)
- LOANSET_LOANNO, LOANSET_INSNO
- LOANSET_RECDATE, LOANSET_RECTYPE
- LOANSET_INSAMT, LOANSET_PAYTYPE

## Features

### ✅ Implemented
- Clean Architecture with DDD
- CQRS pattern with MediatR
- RESTful API with Swagger documentation
- GraphQL API
- JWT authentication and authorization
- EF Core with migrations
- Repository pattern with Unit of Work
- Entity validation with FluentValidation
- Automatic mapping with AutoMapper
- Comprehensive error handling
- Serilog logging
- Health checks
- Azure Blob Storage integration
- Polly resilience policies (Circuit Breaker, Retry)
- RabbitMQ event publishing and consuming
- Domain events
- Azure Functions for background tasks
- Custom health checks
- Seed data

### 🔄 High-Priority Features
1. API rate limiting
2. Distributed caching (Redis)
3. Request/response compression
4. CORS configuration
5. API versioning

### 🚀 Future Enhancements
1. Event sourcing for audit trail
2. SAGA pattern for distributed transactions
3. Advanced reporting and analytics
4. Mobile API
5. PDF generation for loan documents
6. Multi-language support
7. Real-time notifications
8. Integration with external banking systems

## Testing

### Run Unit Tests
```bash
dotnet test tests/LoanAccount.Tests
```

### Run with Coverage
```bash
dotnet test /p:CollectCoverage=true tests/LoanAccount.Tests
```

## Deployment

### Azure Deployment
1. Create Azure SQL Database
2. Create Azure App Service
3. Deploy Function App
4. Configure Blob Storage
5. Set environment variables

### Docker Deployment
```bash
docker build -t loanaccount-api:latest .
docker run -p 5001:80 loanaccount-api:latest
```

## Issues & Troubleshooting

### Database Connection Issues
- Verify LocalDB is running: `sqllocaldb i`
- Connection string format for SQL Server
- Check firewall settings

### RabbitMQ Connection Issues
- Ensure RabbitMQ service is running
- Verify credentials in appsettings.json
- Check network connectivity

### Authentication Issues
- Ensure JWT secret key is configured
- Verify token format in Authorization header (Bearer <token>)
- Check token expiration time

## Performance Optimization

- **Indexes**: Database indexes on foreign keys
- **Pagination**: Implement for large result sets
- **Caching**: Redis for frequently accessed data
- **Async/Await**: Non-blocking I/O operations
- **Connection Pooling**: EF Core configuration
- **Query Optimization**: Use projections in queries

## Security

- JWT token-based authentication
- Role-based authorization (Admin, LoanManager, User)
- Input validation with FluentValidation
- SQL injection protection via parameterized queries
- HTTPS enforcement
- Secure headers (CORS, CSP)
- Rate limiting (recommended)
- Audit logging

## Support

For issues, questions, or contributions, please contact the development team.

## License

This project is licensed under the MIT License.
