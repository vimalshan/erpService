# AIMS Services — API Documentation

---

## 1. Access Service

**Port**: 5010 · **Auth**: JWT Bearer

### REST Endpoints

| Method   | Endpoint                                         | Description                   | Auth        |
| -------- | ------------------------------------------------ | ----------------------------- | ----------- |
| `POST`   | `/api/auth/login`                                | Login with employee ID        | Anonymous   |
| `POST`   | `/api/auth/verify`                               | Verify token validity         | Bearer      |
| `GET`    | `/api/auth/me`                                   | Get current user info         | Bearer      |
| `GET`    | `/api/userroles/{roleId}`                        | Get user role by ID           | Bearer      |
| `GET`    | `/api/userroles/employee/{employeeSystemId}`     | Get roles by employee         | Bearer      |
| `GET`    | `/api/userroles/type/{roleType}`                 | Get roles by type (S/U/C)     | Bearer      |
| `POST`   | `/api/userroles`                                 | Assign role to user           | Bearer      |
| `PUT`    | `/api/userroles/{roleId}`                        | Update user role              | Bearer      |
| `DELETE` | `/api/userroles/{roleId}`                        | Revoke user role              | Bearer      |
| `GET`    | `/api/usermaps/{employeeSystemId}`               | Get user map by employee      | Bearer      |
| `GET`    | `/api/usermaps`                                  | Get all user maps             | Bearer      |
| `POST`   | `/api/usermaps`                                  | Create user map               | Bearer      |
| `PUT`    | `/api/usermaps/{employeeSystemId}/activate`      | Activate user map             | Bearer      |
| `PUT`    | `/api/usermaps/{employeeSystemId}`               | Update user map dates         | Bearer      |
| `DELETE` | `/api/usermaps/{employeeSystemId}`               | Deactivate user map           | Bearer      |

### cURL Examples

```bash
# Login
curl -X POST http://localhost:5010/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"employeeSystemId": 1001, "email": "user@example.com"}'

# Verify token
curl -X POST http://localhost:5010/api/auth/verify \
  -H "Authorization: Bearer <TOKEN>"

# Get current user
curl http://localhost:5010/api/auth/me \
  -H "Authorization: Bearer <TOKEN>"

# Get roles for employee
curl http://localhost:5010/api/userroles/employee/1001 \
  -H "Authorization: Bearer <TOKEN>"

# Get roles by type (S=SuperUser, U=UnitAccess, C=CalendarAccess)
curl http://localhost:5010/api/userroles/type/S \
  -H "Authorization: Bearer <TOKEN>"

# Assign role to user
curl -X POST http://localhost:5010/api/userroles \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "employeeSystemId": 1001,
    "roleType": "U",
    "menuAccess": "Y",
    "organizationId": 1,
    "unitId": 10,
    "calendarId": null
  }'

# Update user role
curl -X PUT http://localhost:5010/api/userroles/1 \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "menuAccess": "N",
    "organizationId": 1,
    "unitId": 10,
    "calendarId": 5
  }'

# Revoke user role
curl -X DELETE http://localhost:5010/api/userroles/1 \
  -H "Authorization: Bearer <TOKEN>"

# Get all user maps
curl "http://localhost:5010/api/usermaps?activeOnly=true" \
  -H "Authorization: Bearer <TOKEN>"

# Create user map
curl -X POST http://localhost:5010/api/usermaps \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"employeeSystemId": 1001}'

# Activate user map
curl -X PUT http://localhost:5010/api/usermaps/1001/activate \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '"2025-04-01T00:00:00"'

# Deactivate user map
curl -X DELETE http://localhost:5010/api/usermaps/1001 \
  -H "Authorization: Bearer <TOKEN>"
```

### GraphQL

**Endpoint**: `POST http://localhost:5010/graphql`

```bash
# Query: Get user map by employee
curl -X POST http://localhost:5010/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getUserMap(employeeSystemId: 1001) { employeeSystemId effectiveDate closureDate isActive } }"
  }'

# Query: Get all user maps (active only)
curl -X POST http://localhost:5010/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getUserMaps(activeOnly: true) { employeeSystemId effectiveDate isActive } }"
  }'

# Query: Get user role by ID
curl -X POST http://localhost:5010/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getUserRole(roleId: 1) { roleId employeeSystemId roleType menuAccess organizationId unitId calendarId } }"
  }'

# Query: Get roles by employee
curl -X POST http://localhost:5010/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getUserRolesByEmployee(employeeSystemId: 1001, activeOnly: true) { roleId roleType menuAccess } }"
  }'

# Query: Get roles by type
curl -X POST http://localhost:5010/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getUserRolesByType(roleType: \"S\") { roleId employeeSystemId menuAccess } }"
  }'

# Query: Get all menus
curl -X POST http://localhost:5010/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getMenus { menuId menuName parentMenuId } }"
  }'

# Query: Get root menus
curl -X POST http://localhost:5010/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getRootMenus { menuId menuName } }"
  }'

# Query: Get SPARSH menus
curl -X POST http://localhost:5010/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getSparshMenus { menuId menuName } }"
  }'

# Mutation: Create user map
curl -X POST http://localhost:5010/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createUserMap(input: { employeeSystemId: 1001 }) { success id message } }"
  }'

# Mutation: Activate user map
curl -X POST http://localhost:5010/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { activateUserMap(input: { employeeSystemId: 1001, effectiveDate: \"2025-04-01\" }) { success message } }"
  }'

# Mutation: Deactivate user map
curl -X POST http://localhost:5010/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { deactivateUserMap(input: { employeeSystemId: 1001, closureDate: \"2025-12-31\" }) { success message } }"
  }'

# Mutation: Assign role
curl -X POST http://localhost:5010/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { assignUserRole(input: { employeeSystemId: 1001, roleType: \"U\", menuAccess: \"Y\", organizationId: 1, unitId: 10 }) { success roleId message } }"
  }'

# Mutation: Update role
curl -X POST http://localhost:5010/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { updateUserRole(input: { roleId: 1, menuAccess: \"N\", organizationId: 1, unitId: 10 }) { success message } }"
  }'

# Mutation: Revoke role
curl -X POST http://localhost:5010/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { revokeUserRole(input: { roleId: 1, closureDate: \"2025-12-31\" }) { success message } }"
  }'
```

---

