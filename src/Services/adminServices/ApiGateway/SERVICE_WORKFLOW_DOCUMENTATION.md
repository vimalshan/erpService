# Complete Service Workflow Documentation

## Table of Contents
1. [Overview](#overview)
2. [Request Flow](#request-flow)
3. [Resilience Patterns](#resilience-patterns)
4. [Authentication & Authorization](#authentication--authorization)
5. [Scaling Strategies](#scaling-strategies)
6. [Service Integration](#service-integration)
7. [Monitoring & Observability](#monitoring--observability)
8. [Error Handling](#error-handling)
9. [Performance Optimization](#performance-optimization)
10. [Deployment Architecture](#deployment-architecture)

---

## Overview

The ERP API Gateway is a comprehensive microservices gateway that manages:
- **8 Microservices** with independent deployments
- **Advanced Resilience** with circuit breakers, retries, and timeouts
- **100% Service Coverage** with routing, authentication, and monitoring
- **Production-Ready** with logging, metrics, and health checks

### Core Technologies
- **Framework**: ASP.NET Core 8.0
- **Gateway**: Ocelot 18.0
- **Resilience**: Polly 8.2.1
- **Authentication**: JWT Bearer
- **Logging**: Serilog
- **Metrics**: Prometheus
- **Health Checks**: AspNetCore.HealthChecks

---

## Request Flow

### Complete Request Lifecycle

```
┌─────────────────────────────────────────────────────────────────┐
│ 1. CLIENT REQUEST                                                │
│    GET /finyear/v1/financialyear                                 │
│    Headers: Authorization, X-Client-ID, X-Correlation-ID        │
└────────┬────────────────────────────────────────────────────────┘
         │
┌────────▼────────────────────────────────────────────────────────┐
│ 2. GATEWAY ENTRY POINT (Program.cs)                              │
│    ✓ CORS enabled                                                │
│    ✓ HTTP redirection (if HTTPS configured)                      │
└────────┬────────────────────────────────────────────────────────┘
         │
┌────────▼────────────────────────────────────────────────────────┐
│ 3. MIDDLEWARE PIPELINE                                            │
├────────────────────────────────────────────────────────────────┤
│ 3a. CorrelationId Middleware                                     │
│     - Extracts or generates X-Correlation-ID                     │
│     - Adds to HttpContext.Items                                  │
│                                                                   │
│ 3b. RequestResponseLogging Middleware                            │
│     - Logs incoming request details                              │
│     - Captures response for logging                              │
│     - Measures request duration                                  │
│                                                                   │
│ 3c. SecurityHeaders Middleware                                   │
│     - Adds X-Content-Type-Options: nosniff                       │
│     - Adds X-Frame-Options: DENY                                 │
│     - Adds X-XSS-Protection                                      │
│     - Adds Strict-Transport-Security                             │
│     - Adds Content-Security-Policy                               │
│                                                                   │
│ 3d. ErrorHandling Middleware                                     │
│     - Catches exceptions                                         │
│     - Normalizes error responses                                 │
│                                                                   │
│ 3e. RequestValidation Middleware                                 │
│     - Validates request size (<10MB)                             │
│     - Checks required headers for POST/PUT/DELETE                │
│                                                                   │
│ 3f. IpRateLimiting Middleware                                    │
│     - Enforces rate limits per client                            │
│     - Returns 429 if exceeded                                    │
└────────┬────────────────────────────────────────────────────────┘
         │
┌────────▼────────────────────────────────────────────────────────┐
│ 4. AUTHENTICATION & AUTHORIZATION                                 │
├────────────────────────────────────────────────────────────────┤
│ 4a. Authentication Handler                                       │
│     - Validates JWT Bearer token                                 │
│     - Verifies signature (uses secret key)                       │
│     - Checks token expiration                                    │
│     - Extracts claims (scope, role, permissions)                │
│     - Returns 401 if invalid/expired                             │
│                                                                   │
│ 4b. Authorization Handler                                        │
│     - Verifies required scopes (e.g., "finyear-api")            │
│     - Checks role-based access                                   │
│     - Matches path to policy requirements                        │
│     - Returns 403 if unauthorized                                │
└────────┬────────────────────────────────────────────────────────┘
         │
┌────────▼────────────────────────────────────────────────────────┐
│ 5. OCELOT ROUTING & TRANSFORMATION                                │
├────────────────────────────────────────────────────────────────┤
│ 5a. Route Matching                                               │
│     - Upstream: /finyear/v1/financialyear                        │
│     - Matched to route definition                                │
│                                                                   │
│ 5b. Downstream Service Resolution                                │
│     - Downloaded: /api/v1/financialyear                          │
│     - URL: http://finyear-service:5001                           │
│                                                                   │
│ 5c. Load Balancing (Round Robin)                                 │
│     - Select instance if multiple available                      │
│                                                                   │
│ 5d. Request Transformation                                       │
│     - Add X-Correlation-ID header                                │
│     - Add X-Request-ID header                                    │
│     - Add X-Service header                                       │
│     - Preserve query parameters                                  │
│     - Forward request body                                       │
└────────┬────────────────────────────────────────────────────────┘
         │
┌────────▼────────────────────────────────────────────────────────┐
│ 6. HTTP CLIENT REQUEST PROCESSING                                 │
├────────────────────────────────────────────────────────────────┤
│ 6a. Request Handlers (Polly + Custom)                            │
│     - Retry Handler (3 attempts, exponential backoff)             │
│     - Circuit Breaker (5 failures, 30s open)                     │
│     - Timeout Handler (10 seconds)                               │
│     - Bulkhead Handler (10 parallel, 20 queued)                  │
│                                                                   │
│ 6b. Request Execution Flow (if failures occur):                  │
│     ┌─────────────────────────────────────────────┐              │
│     │ Attempt 1 (T=0)                             │              │
│     │ - Timeout: 10s                              │              │
│     │ - Result: Failure (ServiceUnavailable)      │              │
│     └─────────────────────────────────────────────┘              │
│                         │                                        │
│                    Wait 1 second                                  │
│                         │                                        │
│     ┌─────────────────────────────────────────────┐              │
│     │ Attempt 2 (T=1)                             │              │
│     │ - Timeout: 10s                              │              │
│     │ - Result: Success ✓                         │              │
│     │ - Response returned                         │              │
│     └─────────────────────────────────────────────┘              │
│                                                                   │
│ If all retries fail → Circuit breaker opens                       │
│ Future requests fail immediately (no actual attempt)              │
└────────┬────────────────────────────────────────────────────────┘
         │
┌────────▼────────────────────────────────────────────────────────┐
│ 7. DOWNSTREAM SERVICE PROCESSING                                  │
│    (Now in Finyear Microservice)                                  │
│    - Authentication (if required)                                │
│    - Business logic                                              │
│    - Database query                                              │
│    - Response generation                                         │
└────────┬────────────────────────────────────────────────────────┘
         │
┌────────▼────────────────────────────────────────────────────────┐
│ 8. RESPONSE PROCESSING                                            │
├────────────────────────────────────────────────────────────────┤
│ 8a. Response Capture (in handlers)                               │
│     - Check HTTP status code                                     │
│     - Add response headers (X-Response-Time, X-Service)          │
│     - Cache if GET and cacheable (if enabled)                    │
│                                                                   │
│ 8b. Response Body                                                │
│     - JSON serialization (System.Text.Json)                      │
│     - Compression (if supported)                                 │
│                                                                   │
│ 8c. Response Headers                                             │
│     - X-Correlation-ID (from request)                            │
│     - X-Response-Time                                            │
│     - X-Service (which service responded)                        │
│     - Cache-Control (if caching enabled)                         │
└────────┬────────────────────────────────────────────────────────┘
         │
┌────────▼────────────────────────────────────────────────────────┐
│ 9. RESPONSE LOGGING                                               │
│    - Response status code                                        │
│    - Response time                                               │
│    - Service name                                                │
│    - Correlation ID                                              │
│    - Any errors/warnings                                         │
└────────┬────────────────────────────────────────────────────────┘
         │
┌────────▼────────────────────────────────────────────────────────┐
│ 10. CLIENT RESPONSE                                               │
│     200 OK, 201 Created, 400 Bad Request, etc.                   │
│     With all headers and body                                    │
└─────────────────────────────────────────────────────────────────┘
```

### Request Timing Breakdown

For a successful request with 1 retry:

```
[00:00.000] Request arrives at gateway
[00:00.050] Correlation ID assigned/validated
[00:00.100] JWT token validated
[00:00.150] Rate limiting checked
[00:00.200] Ocelot routes request
[00:00.250] First HTTP attempt starts
[00:01.000] First attempt times out or fails
[00:01.100] Wait 1 second (exponential backoff)
[00:02.100] Second attempt starts
[00:02.500] Second attempt succeeds
[00:02.550] Response logged
[00:02.600] Response returned to client
────────────────────────────────────
Total: ~2.6 seconds
```

---

## Resilience Patterns

### 1. Circuit Breaker Pattern

**Purpose**: Prevent cascading failures when downstream services are unavailable.

**Implementation** (Polly):
```csharp
CircuitBreakerAsync(
    handledEventsAllowedBeforeBreaking: 5,  // 5 failures before opening
    durationOfBreak: TimeSpan.FromSeconds(30) // Stay open 30 seconds
)
```

**State Machine**:
```
        ┌──────────────┐
        │   CLOSED     │  ◄─────► Normal operation
        │ (requests OK) │         All requests pass through
        └──────┬───────┘
               │ 5 consecutive failures detected
               ▼
        ┌──────────────┐
        │     OPEN     │  ◄─────► Circuit Open
        │ (fail fast)  │         All requests fail immediately
        └──────┬───────┘         (no actual call made)
               │ 30 seconds timeout
               ▼
        ┌──────────────┐
        │  HALF-OPEN   │  ◄─────► Testing recovery
        │ (test call)  │         Single request allowed
        └──────┬───────┘
               │ Success? 
               ├─Yes──► CLOSED (recovery complete)
               └─No───► OPEN (still failing)
```

**Monitoring**:
- Check circuit state in logs
- Monitor `/health` endpoint for service degradation
- Alert on circuit breaker transitions

**Implementation File**: [Policies/ResiliencePolicies.cs](Policies/ResiliencePolicies.cs)

---

### 2. Retry Pattern with Exponential Backoff

**Purpose**: Handle transient failures gracefully.

**Implementation**:
```csharp
WaitAndRetryAsync(
    retryCount: 3,
    sleepDurationProvider: retryAttempt =>
        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
    // Wait times: 1s (2^0), 2s (2^1), 4s (2^2)
)
```

**Retry Candidates** (Automatic Retries):
- `HttpRequestException` (network failures)
- `TimeoutRejectedException` (timeout)
- Status 408 (Request Timeout)
- Status 503 (Service Unavailable)
- Status 504 (Gateway Timeout)

**Non-Retriable** (Fail Immediately):
- Status 400 (Bad Request) - client error
- Status 401 (Unauthorized) - auth error
- Status 403 (Forbidden) - permissions error
- Status 404 (Not Found) - resource doesn't exist
- Status 422 (Unprocessable Entity) - validation error

**Backoff Visualization**:
```
Request attempt 1
    │ Failure at T=0
    │
    └─ Wait 1 second
         │
         ▼ (T=1s)
    Attempt 2
         │ Failure
         │
         └─ Wait 2 seconds
              │
              ▼ (T=3s)
           Attempt 3
                │ Failure
                │
                └─ Wait 4 seconds
                     │
                     ▼ (T=7s)
                  Attempt 4 (last)
                     │ Success/Final failure
                     ▼ Return result
```

**Implementation File**: [Policies/ResiliencePolicies.cs](Policies/ResiliencePolicies.cs)

---

### 3. Timeout Pattern

**Purpose**: Prevent hanging requests and free resources.

**Configuration**:
- **HTTP Client Timeout**: 10 seconds
- **Per-Attempt Timeout**: 10 seconds
- **Global Request Timeout**: Configurable per service

**Timeout Behavior**:
```
T=0:00 Request sent to downstream service
T=0:10 No response → Timeout triggered
       → Exception thrown
       → Logged as timeout
       → Retry logic evaluates (if applicable)
```

**Handling**:
1. First timeout → Logs warning
2. If retries enabled → Automatic retry
3. If all retries timeout → Return 504 Gateway Timeout
4. If circuit open → Return 503 Service Unavailable

**Implementation File**: [Policies/ResiliencePolicies.cs](Policies/ResiliencePolicies.cs)

---

### 4. Bulkhead Isolation Pattern

**Purpose**: Prevent resource exhaustion by limiting concurrent operations.

**Configuration**:
```csharp
BulkheadAsync<HttpResponseMessage>(
    maxParallelization: 10,    // Max 10 concurrent requests
    maxQueuingActions: 20,      // Queue up to 20 pending
    onBulkheadRejectedAsync: ... // Handle rejection
)
```

**Behavior**:
```
Service can handle 10 concurrent requests

Request 1  ✓ (slot 1)
Request 2  ✓ (slot 2)
...
Request 10 ✓ (slot 10)
Request 11 → Queue (queued 1/20)
Request 12 → Queue (queued 2/20)
...
Request 30 → Queue (queued 20/20)
Request 31 → REJECT (queue full) → 429 Too Many Requests

(When Request 1 completes)
Request 1 completes
Request 11 moves from queue to slot 1 ✓
Request 31 now queued (1/20)
```

**Benefits**:
- No thread starvation
- Fair resource allocation
- Prevents cascading overload
- Predictable behavior under load

**Implementation File**: [Policies/ResiliencePolicies.cs](Policies/ResiliencePolicies.cs)

---

### 5. Callback/Event Pattern

**Purpose**: Handle service events and cascading updates.

**Implementation Pattern**:
```
Service A → Event:UserCreated
           ↓
      Event Publisher
           ↓
    ┌──────┴──────┬──────────┬──────────┐
    ▼             ▼          ▼          ▼
Service B    Service C  Service D  Service E
(Subscribe) (Subscribe) (Subscribe) (Subscribe)
```

**Gateway Role**:
- Routes event publishing requests (POST to /events)
- Ensures idempotency (duplicate events ignored)
- Logs all events for audit trail

### 6. Request/Response Transformation

**Request Transformation** (Incoming):
```
Client Request:
GET /finyear/v1/departments

↓ Transformed to ↓

Downstream Request:
GET http://finyear-service:5001/api/v1/departments
Headers:
  - Authorization: Bearer <token>
  - X-Correlation-ID: uuid
  - X-Request-ID: uuid
  - X-Service: Finyear
  - Accept: application/json
```

**Response Transformation** (Outgoing):
```
Downstream Response:
200 OK
{
  "id": 1,
  "name": "Finance"
}
Headers:
  - Content-Type: application/json

↓ Transformed to ↓

Client Response:
200 OK
{
  "id": 1,
  "name": "Finance"
}
Headers:
  - X-Correlation-ID: uuid (from request)
  - X-Response-Time: 2026-03-10T...
  - X-Service-Response: Finyear
  - Content-Type: application/json
```

**Implementation**: [Handlers/RequestResponseHandlers.cs](Handlers/RequestResponseHandlers.cs)

---

## Authentication & Authorization

### JWT Bearer Token Flow

```
1. User Login Request
   POST /auth/login
   {
     "username": "user@erp.com",
     "password": "password"
   }
   
   ↓ (External Auth Service)
   
2. Token Generated
   {
     "token": "eyJhbGciOiJIUzI1NiIs...",
     "expiresIn": 3600
   }

3. Client Stores Token

4. Subsequent Requests
   GET /finyear/v1/departments
   Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
   
   ↓ (Gateway Auth Handler)
   
5. Token Validation
   ✓ Signature verified (using secret key)
   ✓ Expiration checked
   ✓ Claims extracted
   
6. Authorization Check
   ✓ Scope verified (has "finyear-api"?)
   ✓ Role checked (is "user" or "admin"?)
   ✓ Permissions verified (can "read"/'write'?)
   
7. Request Allowed/Denied
```

### Token Structure

```json
Header:
{
  "alg": "HS256",
  "typ": "JWT"
}

Payload:
{
  "sub": "user@erp.com",
  "name": "John Doe",
  "role": "admin",
  "scope": "finyear-api location-api vendor-api",
  "permission": "read write",
  "iat": 1710000000,
  "exp": 1710003600
}

Signature:
HMACSHA256(base64(header) + "." + base64(payload), "secret-key")
```

### Authorization Policies

Defined in [Handlers/AuthenticationHandler.cs](Handlers/AuthenticationHandler.cs):

| Policy | Required Claim | Purpose |
|--------||---|
| `AdminOnly` | `role = "admin"` | Admin-only operations |
| `FinyearAccess` | `scope = "finyear-api"` | Finyear service access |
| `LocationAccess` | `scope = "location-api"` | Location service access |
| `VendorAccess` | `scope = "vendor-api"` | Vendor service access |
| `ScholarshipAccess` | `scope = "scholarship-api"` | Scholarship service access |
| `StationeryAccess` | `scope = "stationery-api"` | Stationery service access |
| `TDSAccess` | `scope = "tds-api"` | TDS service access |
| `LOVAccess` | `scope = "lov-api"` | LOV service access |
| `SharedAccess` | `scope = "shared-api"` | Shared service access |
| `ReadOnlyAccess` | `permission = "read"` | GET operations only |
| `WriteAccess` | `permission = "write"` | POST/PUT/DELETE allowed |
| `FullAccess` | `permission = "admin"` | All operations allowed |

### Configuration

In [appsettings.json](appsettings.json):
```json
{
  "JwtSettings": {
    "SecretKey": "your-super-secret-key-at-least-32-chars",
    "Issuer": "https://erpmicroservice.com",
    "Audience": "erp-api-users",
    "ExpirationMinutes": 60
  }
}
```

**Important**: In production, use:
- Generate strong secret key (64+ characters)
- Use HTTPS only
- Implement key rotation strategy
- Store in secure vault (not in code)

---

## Scaling Strategies

### 1. Horizontal Scaling (Multiple Gateway Instances)

**Architecture**:
```
     Load Balancer (Nginx/HAProxy)
            │
   ┌────────┼────────┐
   ▼        ▼        ▼
Gateway1 Gateway2 Gateway3
(5000)   (5000)   (5000)
   │        │        │
   └────────┼────────┘
            │
   ┌────────┴────────┬──────────┬──────────┐
   ▼        ▼        ▼          ▼          ▼
Finyear  Location  Vendor  Scholarship Stationery
```

**Configuration**:
- Stateless gateway design → No sticky sessions needed
- Deploy 2-3 instances per AZ for high availability
- Use autoscaling based on CPU/memory

**Load Balancer Config** (Nginx):
```nginx
upstream api_gateway {
    least_conn;  # Load balancing algorithm
    server gateway1:5000;
    server gateway2:5000;
    server gateway3:5000;
}

server {
    listen 80;
    server_name api.erp.com;
    
    location / {
        proxy_pass http://api_gateway;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

### 2. Vertical Scaling (Single Gateway Performance)

**Thread Pool Configuration**:
```csharp
// In Program.cs
ThreadPool.GetMinThreads(out int workerThreads, out int ioThreads);
ThreadPool.SetMinThreads(
    Math.Max(workerThreads, Environment.ProcessorCount * 2),
    Math.Max(ioThreads, Environment.ProcessorCount * 2)
);
```

**Connection Pool Tuning**:
```csharp
ServicePointManager.DefaultConnectionLimit = 10;  // Per host
ServicePointManager.ReusePort = true;
```

**Garbage Collection Tuning**:
```xml
<!-- In csproj -->
<TargetFramework>net8.0</TargetFramework>
<ServerGarbageCollection>true</ServerGarbageCollection>
<RetainVMGarbageCollection>true</RetainVMGarbageCollection>
<TieredCompilation>true</TieredCompilation>
```

### 3. Rate Limiting Scaling

**Per-Tier Limits**:
```
Free Tier:        10 req/min
Professional:     100 req/min
Enterprise:       1000 req/min
VIP:             10000 req/min
```

**Implementation**:
```csharp
// Identified by X-Client-ID header
GET /api/resource
X-Client-ID: free-tier-client
→ 429 Too Many Requests (if >10/min)

GET /api/resource
X-Client-ID: enterprise-client
→ OK (if <1000/min)
```

### 4. Service Discovery Scaling

**For Kubernetes**:
```yaml
# service.yaml
apiVersion: v1
kind: Service
metadata:
  name: finyear-service
spec:
  selector:
    app: finyear
  ports:
  - port: 5001
    targetPort: 5001
---
apiVersion: apps/v1
kind: Deployment
metadata:
  name: finyear-deployment
spec:
  replicas: 3  # Auto-scaled by HPA
  selector:
    matchLabels:
      app: finyear
  template:
    metadata:
      labels:
        app: finyear
    spec:
      containers:
      - name: finyear
        image: finyear:latest
        ports:
        - containerPort: 5001
---
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: finyear-hpa
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: finyear-deployment
  minReplicas: 3
  maxReplicas: 10
  metrics:
  - type: Resource
    resource:
      name: cpu
      target:
        type: Utilization
        averageUtilization: 70
  - type: Resource
    resource:
      name: memory
      target:
        type: Utilization
        averageUtilization: 80
```

**Gateway Configuration for Service Discovery**:
```json
{
  "GlobalConfiguration": {
    "ServiceDiscoveryProvider": {
      "Type": "ServiceDiscovery",
      "Host": "consul.default.svc.cluster.local",
      "Port": 8500
    }
  }
}
```

---

## Service Integration

### Adding New Service

#### Step 1: Add to ocelot.json
```json
{
  "DownstreamPathTemplate": "/api/{version}/{controller}/{action?}/{id?}",
  "DownstreamScheme": "http",
  "DownstreamHostAndPorts": [
    {
      "Host": "new-service",
      "Port": 5009
    }
  ],
  "UpstreamPathTemplate": "/newservice/{version}/{controller}/{action?}/{id?}",
  "UpstreamHttpMethod": [ "GET", "POST", "PUT", "DELETE", "PATCH" ],
  "Key": "NewService",
  "AuthenticationOptions": {
    "AuthenticationProviderKey": "Bearer",
    "AllowedScopes": [ "newservice-api" ]
  },
  "RateLimitOptions": {
    "EnableRateLimiting": true,
    "Period": "1m",
    "Limit": 100
  },
  "QoSOptions": {
    "ExceptionsAllowedBeforeBreaking": 5,
    "DurationOfBreak": 30000,
    "TimeoutValue": 5000
  }
}
```

#### Step 2: Add to ServiceConfigurationSetup.cs
```csharp
new()
{
    Name = "NewService",
    BaseUrl = "http://new-service",
    Port = 5009,
    HealthCheckPath = "/health",
    TimeoutSeconds = 10,
    MaxRetries = 3,
    CircuitBreakerThreshold = 5,
    BulkheadMaxParallelization = 10,
    CachingEnabled = false,
    Scopes = new[] { "newservice-api", "read", "write" },
    RequiresAuthentication = true
}
```

#### Step 3: Add Authorization Policy
```csharp
options.AddPolicy("NewServiceAccess", policy =>
    policy.RequireClaim("scope", "newservice-api"));
```

#### Step 4: Add Health Check
```csharp
.AddUrlGroup(
    new Uri("http://new-service:5009/health"),
    name: "NewService",
    timeout: TimeSpan.FromSeconds(5),
    failureStatus: HealthStatus.Degraded)
```

---

## Monitoring & Observability

### Correlation ID Tracking

Every request gets a unique correlation ID:

```
Request 1: GET /finyear/v1/departments
X-Correlation-ID: 550e8400-e29b-41d4-a716-446655440000

↓ Logged in gateway:
[2026-03-10 10:30:45.123] [INF] Request [550e8400-e29b-41d4-a716-446655440000]: GET /finyear/v1/departments

↓ Forwarded to finyear service:
X-Correlation-ID: 550e8400-e29b-41d4-a716-446655440000

↓ Logged in finyear service:
[2026-03-10 10:30:45.234] [INF] [550e8400-e29b-41d4-a716-446655440000] Processing request

↓ Also forwarded to any downstream services
```

**Benefits**:
- Track request across all services
- Correlate logs for debugging
- Measure end-to-end latency
- Monitor distributed traces

### Logging Levels

| Level | Usage | Examples |
|-------|-------|----------|
| **Debug** | Detailed info for investigation | Token validation, cache hits, policy evaluation |
| **Information** | General operational info | Request/response, service startup, health checks |
| **Warning** | Unexpected but recoverable issues | Retry attempts, circuit breaker open, rate limit |
| **Error** | Recoverable errors | Request failures after retries, auth failures |
| **Fatal** | Unrecoverable errors | Startup failures, configuration errors |

**Sample Log Entries**:
```
[2026-03-10 10:35:12.450] [INF] [550e8400-e29b-41d4] Request [GET] /finyear/v1/departments from 192.168.1.100
[2026-03-10 10:35:12.501] [DBG] [550e8400-e29b-41d4] JWT token validated (exp: 2026-03-10 11:35:12)
[2026-03-10 10:35:12.552] [DBG] [550e8400-e29b-41d4] Cache hit for GET /finyear/v1/departments
[2026-03-10 10:35:12.603] [INF] [550e8400-e29b-41d4] Response [200] GET /finyear/v1/departments completed in 153ms
```

### Prometheus Metrics

Available at `/metrics`:

```
# HELP http_requests_received_total Total HTTP requests received
# TYPE http_requests_received_total counter
http_requests_received_total{method="GET",endpoint="/finyear/v1/departments",status="200"} 1250

# HELP http_request_duration_seconds HTTP request duration in seconds
# TYPE http_request_duration_seconds histogram
http_request_duration_seconds_bucket{endpoint="/finyear/v1/departments",le="0.1"} 850
http_request_duration_seconds_bucket{endpoint="/finyear/v1/departments",le="0.5"} 1200
http_request_duration_seconds_bucket{endpoint="/finyear/v1/departments",le="1.0"} 1240

# HELP circuit_breaker_state Circuit breaker state (0=closed, 1=open, 2=half-open)
# TYPE circuit_breaker_state gauge
circuit_breaker_state{service="finyear-service"} 0
circuit_breaker_state{service="vendor-service"} 1
```

### Grafana Dashboard Queries

**Request Rate** (requests/second):
```
rate(http_requests_received_total[5m])
```

**P95 Latency** (95th percentile):
```
histogram_quantile(0.95, http_request_duration_seconds)
```

**Service Health** (% successful):
```
(sum(rate(http_requests_received_total{status=~"2.."}[5m])) /
 sum(rate(http_requests_received_total[5m]))) * 100
```

**Circuit Breaker State** (open services):
```
count(circuit_breaker_state == 1)
```

---

## Error Handling

### Gateway-Level Error Responses

#### 400 Bad Request
```json
{
  "statusCode": 400,
  "message": "Request validation failed",
  "path": "/finyear/v1/departments",
  "timestamp": "2026-03-10T10:35:12Z",
  "details": "Content-Type header is required"
}
```

#### 401 Unauthorized
```json
{
  "statusCode": 401,
  "message": "Authentication failed",
  "path": "/finyear/v1/departments",
  "timestamp": "2026-03-10T10:35:12Z",
  "details": "Invalid or expired JWT token"
}
```

#### 403 Forbidden
```json
{
  "statusCode": 403,
  "message": "Authorization failed",
  "path": "/finyear/v1/departments",
  "timestamp": "2026-03-10T10:35:12Z",
  "details": "Missing required scope: finyear-api"
}
```

#### 429 Too Many Requests
```json
{
  "statusCode": 429,
  "message": "Rate limit exceeded",
  "path": "/finyear/v1/departments",
  "timestamp": "2026-03-10T10:35:12Z",
  "details": "Rate limit: 100 requests per minute"
}
```

#### 502 Bad Gateway
```json
{
  "statusCode": 502,
  "message": "Service is unavailable",
  "path": "/finyear/v1/departments",
  "timestamp": "2026-03-10T10:35:12Z",
  "details": "Downstream service failed to respond"
}
```

#### 503 Service Unavailable
```json
{
  "statusCode": 503,
  "message": "Service is unavailable",
  "path": "/finyear/v1/departments",
  "timestamp": "2026-03-10T10:35:12Z",
  "details": "Circuit breaker is open for finyear-service"
}
```

#### 504 Gateway Timeout
```json
{
  "statusCode": 504,
  "message": "Request timeout",
  "path": "/finyear/v1/departments",
  "timestamp": "2026-03-10T10:35:12Z",
  "details": "Service did not respond within 10 seconds"
}
```

### Retry Decision Tree

```
Request Received
    │
    ├─ Status 400-499 (Client Error)
    │   └─ FAIL (don't retry)
    │
    ├─ Status 408, 429, 500, 502, 503, 504
    │   └─ RETRY (3 times max)
    │       ├─ Attempt 1: Success? → Return
    │       ├─ Attempt 1: Failure → Wait 1s
    │       ├─ Attempt 2: Success? → Return
    │       ├─ Attempt 2: Failure → Wait 2s
    │       ├─ Attempt 3: Success? → Return
    │       └─ Attempt 3: Failure → Return error
    │
    ├─ Timeout
    │   └─ RETRY (same as above)
    │
    └─ Network Error
        └─ RETRY (same as above)
```

---

## Performance Optimization

### 1. Response Caching

**Cached Services** (GET requests only):
- **Finyear**: 5-minute cache
- **Location**: 10-minute cache (stable data)
- **Stationery**: 5-minute cache
- **LOV**: 15-minute cache (rarely changes)
- **Shared**: 5-minute cache

**Not Cached** (transactional data):
- Vendor service
- Scholarship service
- TDS service

**Cache Key Format**:
```
http_<full-url>
E.g., http_http://finyear-service:5001/api/v1/departments
```

**Cache Invalidation**:
```csharp
// On POST/PUT/DELETE to an endpoint
// All cache entries for that resource are cleared
// But gateway doesn't implement yet (leave to downstream services)
```

### 2. Connection Pooling

**SocketsHttpHandler Configuration**:
```csharp
AllowAutoRedirect = true;
AutomaticDecompression = GZip | Deflate;
ConnectTimeout = 5 seconds;
```

**Benefits**:
- Connection reuse (TCP handshake cost reduced)
- Compression (40-60% bandwidth reduction)
- Faster redirects

### 3. Load Balancing

**Round Robin Algorithm** (default):
```
Request 1 → Gateway 1 → Service
Request 2 → Gateway 2 → Service
Request 3 → Gateway 3 → Service
Request 4 → Gateway 1 → Service (cycle repeats)
```

**Least Connections Algorithm** (if implemented):
```
Gateway 1: 5 connections
Gateway 2: 2 connections ← Next request goes here
Gateway 3: 7 connections
```

### 4. Compression

**Automatic Gzip/Deflate** for responses >1KB:
```
Response: 50KB JSON
↓ Compressed
Response: 8-15KB (80% reduction)
```

**Header**: `Accept-Encoding: gzip, deflate`

---

## Deployment Architecture

### Development Deployment (Docker Compose)

```yaml
# Run:
docker-compose up -d

Services Started:
- API Gateway (port 5000)
- Finyear (port 5001)
- Location (port 5002)
- etc.

Network: erp-network (bridge)
```

### Production Deployment (Kubernetes)

**Architecture**:
```
┌─────────────────────────────────────────┐
│ Kubernetes Cluster                      │
├─────────────────────────────────────────┤
│                                         │
│  ┌──────────────────────────────────┐   │
│  │ Ingress (External Load Balancer)  │   │
│  └────────────┬─────────────────────┘   │
│               │                         │
│  ┌────────────▼─────────────────────┐   │
│  │ API Gateway Service (Cluster IP)  │   │
│  └────────────┬─────────────────────┘   │
│               │                         │
│  ┌────────────┴───────────┬──────────┐  │
│  ▼                        ▼          ▼  │
│ Gateway   ← HPA →  Gateway      Gateway  │
│ (3 pods)  monitors  (auto-scale) (max 10)│
│                                         │
└─────────────────────────────────────────┘
```

**Deployment Files** (in k8s/):
- `deployment.yaml` - Gateway pods
- `service.yaml` - Internal service
- `ingress.yaml` - External access
- `hpa.yaml` - Auto-scaling
- `configmap.yaml` - Configuration
- `secret.yaml` - Secrets (JWT key, etc.)
- `rbac.yaml` - Permissions

**Sample Deployment**:
```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: api-gateway
spec:
  replicas: 3
  selector:
    matchLabels:
      app: api-gateway
  template:
    metadata:
      labels:
        app: api-gateway
    spec:
      containers:
      - name: gateway
        image: erp-api-gateway:latest
        ports:
        - containerPort: 5000
        env:
        - name: ASPNETCORE_ENVIRONMENT
          value: Production
        - name: ASPNETCORE_URLS
          value: http://+:5000
        resources:
          requests:
            cpu: 100m
            memory: 256Mi
          limits:
            cpu: 500m
            memory: 512Mi
        livenessProbe:
          httpGet:
            path: /health/live
            port: 5000
          initialDelaySeconds: 10
          periodSeconds: 10
        readinessProbe:
          httpGet:
            path: /health/ready
            port: 5000
          initialDelaySeconds: 10
          periodSeconds: 5
```

**Ingress Configuration**:
```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: api-gateway-ingress
spec:
  ingressClassName: nginx
  rules:
  - host: api.erpmicroservice.com
    http:
      paths:
      - path: /
        pathType: Prefix
        backend:
          service:
            name: api-gateway
            port:
              number: 5000
```

**Access**:
```
http://api.erpmicroservice.com
→ Routes to Ingress Controller
→ Routes to API Gateway Service
→ Load balances to Gateway pods
→ Routes to downstream services
```

---

## Dashboard Endpoints

| Endpoint | Purpose | Output |
|----------|---------|--------|
| `GET /` | Gateway info | JSON with version, status, endpoints |
| `GET /health` | Full health check | All services' health statuses |
| `GET /health/live` | Liveness probe | Gateway running? |
| `GET /health/ready` | Readiness probe | Ready to receive traffic? |
| `GET /metrics` | Prometheus metrics | Request counts, latencies, errors |
| `GET /swagger` | API documentation | Interactive API explorer |

---

## Summary

This complete workflow documentation covers:
1. ✅ **Request Flow**: End-to-end processing
2. ✅ **Resilience**: Circuit breaker, retry, timeout, bulkhead, callbacks
3. ✅ **Authentication**: JWT, scopes, roles, permissions
4. ✅ **Scaling**: Horizontal, vertical, rate limiting, service discovery
5. ✅ **Service Integration**: Adding/configuring services
6. ✅ **Monitoring**: Logging, correlation IDs, metrics, dashboards
7. ✅ **Error Handling**: Standardized error responses, retry logic
8. ✅ **Performance**: Caching, connection pooling, load balancing, compression
9. ✅ **Deployment**: Development (Docker Compose), Production (Kubernetes)

The API Gateway is **production-ready** with enterprise-grade resilience, security, and observability.
