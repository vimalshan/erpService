# Vendor Services - Folder Structure

## Project Overview
Vendor Services is a microservice component of the ERP system responsible for managing vendor-related operations. It follows a clean architecture pattern with separation of concerns across multiple layers.

---

## 📁 Root Level Structure

```
vendorServices/
├── docker-compose.yml          # Docker Compose configuration for local development
├── ReadMe.md                   # Service documentation and quick start guide
├── VendorService.slnx          # Solution file for the service
├── Docker/                     # Docker containerization
├── k8s/                        # Kubernetes deployment configuration
├── scripts/                    # Deployment and utility scripts
├── src/                        # Application source code
├── tests/                      # Test projects
└── VENDOR/                     # Database scripts and schemas
```

---

## 📦 Docker Configuration
**Location:** `Docker/`

```
Docker/
├── Dockerfile                  # Container image definition
├── entrypoint.sh              # Container startup script
└── init-database.sql          # Database initialization script
```

**Purpose:** Contains all Docker-related files for containerizing the service.

---

## ☸️ Kubernetes Configuration
**Location:** `k8s/`

```
k8s/
├── configmap.yaml             # Configuration management for K8s
├── deployment.yaml            # Service deployment specification
├── hpa.yaml                   # Horizontal Pod Autoscaler configuration
├── ingress.yaml               # Network ingress rules
├── rbac.yaml                  # Role-based access control
├── secret.yaml                # Sensitive configuration storage
└── service.yaml               # Service exposure and networking
```

**Purpose:** Kubernetes manifests for production deployment and orchestration.

---

## 🚀 Scripts
**Location:** `scripts/`

```
scripts/
└── run_vendorServices.sh       # Shell script to run the service
```

**Purpose:** Contains utility and deployment scripts for the service.

---

## 💾 Database Scripts
**Location:** `VENDOR/`

```
VENDOR/
├── VENDOR-Procedures.sql       # Stored procedures for vendor operations
├── VENDOR-Tables.sql           # Table definitions and schema
├── VENDOR-Triggers.sql         # Database triggers
├── VENDORDB-DEPLOYMENT.sql     # Complete deployment script
└── VendorDB.sql                # Database creation and initialization
```

**Purpose:** All SQL scripts for database setup and management.

---

## 📂 Source Code Structure
**Location:** `src/`

### VendorService.API
The main API project serving HTTP requests.

```
src/VendorService.API/
├── appsettings.json                # Default configuration
├── appsettings.Development.json    # Development-specific settings
├── appsettings.Production.json     # Production-specific settings
├── Program.cs                      # Application startup and configuration
├── VendorService.API.csproj        # Project file with dependencies
├── VendorService.API.http          # HTTP request examples (for testing)
├── bin/                            # Compiled binaries
│   ├── Debug/
│   └── Release/
├── Controllers/
│   ├── AuthController.cs           # Authentication endpoints
│   └── VendorsController.cs        # Vendor REST API endpoints
├── GraphQL/
│   ├── VendorMutation.cs           # GraphQL mutations for vendors
│   └── VendorQuery.cs              # GraphQL queries for vendors
├── Middleware/
│   └── ExceptionMiddleware.cs      # Global exception handling
├── MinimalApis/
│   └── VendorEndpoints.cs          # Minimal API endpoint definitions
├── obj/                            # Build artifacts
│   ├── project.assets.json
│   ├── VendorService.API.csproj.nuget.dgspec.json
│   ├── VendorService.API.csproj.nuget.g.props
│   ├── VendorService.API.csproj.nuget.g.targets
│   ├── Debug/
│   └── Release/
└── Properties/
    └── launchSettings.json         # Visual Studio launch configuration
```

**Key Components:**
- **Controllers:** Traditional REST API endpoints
- **GraphQL:** GraphQL API support with queries and mutations
- **MinimalApis:** Lightweight endpoint definitions (ASP.NET Core 6+)
- **Middleware:** Cross-cutting concerns like exception handling

---

### VendorService.Application
Business logic and use cases (CQRS pattern).

```
src/VendorService.Application/
├── Class1.cs                       # Placeholder or base class
├── DependencyInjection.cs          # Service registration for DI container
├── VendorService.Application.csproj
├── Behaviours/
│   ├── LoggingBehaviour.cs         # Request logging pipeline behavior
│   └── ValidationBehaviour.cs      # Input validation pipeline behavior
├── Commands/                       # CQRS command handlers
├── DTOs/                           # Data Transfer Objects
├── Mappings/                       # AutoMapper profiles
├── Queries/                        # CQRS query handlers
├── Validators/                     # FluentValidation validators
└── bin/, obj/                      # Build artifacts
```

**Purpose:** Contains business logic, CQRS commands/queries, and validation rules.

---

### VendorService.Domain
Core business entities and domain logic (Domain-Driven Design).

```
src/VendorService.Domain/
├── Class1.cs                       # Placeholder or base class
├── VendorService.Domain.csproj
├── Common/                         # Shared domain classes
├── Entities/                       # Domain entities (Vendor, etc.)
├── Events/                         # Domain events
├── Exceptions/                     # Business exceptions
├── Interfaces/                     # Domain contracts
├── ValueObjects/                   # Value objects
└── bin/, obj/                      # Build artifacts
```

**Purpose:** Pure business domain with no external dependencies.

---

### VendorService.Infrastructure
External service integration and data access.

```
src/VendorService.Infrastructure/
├── Class1.cs                       # Placeholder or base class
├── DependencyInjection.cs          # Infrastructure service registration
├── VendorService.Infrastructure.csproj
├── Data/                           # Database context and migrations
├── Messaging/                      # Message queue integration (RabbitMQ, etc.)
├── Repositories/                   # Data access patterns
├── Resilience/                     # Polly policies, circuit breakers
├── Storage/                        # File storage integration (S3, Blob, etc.)
└── bin/, obj/                      # Build artifacts
```

**Purpose:** Handles external integrations, database access, and infrastructure concerns.

---

### VendorService.Functions
Azure Functions or background processing.

```
src/VendorService.Functions/
├── .gitignore
├── host.json                       # Functions runtime configuration
├── local.settings.json             # Local setting overrides
├── Program.cs                      # Function app startup
├── TdsProcessingFunction.cs        # Background TDS processing
├── VendorCleanupFunction.cs        # Vendor data cleanup
├── VendorService.Functions.csproj
├── Properties/
│   └── launchSettings.json         # Local debug settings
└── bin/, obj/                      # Build artifacts
```

**Purpose:** Serverless function implementations for asynchronous tasks.

---

## 🧪 Test Structure
**Location:** `tests/`

### Unit Tests
```
tests/VendorService.UnitTests/
├── UnitTest1.cs                    # Template/example test
├── VendorService.UnitTests.csproj
├── Commands/                       # Command handler tests
├── Domain/                         # Domain logic tests
├── Queries/                        # Query handler tests
└── bin/, obj/                      # Build artifacts
```

**Purpose:** Test business logic in isolation without external dependencies.

---

### Integration Tests
```
tests/VendorService.IntegrationTests/
├── UnitTest1.cs                    # Template/example test
├── VendorsApiIntegrationTests.cs   # Full API integration tests
├── VendorService.IntegrationTests.csproj
└── bin/, obj/                      # Build artifacts
```

**Purpose:** Test API endpoints and interactions with real services.

---

## 🏗️ Architecture Layers

```
┌─────────────────────────────────────────┐
│   VendorService.API                    │  HTTP/REST/GraphQL Interface
├─────────────────────────────────────────┤
│   Controllers, GraphQL, MinimalApis     │  API Endpoints
├─────────────────────────────────────────┤
│   VendorService.Application            │  Business Logic (CQRS)
│   Commands, Queries, Behaviours         │
├─────────────────────────────────────────┤
│   VendorService.Domain                 │  Pure Business Entities
│   Entities, Events, ValueObjects        │
├─────────────────────────────────────────┤
│   VendorService.Infrastructure         │  Data & External Services
│   Repositories, Messaging, Storage      │
└─────────────────────────────────────────┘
```

---

## 📋 Key Files Summary

| File | Purpose |
|------|---------|
| `docker-compose.yml` | Local development environment setup |
| `VendorService.slnx` | Visual Studio solution configuration |
| `Program.cs` (API) | Application startup and dependency injection |
| `appsettings.*.json` | Environment-specific configuration |
| `Dockerfile` | Container image build instructions |
| `VENDOR/*.sql` | Database schema and procedures |
| `k8s/*.yaml` | Kubernetes deployment manifests |

---

## 🔄 Dependency Flow

```
VendorService.API (Presentation)
    ↓
VendorService.Application (Business Logic)
    ↓
VendorService.Domain (Core Business Rules)
    ↓
VendorService.Infrastructure (Data Access)
```

Each layer depends on the layers below it, but NOT on layers above it (Dependency Inversion).

---

## 🚀 Quick Navigation

- **Want to add API endpoints?** → `src/VendorService.API/Controllers/` or `MinimalApis/`
- **Need to add business logic?** → `src/VendorService.Application/Commands/` or `Queries/`
- **Adding domain entities?** → `src/VendorService.Domain/Entities/`
- **Data access?** → `src/VendorService.Infrastructure/Repositories/`
- **Configuration?** → `appsettings.json` or `k8s/configmap.yaml`
- **Database changes?** → `VENDOR/` SQL scripts
- **Deployment?** → `Docker/` or `k8s/`

---

## 📝 Development Notes

- The service uses **Clean Architecture** with clear separation of concerns
- **CQRS pattern** is implemented in the Application layer
- **Dependency Injection** is configured in `DependencyInjection.cs` files
- **GraphQL** support is available alongside REST APIs
- **Minimal APIs** provide lightweight endpoint definitions
- **Resilience patterns** are configured in Infrastructure layer
- **Database-first** approach with SQL scripts in VENDOR folder
- **Docker & Kubernetes** ready for containerized deployment
