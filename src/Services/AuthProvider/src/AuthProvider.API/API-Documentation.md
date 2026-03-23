# AuthProvider API Documentation

> **Base URL:** `http://localhost:5200`  
> **Swagger UI:** `http://localhost:5200/swagger`  
> **GraphQL Playground:** `http://localhost:5200/graphql`  
> **API Versions:** `v1.0`, `v2.0`

---

## Table of Contents

- [Authentication](#authentication)
- [REST API Endpoints](#rest-api-endpoints)
  - [Health & Info](#health--info)
  - [Auth Endpoints](#auth-endpoints)
  - [User Endpoints](#user-endpoints)
  - [Role Endpoints](#role-endpoints)
- [GraphQL API](#graphql-api)
  - [Queries](#queries)
  - [Mutations](#mutations)
  - [Subscriptions](#subscriptions)
- [Data Models](#data-models)

---

## Authentication

All protected endpoints require a **JWT Bearer** token in the `Authorization` header.

```
Authorization: Bearer <access_token>
```

### Authorization Policies

| Policy       | Description                          |
|--------------|--------------------------------------|
| `AdminOnly`  | Requires `ADMIN` role                |
| `UserOrAdmin`| Requires `USER` or `ADMIN` role      |
| `[Authorize]`| Any valid authenticated user          |

---

## REST API Endpoints

### Health & Info

#### Health Check

```bash
curl -X GET http://localhost:5200/health
```

**Response:** `200 OK`
```json
"Healthy"
```

---

#### Minimal Health Check

```bash
curl -X GET http://localhost:5200/api/v1/minimal/auth/health
```

**Response:** `200 OK`
```json
{
  "status": "Healthy",
  "service": "AuthProvider",
  "timestamp": "2026-03-23T10:17:53.209Z"
}
```

---

#### Version Info

```bash
curl -X GET http://localhost:5200/api/v1/minimal/auth/version
```

**Response:** `200 OK`
```json
{
  "version": "1.0",
  "framework": ".NET 10.0.1"
}
```

---

### Auth Endpoints

#### Register User

```bash
curl -X POST http://localhost:5200/api/v1/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "johndoe",
    "email": "johndoe@example.com",
    "password": "MyP@ssw0rd",
    "firstName": "John",
    "lastName": "Doe"
  }'
```

**Response:** `201 Created`
```json
{
  "id": "27b152cb-cd6f-4b6f-a9cb-b03216831176",
  "username": "johndoe",
  "email": "johndoe@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "isActive": true,
  "isEmailVerified": false,
  "createdAt": "2026-03-23T10:18:12.014Z",
  "roles": []
}
```

| Status | Description                       |
|--------|-----------------------------------|
| 201    | User created                      |
| 400    | Validation error                  |
| 409    | Username or email already exists  |

---

#### Login (v1)

```bash
curl -X POST http://localhost:5200/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "usernameOrEmail": "johndoe@example.com",
    "password": "MyP@ssw0rd"
  }'
```

**Response:** `200 OK`
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "9g5lJPl90D5sGMdCr7mj7Bwv...",
  "expiresAt": "2026-03-23T11:18:24.169Z",
  "tokenType": "Bearer"
}
```

| Status | Description             |
|--------|-------------------------|
| 200    | Login successful        |
| 401    | Invalid credentials     |

---

#### Login (v2)

Returns an additional `ApiVersion` field in the response.

```bash
curl -X POST http://localhost:5200/api/v2/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "usernameOrEmail": "johndoe@example.com",
    "password": "MyP@ssw0rd"
  }'
```

---

#### Refresh Token

```bash
curl -X POST http://localhost:5200/api/v1/auth/refresh-token \
  -H "Content-Type: application/json" \
  -d '{
    "refreshToken": "9g5lJPl90D5sGMdCr7mj7Bwv..."
  }'
```

**Response:** `200 OK`
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "newRefreshTokenValue...",
  "expiresAt": "2026-03-23T12:00:00.000Z",
  "tokenType": "Bearer"
}
```

| Status | Description             |
|--------|-------------------------|
| 200    | Token refreshed         |
| 401    | Invalid refresh token   |

---

#### Revoke Token

> **Auth Required:** Bearer Token

```bash
curl -X POST http://localhost:5200/api/v1/auth/revoke-token \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <access_token>" \
  -d '{
    "refreshToken": "9g5lJPl90D5sGMdCr7mj7Bwv..."
  }'
```

**Response:** `204 No Content`

---

#### Get Current User

> **Auth Required:** Bearer Token

```bash
curl -X GET http://localhost:5200/api/v1/auth/me \
  -H "Authorization: Bearer <access_token>"
```

**Response:** `200 OK`
```json
{
  "id": "27b152cb-cd6f-4b6f-a9cb-b03216831176",
  "username": "johndoe",
  "email": "johndoe@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "isActive": true,
  "isEmailVerified": false,
  "createdAt": "2026-03-23T10:18:12.014Z",
  "lastLoginAt": "2026-03-23T10:18:24.060Z",
  "roles": []
}
```

---

### User Endpoints

#### Get All Users (Paginated)

> **Auth Required:** `AdminOnly`

```bash
curl -X GET "http://localhost:5200/api/v1/users?page=1&pageSize=20" \
  -H "Authorization: Bearer <admin_token>"
```

**Response:** `200 OK`
```json
{
  "items": [
    {
      "id": "27b152cb-cd6f-4b6f-a9cb-b03216831176",
      "username": "johndoe",
      "email": "johndoe@example.com",
      "firstName": "John",
      "lastName": "Doe",
      "isActive": true,
      "isEmailVerified": false,
      "createdAt": "2026-03-23T10:18:12.014Z",
      "lastLoginAt": "2026-03-23T10:18:24.060Z",
      "roles": ["USER"]
    }
  ],
  "totalCount": 1,
  "page": 1,
  "pageSize": 20
}
```

---

#### Get User by ID

> **Auth Required:** `UserOrAdmin`

```bash
curl -X GET http://localhost:5200/api/v1/users/27b152cb-cd6f-4b6f-a9cb-b03216831176 \
  -H "Authorization: Bearer <access_token>"
```

**Response:** `200 OK` — Returns `UserDto`

| Status | Description    |
|--------|----------------|
| 200    | User found     |
| 404    | User not found |

---

#### Get User by Email

> **Auth Required:** `AdminOnly`

```bash
curl -X GET "http://localhost:5200/api/v1/users/by-email?email=johndoe@example.com" \
  -H "Authorization: Bearer <admin_token>"
```

**Response:** `200 OK` — Returns `UserDto`

| Status | Description    |
|--------|----------------|
| 200    | User found     |
| 404    | User not found |

---

#### Update User

> **Auth Required:** Bearer Token

```bash
curl -X PUT http://localhost:5200/api/v1/users/27b152cb-cd6f-4b6f-a9cb-b03216831176 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <access_token>" \
  -d '{
    "id": "27b152cb-cd6f-4b6f-a9cb-b03216831176",
    "firstName": "Jonathan",
    "lastName": "Doe"
  }'
```

**Response:** `200 OK` — Returns updated `UserDto`

| Status | Description    |
|--------|----------------|
| 200    | User updated   |
| 404    | User not found |

---

#### Delete User (Soft Delete)

> **Auth Required:** `AdminOnly`

```bash
curl -X DELETE http://localhost:5200/api/v1/users/27b152cb-cd6f-4b6f-a9cb-b03216831176 \
  -H "Authorization: Bearer <admin_token>"
```

**Response:** `204 No Content`

| Status | Description    |
|--------|----------------|
| 204    | User deleted   |
| 404    | User not found |

---

#### Assign Role to User

> **Auth Required:** `AdminOnly`

```bash
curl -X POST http://localhost:5200/api/v1/users/27b152cb-cd6f-4b6f-a9cb-b03216831176/roles \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <admin_token>" \
  -d '{
    "userId": "27b152cb-cd6f-4b6f-a9cb-b03216831176",
    "roleName": "USER"
  }'
```

**Response:** `204 No Content`

| Status | Description    |
|--------|----------------|
| 204    | Role assigned  |
| 404    | User not found |

---

### Role Endpoints

#### Get All Roles

> **Auth Required:** `AdminOnly`

```bash
curl -X GET http://localhost:5200/api/v1/roles \
  -H "Authorization: Bearer <admin_token>"
```

**Response:** `200 OK`
```json
[
  { "id": "22222222-0001-0001-0001-000000000001", "name": "ADMIN", "description": "..." },
  { "id": "22222222-0002-0001-0001-000000000001", "name": "USER", "description": "..." },
  { "id": "22222222-0003-0001-0001-000000000001", "name": "AUDITOR", "description": "..." }
]
```

---

## GraphQL API

**Endpoint:** `POST http://localhost:5200/graphql`

All GraphQL requests use `POST` with `Content-Type: application/json`. The request body must include a `query` field.

```bash
curl -X POST http://localhost:5200/graphql \
  -H "Content-Type: application/json" \
  -d '{ "query": "{ ... }" }'
```

For authenticated operations, include the JWT token:

```bash
curl -X POST http://localhost:5200/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <access_token>" \
  -d '{ "query": "{ ... }" }'
```

---

### Queries

#### userById

> **Auth Required:** Bearer Token

```bash
curl -X POST http://localhost:5200/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <access_token>" \
  -d '{
    "query": "{ userById(userId: \"27b152cb-cd6f-4b6f-a9cb-b03216831176\") { id username email firstName lastName isActive isEmailVerified createdAt lastLoginAt roles } }"
  }'
```

**Response:**
```json
{
  "data": {
    "userById": {
      "id": "27b152cb-cd6f-4b6f-a9cb-b03216831176",
      "username": "johndoe",
      "email": "johndoe@example.com",
      "firstName": "John",
      "lastName": "Doe",
      "isActive": true,
      "isEmailVerified": false,
      "createdAt": "2026-03-23T10:18:12.014Z",
      "lastLoginAt": "2026-03-23T10:18:24.060Z",
      "roles": []
    }
  }
}
```

---

#### userByEmail

> **Auth Required:** Bearer Token

```bash
curl -X POST http://localhost:5200/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <access_token>" \
  -d '{
    "query": "{ userByEmail(email: \"johndoe@example.com\") { id username email firstName lastName isActive } }"
  }'
```

**Response:**
```json
{
  "data": {
    "userByEmail": {
      "id": "27b152cb-cd6f-4b6f-a9cb-b03216831176",
      "username": "johndoe",
      "email": "johndoe@example.com",
      "firstName": "John",
      "lastName": "Doe",
      "isActive": true
    }
  }
}
```

---

#### users (Paginated)

> **Auth Required:** `ADMIN` role

```bash
curl -X POST http://localhost:5200/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <admin_token>" \
  -d '{
    "query": "{ users(page: 1, pageSize: 10) { items { id username email firstName lastName isActive roles } totalCount page pageSize } }"
  }'
```

**Response:**
```json
{
  "data": {
    "users": {
      "items": [
        {
          "id": "27b152cb-cd6f-4b6f-a9cb-b03216831176",
          "username": "johndoe",
          "email": "johndoe@example.com",
          "firstName": "John",
          "lastName": "Doe",
          "isActive": true,
          "roles": ["USER"]
        }
      ],
      "totalCount": 1,
      "page": 1,
      "pageSize": 10
    }
  }
}
```

---

#### roles

> **Auth Required:** Bearer Token

```bash
curl -X POST http://localhost:5200/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <access_token>" \
  -d '{
    "query": "{ roles { id name description } }"
  }'
```

**Response:**
```json
{
  "data": {
    "roles": [
      { "id": "22222222-0001-0001-0001-000000000001", "name": "ADMIN", "description": "..." },
      { "id": "22222222-0002-0001-0001-000000000001", "name": "USER", "description": "..." },
      { "id": "22222222-0003-0001-0001-000000000001", "name": "AUDITOR", "description": "..." }
    ]
  }
}
```

---

### Mutations

#### registerUser

> **Auth:** Anonymous

```bash
curl -X POST http://localhost:5200/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { registerUser(input: { username: \"janedoe\", email: \"janedoe@example.com\", password: \"MyP@ssw0rd\", firstName: \"Jane\", lastName: \"Doe\" }) { id username email firstName lastName isActive } }"
  }'
```

**Response:**
```json
{
  "data": {
    "registerUser": {
      "id": "09101417-0896-4a55-b23f-5324089315ef",
      "username": "janedoe",
      "email": "janedoe@example.com",
      "firstName": "Jane",
      "lastName": "Doe",
      "isActive": true
    }
  }
}
```

---

#### login

> **Auth:** Anonymous

```bash
curl -X POST http://localhost:5200/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { login(input: { usernameOrEmail: \"johndoe@example.com\", password: \"MyP@ssw0rd\" }) { accessToken refreshToken expiresAt tokenType } }"
  }'
```

**Response:**
```json
{
  "data": {
    "login": {
      "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
      "refreshToken": "SPTYpLrzDMGDf3up0B3CTnWCMQz...",
      "expiresAt": "2026-03-23T11:33:00.341Z",
      "tokenType": "Bearer"
    }
  }
}
```

---

#### updateUser

> **Auth Required:** Bearer Token

```bash
curl -X POST http://localhost:5200/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <access_token>" \
  -d '{
    "query": "mutation { updateUser(input: { userId: \"27b152cb-cd6f-4b6f-a9cb-b03216831176\", firstName: \"Jonathan\", lastName: \"Doe\" }) { id username firstName lastName } }"
  }'
```

**Response:**
```json
{
  "data": {
    "updateUser": {
      "id": "27b152cb-cd6f-4b6f-a9cb-b03216831176",
      "username": "johndoe",
      "firstName": "Jonathan",
      "lastName": "Doe"
    }
  }
}
```

---

#### assignRole

> **Auth Required:** `ADMIN` role

```bash
curl -X POST http://localhost:5200/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <admin_token>" \
  -d '{
    "query": "mutation { assignRole(input: { userId: \"27b152cb-cd6f-4b6f-a9cb-b03216831176\", roleName: \"USER\" }) }"
  }'
```

**Response:**
```json
{
  "data": {
    "assignRole": true
  }
}
```

---

#### deleteUser

> **Auth Required:** `ADMIN` role

```bash
curl -X POST http://localhost:5200/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <admin_token>" \
  -d '{
    "query": "mutation { deleteUser(userId: \"27b152cb-cd6f-4b6f-a9cb-b03216831176\") }"
  }'
```

**Response:**
```json
{
  "data": {
    "deleteUser": true
  }
}
```

---

### Subscriptions

Subscriptions use **WebSocket** at `ws://localhost:5200/graphql`.

#### onUserRegistered

> **Auth Required:** `ADMIN` role

Subscribes to new user registration events.

```graphql
subscription {
  onUserRegistered {
    id
    username
    email
    firstName
    lastName
    isActive
  }
}
```

---

#### onTokenEvent

> **Auth Required:** Bearer Token

Subscribes to token revocation/expiration events.

```graphql
subscription {
  onTokenEvent
}
```

---

## Data Models

### UserDto

| Field             | Type        | Nullable | Description               |
|-------------------|-------------|----------|---------------------------|
| `id`              | `UUID`      | No       | Unique user ID            |
| `username`        | `String`    | No       | Username                  |
| `email`           | `String`    | No       | Email address             |
| `firstName`       | `String`    | No       | First name                |
| `lastName`        | `String`    | No       | Last name                 |
| `isActive`        | `Boolean`   | No       | Account active status     |
| `isEmailVerified` | `Boolean`   | No       | Email verified status     |
| `createdAt`       | `DateTime`  | No       | Account creation date     |
| `lastLoginAt`     | `DateTime`  | Yes      | Last login timestamp      |
| `roles`           | `[String]`  | No       | Assigned role names       |

### CreateUserDto / CreateUserInput

| Field       | Type     | Required | Description              |
|-------------|----------|----------|--------------------------|
| `username`  | `String` | Yes      | Unique username          |
| `email`     | `String` | Yes      | Valid email address      |
| `password`  | `String` | Yes      | Min 8 chars, upper+lower+digit+special |
| `firstName` | `String` | Yes      | First name               |
| `lastName`  | `String` | Yes      | Last name                |

### LoginRequestDto / LoginInput

| Field             | Type     | Required | Description                  |
|-------------------|----------|----------|------------------------------|
| `usernameOrEmail` | `String` | Yes      | Username or email address    |
| `password`        | `String` | Yes      | Account password             |

### TokenResponseDto

| Field          | Type       | Description               |
|----------------|------------|---------------------------|
| `accessToken`  | `String`   | JWT access token          |
| `refreshToken` | `String`   | Refresh token             |
| `expiresAt`    | `DateTime` | Access token expiry       |
| `tokenType`    | `String`   | Always `"Bearer"`         |

### UpdateUserDto / UpdateUserInput

| Field       | Type     | Required | Description     |
|-------------|----------|----------|-----------------|
| `id`/`userId` | `UUID` | Yes      | User ID         |
| `firstName` | `String` | Yes      | New first name  |
| `lastName`  | `String` | Yes      | New last name   |

### AssignRoleDto / AssignRoleInput

| Field      | Type     | Required | Description                    |
|------------|----------|----------|--------------------------------|
| `userId`   | `UUID`   | Yes      | Target user ID                 |
| `roleName` | `String` | Yes      | Role name (ADMIN, USER, AUDITOR) |

### RoleDto

| Field         | Type     | Description       |
|---------------|----------|-------------------|
| `id`          | `UUID`   | Role ID           |
| `name`        | `String` | Role name         |
| `description` | `String` | Role description  |

### PagedResult\<T\>

| Field        | Type    | Description             |
|--------------|---------|-------------------------|
| `items`      | `[T]`   | Page of items           |
| `totalCount` | `Int`   | Total matching records  |
| `page`       | `Int`   | Current page number     |
| `pageSize`   | `Int`   | Items per page          |
