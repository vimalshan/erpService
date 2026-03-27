# AuthProvider API Documentation

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

