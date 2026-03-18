# Email Notification Service - API Testing Guide

## Overview
This guide provides endpoints, request/response examples, and instructions for testing the Email Notification Service API using Postman, cURL, or VS Code REST Client.

---

## Base Configuration

| Setting | Value |
|---------|-------|
| **Base URL** | `https://localhost:5001` or `http://localhost:5000` |
| **API Version** | v1 |
| **Date Generated** | March 12, 2026 |

---

## Authentication

The API uses JWT Bearer token authentication. Include the authorization header:

```
Authorization: Bearer <your-jwt-token>
```

### Sample JWT Token Header
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI1MDAwIiwibmFtZSI6ImpvaG4ucm9kZXJ0c0BleGFtcGxlLmNvbSIsImFkbWluIjp0cnVlfQ...
```

---

## Health Check Endpoint

### GET /health

Check the health status of the API and connected services.

**Request:**
```http
GET /health HTTP/1.1
Host: localhost:5000
```

**Response (200 OK):**
```json
{
  "status": "Healthy",
  "checks": {
    "database": "Healthy"
  }
}
```

---

## Email Type Endpoints

### 1. Get All Email Types

**GET** `/api/v1/email-types`

Retrieve all registered email types.

**Request:**
```http
GET /api/v1/email-types HTTP/1.1
Host: localhost:5000
Authorization: Bearer <token>
Content-Type: application/json
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Email types retrieved successfully",
  "data": [
    {
      "id": 1,
      "emailName": "Daily Treasury Report",
      "emailType": "D",
      "emailProcName": "usp_GenerateTreasuryReport",
      "createdDate": "2026-03-12T10:30:00Z"
    },
    {
      "id": 2,
      "emailName": "Weekly Financial Summary",
      "emailType": "W",
      "emailProcName": "usp_GenerateWeeklyReport",
      "createdDate": "2026-03-12T10:30:00Z"
    }
  ]
}
```

---

### 2. Get Email Type by ID

**GET** `/api/v1/email-types/{id}`

Retrieve a specific email type by ID.

**Request:**
```http
GET /api/v1/email-types/1 HTTP/1.1
Host: localhost:5000
Authorization: Bearer <token>
Content-Type: application/json
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Email type retrieved successfully",
  "data": {
    "id": 1,
    "emailName": "Daily Treasury Report",
    "emailType": "D",
    "emailProcName": "usp_GenerateTreasuryReport",
    "createdDate": "2026-03-12T10:30:00Z"
  }
}
```

**Response (404 Not Found):**
```json
{
  "success": false,
  "message": "Email type not found",
  "data": null
}
```

---

### 3. Create Email Type

**POST** `/api/v1/email-types`

Create a new email type.

**Request:**
```http
POST /api/v1/email-types HTTP/1.1
Host: localhost:5000
Authorization: Bearer <token>
Content-Type: application/json

{
  "emailName": "Monthly Compliance Report",
  "emailType": "M",
  "emailProcName": "usp_GenerateComplianceReport"
}
```

**Response (201 Created):**
```json
{
  "success": true,
  "message": "Email type created successfully",
  "data": {
    "id": 3,
    "emailName": "Monthly Compliance Report",
    "emailType": "M",
    "emailProcName": "usp_GenerateComplianceReport",
    "createdDate": "2026-03-12T11:45:00Z"
  }
}
```

**Response (400 Bad Request):**
```json
{
  "success": false,
  "message": "Validation failed",
  "errors": [
    {
      "field": "emailName",
      "message": "Email name is required"
    }
  ]
}
```

---

### 4. Update Email Type

**PUT** `/api/v1/email-types/{id}`

Update an existing email type.

**Request:**
```http
PUT /api/v1/email-types/3 HTTP/1.1
Host: localhost:5000
Authorization: Bearer <token>
Content-Type: application/json

{
  "emailName": "Monthly Compliance Report - Updated",
  "emailType": "M",
  "emailProcName": "usp_GenerateComplianceReport_v2"
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Email type updated successfully",
  "data": {
    "id": 3,
    "emailName": "Monthly Compliance Report - Updated",
    "emailType": "M",
    "emailProcName": "usp_GenerateComplianceReport_v2",
    "createdDate": "2026-03-12T11:45:00Z",
    "modifiedDate": "2026-03-12T12:00:00Z"
  }
}
```

---

### 5. Delete Email Type

**DELETE** `/api/v1/email-types/{id}`

Delete an email type (soft delete).

**Request:**
```http
DELETE /api/v1/email-types/3 HTTP/1.1
Host: localhost:5000
Authorization: Bearer <token>
```

**Response (204 No Content):**
```
(Empty response body)
```

---

## Mail Access Endpoints

### 1. Get All Mail Recipients

**GET** `/api/v1/mail-access`

Retrieve all mail recipients and their access configurations.

**Request:**
```http
GET /api/v1/mail-access HTTP/1.1
Host: localhost:5000
Authorization: Bearer <token>
Content-Type: application/json
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Mail recipients retrieved successfully",
  "data": [
    {
      "id": 1,
      "emailTypeId": 1,
      "organizationId": 100,
      "businessId": 200,
      "employeeSystemId": 5000,
      "emailAddress": "john.roderts@example.com",
      "recipientName": "John Roberts",
      "createdDate": "2026-03-12T10:35:00Z"
    }
  ]
}
```

---

### 2. Get Mail Recipient by ID

**GET** `/api/v1/mail-access/{id}`

Retrieve a specific mail recipient configuration.

**Request:**
```http
GET /api/v1/mail-access/1 HTTP/1.1
Host: localhost:5000
Authorization: Bearer <token>
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Mail recipient retrieved successfully",
  "data": {
    "id": 1,
    "emailTypeId": 1,
    "organizationId": 100,
    "businessId": 200,
    "employeeSystemId": 5000,
    "emailAddress": "john.roderts@example.com",
    "recipientName": "John Roberts",
    "createdDate": "2026-03-12T10:35:00Z"
  }
}
```

---

### 3. Create Mail Recipient

**POST** `/api/v1/mail-access`

Create a new mail recipient configuration.

**Request:**
```http
POST /api/v1/mail-access HTTP/1.1
Host: localhost:5000
Authorization: Bearer <token>
Content-Type: application/json

{
  "emailTypeId": 1,
  "organizationId": 100,
  "businessId": 200,
  "employeeSystemId": 5000,
  "emailAddress": "jane.smith@example.com",
  "recipientName": "Jane Smith"
}
```

**Response (201 Created):**
```json
{
  "success": true,
  "message": "Mail recipient created successfully",
  "data": {
    "id": 2,
    "emailTypeId": 1,
    "organizationId": 100,
    "businessId": 200,
    "employeeSystemId": 5000,
    "emailAddress": "jane.smith@example.com",
    "recipientName": "Jane Smith",
    "createdDate": "2026-03-12T12:15:00Z"
  }
}
```

---

### 4. Update Mail Recipient

**PUT** `/api/v1/mail-access/{id}`

Update an existing mail recipient configuration.

**Request:**
```http
PUT /api/v1/mail-access/2 HTTP/1.1
Host: localhost:5000
Authorization: Bearer <token>
Content-Type: application/json

{
  "emailTypeId": 2,
  "organizationId": 100,
  "businessId": 200,
  "employeeSystemId": 5001,
  "emailAddress": "jane.smith@company.local",
  "recipientName": "Jane Smith - Updated"
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Mail recipient updated successfully",
  "data": {
    "id": 2,
    "emailTypeId": 2,
    "organizationId": 100,
    "businessId": 200,
    "employeeSystemId": 5001,
    "emailAddress": "jane.smith@company.local",
    "recipientName": "Jane Smith - Updated",
    "createdDate": "2026-03-12T12:15:00Z",
    "modifiedDate": "2026-03-12T12:30:00Z"
  }
}
```

---

### 5. Delete Mail Recipient

**DELETE** `/api/v1/mail-access/{id}`

Delete a mail recipient (soft delete).

**Request:**
```http
DELETE /api/v1/mail-access/2 HTTP/1.1
Host: localhost:5000
Authorization: Bearer <token>
```

**Response (204 No Content):**
```
(Empty response body)
```

---

## Send Email Endpoint

### POST /api/v1/emails/send

Send an email notification to configured recipients.

**Request:**
```http
POST /api/v1/emails/send HTTP/1.1
Host: localhost:5000
Authorization: Bearer <token>
Content-Type: application/json

{
  "emailTypeId": 1,
  "subject": "Daily Treasury Report",
  "body": "<h1>Treasury Report</h1><p>Report data here...</p>",
  "recipients": [
    {
      "email": "john.roderts@example.com",
      "name": "John Roberts"
    }
  ],
  "isHtml": true,
  "priority": "High"
}
```

**Response (202 Accepted):**
```json
{
  "success": true,
  "message": "Email queued for delivery",
  "data": {
    "emailId": "550e8400-e29b-41d4-a716-446655440000",
    "status": "Queued",
    "queuedTime": "2026-03-12T12:45:00Z",
    "recipientCount": 1
  }
}
```

---

## Error Responses

### 401 Unauthorized
```json
{
  "success": false,
  "message": "Unauthorized access",
  "errors": [
    {
      "code": "UNAUTHORIZED",
      "message": "Invalid or expired token"
    }
  ]
}
```

### 403 Forbidden
```json
{
  "success": false,
  "message": "Access denied",
  "errors": [
    {
      "code": "FORBIDDEN",
      "message": "Insufficient permissions"
    }
  ]
}
```

### 500 Internal Server Error
```json
{
  "success": false,
  "message": "An error occurred while processing your request",
  "errors": [
    {
      "code": "INTERNAL_ERROR",
      "message": "An unexpected error occurred"
    }
  ]
}
```

---

## Testing with cURL

### Test Health Endpoint
```bash
curl -X GET https://localhost:5000/health
```

### Test Get All Email Types
```bash
curl -X GET https://localhost:5000/api/v1/email-types \
  -H "Authorization: Bearer <your-jwt-token>" \
  -H "Content-Type: application/json"
```

### Test Create Email Type
```bash
curl -X POST https://localhost:5000/api/v1/email-types \
  -H "Authorization: Bearer <your-jwt-token>" \
  -H "Content-Type: application/json" \
  -d '{
    "emailName": "Test Email Type",
    "emailType": "T",
    "emailProcName": "usp_TestProc"
  }'
```

---

## Testing with Postman

1. **Create a new collection** for Email Notification Service
2. **Set environment variables:**
   - `base_url`: `https://localhost:5000`
   - `auth_token`: Your JWT token
3. **Add requests** using the endpoints above
4. **Use pre-request scripts** to auto-refresh JWT tokens if needed
5. **Set up tests** to validate response status codes and body structure

---

## Integration Testing Checklist

- [ ] Health check endpoint returns 200
- [ ] Retrieve all email types (GET)
- [ ] Create a new email type (POST)
- [ ] Retrieve specific email type (GET)
- [ ] Update email type (PUT)
- [ ] Delete email type (DELETE)
- [ ] Retrieve all mail recipients (GET)
- [ ] Create a new mail recipient (POST)
- [ ] Retrieve specific mail recipient (GET)
- [ ] Update mail recipient (PUT)
- [ ] Delete mail recipient (DELETE)
- [ ] Send email notification (POST)
- [ ] Verify JWT authentication is enforced
- [ ] Test error handling for invalid requests
- [ ] Verify database migrations applied
- [ ] Confirm seed data loaded successfully

---

## Database Setup Instructions

### Prerequisites
- SQL Server 2019 or later
- .NET SDK 10.0 or later
- Entity Framework Core CLI tools

### Steps

1. **Update Connection String**
   Update `appsettings.json` with your SQL Server connection:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=.;Database=EmailNotificationService;Trusted_Connection=true;TrustServerCertificate=true;"
     }
   }
   ```

2. **Apply Migrations**
   ```bash
   cd src/EmailNotification.Infrastructure
   dotnet ef database update
   ```

3. **Verify Database**
   - Check that `EmailNotificationService` database was created
   - Verify tables: `EMAIL_TYPEMAST`, `MAIL_ACCESS`

4. **Seed Data**
   - Data is automatically seeded on first application run
   - Check database for sample email types and recipients

---

## Performance Testing

### Load Testing Setup
```bash
# Using Apache JMeter, k6, or similar tools
# Test concurrent requests to /api/v1/email-types

# Expected Performance Metrics:
# - Response Time: < 200ms for GET requests
# - Response Time: < 500ms for POST requests
# - Throughput: > 100 requests/second
# - Error Rate: < 1%
```

---

## Additional Resources

- [API Documentation](./API_DOCUMENTATION.md)
- [Architecture Guide](./ARCHITECTURE.md)
- [Database Schema](./06-EmailNotification_Create_Schema.sql)
- [Module Guide](./MODULE_GUIDE.md)

---

**Last Updated:** March 12, 2026
**API Version:** 1.0
**By:** GitHub Copilot
