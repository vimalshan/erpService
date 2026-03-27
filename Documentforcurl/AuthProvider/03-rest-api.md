# AuthProvider API Documentation

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

