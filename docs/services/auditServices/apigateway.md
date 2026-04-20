# API Gateway Documentation

## Service Overview

**Service Name:** apigateway  
**Description:** Gateway service providing routing and API aggregation for audit services  
**Base URL:** https://localhost:5000  
**Port:** 5000  

---

## Overview

The API Gateway serves as the central entry point for all client requests to the auditServices module. It handles:

- Request routing to appropriate services
- Authentication and authorization
- Rate limiting
- Request/response transformation
- Service discovery
- Load balancing

---

## Architecture

### Services Behind Gateway

The gateway routes requests to the following services:

1. **actionapiServices** (Port 5001)
   - Action management and tracking
   - Endpoint: `/api/actions/`

2. **auditapiServices** (Port 5001)
   - Audit management
   - Endpoint: `/api/audits/`

3. **certificateapiServices** (Port 5001)
   - Certificate management
   - Endpoint: `/api/certificates/`

4. **contractapiServices** (Port 5001)
   - Contract management
   - Endpoint: `/api/contracts/`

5. **financeapiServices** (Port 5001)
   - Finance and invoice management
   - Endpoint: `/api/finance/`

6. **findingsapiServices** (Port 5001)
   - Findings management
   - Endpoint: `/api/findings/`

7. **notificationapiServices** (Port 5001)
   - Notification management
   - Endpoint: `/api/notifications/`

8. **scheduleapiServices** (Port 5001)
   - Schedule management
   - Endpoint: `/api/schedules/`

9. **settingsapiServices** (Port 5001)
   - Settings and user management
   - Endpoint: `/api/settings/`

---

## Gateway Endpoints

### GraphQL Endpoint
All services expose GraphQL endpoints through the gateway:

```
POST https://localhost:5000/graphql
```

The gateway forwards GraphQL queries to the appropriate service based on the query/mutation type.

---

## Request Flow

### 1. Authentication

All requests must include authentication headers:

```bash
Authorization: Bearer YOUR_AUTH_TOKEN
```

### 2. Request Routing

The gateway inspects the request path and routes it to the appropriate service:

```
POST /api/audits/graphql → auditapiServices
POST /api/findings/graphql → findingsapiServices
POST /api/finance/graphql → financeapiServices
...
```

### 3. Response Handling

Responses from services are passed back to the client with appropriate status codes and headers.

---

## Gateway Features

### Rate Limiting

Rate limits are applied per user/API key:
- **Unauthenticated requests:** 10 requests/minute
- **Authenticated requests:** 1000 requests/minute
- **Admin users:** 10000 requests/minute

### Request Transformation

The gateway can transform requests/responses:
- Convert between REST and GraphQL
- Normalize response formats
- Handle versioning

### Caching

Frequently accessed data is cached:
- **Cache TTL:** 5 minutes (configurable)
- **Cached endpoints:** Master data lists, user profiles

### Monitoring

All requests through the gateway are logged and monitored:
- Request latency
- Error rates
- Service health
- Usage statistics

---

## Routing Examples

### Example 1: Access Audit Service
```bash
curl -X POST https://localhost:5000/api/audits/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_AUTH_TOKEN" \
  -d '{
    "query": "query { viewAudits { data { id auditName } } }"
  }'
```

The gateway routes this to: `https://localhost:5001/graphql` (auditapiServices)

### Example 2: Access Findings Service
```bash
curl -X POST https://localhost:5000/api/findings/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_AUTH_TOKEN" \
  -d '{
    "query": "query { getFindings { id findingNumber } }"
  }'
```

The gateway routes this to: `https://localhost:5001/graphql` (findingsapiServices)

### Example 3: Access Finance Service
```bash
curl -X POST https://localhost:5000/api/finance/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_AUTH_TOKEN" \
  -d '{
    "query": "query { InvoiceListPage(pageNumber: 1, pageSize: 10) { data { id invoiceNumber } } }"
  }'
```

The gateway routes this to: `https://localhost:5001/graphql` (financeapiServices)

---

## Error Handling

### Gateway Errors

The gateway can return specific error responses:

#### 404 Not Found
```json
{
  "error": "Not Found",
  "message": "The requested resource was not found",
  "statusCode": 404
}
```

#### 401 Unauthorized
```json
{
  "error": "Unauthorized",
  "message": "Authentication is required",
  "statusCode": 401
}
```

#### 429 Too Many Requests
```json
{
  "error": "Too Many Requests",
  "message": "Rate limit exceeded. Maximum 1000 requests per minute.",
  "statusCode": 429,
  "retryAfter": 60
}
```

#### 503 Service Unavailable
```json
{
  "error": "Service Unavailable",
  "message": "The requested service is temporarily unavailable",
  "statusCode": 503
}
```

---

## Common cURL Examples

### Get Audits through Gateway
```bash
curl -X POST https://localhost:5000/api/audits/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_AUTH_TOKEN" \
  -d '{
    "query": "query { viewAudits { data { id auditName status } } }"
  }'
```

### Get Findings through Gateway
```bash
curl -X POST https://localhost:5000/api/findings/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_AUTH_TOKEN" \
  -d '{
    "query": "query { getFindings { id findingNumber severity } }"
  }'
```

### Get Invoices through Gateway
```bash
curl -X POST https://localhost:5000/api/finance/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_AUTH_TOKEN" \
  -d '{
    "query": "query { InvoiceListPage(pageNumber: 1, pageSize: 10) { data { id invoiceNumber amount } } }"
  }'
```

### Get Schedules through Gateway
```bash
curl -X POST https://localhost:5000/api/schedules/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_AUTH_TOKEN" \
  -d '{
    "query": "query { viewAuditSchedules(calendarScheduleFilter: {}) { data { id auditName scheduledDate } } }"
  }'
```

---

## Service Availability

All services are expected to be available at:
- **Audits:** https://localhost:5000/api/audits/
- **Findings:** https://localhost:5000/api/findings/
- **Finance:** https://localhost:5000/api/finance/
- **Schedules:** https://localhost:5000/api/schedules/
- **Certificates:** https://localhost:5000/api/certificates/
- **Contracts:** https://localhost:5000/api/contracts/
- **Notifications:** https://localhost:5000/api/notifications/
- **Settings:** https://localhost:5000/api/settings/
- **Actions:** https://localhost:5000/api/actions/

---

## Authentication

All requests require a valid Bearer token:
```
Authorization: Bearer YOUR_AUTH_TOKEN
```

For local development, you can generate a test token. Refer to the authentication service documentation for details.

---

## Health Checks

To check the gateway health:

```bash
curl -X GET https://localhost:5000/health
```

Expected response:
```json
{
  "status": "healthy",
  "timestamp": "2024-01-15T10:30:00Z",
  "uptime": 3600,
  "services": {
    "auditapiServices": "available",
    "findingsapiServices": "available",
    "financeapiServices": "available",
    "scheduleapiServices": "available",
    "certificateapiServices": "available",
    "contractapiServices": "available",
    "notificationapiServices": "available",
    "settingsapiServices": "available",
    "actionapiServices": "available"
  }
}
```

---

## Notes

- Use `https://localhost:5000` for gateway access
- Individual services run on port `5001`
- All requests are rate-limited per user
- GraphQL endpoint is centralized through the gateway
- For direct service access (debugging), use port 5001
- Gateway logs all requests for audit trail purposes
