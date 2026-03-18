# ERP API Gateway

A comprehensive API Gateway implementation for ERP Microservices using Ocelot, with advanced resilience patterns, authentication, rate limiting, and comprehensive monitoring.

## Architecture Overview

The API Gateway serves as the single entry point for all client requests to the ERP microservices ecosystem. It provides:

```
┌─────────────────┐
│   API Clients   │
└────────┬────────┘
         │ HTTP/HTTPS
         ▼
┌──────────────────────────────────────────────────┐
│         ERP API Gateway (Port 5000)              │
├──────────────────────────────────────────────────┤
│ ✓ Request/Response Logging                       │
│ ✓ Rate Limiting & Throttling                     │
│ ✓ JWT Authentication & Authorization             │
│ ✓ Correlation ID Tracking                        │
│ ✓ Circuit Breaker Pattern                        │
│ ✓ Retry & Timeout Handling                       │
│ ✓ Bulkhead Isolation                             │
│ ✓ Health Checks & Monitoring                     │
│ ✓ Response Caching                               │
│ ✓ Load Balancing (Round Robin)                   │
└────────────┬─────────────────────────────────────┘
             │ Routes to specific services
   ┌─────────┼─────────┬────────────┬──────────────┐
   ▼         ▼         ▼            ▼              ▼
Finyear  Location  Vendor      Scholarship    Stationery
(5001)   (5002)    (5003)      (5004)         (5005)
   │         │         │            │              │
   ▼         ▼         ▼            ▼              ▼
  TDS       LOV      Shared
(5006)    (5007)    (5008)
```

## Features

### 1. **Resilience Patterns**
- **Circuit Breaker**: Prevents cascading failures when services are down
  - Opens after 5 consecutive failures
  - Stays open for 30 seconds
  - Automatically resets when service recovers

- **Retry Policy**: Exponential backoff with jitter
  - Max 3 retries
  - Wait pattern: 1s → 2s → 4s

- **Timeout Handling**: 10-second timeout per request
  - Prevents hanging requests
  - Graceful degradation

- **Bulkhead Isolation**: Limits concurrent requests
  - Max 10 parallel requests per service
  - Queue up to 20 pending requests
  - Prevents resource exhaustion

### 2. **Authentication & Authorization**
- JWT Bearer token validation
- Service-specific scope requirements
- Role-based access control (RBAC)
- Policy-based authorization

### 3. **Rate Limiting**
- Per-client rate limiting (100 requests/minute default)
- Graceful handling of quota exceeded (429 status)
- Client identification via `X-Client-ID` header

### 4. **Monitoring & Observability**
- Comprehensive request/response logging via Serilog
- Correlation ID tracking across service boundaries
- Prometheus metrics endpoint (`/metrics`)
- Health check endpoints:
  - `/health` - Full status
  - `/health/live` - Liveness probe
  - `/health/ready` - Readiness probe

### 5. **Request/Response Processing**
- Request validation (size, content-type checks)
- Automatic correlation ID injection
- Request timing and performance tracking
- Error response normalization
- Security headers injection

### 6. **Caching**
- Response caching for GET requests
- Configurable TTL per service
- Cache-aware headers

## Service Configuration

Each microservice is configured with:
- **Endpoint**: Base URL and port
- **Resilience**: Circuit breaker, retry, timeout settings
- **Bulkhead**: Max parallelization and queue limits
- **Authentication**: Required scopes and permissions
- **Rate Limiting**: Request limits per time period
- **Caching**: TTL and cache-ability settings

### Configured Services

| Service | Port | Timeout | Max Retries | Cache TTL | Notes |
|---------|------|---------|-------------|-----------|-------|
| Finyear | 5001 | 10s | 3 | 5min | Financial year management |
| Location | 5002 | 10s | 3 | 10min | Location/hierarchy data |
| Vendor | 5003 | 10s | 3 | None | Vendor management |
| Scholarship | 5004 | 10s | 3 | None | Scholarship processing |
| Stationery | 5005 | 10s | 3 | 5min | Stationery inventory |
| TDS | 5006 | 10s | 3 | None | Tax deduction tracking |
| LOV | 5007 | 10s | 3 | 15min | List of values (mostly read-only) |
| Shared | 5008 | 10s | 3 | 5min | Shared utilities & services |

## API Routes

### Base Gateway URL
```
http://localhost:5000
```

### Service Routes
- Finyear: `/finyear/{version}/{controller}/{action?}/{id?}`
- Location: `/location/{version}/{controller}/{action?}/{id?}`
- Vendor: `/vendor/{version}/{controller}/{action?}/{id?}`
- Scholarship: `/scholarship/{version}/{controller}/{action?}/{id?}`
- Stationery: `/stationery/{version}/{controller}/{action?}/{id?}`
- TDS: `/tds/{version}/{controller}/{action?}/{id?}`
- LOV: `/lov/{version}/{controller}/{action?}/{id?}`
- Shared: `/shared/{version}/{controller}/{action?}/{id?}`

### Management Endpoints
- Health: `GET /health`
- Swagger UI: `GET /`
- Metrics: `GET /metrics`
- Gateway Info: `GET /`

## Authentication

### Header Requirements
```bash
Authorization: Bearer <JWT_TOKEN>
X-Client-ID: <client-identifier>
X-Correlation-ID: <correlation-id> (optional - generated if missing)
```

### Token Claims Required
```json
{
  "scope": "finyear-api",  // Service-specific scope
  "permission": "read|write",
  "role": "user|admin"
}
```

## Configuration Files

### appsettings.json
Main configuration file containing:
- Logging levels
- JWT settings (issuer, audience, expiration)
- Service endpoints
- Rate limiting policies
- Circuit breaker settings

### ocelot.json
Ocelot gateway configuration with:
- Route definitions
- Upstream/downstream templates
- Authentication options
- Rate limiting per route
- QoS settings (circuit breaker)
- Load balancer options

### appsettings.Development.json
Development-specific overrides:
- Debug logging level
- Disabled rate limiting
- Extended timeouts
- Relaxed JWT expiration

## Middleware Pipeline

Requests flow through:
1. **CorrelationId Middleware** - Add/track request correlation
2. **Request/Response Logging** - Log all requests and responses
3. **Security Headers** - Add security headers to responses
4. **Error Handling** - Catch and normalize exceptions
5. **Request Validation** - Validate size, content-type
6. **Rate Limiting** - Enforce per-client limits
7. **Authentication** - Validate JWT tokens
8. **Authorization** - Check permissions/scopes
9. **Ocelot** - Route to downstream service
10. **Response Transformation** - Normalize response

## Building & Deployment

### Local Development
```bash
# Build
dotnet build

# Run
dotnet run

# Access
- Gateway: http://localhost:5000
- Swagger: http://localhost:5000/swagger
- Health: http://localhost:5000/health
```

### Docker Deployment
```bash
# Build image
docker build -t erp-api-gateway:latest .

# Run container
docker run -p 5000:5000 -e ASPNETCORE_ENVIRONMENT=Production erp-api-gateway:latest
```

### Docker Compose
```bash
# Start all services
docker-compose up -d

# Check logs
docker-compose logs -f api-gateway
```

## Scaling Considerations

### Horizontal Scaling
- **Stateless Design**: Gateway is stateless, can scale horizontally
- **Load Balancer**: Place Nginx/HAProxy in front for distribution
- **Service Discovery**: Configure Consul for dynamic service registration

### Vertical Scaling
- **Thread Pool**: Increase via ThreadPool.SetMinThreads
- **Connection Pool**: Configure HttpClient pool sizes
- **Memory**: Adjust GC settings for high-throughput scenarios

### Rate Limiting
- Default: 100 requests/minute per client
- Burst: 20 requests allowed in burst
- Configure via `X-Client-ID` header

## Monitoring & Observability

### Logging
- **Location**: `logs/` directory
- **Format**: Structured JSON with correlation IDs
- **Retention**: 30 days (configurable)
- **Levels**: Debug (dev), Information (prod)

### Health Checks
- Gateway checks each service's health every 30 seconds
- Failed services marked as degraded (still operational)
- Circuit breaker opens if repeated failures

### Metrics (Prometheus)
Available at `/metrics`:
- HTTP request duration
- Request count
- Request size (bytes)
- Circuit breaker state per service
- HTTP status code distribution

### Performance Tracking
Each request includes:
- **Correlation ID**: Cross-service request tracking
- **Request ID**: Unique request identifier
- **Duration**: Total request/response time
- **Service Latency**: Time spent in each service

## Troubleshooting

### Circuit Breaker Open
**Symptom**: `503 Service Unavailable`
**Cause**: Service has 5+ consecutive failures
**Solution**: 
1. Check service health: `curl http://service:port/health`
2. Wait 30 seconds for circuit to reset
3. Check logs for detailed error messages

### Rate Limit Exceeded
**Symptom**: `429 Too Many Requests`
**Solution**:
1. Provide unique `X-Client-ID` header
2. Reduce request rate
3. Contact admin to adjust limits

### Timeout
**Symptom**: `504 Gateway Timeout`
**Cause**: Service didn't respond within 10 seconds
**Solution**:
1. Check service performance
2. Increase timeout (if persistent slowness expected)
3. Scale service horizontally

### Authentication Failed
**Symptom**: `401 Unauthorized`
**Solution**:
1. Verify JWT token in Authorization header
2. Check token expiration
3. Ensure required scopes are present

### Path Not Found
**Symptom**: `404 Not Found`
**Solution**:
1. Verify service route in request
2. Check ocelot.json route definitions
3. Ensure downstream service is running

## Performance Tuning

### Connection Pooling
```csharp
// Configured in ServiceConfigurationSetup.cs
SocketsHttpHandler with:
- ConnectTimeout: 5 seconds
- AllowAutoRedirect: true
- Compression: GZip + Deflate
```

### Caching Strategy
- **Static Data** (LOV): 15-minute cache
- **Configuration Data** (Location): 10-minute cache
- **Transactional Data**: 5-minute cache or no cache

### Bulkhead Sizes
- **Standard Services**: 10 parallel, 20 queued
- **Shared Service**: 15 parallel, 30 queued (higher traffic)

## Security Best Practices

1. **Always use HTTPS in production**
2. **Rotate JWT secret keys regularly**
3. **Implement API key rotation**
4. **Monitor for suspicious patterns**
5. **Enable rate limiting**
6. **Validate all input**
7. **Use security headers** (CSP, HSTS, etc.)
8. **Log security events**
9. **audit service access**
10. **Update dependencies regularly**

## Contributing

When adding new services:
1. Add route to `ocelot.json`
2. Add service to `ServiceConfigurationSetup.cs`
3. Configure HTTP client in `ConfigureHttpClients`
4. Add authorization policy in `AuthorizationHandler.cs`
5. Update route list in this README

## Support

For issues or questions:
- Check logs: `logs/gateway-*.txt`
- Review health checks: `GET /health`
- Enable debug logging: Set LogLevel to Debug
- Check service-specific logs

---

**Version**: 1.0.0  
**Last Updated**: 2026-03-10  
**Status**: Production Ready
