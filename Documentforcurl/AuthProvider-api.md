# AuthProvider API Documentation

> **Service:** AuthProvider – Authentication & Authorization Microservice  
> **Base URL:** `http://localhost:5200`  
> **Swagger:** `http://localhost:5200/swagger`  
> **GraphQL Playground:** `http://localhost:5200/graphql`  
> **Health Check:** `http://localhost:5200/health`

---

## Table of Contents

1. [Overview](#overview)
2. [Authentication & Authorization](#authentication--authorization)
3. [REST API Endpoints](#rest-api-endpoints)
   - [Auth Controller (v1)](#auth-controller-v1)
   - [Auth Controller (v2)](#auth-controller-v2)
   - [Users Controller](#users-controller)
   - [Roles Endpoint](#roles-endpoint)
4. [Minimal API Endpoints](#minimal-api-endpoints)
5. [GraphQL API](#graphql-api)
   - [Queries](#graphql-queries)
   - [Mutations](#graphql-mutations)
   - [Subscriptions](#graphql-subscriptions)
6. [Azure Functions](#azure-functions)
7. [Docker Compose Setup](#docker-compose-setup)
8. [JWT Configuration](#jwt-configuration)

---

## Overview

AuthProvider is a standalone authentication and authorization microservice built with:

- **ASP.NET Core** (.NET 8+) with CQRS (MediatR), Repository + Unit of Work pattern
- **HotChocolate** GraphQL server (queries, mutations, subscriptions via WebSocket)
- **Entity Framework Core** + **Dapper** (read side) with SQL Server 2022
- **JWT Bearer** authentication with refresh token rotation
- **API Versioning** (URL segment `/v1/`, `/v2/` + `X-Api-Version` header)
- **Azure Functions** (Token cleanup timer, User created Service Bus trigger)
- **RabbitMQ** for domain event publishing
- **Azure Blob Storage** for event archival
- **Polly** resilience (retry + circuit breaker for external auth)
- **Serilog** structured logging

### Authorization Policies

| Policy | Requirement |
|---|---|
| `AdminOnly` | Role = `ADMIN` |
| `UserOrAdmin` | Role = `USER` or `ADMIN` |
| `RequireEmailVerified` | Custom assertion on email verification |

---

## Authentication & Authorization

All protected endpoints require a JWT Bearer token in the `Authorization` header:

```
Authorization: Bearer <access_token>
```

**Obtain a token** via the [Login endpoint](#post-apiv1authlogin) or [GraphQL Login mutation](#graphql-mutations).

---

## REST API Endpoints

### Auth Controller (v1)

Route prefix: `/api/v1/auth`

---

#### POST `/api/v1/auth/register`

Register a new user account.

- **Auth:** Anonymous
- **Version:** v1 only

**Request Body:**
```json
{
  "username": "johndoe",
  "email": "john@example.com",
  "password": "SecureP@ss123",
  "firstName": "John",
  "lastName": "Doe"
}
```

**cURL:**
```bash
curl -X POST http://localhost:5200/api/v1/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "johndoe",
    "email": "john@example.com",
    "password": "SecureP@ss123",
    "firstName": "John",
    "lastName": "Doe"
  }'
```

**Response:** `201 Created`
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "username": "johndoe",
  "email": "john@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "isActive": true,
  "isEmailVerified": false,
  "createdAt": "2025-01-15T10:00:00Z",
  "lastLoginAt": null,
  "roles": []
}
```

**Error Responses:**
- `400 Bad Request` – Validation errors
- `409 Conflict` – Email already registered

---

#### POST `/api/v1/auth/login`

Authenticate and receive JWT tokens.

- **Auth:** Anonymous
- **Version:** v1 and v2

**Request Body:**
```json
{
  "usernameOrEmail": "john@example.com",
  "password": "SecureP@ss123"
}
```

**cURL:**
```bash
curl -X POST http://localhost:5200/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "usernameOrEmail": "john@example.com",
    "password": "SecureP@ss123"
  }'
```

**Response:** `200 OK`
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "dGhpcyBpcyBhIHJlZnJlc2ggdG9rZW4...",
  "expiresAt": "2025-01-15T11:00:00Z",
  "tokenType": "Bearer"
}
```

**Error Responses:**
- `401 Unauthorized` – Invalid credentials or inactive account

---

#### POST `/api/v1/auth/refresh-token`

Refresh an expired access token using a refresh token (token rotation).

- **Auth:** Anonymous

**Request Body:**
```json
{
  "refreshToken": "dGhpcyBpcyBhIHJlZnJlc2ggdG9rZW4..."
}
```

**cURL:**
```bash
curl -X POST http://localhost:5200/api/v1/auth/refresh-token \
  -H "Content-Type: application/json" \
  -d '{
    "refreshToken": "dGhpcyBpcyBhIHJlZnJlc2ggdG9rZW4..."
  }'
```

**Response:** `200 OK`
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "bmV3IHJlZnJlc2ggdG9rZW4...",
  "expiresAt": "2025-01-15T12:00:00Z",
  "tokenType": "Bearer"
}
```

**Error Responses:**
- `401 Unauthorized` – Invalid or expired/revoked refresh token

---

#### POST `/api/v1/auth/revoke-token`

Revoke a refresh token (logout).

- **Auth:** Bearer token required

**Request Body:**
```json
{
  "refreshToken": "dGhpcyBpcyBhIHJlZnJlc2ggdG9rZW4..."
}
```

**cURL:**
```bash
curl -X POST http://localhost:5200/api/v1/auth/revoke-token \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <access_token>" \
  -d '{
    "refreshToken": "dGhpcyBpcyBhIHJlZnJlc2ggdG9rZW4..."
  }'
```

**Response:** `204 No Content`

---

#### GET `/api/v1/auth/me`

Get the currently authenticated user's profile.

- **Auth:** Bearer token required

**cURL:**
```bash
curl -X GET http://localhost:5200/api/v1/auth/me \
  -H "Authorization: Bearer <access_token>"
```

**Response:** `200 OK`
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "username": "johndoe",
  "email": "john@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "isActive": true,
  "isEmailVerified": false,
  "createdAt": "2025-01-15T10:00:00Z",
  "lastLoginAt": "2025-01-15T10:05:00Z",
  "roles": ["USER"]
}
```

---

### Auth Controller (v2)

Route prefix: `/api/v2/auth`

---

#### POST `/api/v2/auth/login`

v2 login – returns additional API version metadata in the response.

- **Auth:** Anonymous
- **Version:** v2 only

**cURL:**
```bash
curl -X POST http://localhost:5200/api/v2/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "usernameOrEmail": "john@example.com",
    "password": "SecureP@ss123"
  }'
```

**Response:** `200 OK`
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "dGhpcyBpcyBhIHJlZnJlc2ggdG9rZW4...",
  "expiresAt": "2025-01-15T11:00:00Z",
  "apiVersion": "2.0"
}
```

---

### Users Controller

Route prefix: `/api/v1/users`

All endpoints require authentication. Some require `AdminOnly` or `UserOrAdmin` policy.

---

#### GET `/api/v1/users`

Get a paged list of all users.

- **Auth:** `AdminOnly` policy

**Query Parameters:**
| Parameter | Type | Default | Description |
|---|---|---|---|
| `page` | int | 1 | Page number |
| `pageSize` | int | 20 | Items per page |

**cURL:**
```bash
curl -X GET "http://localhost:5200/api/v1/users?page=1&pageSize=10" \
  -H "Authorization: Bearer <admin_token>"
```

**Response:** `200 OK`
```json
{
  "items": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "username": "johndoe",
      "email": "john@example.com",
      "firstName": "John",
      "lastName": "Doe",
      "isActive": true,
      "isEmailVerified": false,
      "createdAt": "2025-01-15T10:00:00Z",
      "lastLoginAt": "2025-01-15T10:05:00Z",
      "roles": ["USER"]
    }
  ],
  "totalCount": 1,
  "page": 1,
  "pageSize": 10
}
```

---

#### GET `/api/v1/users/{id}`

Get a user by their ID.

- **Auth:** `UserOrAdmin` policy

**cURL:**
```bash
curl -X GET http://localhost:5200/api/v1/users/3fa85f64-5717-4562-b3fc-2c963f66afa6 \
  -H "Authorization: Bearer <access_token>"
```

**Response:** `200 OK` – Returns `UserDto` (same as register response)

**Error Responses:**
- `404 Not Found` – User not found

---

#### GET `/api/v1/users/by-email?email={email}`

Get a user by email address.

- **Auth:** `AdminOnly` policy

**cURL:**
```bash
curl -X GET "http://localhost:5200/api/v1/users/by-email?email=john@example.com" \
  -H "Authorization: Bearer <admin_token>"
```

**Response:** `200 OK` – Returns `UserDto`

**Error Responses:**
- `404 Not Found` – User not found

---

#### PUT `/api/v1/users/{id}`

Update a user's profile.

- **Auth:** Bearer token required

**Request Body:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "firstName": "Jonathan",
  "lastName": "Doe-Smith"
}
```

**cURL:**
```bash
curl -X PUT http://localhost:5200/api/v1/users/3fa85f64-5717-4562-b3fc-2c963f66afa6 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <access_token>" \
  -d '{
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "firstName": "Jonathan",
    "lastName": "Doe-Smith"
  }'
```

**Response:** `200 OK` – Returns updated `UserDto`

**Error Responses:**
- `404 Not Found` – User not found

---

#### DELETE `/api/v1/users/{id}`

Deactivate (soft-delete) a user.

- **Auth:** `AdminOnly` policy

**cURL:**
```bash
curl -X DELETE http://localhost:5200/api/v1/users/3fa85f64-5717-4562-b3fc-2c963f66afa6 \
  -H "Authorization: Bearer <admin_token>"
```

**Response:** `204 No Content`

**Error Responses:**
- `404 Not Found` – User not found

---

#### POST `/api/v1/users/{id}/roles`

Assign a role to a user.

- **Auth:** `AdminOnly` policy

**Request Body:**
```json
{
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "roleName": "ADMIN"
}
```

**cURL:**
```bash
curl -X POST http://localhost:5200/api/v1/users/3fa85f64-5717-4562-b3fc-2c963f66afa6/roles \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <admin_token>" \
  -d '{
    "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "roleName": "ADMIN"
  }'
```

**Response:** `204 No Content`

**Error Responses:**
- `404 Not Found` – User not found

---

### Roles Endpoint

---

#### GET `/api/v1/roles`

Get all available roles.

- **Auth:** `AdminOnly` policy

**cURL:**
```bash
curl -X GET http://localhost:5200/api/v1/roles \
  -H "Authorization: Bearer <admin_token>"
```

**Response:** `200 OK`
```json
[
  {
    "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
    "name": "ADMIN",
    "description": "System Administrator"
  },
  {
    "id": "b2c3d4e5-f6a7-8901-bcde-f12345678901",
    "name": "USER",
    "description": "Standard User"
  }
]
```

---

## Minimal API Endpoints

These lightweight endpoints are available on both v1 and v2.

---

#### GET `/api/v1/minimal/auth/health`

Health check endpoint (Minimal API).

- **Auth:** Anonymous

**cURL:**
```bash
curl http://localhost:5200/api/v1/minimal/auth/health
```

**Response:** `200 OK`
```json
{
  "status": "Healthy",
  "service": "AuthProvider",
  "timestamp": "2025-01-15T10:00:00Z"
}
```

---

#### GET `/api/v1/minimal/auth/version`

Service version information.

- **Auth:** Anonymous

**cURL:**
```bash
curl http://localhost:5200/api/v1/minimal/auth/version
```

**Response:** `200 OK`
```json
{
  "version": "1.0",
  "framework": ".NET 8.0.1"
}
```

---

#### GET `/health`

ASP.NET Core Health Checks endpoint.

- **Auth:** Anonymous

**cURL:**
```bash
curl http://localhost:5200/health
```

**Response:** `200 OK`
```
Healthy
```

---

## GraphQL API

**Endpoint:** `http://localhost:5200/graphql`  
**WebSocket (Subscriptions):** `ws://localhost:5200/graphql`

### GraphQL Queries

All queries require authentication (JWT Bearer token in the `Authorization` header).

---

#### `userById`

Get a user by their ID.

- **Auth:** Any authenticated user

```graphql
query GetUserById {
  userById(userId: "3fa85f64-5717-4562-b3fc-2c963f66afa6") {
    id
    username
    email
    firstName
    lastName
    isActive
    isEmailVerified
    createdAt
    lastLoginAt
    roles
  }
}
```

**cURL:**
```bash
curl -X POST http://localhost:5200/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <access_token>" \
  -d '{
    "query": "query { userById(userId: \"3fa85f64-5717-4562-b3fc-2c963f66afa6\") { id username email firstName lastName isActive isEmailVerified createdAt lastLoginAt roles } }"
  }'
```

---

#### `userByEmail`

Get a user by email.

- **Auth:** Any authenticated user

```graphql
query GetUserByEmail {
  userByEmail(email: "john@example.com") {
    id
    username
    email
    firstName
    lastName
    isActive
    roles
  }
}
```

**cURL:**
```bash
curl -X POST http://localhost:5200/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <access_token>" \
  -d '{
    "query": "query { userByEmail(email: \"john@example.com\") { id username email firstName lastName isActive roles } }"
  }'
```

---

#### `users`

Get a paged list of all users.

- **Auth:** `ADMIN` role required

```graphql
query GetUsers {
  users(page: 1, pageSize: 10) {
    items {
      id
      username
      email
      firstName
      lastName
      isActive
      roles
    }
    totalCount
    page
    pageSize
  }
}
```

**cURL:**
```bash
curl -X POST http://localhost:5200/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <admin_token>" \
  -d '{
    "query": "query { users(page: 1, pageSize: 10) { items { id username email firstName lastName isActive roles } totalCount page pageSize } }"
  }'
```

---

#### `roles`

Get all available roles.

- **Auth:** Any authenticated user

```graphql
query GetRoles {
  roles {
    id
    name
    description
  }
}
```

**cURL:**
```bash
curl -X POST http://localhost:5200/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <access_token>" \
  -d '{
    "query": "query { roles { id name description } }"
  }'
```

---

### GraphQL Mutations

---

#### `registerUser`

Register a new user. Publishes a `UserRegistered` subscription event.

- **Auth:** Anonymous

```graphql
mutation RegisterUser {
  registerUser(input: {
    username: "janedoe",
    email: "jane@example.com",
    password: "SecureP@ss456",
    firstName: "Jane",
    lastName: "Doe"
  }) {
    id
    username
    email
    firstName
    lastName
    isActive
    createdAt
    roles
  }
}
```

**cURL:**
```bash
curl -X POST http://localhost:5200/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { registerUser(input: { username: \"janedoe\", email: \"jane@example.com\", password: \"SecureP@ss456\", firstName: \"Jane\", lastName: \"Doe\" }) { id username email firstName lastName isActive createdAt roles } }"
  }'
```

---

#### `login`

Authenticate and receive JWT tokens.

- **Auth:** Anonymous

```graphql
mutation Login {
  login(input: {
    usernameOrEmail: "jane@example.com",
    password: "SecureP@ss456"
  }) {
    accessToken
    refreshToken
    expiresAt
    tokenType
  }
}
```

**cURL:**
```bash
curl -X POST http://localhost:5200/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { login(input: { usernameOrEmail: \"jane@example.com\", password: \"SecureP@ss456\" }) { accessToken refreshToken expiresAt tokenType } }"
  }'
```

---

#### `updateUser`

Update user profile.

- **Auth:** Any authenticated user

```graphql
mutation UpdateUser {
  updateUser(input: {
    userId: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    firstName: "Jane Updated",
    lastName: "Doe-Smith"
  }) {
    id
    firstName
    lastName
  }
}
```

**cURL:**
```bash
curl -X POST http://localhost:5200/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <access_token>" \
  -d '{
    "query": "mutation { updateUser(input: { userId: \"3fa85f64-5717-4562-b3fc-2c963f66afa6\", firstName: \"Jane Updated\", lastName: \"Doe-Smith\" }) { id firstName lastName } }"
  }'
```

---

#### `assignRole`

Assign a role to a user.

- **Auth:** `ADMIN` role required

```graphql
mutation AssignRole {
  assignRole(input: {
    userId: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    roleName: "ADMIN"
  })
}
```

**cURL:**
```bash
curl -X POST http://localhost:5200/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <admin_token>" \
  -d '{
    "query": "mutation { assignRole(input: { userId: \"3fa85f64-5717-4562-b3fc-2c963f66afa6\", roleName: \"ADMIN\" }) }"
  }'
```

---

#### `deleteUser`

Deactivate (soft-delete) a user.

- **Auth:** `ADMIN` role required

```graphql
mutation DeleteUser {
  deleteUser(userId: "3fa85f64-5717-4562-b3fc-2c963f66afa6")
}
```

**cURL:**
```bash
curl -X POST http://localhost:5200/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <admin_token>" \
  -d '{
    "query": "mutation { deleteUser(userId: \"3fa85f64-5717-4562-b3fc-2c963f66afa6\") }"
  }'
```

---

### GraphQL Subscriptions

Subscriptions use WebSocket transport. Connect to `ws://localhost:5200/graphql`.

---

#### `onUserRegistered`

Subscribe to new user registration events.

- **Auth:** `ADMIN` role required

```graphql
subscription OnUserRegistered {
  onUserRegistered {
    id
    username
    email
    firstName
    lastName
    createdAt
  }
}
```

---

#### `onTokenEvent`

Subscribe to token events (revocations, expirations).

- **Auth:** Any authenticated user

```graphql
subscription OnTokenEvent {
  onTokenEvent
}
```

---

## Azure Functions

AuthProvider includes two Azure Functions in the `functions/AuthProvider.Functions` project.

### TokenCleanupFunction

**Trigger:** Timer (`0 0 * * * *` – every hour at :00)  
**Purpose:** Cleans up expired and revoked refresh tokens from the database.

- Queries `RefreshTokens` table for tokens where `ExpiresAt < NOW()` or `RevokedAt IS NOT NULL`
- Deletes matching records via EF Core
- Logs the count of deleted tokens

### UserCreatedFunction

**Trigger:** Azure Service Bus (`auth.events.usercreatedevent` topic)  
**Purpose:** Processes new user creation events published via RabbitMQ/Service Bus.

**Message Schema:**
```json
{
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "email": "john@example.com",
  "username": "johndoe"
}
```

**Actions Performed:**
1. Sends a welcome email (stub/placeholder)
2. Archives the user creation event to Azure Blob Storage at `auth-user-events/users/{userId}/created-{timestamp}.json`

---

## Docker Compose Setup

Start the full stack:

```bash
cd src/Services/AuthProvider
docker-compose up -d
```

### Services

| Service | Container | Port | Description |
|---|---|---|---|
| SQL Server 2022 | `mssql-auth-db` | `1433` | Database (AuthProviderDB) |
| AuthProvider API | `auth-provider` | `5200` | API service |

### Connection String

```
Data Source=localhost,1433;Initial Catalog=AuthProviderDB;User ID=sa;Password=YourPassword123!;Encrypt=False;TrustServerCertificate=True;
```

---

## JWT Configuration

| Setting | Value |
|---|---|
| Secret Key | `AuthProviderSuperSecretKey_ChangeInProduction_Min32Chars!` |
| Issuer | `AuthProvider` |
| Audience | `AuthProviderClients` |
| Access Token Expiry | 60 minutes |
| Refresh Token Expiry | 7 days |
| Algorithm | HMAC-SHA256 |
| Clock Skew | Zero |

### JWT Claims

| Claim | Description |
|---|---|
| `sub` | User ID (GUID) |
| `email` | User email address |
| `unique_name` | Username |
| `jti` | Unique token identifier |
| `firstName` | User first name |
| `lastName` | User last name |
| `role` | User roles (one claim per role) |

---

## Quick Start – End-to-End Flow

```bash
# 1. Start the infrastructure
cd src/Services/AuthProvider
docker-compose up -d

# 2. Register a new user
curl -X POST http://localhost:5200/api/v1/auth/register \
  -H "Content-Type: application/json" \
  -d '{"username":"testuser","email":"test@example.com","password":"Test@123456","firstName":"Test","lastName":"User"}'

# 3. Login to get tokens
TOKEN=$(curl -s -X POST http://localhost:5200/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{"usernameOrEmail":"test@example.com","password":"Test@123456"}' | jq -r '.accessToken')

# 4. Get current user profile
curl http://localhost:5200/api/v1/auth/me \
  -H "Authorization: Bearer $TOKEN"

# 5. List all roles (requires ADMIN)
curl http://localhost:5200/api/v1/roles \
  -H "Authorization: Bearer $TOKEN"

# 6. Check health
curl http://localhost:5200/api/v1/minimal/auth/health

# 7. GraphQL query
curl -X POST http://localhost:5200/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{"query":"{ roles { id name description } }"}'
```
