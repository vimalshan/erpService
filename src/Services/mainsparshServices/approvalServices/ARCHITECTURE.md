# Approval Service - Architecture & Design Document

## 🏗️ System Architecture

### High-Level Architecture

```
┌─────────────────────────────────────────────────────────┐
│                    CLIENT LAYER                         │
│  (Web Browsers, Mobile Apps, External Services)         │
└──────────────────────┬──────────────────────────────────┘
                       │ HTTPS/REST/GraphQL
┌──────────────────────────────────────────────────────────────┐
│              API GATEWAY / LOAD BALANCER                      │
│                                                               │
│  - Routing                                                    │
│  - Rate Limiting                                              │
│  - SSL/TLS Termination                                        │
└──────────────────────┬───────────────────────────────────────┘
                       │
┌──────────────────────────────────────────────────────────┐
│                  API LAYER                               │
│  ┌─────────────────────────────────────────────────┐    │
│  │ REST Controllers                                │    │
│  │ - ApprovalsController                           │    │
│  │ - ApproversController                           │    │
│  │ - AuthController                                │    │
│  └─────────────────────────────────────────────────┘    │
│                       │                                  │
│  ┌─────────────────────────────────────────────────┐    │
│  │ Middleware Stack                                │    │
│  │ - JWT Authentication                            │    │
│  │ - Exception Handling                            │    │
│  │ - CORS                                           │    │
│  │ - Logging                                        │    │
│  │ - Correlation IDs                               │    │
│  └─────────────────────────────────────────────────┘    │
└──────────────────────┬───────────────────────────────────┘
                       │
┌──────────────────────────────────────────────────────────┐
│            APPLICATION LAYER (CQRS)                      │
│  ┌─────────────────────────────────────────────────┐    │
│  │ Command Handlers                                │    │
│  │ - CreateApprovalMasterHandler                   │    │
│  │ - UpdateApprovalMasterHandler                   │    │
│  │ - CreateApproverEmployeeHandler                 │    │
│  │ - UpdateApproverEmployeeHandler                 │    │
│  │ - ActivateDeactivateHandlers                    │    │
│  └─────────────────────────────────────────────────┘    │
│  ┌─────────────────────────────────────────────────┐    │
│  │ Query Handlers                                  │    │
│  │ - GetApprovalMasterByIdHandler                  │    │
│  │ - GetApprovalsByModuleHandler                   │    │
│  │ - GetApproverEmployeeByIdHandler                │    │
│  │ - GetApproversByApprovalMasterHandler           │    │
│  └─────────────────────────────────────────────────┘    │
│  ┌─────────────────────────────────────────────────┐    │
│  │ MediatR Pipeline Behaviors                      │    │
│  │ - Validation (FluentValidation)                 │    │
│  │ - Logging                                        │    │
│  │ - Transaction Management                         │    │
│  └─────────────────────────────────────────────────┘    │
└──────────────────────┬───────────────────────────────────┘
                       │
┌──────────────────────────────────────────────────────────┐
│              DOMAIN LAYER (DDD)                          │
│  ┌─────────────────────────────────────────────────┐    │
│  │ Aggregates                                      │    │
│  │ - ApprovalMaster (Aggregate Root)               │    │
│  │   ├── Approvers (Collection)                    │    │
│  │   └── Domain Events                             │    │
│  │ - ApproverEmployee (Entity)                     │    │
│  │   └── Domain Events                             │    │
│  └─────────────────────────────────────────────────┘    │
│  ┌─────────────────────────────────────────────────┐    │
│  │ Domain Events                                   │    │
│  │ - ApprovalMasterCreatedEvent                    │    │
│  │ - ApprovalMasterStatusChangedEvent              │    │
│  │ - ApproverEmployeeCreatedEvent                  │    │
│  │ - ApproverEmployeeStatusChangedEvent            │    │
│  └─────────────────────────────────────────────────┘    │
│  ┌─────────────────────────────────────────────────┐    │
│  │ Value Objects                                   │    │
│  │ - ApprovalStatus (Active/Inactive)              │    │
│  │ - ApproverStatus (Active/Inactive)              │    │
│  │ - EffectiveDateRange                            │    │
│  └─────────────────────────────────────────────────┘    │
│  ┌─────────────────────────────────────────────────┐    │
│  │ Repository Interfaces                           │    │
│  │ - IApprovalMasterRepository                     │    │
│  │ - IApproverEmployeeRepository                   │    │
│  │ - IUnitOfWork                                   │    │
│  └─────────────────────────────────────────────────┘    │
└──────────────────────┬───────────────────────────────────┘
                       │
┌──────────────────────────────────────────────────────────┐
│          INFRASTRUCTURE LAYER                            │
│  ┌─────────────────────────────────────────────────┐    │
│  │ Database (SQL Server)                           │    │
│  │ - Entity Framework Core ORM                     │    │
│  │ - Migrations & DbContext                        │    │
│  │ - Connection Pooling                            │    │
│  │ - Retry Policies                                │    │
│  └─────────────────────────────────────────────────┘    │
│  ┌─────────────────────────────────────────────────┐    │
│  │ Repositories                                    │    │
│  │ - ApprovalMasterRepository                      │    │
│  │ - ApproverEmployeeRepository                    │    │
│  │ - UnitOfWork Implementation                     │    │
│  └─────────────────────────────────────────────────┘    │
│  ┌─────────────────────────────────────────────────┐    │
│  │ External Services                               │    │
│  │ - JwtTokenService (Authentication)              │    │
│  │ - BlobStorageService (Azure)                    │    │
│  │ - RabbitMqMessagePublisher (Messaging)          │    │
│  │ - EventConsumerHost (Background)                │    │
│  └─────────────────────────────────────────────────┘    │
│  ┌─────────────────────────────────────────────────┐    │
│  │ Cross-Cutting Concerns                          │    │
│  │ - Logging (Serilog)                             │    │
│  │ - Health Checks                                 │    │
│  │ - Circuit Breaker (Polly)                       │    │
│  │ - Exception Handling                            │    │
│  └─────────────────────────────────────────────────┘    │
└──────────────────────┬───────────────────────────────────┘
                       │
        ┌──────────────┼──────────────┐
        │              │              │
        ▼              ▼              ▼
    ┌────────┐  ┌──────────┐  ┌──────────┐
    │SQL DB  │  │RabbitMQ  │  │Blob      │
    │Server  │  │(Events)  │  │Storage   │
    └────────┘  └──────────┘  └──────────┘
```

### CQRS Pattern

```
Command Side (Write)         Query Side (Read)
┌─────────────────┐          ┌─────────────────┐
│   Commands      │          │    Queries      │
├─────────────────┤          ├─────────────────┤
│ Create          │          │ GetById         │
│ Update          │          │ GetByCode       │
│ Deactivate      │          │ GetByModule     │
│ Activate        │          │ GetAll          │
└────────┬────────┘          └────────┬────────┘
         │                           │
         ▼                           ▼
    ┌─────────────────────────────────────┐
    │   Command/Query Handlers            │
    │   (Business Logic Execution)        │
    └────────┬────────────────────────────┘
             │
             ▼
    ┌─────────────────────────────────────┐
    │   Domain Model                      │
    │   (Apply Business Rules)            │
    └────────┬────────────────────────────┘
             │
             ▼
    ┌─────────────────────────────────────┐
    │   Repository Pattern                │
    │   (Data Access Abstraction)         │
    └────────┬────────────────────────────┘
             │
             ▼
    ┌─────────────────────────────────────┐
    │   Database (SQL Server)             │
    │   APPR_MAST & APPROVER_EMP tables   │
    └─────────────────────────────────────┘
```

## 📊 Entity Relationship Diagram

```
APPR_MAST (Approval Master)
├── APPR_ID (PK, BigInt, Identity)
├── APPR_CODE (Varchar(50), Unique)
├── APPR_NAME (Varchar(255))
├── APPR_MODULE (Varchar(100))  -- PER, DDP, LET
├── APPR_LEVEL (Int, Default 1)
├── APPR_STATUS (Char(1))       -- A=Active, I=Inactive
├── CREATED_BY (BigInt)
├── CREATED_ON (DateTime2)
├── UPDATED_BY (BigInt, nullable)
├── UPDATED_ON (DateTime2, nullable)
└── Relationships:
    └── 1:N with APPROVER_EMP (cascade delete)

APPROVER_EMP (Approver Employee)
├── APPROVER_ID (PK, BigInt, Identity)
├── APPR_ID (FK → APPR_MAST.APPR_ID, BigInt)
├── EMP_SYSID (BigInt)          -- Employee ID
├── APPROVER_LEVEL (Int)         -- Level in chain
├── APPROVER_STATUS (Char(1))   -- A=Active, I=Inactive
├── EFFECTIVE_FROM (Date)
├── EFFECTIVE_TO (Date, nullable)
├── CREATED_BY (BigInt)
├── CREATED_ON (DateTime2)
├── UPDATED_BY (BigInt, nullable)
└── UPDATED_ON (DateTime2, nullable)

Indexes:
├── APPR_MAST
│   ├── PK_APPR_MAST (clustered)
│   ├── UQ_APPR_CODE (unique)
│   └── IX_APPR_MAST_MODULE (non-clustered)
└── APPROVER_EMP
    ├── PK_APPROVER_EMP (clustered)
    ├── IX_APPROVER_EMP_APPR_ID (non-clustered)
    └── IX_APPROVER_EMP_EMP_SYSID (non-clustered)
```

## 🔄 Domain Event Flow

```
API Request (Command)
         │
         ▼
Validation & Authorization
         │
         ▼
Create/Retrieve Domain Aggregate
         │
         ▼
Apply Business Logic (raises Domain Events)
         │
         ▼
Domain Events Generated:
├── ApprovalMasterCreatedEvent
├── ApprovalMasterStatusChangedEvent
├── ApproverEmployeeCreatedEvent
└── ApproverEmployeeStatusChangedEvent
         │
         ▼
Persist Changes (EF Core SaveChanges)
         │
         ▼
Publish Domain Events to Message Bus
         │
         ▼
RabbitMQ Exchange (approval-service)
         │
         ├─ approval.master.* routing key
         ├─ approver.employee.* routing key
         └─ Other event topics
         │
         ▼
Message Consumers
├── ApprovalMasterEventConsumer
├── ApproverEmployeeEventConsumer
└── External Services (Audit, Notification, etc.)
```

## 🔒 Security Architecture

```
Client Request
         │
         ▼
┌──────────────────────────┐
│ HTTPS/TLS Encryption    │
│ (Port 443 in Prod)       │
└──────────────┬───────────┘
               │
         ▼
┌──────────────────────────┐
│ JWT Token in Header      │
│ Authorization: Bearer... │
└──────────────┬───────────┘
               │
         ▼
┌──────────────────────────┐
│ JWT Validation           │
│ - Signature Verify       │
│ - Expiry Check           │
│ - Issuer/Audience        │
└──────────────┬───────────┘
               │
         ▼
┌──────────────────────────┐
│ Role-Based Authorization │
│ [Authorize] Attributes   │
└──────────────┬───────────┘
               │
         ▼
┌──────────────────────────┐
│ Request Processing       │
│ In Authenticated Context │
└──────────────┬───────────┘
               │
         ▼
Response with CORS Headers
```

## 🚀 Deployment Architecture

```
                    ┌────────────────┐
                    │  DNS / CDN     │
                    └────────┬───────┘
                             │
            ┌────────────────┼────────────────┐
            │                │                │
            ▼                ▼                ▼
    ┌────────────┐  ┌────────────┐  ┌────────────┐
    │   API      │  │   API      │  │   API      │
    │  Instance  │  │  Instance  │  │  Instance  │
    │     1      │  │     2      │  │     3      │
    └──────┬─────┘  └──────┬─────┘  └──────┬─────┘
           │                │               │
           └────────────────┼───────────────┘
                            │
            ┌───────────────┼───────────────┐
            │               │               │
            ▼               ▼               ▼
    ┌──────────────┐  ┌──────────────┐  ┌─────────────┐
    │ SQL Server   │  │  RabbitMQ    │  │ Blob        │
    │ (Primary &   │  │  (5 nodes)   │  │ Storage     │
    │  Replicas)   │  │              │  │ (Premium)   │
    └──────────────┘  └──────────────┘  └─────────────┘
            │
            ▼
    ┌──────────────┐
    │ Monitoring & │
    │ Logging      │
    │ (ELK/Serilog)│
    └──────────────┘
```

## 📈 Scalability Considerations

### Horizontal Scaling
- **Stateless API**: Scale API instances independently
- **Connection Pooling**: SQL Server connections pooled efficiently
- **Message Queue**: RabbitMQ distributes events across consumers
- **Azure Functions**: Auto-scale based on workload

### Caching Strategy
```
Request
   │
   ▼
Check Redis Cache (if implemented)
   │
   ├─ Cache Hit → Return cached response
   │
   └─ Cache Miss → Query Database
                     │
                     ▼
                  Get from DB
                     │
                     ▼
                  Cache Result (TTL)
                     │
                     ▼
                  Return Response
```

### Performance Optimization
- ✅ Dapper for high-volume queries
- ✅ Connection pooling in SQL Server
- ✅ Indexed queries (APPR_MODULE, EMP_SYSID)
- ✅ Async/await for non-blocking I/O
- ✅ Circuit breaker for external services

## 🛡️ Resilience Patterns

### Circuit Breaker
```
Normal State
   │
   ├─ Failure Threshold Exceeded
   │
   ▼
Open State (fail fast)
   │
   └─ Timeout → Half-Open
                  │
                  ├─ Success → Closed
                  │
                  └─ Failure → Open
```

### Retry Policy
```
Request Fails
   │
   └─ Retry with Exponential Backoff
      - Attempt 1: immediate
      - Attempt 2: 2 seconds
      - Attempt 3: 4 seconds
      - Attempt 4: 8 seconds
      - Max Attempts: 3
```

### Health Checks
```
Health Check Endpoint
   │
   ├─ SQL Server connectivity
   ├─ RabbitMQ connectivity
   ├─ Azure Blob Storage
   │
   └─ Return status (Healthy/Degraded/Unhealthy)
```

## 📝 Data Flow Example: Create Approval Master

```
1. Client sends HTTP POST request
   POST /api/approvals
   Authorization: Bearer {token}
   {
     "code": "TRAVEL_APR",
     "name": "Travel Request",
     "module": "PER",
     "level": 3
   }

2. API Layer
   - Authenticates token
   - Deserializes DTO
   - Routes to handler

3. Application Layer
   - Validates command (FluentValidation)
   - Logs request
   - Invokes command handler

4. Domain Layer
   - ApprovalMaster.Create() (domain logic)
   - Raises ApprovalMasterCreatedEvent
   - Validates business rules
   - Aggregates are populated

5. Infrastructure Layer
   - Repository adds aggregate
   - EF Core translates to SQL
   - Database transaction begins
   - INSERT into APPR_MAST
   - Transaction commits

6. Event Publishing
   - Domain events extracted
   - Serialized to JSON
   - Published to RabbitMQ
   - Routing key: approval.master.created

7. Response
   - Returns HTTP 201 Created
   - Response body contains created resource
   - ID and metadata included

8. Message Consumers
   - Approval Master Event Consumer receives message
   - Processes in background
   - Updates denormalized data if needed
   - Sends notifications to other services
```

## 🎯 Key Design Principles

1. **Single Responsibility Principle**: Each class has one reason to change
2. **Dependency Inversion**: Depend on abstractions, not implementations
3. **Don't Repeat Yourself**: Shared logic in utility classes/services
4. **SOLID Principles**: Applied throughout the architecture
5. **Clean Code**: Self-documenting, well-structured code
6. **Testability**: Dependency injection for unit testing
7. **Maintainability**: Clear separation of concerns
8. **Scalability**: Stateless design, async operations
9. **Security**: JWT tokens, input validation, parameterized queries
10. **Monitoring**: Structured logging, health checks, correlations

---

**This architecture ensures:**
- ✅ High availability through redundancy
- ✅ Performance through optimization
- ✅ Security through authentication & authorization
- ✅ Maintainability through clean architecture
- ✅ Scalability through stateless design
- ✅ Resilience through retry & circuit breaker patterns
