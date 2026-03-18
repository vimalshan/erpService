# Email Notification Microservice - Implementation Guide

## Project Status: COMPLETED CORE ARCHITECTURE

###建立Date: March 12, 2026
**Solution Path**: `E:\ERPMicroservice\src\Services\cashServices\emailnotificationServices\EmailNotification\`

---

## ✅ COMPLETED COMPONENTS

### 1. **Solution & Project Structure**
- ✅ Visual Studio Solution (`EmailNotificationService.slnx`)
- ✅ Domain Layer Project (`EmailNotification.Domain`)
- ✅ Application Layer Project (`EmailNotification.Application`)
- ✅ Infrastructure Layer Project (`EmailNotification.Infrastructure`)
- ✅ API Layer Project (`EmailNotification.API`)
- ✅ Project-to-Project References configured

### 2. **Domain Layer (EmailNotification.Domain)**
#### Entities & Aggregates:
- ✅ `Entity.cs` - Base entity class with audit fields (CreatedBy, ModifiedBy, CreatedAt, ModifiedAt)
- ✅ `IDomainEvent.cs` - Domain event interface
- ✅ `ValueObject.cs` - Base value object class with equality comparison
- ✅ `EmailTypeAggregate.cs` - Root aggregate for email type management
- ✅ `MailAccess.cs` - Entity for email recipients

#### Value Objects:
- ✅ `EmailAddress.cs` - Email address value object with validation
- ✅ `EmailTypeEnum.cs` - Enumeration for Daily (D) and Event (E) email types

#### Domain Events:
- ✅ `EmailTypeCreatedEvent.cs` - Triggered when email type is created
- ✅ `EmailTypeUpdatedEvent.cs` - Triggered when email type is updated
- ✅ `RecipientAddedEvent.cs` - Triggered when recipient is added

#### Repositories (Interfaces):
- ✅ `IEmailTypeRepository.cs` - Repository contract for email types
- ✅ `IMailAccessRepository.cs` - Repository contract for mail recipients

### 3. **Application Layer (EmailNotification.Application)**
#### DTOs (Data Transfer Objects):
- ✅ `EmailAddressDto.cs` - DTO for email addresses
- ✅ `EmailTypeDto.cs` - DTO for email types
- ✅ `MailAccessDto.cs` - DTO for mail recipients

#### CQRS Commands:
- ✅ `CreateEmailTypeCommand.cs` - Command to create email type
- ✅ `UpdateEmailTypeCommand.cs` - Command to update email type
- ✅ `AddRecipientCommand.cs` - Command to add recipient
- ✅ `RemoveRecipientCommand.cs` - Command to remove recipient

#### CQRS Queries:
- ✅ `GetEmailTypeByIdQuery.cs` - Query to get email type by ID
- ✅ `GetAllEmailTypesQuery.cs` - Query to get all email types
- ✅ `GetEmailTypesByTypeQuery.cs` - Query to get email types by type (D/E)
- ✅ `GetRecipientsByOrgAndBusinessQuery.cs` - Query to get recipients by org/business

#### Command Handlers:
- ✅ `CreateEmailTypeCommandHandler.cs` - Handles email type creation
- ✅ `UpdateEmailTypeCommandHandler.cs` - Handles email type updates
- ✅ `AddRecipientCommandHandler.cs` - Handles recipient addition
- ✅ `RemoveRecipientCommandHandler.cs` - Handles recipient removal

#### Query Handlers:
- ✅ `GetEmailTypeByIdQueryHandler.cs` - Handles email type retrieval
- ✅ `GetAllEmailTypesQueryHandler.cs` - Handles listing all email types
- ✅ `GetEmailTypesByTypeQueryHandler.cs` - Handles filtering by type
- ✅ `GetRecipientsByOrgAndBusinessQueryHandler.cs` - Handles recipient filtering

#### Validators (FluentValidation):
- ✅ `CreateEmailTypeCommandValidator.cs` - Validates email type creation
- ✅ `UpdateEmailTypeCommandValidator.cs` - Validates email type updates
- ✅ `AddRecipientCommandValidator.cs` - Validates recipient addition

#### Infrastructure:
- ✅ `MappingProfile.cs` - AutoMapper configuration
- ✅ `ServiceCollectionExtensions.cs` - DI registration for application layer

### 4. **Infrastructure Layer (EmailNotification.Infrastructure)**
#### Database Context:
- ✅ `EmailNotificationDbContext.cs` - Entity Framework DbContext with:
  - Initial configuration for `EMAIL_TYPEMAST` table
  - Complex property mapping for `EmailTypeEnum`
  - Value object conversion for `EmailAddress`
  - Configuration for `MAIL_ACCESS` table
  - Foreign key relationships
  - Database indexes

#### Repositories:
- ✅ `EmailTypeRepository.cs` - Implementation for IEmailTypeRepository
- ✅ `MailAccessRepository.cs` - Implementation for IMailAccessRepository

#### DI Configuration:
- ✅ `ServiceCollectionExtensions.cs` - Registers DbContext and repositories

### 5. **API Layer (EmailNotification.API)**
#### Controllers:
- ✅ `BaseApiController.cs` - Base controller with MediatR injection
- ✅ `EmailTypesController.cs` - REST API endpoints for email types:
  - `GET /api/emailtypes` - Get all email types
  - `GET /api/emailtypes/{id}` - Get email type by ID
  - `GET /api/emailtypes/bytype/{emailType}` - Get by type (D/E)
  - `POST /api/emailtypes` - Create new email type
  - `PUT /api/emailtypes/{id}` - Update email type
  
- ✅ `MailAccessController.cs` - REST API endpoints for recipients:
  - `GET /api/emailtypes/{id}/recipients/byorg` - Get recipients by org/business
  - `POST /api/emailtypes/{id}/recipients` - Add recipient
  - `DELETE /api/emailtypes/{id}/recipients/{mailAccessId}` - Remove recipient

#### Middleware:
- ✅ `ExceptionHandlingMiddleware.cs` - Global exception handling with HTTP status mapping

#### Configuration:
- ✅ `Program.cs` - Complete startup configuration including:
  - CORS policy ("AllowAll")
  - Application and Infrastructure service registration
  - Health checks (Database availability)
  - JWT Bearer authentication (configured but not enforced in dev)
  - Authorization middleware
  - Exception handling middleware
  
- ✅ `appsettings.json` - Configuration file with:
  - Database connection string (localdb)
  - JWT settings
  - RabbitMQ settings
  - Azure Blob Storage settings

---

## 🔧 NuGet PACKAGES INSTALLED

### Domain Layer:
- `MediatR` - Event publishing and CQRS patterns

### Application Layer:
- `MediatR` - Request/response pattern implementation
- `AutoMapper` - Object-to-object mapping
- `AutoMapper.Extensions.Microsoft.DependencyInjection` - DI integration
- `FluentValidation` - Data validation
- `FluentValidation.AspNetCore` - ASP.NET Core validation integration

### Infrastructure Layer:
- `Microsoft.EntityFrameworkCore.SqlServer` - EF Core SQL Server provider
- `Microsoft.EntityFrameworkCore.Tools` - EF Core command-line tools
- `Dapper` - Lightweight ORM (ready for use)
- `MediatR` - Event dispatch

### API Layer:
- `MediatR` - Request handling
- `Polly` - Resilience patterns (Circuit Breaker ready)
- `AspNetCore.HealthChecks.SqlServer` - Database health checks
- `System.IdentityModel.Tokens.Jwt` - JWT token support
- `Microsoft.AspNetCore.Authentication.JwtBearer` - JWT authentication
- `Microsoft.EntityFrameworkCore.Design` - EF Core tools support

---

## 📊 BUILD STATUS

✅ **Solution builds successfully with no errors**

```
EmailNotification.Domain net10.0 succeeded
EmailNotification.Application net10.0 succeeded (1 warning - AutoMapper version)
EmailNotification.Infrastructure net10.0 succeeded (1 warning - AutoMapper version)
EmailNotification.API net10.0 succeeded (1 warning - AutoMapper version)
Build succeeded with 6 warning(s)
```

⚠️ **Warnings are non-critical** (AutoMapper version mismatch between direct and transitive dependencies)

---

## 📋 DATABASE SCHEMA MAPPING

### EMAIL_TYPEMAST Table
| Domain Property | Column Name | Type | Notes |
|---|---|---|---|
| Id | EMAIL_TYPEID | BIGINT | Primary Key |
| EmailName | EMAIL_NAME | VARCHAR(500) | Email alert name |
| EmailType | EMAIL_TYPE | CHAR(1) | D=Daily, E=Event |
| EmailProcName | EMAIL_PRCNAME | VARCHAR(100) | Procedure name |
| ModifiedBy | EMAIL_MODIFIEDBY | DECIMAL(19,0) | User ID |
| ModifiedAt | EMAIL_MODIFIEDON | DATETIME2(3) | Timestamp |

### MAIL_ACCESS Table
| Domain Property | Column Name | Type | Notes |
|---|---|---|---|
| Id | MAIL_ACCESSID | BIGINT | Primary Key |
| MailTypeId | MAIL_TYPEID | BIGINT | Foreign Key |
| MailOrgId | MAIL_ORGID | BIGINT | Organization (0=All) |
| MailBusinessId | MAIL_BUSINESSID | BIGINT | Business Unit (0=All) |
| MailEmpSysId | MAIL_EMPSYSID | BIGINT | Employee ID |
| MailEmail | MAIL_EMAILID | VARCHAR(200) | Email address |
| MailName | MAIL_NAME | VARCHAR(100) | Non-emp name |
| ModifiedBy | MAIL_MODIFIEDBY | DECIMAL(19,0) | User ID |
| ModifiedAt | MAIL_MODIFIEDON | DATETIME2(3) | Timestamp |

---

## 🚀 NEXT STEPS (REMAINING TASKS)

### 1. **EF Core Migrations** ⏳
- Fix DbContext constructor binding issue
- Generate and apply `InitialCreate` migration
- Create database schema based on existing SQL

### 2. **Database Integration** ⏳
- Connection String: `Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EmailNotificationDb;...`
- Run EF migrations to create database
- Import existing data from CASHDB

### 3. **JWT Authentication & Authorization** 🔄
- Configure JWT token generation endpoint
- Implement token validation and claims extraction
- Add role-based access control (RBAC)
- Secure controllers with `[Authorize]` attributes

### 4. **RabbitMQ Message Consumers** 🔄
- Create message consumer classes
- Implement event handlers for domain events
- Configure RabbitMQ connection settings
- Add message retry and dead-letter queue handling

### 5. **Azure Functions** 🔄
- Create Azure Function project
- Implement background email sending logic
- Add timer-triggered functions for daily alerts
- Integrate with Event-based alert triggers

### 6. **Azure Blob Storage** 🔄
- Create Blob Storage service/repository
- Implement image upload for email stationery
- Add download/access logic
- Configure SAS tokens for secure access

### 7. **Polly Circuit Breaker** 🔄
- Create resilience policies for external calls
- Implement retry logic with exponential backoff
- Add circuit breaker for dependent services
- Configure timeout and bulkhead patterns

### 8. **Health Checks** ✅ (Configured)
- Endpoint: `/health`
- Currently checks: Database availability
- Add checks for: RabbitMQ, Azure Storage, External APIs

### 9. **Domain Events Processing** ✅ (Architecture Ready)
- Implement domain event handlers
- Configure event bus (RabbitMQ or In-Memory)
- Add event sourcing if needed

### 10. **Additional Features**
- Swagger/OpenAPI documentation (package removed due to .NET 10 issues)
- GraphQL support (ready to add)
- Minimal APIs for lightweight endpoints
- Request/Response logging
- Performance monitoring

---

## 🏗️ ARCHITECTURE OVERVIEW

```
┌─────────────────────────────────────────────────────────────┐
│                    API Layer                                 │
│  ┌─────────────────────────────────────────────────────┐   │
│  │ Controllers (REST API)                              │   │
│  │ - EmailTypesController                              │   │
│  │ - MailAccessController                              │   │
│  └─────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
                           │
                    Mediator (MediatR)
                           │
┌─────────────────────────────────────────────────────────────┐
│              Application Layer (CQRS)                        │
│  ┌──────────────────────────────────────────────────────┐  │
│  │ Commands & Queries          │  Handlers            │  │
│  │ - CreateEmailTypeCommand    │  - CommandHandlers   │  │
│  │ - UpdateEmailTypeCommand    │  - QueryHandlers     │  │
│  │ - GetEmailTypeByIdQuery     │                      │  │
│  └──────────────────────────────────────────────────────┘  │
│  ┌──────────────────────────────────────────────────────┐  │
│  │ Validators (FluentValidation)                       │  │
│  │ - CreateEmailTypeCommandValidator                   │  │
│  │ - UpdateEmailTypeCommandValidator                   │  │
│  └──────────────────────────────────────────────────────┘  │
│  ┌──────────────────────────────────────────────────────┐  │
│  │ Mapping (AutoMapper)                                │  │
│  │ - Domain ↔ DTO Conversions                          │  │
│  └──────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                           │
┌─────────────────────────────────────────────────────────────┐
│              Domain Layer (DDD)                              │
│  ┌──────────────────────────────────────────────────────┐  │
│  │ Aggregates & Entities                               │  │
│  │ - EmailTypeAggregate (Root)                          │  │
│  │ - MailAccess (Entity)                               │  │
│  ├──────────────────────────────────────────────────────┤  │
│  │ Value Objects                                       │  │
│  │ - EmailAddress (validated)                          │  │
│  - EmailTypeEnum (Daily/Event)                         │  │
│  ├──────────────────────────────────────────────────────┤  │
│  │ Domain Events                                       │  │
│  │ - EmailTypeCreatedEvent                             │  │
│  │ - EmailTypeUpdatedEvent                             │  │
│  │ - RecipientAddedEvent                               │  │
│  ├──────────────────────────────────────────────────────┤  │
│  │ Repository Interfaces                               │  │
│  │ - IEmailTypeRepository                              │  │
│  │ - IMailAccessRepository                             │  │
│  └──────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                           │
┌─────────────────────────────────────────────────────────────┐
│            Infrastructure Layer                              │
│  ┌──────────────────────────────────────────────────────┐  │
│  │ Data Access (EF Core)                               │  │
│  │ - EmailNotificationDbContext                        │  │
│  │ - EmailTypeRepository (Implementation)              │  │
│  │ - MailAccessRepository (Implementation)             │  │
│  └──────────────────────────────────────────────────────┘  │
│  ┌──────────────────────────────────────────────────────┐  │
│  │ Database                                            │  │
│  │ - SQL Server (LocalDB)                              │  │
│  │ - EMAIL_TYPEMAST Table                              │  │
│  │ - MAIL_ACCESS Table                                 │  │
│  └──────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
```

---

## 🔌 API ENDPOINTS

### Email Types
- `GET /api/emailtypes` - List all email types
- `GET /api/emailtypes/{id}` - Get single email type
- `GET /api/emailtypes/bytype/{emailType}` - Filter by type
- `POST /api/emailtypes` - Create new email type
- `PUT /api/emailtypes/{id}` - Update email type

### Mail Recipients
- `GET /api/emailtypes/{emailTypeId}/recipients/byorg` - Get recipients
- `POST /api/emailtypes/{emailTypeId}/recipients` - Add recipient
- `DELETE /api/emailtypes/{emailTypeId}/recipients/{mailAccessId}` - Remove recipient

### Health Check
- `GET /health` - Service health status

---

## 🔐 CONFIGURATION

**Database Connection String:**
```
Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EmailNotificationDb;Integrated Security=True;...
```

**JWT Configuration:**
- Authority: (Configure your auth server)
- Audience: emailnotification-api
- Token Expiration: 60 minutes (configurable)

**RabbitMQ:**
- Host: localhost
- Port: 5672
- Username: guest
- Password: guest

**Azure Storage:**
- Container: email-attachments
- (Configure connection string in secure vault)

---

## 📝 CODE EXAMPLES

### Creating an Email Type
```csharp
var command = new CreateEmailTypeCommand
{
    EmailName = "Daily Treasury Report",
    EmailType = "D",
    EmailProcName = "usp_GenerateTreasuryReport",
    CreatedBy = 1
};

var emailTypeId = await mediator.Send(command);
```

### Retrieving Email Types
```csharp
var query = new GetEmailTypeByIdQuery(emailTypeId);
var emailType = await mediator.Send(query);
```

### Adding a Recipient
```csharp
var command = new AddRecipientCommand
{
    EmailTypeId = 1,
    EmailAddress = "treasurer@bank.com",
    OrgId = null,  // All organizations
    CreatedBy = 1
};

var mailAccessId = await mediator.Send(command);
```

---

## 🧪 TESTING

Recommended testing approach:
1. Unit tests for domain logic
2. Integration tests for repositories
3. API integration tests for endpoints
4. Domain event handler tests

---

## 📞 Questions & Support

For implementation details, refer to:
- Domain model `EmailNotification.Domain`
- API controllers in `EmailNotification.API`
- Database schema in `06-EmailNotification_Create_Schema.sql`
- Configuration in `appsettings.json`

---

**Last Updated**: March 12, 2026
**Status**: Core Architecture Complete, Ready for Integration
