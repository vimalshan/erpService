# API Gateway Architecture & Design Patterns

## Table of Contents
1. [System Architecture](#system-architecture)
2. [Component Overview](#component-overview)
3. [Design Patterns](#design-patterns)
4. [Data Flow Diagrams](#data-flow-diagrams)
5. [Service Interactions](#service-interactions)
6. [Deployment Models](#deployment-models)
7. [Quality Attributes](#quality-attributes)

---

## System Architecture

### Three-Tier Architecture

```
┌────────────────────────────────────────────────────────────┐
│ PRESENTATION LAYER                                          │
│ Clients: Web, Mobile, Desktop, Third-party APIs            │
└────────────────────┬───────────────────────────────────────┘
                     │ HTTP/HTTPS
                     ▼
┌────────────────────────────────────────────────────────────┐
│ API GATEWAY LAYER (This Implementation)                     │
├────────────────────────────────────────────────────────────┤
│ ✓ Request Routing & Load Balancing                          │
│ ✓ Authentication & Authorization                            │
│ ✓ Rate Limiting & Throttling                                │
│ ✓ Request/Response Transformation                           │
│ ✓ Resilience (Circuit Breaker, Retry, Timeout, Bulkhead)  │
│ ✓ Logging & Monitoring                                      │
│ ✓ Caching & Performance Optimization                        │
│ ✓ Error Handling & API Consistency                          │
└────────────────────┬───────────────────────────────────────┘
                     │ Service-to-Service Communication
                     ▼
┌────────────────────────────────────────────────────────────┐
│ MICROSERVICES LAYER                                         │
├────────────────────────────────────────────────────────────┤
│  Finyear    Location    Vendor   Scholarship               │
│  Stationery    TDS       LOV      Shared                    │
│  (Each with own database & business logic)                 │
└────────────────────┬───────────────────────────────────────┘
                     │ SQL Queries
                     ▼
┌────────────────────────────────────────────────────────────┐
│ DATA LAYER                                                  │
│ Databases: SQL Server, PostgreSQL, MySQL                   │
└────────────────────────────────────────────────────────────┘
```

### Gateway Architecture Details

```
┌──────────────────────────────────────────────────┐
│ API GATEWAY (Port 5000)                           │
├──────────────────────────────────────────────────┤
│                                                   │
│ ┌─────────────────────────────────────────────┐  │
│ │ 1. ENTRY POINT                              │  │
│ │ - HTTP/HTTPS Listener                       │  │
│ │ - CORS Handler                              │  │
│ │ - Request Size Validation                   │  │
│ └─────────────────────────────────────────────┘  │
│                 │                                 │
│                 ▼                                 │
│ ┌─────────────────────────────────────────────┐  │
│ │ 2. MIDDLEWARE PIPELINE                      │  │
│ │ ┌────────────────────────────────────────┐  │  │
│ │ │ a. Correlation ID                      │  │  │
│ │ │    - Extract or generate unique ID     │  │  │
│ │ └────────────────────────────────────────┘  │  │
│ │ ┌────────────────────────────────────────┐  │  │
│ │ │ b. Request/Response Logging            │  │  │
│ │ │    - Log request details               │  │  │
│ │ │    - Measure timing                    │  │  │
│ │ │    - Log response status               │  │  │
│ │ └────────────────────────────────────────┘  │  │
│ │ ┌────────────────────────────────────────┐  │  │
│ │ │ c. Security Headers                    │  │  │
│ │ │    - Add HSTS, CSP, X-Frame-Options    │  │  │
│ │ └────────────────────────────────────────┘  │  │
│ │ ┌────────────────────────────────────────┐  │  │
│ │ │ d. Error Handling                      │  │  │
│ │ │    - Catch & normalize errors          │  │  │
│ │ └────────────────────────────────────────┘  │  │
│ │ ┌────────────────────────────────────────┐  │  │
│ │ │ e. Request Validation                  │  │  │
│ │ │    - Size checks                       │  │  │
│ │ │    - Content-Type validation           │  │  │
│ │ └────────────────────────────────────────┘  │  │
│ │ ┌────────────────────────────────────────┐  │  │
│ │ │ f. Rate Limiting                       │  │  │
│ │ │    - Per-client limits                 │  │  │
│ │ │    - Token bucket algorithm            │  │  │
│ │ └────────────────────────────────────────┘  │  │
│ └─────────────────────────────────────────────┘  │
│                 │                                 │
│                 ▼                                 │
│ ┌─────────────────────────────────────────────┐  │
│ │ 3. AUTHENTICATION & AUTHORIZATION            │  │
│ │ ┌────────────────────────────────────────┐  │  │
│ │ │ a. Authentication                      │  │  │
│ │ │    - JWT token validation              │  │  │
│ │ │    - Signature verification            │  │  │
│ │ │    - Expiration check                  │  │  │
│ │ │    - Claim extraction                  │  │  │
│ │ └────────────────────────────────────────┘  │  │
│ │ ┌────────────────────────────────────────┐  │  │
│ │ │ b. Authorization                       │  │  │
│ │ │    - Role verification                 │  │  │
│ │ │    - Scope validation                  │  │  │
│ │ │    - Permission check                  │  │  │
│ │ └────────────────────────────────────────┘  │  │
│ └─────────────────────────────────────────────┘  │
│                 │                                 │
│                 ▼                                 │
│ ┌─────────────────────────────────────────────┐  │
│ │ 4. OCELOT GATEWAY                           │  │
│ │ ┌────────────────────────────────────────┐  │  │
│ │ │ a. Route Matching                      │  │  │
│ │ │    - URL pattern matching              │  │  │
│ │ │    - HTTP method matching              │  │  │
│ │ │    - Query parameter preservation      │  │  │
│ │ └────────────────────────────────────────┘  │  │
│ │ ┌────────────────────────────────────────┐  │  │
│ │ │ b. Load Balancing                      │  │  │
│ │ │    - Round Robin selection             │  │  │
│ │ │    - Health checking                   │  │  │
│ │ │    - Instance selection                │  │  │
│ │ └────────────────────────────────────────┘  │  │
│ │ ┌────────────────────────────────────────┐  │  │
│ │ │ c. Downstream Request                  │  │  │
│ │ │    - URL transformation                │  │  │
│ │ │    - Header injection                  │  │  │
│ │ │    - Request forwarding                │  │  │
│ │ └────────────────────────────────────────┘  │  │
│ └─────────────────────────────────────────────┘  │
│                 │                                 │
│                 ▼                                 │
│ ┌─────────────────────────────────────────────┐  │
│ │ 5. HTTP CLIENT RESILIENCE                    │  │
│ │ Polly Policies:                               │  │
│ │ ┌────────────────────────────────────────┐  │  │
│ │ │ a. Retry Policy                        │  │  │
│ │ │    - 3 attempts max                    │  │  │
│ │ │    - Exponential backoff (1s→2s→4s)   │  │  │
│ │ │    - Handles transient failures        │  │  │
│ │ └────────────────────────────────────────┘  │  │
│ │ ┌────────────────────────────────────────┐  │  │
│ │ │ b. Circuit Breaker                     │  │  │
│ │ │    - Opens on 5 failures               │  │  │
│ │ │    - 30s timeout before reset          │  │  │
│ │ │    - Half-open testing mode            │  │  │
│ │ └────────────────────────────────────────┘  │  │
│ │ ┌────────────────────────────────────────┐  │  │
│ │ │ c. Timeout Policy                      │  │  │
│ │ │    - 10 second timeout                 │  │  │
│ │ │    - Prevents hanging requests         │  │  │
│ │ └────────────────────────────────────────┘  │  │
│ │ ┌────────────────────────────────────────┐  │  │
│ │ │ d. Bulkhead Policy                     │  │  │
│ │ │    - Max 10 parallel requests          │  │  │
│ │ │    - 20 queued requests                │  │  │
│ │ │    - Rate limiting per service         │  │  │
│ │ └────────────────────────────────────────┘  │  │
│ └─────────────────────────────────────────────┘  │
│                 │                                 │
│                 ▼                                 │
│ ┌─────────────────────────────────────────────┐  │
│ │ 6. RESPONSE PROCESSING                       │  │
│ │ ┌────────────────────────────────────────┐  │  │
│ │ │ a. Response Handlers                    │  │  │
│ │ │    - Response transformation            │  │  │
│ │ │    - Header injection                  │  │  │
│ │ │    - Caching (if GET)                  │  │  │
│ │ └────────────────────────────────────────┘  │  │
│ │ ┌────────────────────────────────────────┐  │  │
│ │ │ b. Response Logging                    │  │  │
│ │ │    - Status code                       │  │  │
│ │ │    - Response time                     │  │  │
│ │ │    - Service name                      │  │  │
│ │ └────────────────────────────────────────┘  │  │
│ └─────────────────────────────────────────────┘  │
│                 │                                 │
│                 ▼                                 │
│ ┌─────────────────────────────────────────────┐  │
│ │ 7. CLIENT RESPONSE                           │  │
│ │ Status: 200, 400, 401, 403, 429, 500, etc. │  │
│ │ Headers: Content-Type, X-Correlation-ID    │  │
│ │ Body: JSON response data                    │  │
│ └─────────────────────────────────────────────┘  │
│                                                   │
└──────────────────────────────────────────────────┘
```

---

## Component Overview

### Core Components

#### 1. **Program.cs** - Application Entry Point
- Configures dependency injection
- Sets up middleware pipeline
- Initializes Ocelot
- Configures health checks
- Starts web server

#### 2. **Ocelot Configuration** (ocelot.json)
- Defines routes for all 8 services
- Configures QoS options (circuit breaker)
- Sets rate limiting per service
- Specifies load balancer strategy

#### 3. **Middleware Components**
- `RequestResponseLoggingMiddleware` - Logs all requests/responses
- `SecurityHeadersMiddleware` - Adds security headers
- `ErrorHandlingMiddleware` - Normalizes errors
- `RequestValidationMiddleware` - Validates incoming requests

#### 4. **Authentication & Authorization**
- `AuthenticationHandler` - Configures JWT validation
- `AuthorizationHandler` - Defines authorization policies
- Service-specific scope enforcement

#### 5. **Resilience Policies** (Polly)
- `ResiliencePolicies.cs` - Retry, circuit breaker, timeout, bulkhead
- Combined policies for maximum resilience
- Policy-per-service customization

#### 6. **HTTP Handlers**
- `GatewayHttpHandler` - Logs requests/responses
- `ResponseTransformationHandler` - Modifies responses
- `ResilienceHandler` - Implements retry logic
- `CachingHandler` - Caches GET responses

#### 7. **Health Checks**
- `ServiceHealthCheck` - Checks individual service health
- `GatewayHealthCheck` - Combined health status
- Kubernetes probe support (/health/live, /health/ready)

#### 8. **Configuration Models**
- `ServiceConfiguration` - Per-service settings
- `GatewayConfiguration` - Global settings
- `JwtSettings` - Authentication settings

---

## Design Patterns

### 1. **Facade Pattern**
Gateway acts as a single unified API for multiple heterogeneous microservices.

```
Clients → Gateway (Unified API)
           ├─ Service A → Database A
           ├─ Service B → Database B
           └─ Service C → Database C
```

### 2. **API Gateway Pattern**
Central entry point for all client requests with cross-cutting concerns.

**Concerns Handled**:
- Request routing
- Authentication
- Rate limiting
- Logging
- Request validation

### 3. **Circuit Breaker Pattern**
Prevents cascading failures by stopping requests to failing services.

**States**:
- **Closed**: Normal operation (requests pass through)
- **Open**: Service failing (requests fail immediately)
- **Half-Open**: Testing if service recovered (single request allowed)

### 4. **Retry Pattern**
Gracefully handles transient failures with exponential backoff.

**Transient Failures**:
- Network timeouts
- Service temporarily unavailable
- Temporary service slowdowns

### 5. **Timeout Pattern**
Prevents hanging requests and ensures timely responses.

**Implementation**:
- HTTP client timeout: 10 seconds
- Per-service timeout
- Configurable per endpoint

### 6. **Bulkhead Pattern**
Isolates resources to prevent complete system failure.

```
Bulkhead 1 (Service A)
  ├─ Active: 5/10 concurrent
  └─ Queue: 8/20 pending

Bulkhead 2 (Service B)
  ├─ Active: 10/10 concurrent
  └─ Queue: 20/20 (full, new requests rejected)

Bulkhead 3 (Service C)
  ├─ Active: 2/10 concurrent
  └─ Queue: 0/20
```

### 7. **Load Balancer Pattern**
Distributes load across multiple service instances.

**Algorithms**:
- Round Robin (default)
- Least Connections
- Weighted Distribution

### 8. **Caching Pattern**
Improves performance by caching read-only responses.

**Cache Strategy**:
- GET requests only
- TTL-based expiration
- Service-specific TTL

### 9. **Correlation ID Pattern**
Tracks requests across services for debugging and monitoring.

```
Client → Gateway [ID: abc123] → Service A [ID: abc123]
                                   ↓
                              Service B [ID: abc123]
                                   ↓
                              Database [ID: abc123]

Logs can be correlated using ID: abc123
```

### 10. **Authentication Pattern**
Centralized JWT-based authentication at gateway.

```
1. Client provides JWT token
2. Gateway validates token
3. Gateway extracts claims (role, scope, permissions)
4. Gateway enforces authorization policies
5. Request forwarded to service with claims in headers
```

---

## Data Flow Diagrams

### Successful Request Flow

```
User Request (200 OK)
│
GET /finyear/v1/departments?id=1
Authorization: Bearer eyJhbGc...
X-Correlation-ID: abc123
│
├─ GATEWAY PROCESSING
│  ├─ [10ms] Correlation ID assigned
│  ├─ [15ms] JWT token validated ✓
│  ├─ [10ms] Scope "finyear-api" verified ✓
│  ├─ [5ms] Rate limit checked (90/100 requests used) ✓
│  └─ [20ms] Ocelot routes to finyear-service:5001
│
├─ SERVICE CALL (1st attempt)
│  ├─ [500ms] Finyear service processes request
│  │           SQL: SELECT * FROM Departments WHERE id=1
│  ├─ [200ms] Database returns 1 row
│  └─ [0ms] Service returns 200 OK with data
│
├─ GATEWAY RESPONSE PROCESSING
│  ├─ [5ms] Response cached (5-min TTL)
│  ├─ [10ms] Headers transformed
│  │          X-Correlation-ID: abc123
│  │          X-Response-Time: 2026-03-10T...
│  │          X-Service: Finyear
│  └─ [15ms] Response logged
│
└─ USER RECEIVES
   200 OK
   {
     "id": 1,
     "name": "Finance"
   }
   Timeline: ~800ms total
```

### Failed Request with Retry

```
User Request (503 Service Unavailable → 200 OK)
│
GET /location/v1/states
Authorization: Bearer eyJhbGc...
│
├─ GATEWAY PROCESSING
│  ├─ [10ms] Correlation ID: def456
│  ├─ [15ms] JWT token validated ✓
│  ├─ [10ms] Scope verified ✓
│  ├─ [5ms] Rate limit checked ✓
│  └─ [20ms] Ocelot routes to location-service:5002
│
├─ SERVICE CALL #1 (FAILURE)
│  ├─ [10000ms] Timeout (10 second limit)
│  ├─ [5ms] Logged: "Retrying request (attempt 1/3)"
│  └─ [1000ms] Wait 1 second (exponential backoff)
│
├─ SERVICE CALL #2 (FAILURE)
│  ├─ [10000ms] Timeout again
│  ├─ [5ms] Logged: "Retrying request (attempt 2/3)"
│  └─ [2000ms] Wait 2 seconds
│
├─ SERVICE CALL #3 (SUCCESS)
│  ├─ [100ms] Location service responds ✓
│  ├─ [10ms] Returns 200 OK with states list
│  └─ [5ms] Response logged
│
├─ GATEWAY RESPONSE PROCESSING
│  ├─ [5ms] Response cached
│  ├─ [10ms] Headers added
│  └─ [15ms] Response logged
│
└─ USER RECEIVES
   200 OK
   [
     { "id": 1, "name": "California" },
     { "id": 2, "name": "Texas" },
     ...
   ]
   Timeline: ~13.2 seconds
   Notes: 2 retries, exponential backoff, eventual success
```

### Circuit Breaker Open State

```
User Requests (Service Failing)
│
Sequence of 5 failed requests:

Request 1: GET /vendor/v1/vendors
├─ Timeout (10s) → Logged: Failure 1/5
└─ Return 504

Request 2: GET /vendor/v1/vendors/1
├─ Timeout (10s) → Logged: Failure 2/5
└─ Return 504

Request 3: POST /vendor/v1/vendors
├─ ConnectionError → Logged: Failure 3/5
└─ Return 502

Request 4: GET /vendor/v1/vendors
├─ Timeout (10s) → Logged: Failure 4/5
└─ Return 504

Request 5: GET /vendor/v1/vendors
├─ Timeout (10s) → Logged: Failure 5/5
├─ CIRCUIT BREAKER OPENS!
│  Vendor service marked as unavailable for 30 seconds
└─ Return 503

Requests 6-N (while circuit open):
├─ [1ms] Fail immediately (no actual service call)
├─ Circuit Breaker: vendor-service is OPEN
└─ Return 503 Service Unavailable

After 30 seconds (Half-Open state):
Request N+1:
├─ Circuit enters HALF-OPEN state
├─ [100ms] Single test request succeeds ✓
├─ Service has recovered!
└─ Circuit CLOSES (returns to normal)

Subsequent Requests:
├─ Circuit is CLOSED
├─ Normal processing resumes
└─ Service operational again
```

---

## Service Interactions

### Service-to-Gateway Communication

```
┌─────────┐
│ Client  │
└────┬────┘
     │ HTTP Request
     │ GET /finyear/v1/departments
     │ Authorization: Bearer token
     │ X-Correlation-ID: abc123
     │ X-Client-ID: client-001
     ↓
┌─────────────────────┐
│   API Gateway       │
│ (Port 5000)         │
├─────────────────────┤
│ INBOUND:            │
│ - Validate JWT      │
│ - Check scopes      │
│ - Rate limit        │
│ - Validate request  │
│ - Log request       │
│                     │
│ ROUTE:              │
│ /finyear → svc:5001 │
│                     │
│ OUTBOUND:           │
│ - Add headers       │
│ - Inject ID         │
│ - Retry/CB logic    │
│ - Transform body    │
└────┬────────────────┘
     │ HTTP Request
     │ GET /api/v1/departments
     │ Authorization: Bearer token
     │ X-Correlation-ID: abc123
     │ X-Request-ID: xyz789
     │ X-Service: Finyear
     ↓
┌─────────────────────┐
│ Finyear Service     │
│ (Port 5001)         │
├─────────────────────┤
│ - Receive request   │
│ - Validate auth     │
│ - Process logic     │
│ - Query database    │
│ - Build response    │
└────┬────────────────┘
     │ HTTP Response
     │ 200 OK
     │ [
     │   {id:1, name:Finance},
     │   {id:2, name:Accounting}
     │ ]
     ↓
┌─────────────────────┐
│   API Gateway       │
├─────────────────────┤
│ RESPONSE:           │
│ - Log response      │
│ - Cache (if GET)    │
│ - Add headers       │
│ - Transform body    │
│ - Track metrics     │
└────┬────────────────┘
     │ HTTP Response
     │ 200 OK
     │ X-Correlation-ID: abc123
     │ X-Response-Time: 2026-03-10...
     │ X-Service: Finyear
     │ [
     │   {id:1, name:Finance},
     │   {id:2, name:Accounting}
     │ ]
     ↓
┌─────────┐
│ Client  │
└─────────┘
```

### Multi-Service Request

```
Client Request
│
GET /api/v1/employee-masters
(Requires data from multiple services)
│
        ↓ Gateway
   ┌────┴─────────┐
   │              │
   ▼              ▼
Location     Vendor
Service      Service
(5002)       (5003)
   │              │
   │ Response:    │ Response:
   │ Locations    │ Vendors
   │              │
   └────┬─────────┘
        │ Gateway combines responses
        │
        └─→ Returns {
              locations: [...],
              vendors: [...]
            }
```

---

## Deployment Models

### Model 1: Docker Compose (Development)

**File**: docker-compose.yml
```
Gateway + All 8 Services
All in containers
Network: erp-network
Volumes: logs, data
```

### Model 2: Kubernetes (Production)

**Components**:
- **Deployment**: Gateway pods (3+ replicas)
- **Service**: Internal service (ClusterIP)
- **Ingress**: External access (LoadBalancer/Ingress)
- **ConfigMap**: Configuration
- **Secret**: Credentials
- **HPA**: Auto-scaling

**Scaling Strategy**:
- Min 3 replicas
- Max 10 replicas
- Scale on 70% CPU / 80% memory

### Model 3: Serverless (Future)

```
Cloud Function → Services
(Could use Azure Functions, AWS Lambda)
```

---

## Quality Attributes

### Reliability
✓ Circuit breaker prevents cascading failures
✓ Retry logic handles transient failures
✓ Bulkhead isolation limits damage
✓ Health checks verify service status
**Target**: 99.9% uptime (9 hours downtime/year)

### Performance
✓ Response caching reduces latency
✓ Connection pooling improves throughput
✓ Compression reduces bandwidth
✓ Load balancing distributes load
**Target**: <500ms p95 latency

### Security
✓ JWT authentication
✓ Authorization policies
✓ Rate limiting prevents abuse
✓ Security headers protect clients
✓ HTTPS/TLS encryption
**Target**: Zero unauthorized access

### Scalability
✓ Stateless design enables horizontal scaling
✓ Load balancing for distribution
✓ Auto-scaling based on metrics
✓ Service discovery for dynamic topology
**Target**: Handle 10x traffic increase

### Maintainability
✓ Clear separation of concerns
✓ Well-documented patterns
✓ Centralized configuration
✓ Comprehensive logging
✓ Health checks for monitoring
**Target**: <1 hour MTTR (Mean Time To Repair)

### Observability
✓ Correlation IDs for tracing
✓ Structured logging
✓ Prometheus metrics
✓ Health check endpoints
✓ Error tracking
**Target**: <5 minute issue detection

---

End of Architecture Documentation
