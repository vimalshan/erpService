# System Architecture Overview

## 🏗️ Layered Architecture Diagram

```
┌────────────────────────────────────────────────────────────────┐
│                         CLIENT LAYER                           │
│  (Web Browser, Mobile App, Third-party Systems)                │
└────────────────────────┬─────────────────────────────────────────┘
                         │
           ┌─────────────┴────────────────┐
           │                              │
    ┌──────▼────────┐            ┌───────▼──────────┐
    │  REST API     │            │  GraphQL API     │
    │  Controllers  │            │  (Hot Chocolate) │
    │               │            │                  │
    │ • Locations   │            │ • Queries        │
    │ • Rooms       │            │ • Mutations      │
    │ • Resources   │            │                  │
    └──────┬────────┘            └──────────┬───────┘
           │                                 │
           └─────────────────┬───────────────┘
                             │
                    ┌────────▼────────┐
                    │  MIDDLEWARE     │
                    │                 │
                    │ • JWT Auth      │
                    │ • Exception     │
                    │   Handling      │
                    │ • CORS          │
                    │ • Logging       │
                    └────────┬────────┘
                             │
           ┌─────────────────┼─────────────────┐
           │                 │                 │
    ┌──────▼────────┐  ┌──────▼──────┐  ┌────▼──────────┐
    │  COMMANDS     │  │  QUERIES    │  │  MEDIATOR    │
    │  (Write Ops)  │  │  (Read Ops) │  │  (CQRS)      │
    │               │  │             │  │               │
    │ • Create*     │  │ • GetById*  │  │ Validates &   │
    │ • Update*     │  │ • GetAll*   │  │ Routes Reqs   │
    │ • Delete*     │  │ • Search*   │  │               │
    │ • ChangeStatus│  │           │  │               │
    └──────┬────────┘  └──────┬──────┘  └────────────────┘
           │                  │
           └──────────┬───────┘
                      │
          ┌───────────▼──────────────┐
          │  APPLICATION LAYER       │
          │  (Business Logic)        │
          │                          │
          │ • Command Handlers       │
          │ • Query Handlers         │
          │ • DTOs                   │
          │ • AutoMapper             │
          │ • Validators             │
          │ • Event Handlers         │
          │                          │
          └───────────┬──────────────┘
                      │
          ┌───────────▼──────────────────────┐
          │  DOMAIN LAYER (DDD)              │
          │  (Core Business Rules)           │
          │                                  │
          │ ┌──────────────────────────────┐ │
          │ │ Aggregates:                  │ │
          │ │ • LocationAggregate          │ │
          │ │ • RoomAggregate              │ │
          │ │ • RoomResourceAggregate      │ │
          │ └──────────────────────────────┘ │
          │ ┌──────────────────────────────┐ │
          │ │ Value Objects:               │ │
          │ │ • Address                    │ │
          │ │ • Contact                    │ │
          │ │ • Status                     │ │
          │ └──────────────────────────────┘ │
          │ ┌──────────────────────────────┐ │
          │ │ Domain Events:               │ │
          │ │ • LocationCreated            │ │
          │ │ • RoomUpdated                │ │
          │ │ • ResourceQuantityChanged    │ │
          │ └──────────────────────────────┘ │
          │                                  │
          └───────────┬──────────────────────┘
                      │
          ┌───────────▼──────────────────────┐
          │  INFRASTRUCTURE LAYER            │
          │  (Data Access & External Svcs)   │
          │                                  │
          │ ┌──────────────────────────────┐ │
          │ │ Data Access:                 │ │
          │ │ • LocationRepository         │ │
          │ │ • RoomRepository             │ │
          │ │ • ResourceRepository         │ │
          │ │ • UnitOfWork                 │ │
          │ └──────────────────────────────┘ │
          │ ┌──────────────────────────────┐ │
          │ │ External Services:           │ │
          │ │ • RabbitMQ (Messaging)       │ │
          │ │ • Blob Storage (Files)       │ │
          │ │ • Dapper (SQL Queries)       │ │
          │ │ • Polly (Resilience)         │ │
          │ │ • Redis (Cache)              │ │
          │ └──────────────────────────────┘ │
          │ ┌──────────────────────────────┐ │
          │ │ Database Context:            │ │
          │ │ • LocationServiceDbContext   │ │
          │ │ • EF Core Mappings           │ │
          │ │ • Migrations                 │ │
          │ └──────────────────────────────┘ │
          │                                  │
          └───────────┬──────────────────────┘
                      │
           ┌──────────┴──────────────────────┐
           │                                  │
     ┌─────▼──────┐  ┌───────────────┐  ┌───▼──────────┐
     │ SQL Server │  │    RabbitMQ   │  │ Azure Blob   │
     │ (LocalDB)  │  │   (Events)    │  │ Storage      │
     │            │  │               │  │ (Files)      │
     │ • Locations│  │ • Event Broker│  │ • Images     │
     │ • Rooms    │  │ • Consumers   │  │ • Documents  │
     │ • Resources│  │ • Replay      │  │              │
     └────────────┘  └───────────────┘  └──────────────┘
           │                                  │
           │          ┌──────────────────────┘
           │          │
           │    ┌─────▼─────────┐
           │    │ Azure Services│
           │    │               │
           │    │ • Functions   │
           │    │ • App Service │
           │    │ • App Insights│
           │    └───────────────┘
           │
     ┌─────▼────────────────┐
     │  OPTIONAL SERVICES   │
     │                      │
     │  • Redis Cache       │
     │  • Auth Service      │
     │  • Notification Svc  │
     └──────────────────────┘
```

---

## 🔄 Data Flow Diagrams

### Request/Response Flow
```
Client Request
    │
    ▼
HTTP Controller (Swagger/REST)
    │
    ▼
Authorization Middleware (JWT)
    │
    ▼
MediatR Request Pipeline
    │
    ├─► Logging Behavior (log incoming request)
    │
    ├─► Validation Behavior (validate command)
    │
    ▼
Command/Query Handler
    │
    ▼
Domain Logic (Aggregates)
    │
    ├─► Raise Domain Events
    │
    └─► Update State
    │
    ▼
Infrastructure (Repositories)
    │
    ▼
EF Core / Database
    │
    ▼
Processed Result
    │
    ├─► AutoMapper (Entity → DTO)
    │
    ▼
Response (JSON)
    │
    ▼
Client Response
```

### Event Publishing Flow
```
Domain Change (Create/Update)
    │
    ▼
Domain Event Raised
    │
    ▼
Event Added to Entity.DomainEvents
    │
    ▼
Repository.SaveChanges()
    │
    ▼
Unit of Work Control
    │
    ▼
MediatR Publishes Events
    │
    ▼
Event Handlers Execute
    │
    ├─► Log Event
    │
    ├─► Publish to RabbitMQ
    │
    ├─► Update Related Data
    │
    ├─► Send Notifications
    │
    └─► External System Integration
```

### Resilience Pattern
```
External API Call
    │
    ▼
──────────────────────────────────────────
│ Polly Policy #1: Timeout (10s)         │
│                                         │
│ ┌────────────────────────────────────┐ │
│ │ Polly Policy #2: Retry (3x)        │ │
│ │ with exponential backoff           │ │
│ │                                    │ │
│ │ ┌──────────────────────────────┐  │ │
│ │ │ Polly Policy #3:             │  │ │
│ │ │ Circuit Breaker              │  │ │
│ │ │ (Fail after 3 attempts)      │  │ │
│ │ │ Break for 30 seconds         │  │ │
│ │ └──────────────────────────────┘  │ │
│ └────────────────────────────────────┘ │
└──────────────────────────────────────────
    │
    ├─► Success (200-299)
    │    └─► Return Response
    │
    ├─► Transient Error
    │    └─► Retry with Backoff
    │
    └─► Permanent Error
         └─► Circuit Open (cached response/error)
```

---

## 📦 Dependency Flow

```
LocationService.API
    ├─► Depends On: LocationService.Application
    ├─► Depends On: LocationService.Infrastructure
    └─► Depends On: LocationService.Domain

LocationService.Application
    ├─► Depends On: LocationService.Domain
    ├─► Uses: MediatR, AutoMapper, FluentValidation
    └─► Implements: Commands, Queries, DTOs

LocationService.Infrastructure
    ├─► Depends On: LocationService.Domain
    ├─► Depends On: LocationService.Application
    ├─► Uses: EF Core, Dapper, RabbitMQ, Azure SDK, Polly, Redis
    └─► Implements: Repositories, UnitOfWork, External Services

LocationService.Domain
    ├─► Has NO external dependencies
    ├─► Uses ONLY: System libraries
    └─► Implements: Entities, Aggregates, Value Objects, Events, Exceptions
```

---

## 🔗 Service Integration Points

### RabbitMQ Integration
```
Domain Event Raised
    │
    ▼
Event Handler (Application)
    │
    ▼
RabbitMqMessagePublisher
    │
    ▼
RabbitMQ Exchange
    │
    ├─► Consumer #1: Analytics Service
    ├─► Consumer #2: Notification Service
    ├─► Consumer #3: Audit Service
    └─► Consumer #4: Azure Function
```

### Caching Strategy
```
Find in Cache
    │
    ├─► Cache Hit: Return cached value
    │
    └─► Cache Miss:
        │
        ▼
        Query Database
        │
        ▼
        Set in Cache (with TTL)
        │
        ▼
        Return value
```

### Authentication Flow
```
User Login
    │
    ▼
Verify Credentials
    │
    ▼
Generate JWT Token
    │
    ├─► Claims: UserId, Email, Roles
    ├─► Signing: HS256
    ├─► Expiry: Configurable
    │
    ▼
Return Token to Client
    │
    ▼
Client Adds to Authorization Header
    │
    ▼
Each Request Validated by JWT Middleware
    │
    └─► Valid: Process request
    └─► Invalid: Return 401 Unauthorized
```

---

## 🗂️ Database Relationships

```
┌──────────────────────┐
│  LOCATION_CONTACT    │
│ ────────────────────│
│ • LOCATION_ID (PK)  │
│ • LOCATION_CODE (U) │
│ • LOCATION_NAME     │
│ • CITY              │
│ • STATE             │
│ • COUNTRY           │
│ • STATUS            │
│ • CREATED_ON        │
│ • CREATED_BY        │
│ • UPDATED_ON        │
│ • UPDATED_BY        │
└──────────────────────┘
         │
         │ 1:N
         │
         ▼
┌──────────────────────┐
│    ROOM_MAST         │
│ ────────────────────│
│ • ROOM_ID (PK)      │
│ • LOCATION_ID (FK)  │
│ • ROOM_CODE         │
│ • ROOM_NAME         │
│ • ROOM_CAPACITY     │
│ • ROOM_TYPE         │
│ • FLOOR_NUMBER      │
│ • STATUS            │
│ • CREATED_ON        │
│ • CREATED_BY        │
│ • UPDATED_ON        │
│ • UPDATED_BY        │
└──────────────────────┘
         │
         │ 1:N
         │
         ▼
┌──────────────────────┐
│   ROOM_RESOURCE      │
│ ────────────────────│
│ • RESOURCE_ID (PK)  │
│ • ROOM_ID (FK)      │
│ • LOCATION_ID (FK)  │
│ • RESOURCE_CODE     │
│ • RESOURCE_NAME     │
│ • RESOURCE_TYPE     │
│ • RESOURCE_QUANTITY │
│ • STATUS            │
│ • CREATED_ON        │
│ • CREATED_BY        │
│ • UPDATED_ON        │
│ • UPDATED_BY        │
└──────────────────────┘
```

---

## 🔐 Security Model

```
Incoming Request
    │
    ▼
HTTPS Encryption
    │
    ▼
Authorization Header Check
    │
    ├─► Missing/Invalid: Return 401
    │
    ▼
JWT Token Validation
    │
    ├─► Signature: Verify using secret key
    ├─► Issuer: Verify matches configuration
    ├─► Audience: Verify matches configuration
    ├─► Expiry: Check not expired
    │
    ├─► Invalid: Return 401 Unauthorized
    │
    ▼
Extract Claims (UserId, Email, Roles)
    │
    ▼
Authorize Based on Roles
    │
    ├─► Insufficient: Return 403 Forbidden
    │
    ▼
Process Request with User Context
    │
    ▼
Audit Log User Action
```

---

**Architecture Version**: 1.0  
**Last Updated**: March 15, 2026
