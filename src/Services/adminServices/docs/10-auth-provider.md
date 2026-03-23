# Auth Provider

> **Port:** 7136 | **Swagger:** http://localhost:7136/swagger | **GraphQL:** http://localhost:7136/graphql

---

## REST Endpoints

### Register User
```bash
curl -X POST http://localhost:7136/api/v1/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "john.doe",
    "email": "john.doe@example.com",
    "password": "SecureP@ss123",
    "firstName": "John",
    "lastName": "Doe"
  }'
```
**Response:**
```json
{
  "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
  "username": "john.doe",
  "email": "john.doe@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "isActive": true,
  "isEmailVerified": false,
  "createdAt": "2026-03-23T10:00:00Z",
  "roles": ["User"]
}
```

### Login
```bash
curl -X POST http://localhost:7136/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "usernameOrEmail": "john.doe",
    "password": "SecureP@ss123"
  }'
```
**Response:**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "dGhpcyBpcyBhIHJlZnJlc2ggdG9rZW4...",
  "expiresAt": "2026-03-23T11:00:00Z",
  "tokenType": "Bearer"
}
```

### Login v2 (with metadata)
```bash
curl -X POST http://localhost:7136/api/v2/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "usernameOrEmail": "john.doe",
    "password": "SecureP@ss123"
  }'
```

### Refresh Token
```bash
curl -X POST http://localhost:7136/api/v1/auth/refresh-token \
  -H "Content-Type: application/json" \
  -d '{
    "refreshToken": "dGhpcyBpcyBhIHJlZnJlc2ggdG9rZW4..."
  }'
```
**Response:**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...(new)",
  "refreshToken": "bmV3IHJlZnJlc2ggdG9rZW4...",
  "expiresAt": "2026-03-23T12:00:00Z",
  "tokenType": "Bearer"
}
```

### Revoke Token (Requires Auth)
```bash
curl -X POST http://localhost:7136/api/v1/auth/revoke-token \
  -H "Authorization: Bearer <ACCESS_TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "refreshToken": "dGhpcyBpcyBhIHJlZnJlc2ggdG9rZW4..."
  }'
```

### Get Current User (Requires Auth)
```bash
curl -X GET http://localhost:7136/api/v1/auth/me \
  -H "Authorization: Bearer <ACCESS_TOKEN>"
```
**Response:**
```json
{
  "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
  "username": "john.doe",
  "email": "john.doe@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "isActive": true,
  "isEmailVerified": false,
  "createdAt": "2026-03-23T10:00:00Z",
  "lastLoginAt": "2026-03-23T10:30:00Z",
  "roles": ["User"]
}
```

### Get All Users (Admin Only)
```bash
curl -X GET "http://localhost:7136/api/v1/users?page=1&size=20" \
  -H "Authorization: Bearer <ADMIN_TOKEN>"
```
**Response:**
```json
{
  "items": [
    {
      "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
      "username": "john.doe",
      "email": "john.doe@example.com",
      "firstName": "John",
      "lastName": "Doe",
      "isActive": true,
      "roles": ["User"]
    }
  ],
  "totalCount": 1,
  "page": 1,
  "pageSize": 20
}
```

### Get User by ID
```bash
curl -X GET http://localhost:7136/api/v1/users/a1b2c3d4-e5f6-7890-abcd-ef1234567890 \
  -H "Authorization: Bearer <ACCESS_TOKEN>"
```

### Get User by Email (Admin Only)
```bash
curl -X GET "http://localhost:7136/api/v1/users/by-email?email=john.doe@example.com" \
  -H "Authorization: Bearer <ADMIN_TOKEN>"
```

### Update User Profile
```bash
curl -X PUT http://localhost:7136/api/v1/users/a1b2c3d4-e5f6-7890-abcd-ef1234567890 \
  -H "Authorization: Bearer <ACCESS_TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
    "firstName": "Jonathan",
    "lastName": "Doe"
  }'
```

### Deactivate User (Admin Only)
```bash
curl -X DELETE http://localhost:7136/api/v1/users/a1b2c3d4-e5f6-7890-abcd-ef1234567890 \
  -H "Authorization: Bearer <ADMIN_TOKEN>"
```

### Assign Role (Admin Only)
```bash
curl -X POST http://localhost:7136/api/v1/users/a1b2c3d4-e5f6-7890-abcd-ef1234567890/roles \
  -H "Authorization: Bearer <ADMIN_TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
    "roleName": "Admin"
  }'
```

---

## GraphQL

### Query: Get User by ID
```bash
curl -X POST http://localhost:7136/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <ACCESS_TOKEN>" \
  -d '{
    "query": "{ userById(id: \"a1b2c3d4-e5f6-7890-abcd-ef1234567890\") { id username email firstName lastName isActive roles createdAt lastLoginAt } }"
  }'
```
**Response:**
```json
{
  "data": {
    "userById": {
      "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
      "username": "john.doe",
      "email": "john.doe@example.com",
      "firstName": "John",
      "lastName": "Doe",
      "isActive": true,
      "roles": ["User"],
      "createdAt": "2026-03-23T10:00:00Z",
      "lastLoginAt": "2026-03-23T10:30:00Z"
    }
  }
}
```

### Query: Get Users (Paginated)
```bash
curl -X POST http://localhost:7136/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <ADMIN_TOKEN>" \
  -d '{
    "query": "{ users(page: 1, size: 10) { items { id username email firstName lastName isActive roles } totalCount page pageSize } }"
  }'
```
**Response:**
```json
{
  "data": {
    "users": {
      "items": [
        {
          "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
          "username": "john.doe",
          "email": "john.doe@example.com",
          "firstName": "John",
          "lastName": "Doe",
          "isActive": true,
          "roles": ["User"]
        }
      ],
      "totalCount": 1,
      "page": 1,
      "pageSize": 10
    }
  }
}
```

### Mutation: Register User
```bash
curl -X POST http://localhost:7136/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { registerUser(input: { username: \"jane.smith\", email: \"jane@example.com\", password: \"SecureP@ss456\", firstName: \"Jane\", lastName: \"Smith\" }) { id username email roles } }"
  }'
```
**Response:**
```json
{
  "data": {
    "registerUser": {
      "id": "b2c3d4e5-f678-9012-bcde-f23456789012",
      "username": "jane.smith",
      "email": "jane@example.com",
      "roles": ["User"]
    }
  }
}
```

### Mutation: Login
```bash
curl -X POST http://localhost:7136/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { login(input: { usernameOrEmail: \"jane.smith\", password: \"SecureP@ss456\" }) { accessToken refreshToken expiresAt tokenType } }"
  }'
```
**Response:**
```json
{
  "data": {
    "login": {
      "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
      "refreshToken": "cmVmcmVzaCB0b2tlbg...",
      "expiresAt": "2026-03-23T11:00:00Z",
      "tokenType": "Bearer"
    }
  }
}
```

### Mutation: Assign Role
```bash
curl -X POST http://localhost:7136/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <ADMIN_TOKEN>" \
  -d '{
    "query": "mutation { assignRole(input: { userId: \"b2c3d4e5-f678-9012-bcde-f23456789012\", roleName: \"Admin\" }) { id username roles } }"
  }'
```

### Mutation: Delete User
```bash
curl -X POST http://localhost:7136/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <ADMIN_TOKEN>" \
  -d '{
    "query": "mutation { deleteUser(id: \"b2c3d4e5-f678-9012-bcde-f23456789012\") }"
  }'
```
