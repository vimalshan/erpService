# AIMS Services — API Documentation

---

## 6. Group Incentive Service

**Port**: 5015 · **Auth**: JWT Bearer

### REST Endpoints

| Method | Endpoint                                     | Description                  | Auth            |
| ------ | -------------------------------------------- | ---------------------------- | --------------- |
| `POST` | `/api/auth/login`                            | Get JWT token                | Anonymous       |
| `GET`  | `/api/groups`                                | Get all groups               | Bearer          |
| `GET`  | `/api/groups/{id}`                           | Get group by ID              | Bearer          |
| `POST` | `/api/groups`                                | Create group                 | Bearer          |
| `POST` | `/api/groups/{groupId}/employees`            | Add employee to group        | Bearer          |
| `GET`  | `/api/groupincentives/pending`               | Get pending incentives       | Bearer          |
| `GET`  | `/api/groupincentives/group/{groupId}`       | Get incentives by group      | Bearer          |
| `GET`  | `/api/groupincentives/{id}`                  | Get incentive by ID          | Bearer          |
| `POST` | `/api/groupincentives`                       | Create incentive             | Bearer          |
| `POST` | `/api/groupincentives/{id}/approve`          | Approve incentive            | Approver,Admin  |
| `POST` | `/api/groupincentives/{id}/reject`           | Reject incentive             | Approver,Admin  |

### Minimal API (v2)

| Method | Endpoint                                           | Description              |
| ------ | -------------------------------------------------- | ------------------------ |
| `GET`  | `/api/v2/groups/`                                  | Get all groups (v2)      |
| `POST` | `/api/v2/groups/`                                  | Create group (v2)        |
| `GET`  | `/api/v2/employees/{employeeId}/incentive`         | Employee incentive       |

### cURL Examples

```bash
# Login
curl -X POST http://localhost:5015/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "Admin@1234", "role": "Admin"}'

# Get all active groups
curl "http://localhost:5015/api/groups?activeOnly=true" \
  -H "Authorization: Bearer <TOKEN>"

# Create group
curl -X POST http://localhost:5015/api/groups \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "groupName": "Engineering Team A",
    "description": "Software engineering team",
    "createdBy": 1
  }'

# Add employee to group
curl -X POST http://localhost:5015/api/groups/1/employees \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"groupId": 1, "employeeId": 1001, "addedBy": 1}'

# Get pending incentives
curl http://localhost:5015/api/groupincentives/pending \
  -H "Authorization: Bearer <TOKEN>"

# Create group incentive
curl -X POST http://localhost:5015/api/groupincentives \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "groupId": 1,
    "month": 4,
    "year": 2025,
    "totalAmount": 50000,
    "details": [
      {"employeeId": 1001, "amount": 10000},
      {"employeeId": 1002, "amount": 10000}
    ],
    "createdBy": 1
  }'

# Approve incentive
curl -X POST http://localhost:5015/api/groupincentives/1/approve \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"incentiveId": 1, "approvedBy": 1, "remarks": "Approved"}'

# Reject incentive
curl -X POST http://localhost:5015/api/groupincentives/1/reject \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"incentiveId": 1, "rejectedBy": 1, "reason": "Budget exceeded"}'

# v2: Get employee incentive summary
curl "http://localhost:5015/api/v2/employees/1001/incentive?month=4&year=2025" \
  -H "Authorization: Bearer <TOKEN>"
```

### GraphQL

**Endpoint**: `POST http://localhost:5015/graphql`

```bash
# Query: Get all groups (supports filtering & sorting)
curl -X POST http://localhost:5015/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getGroups(activeOnly: true) { groupId groupName description isActive } }"
  }'

# Query: Get group by ID
curl -X POST http://localhost:5015/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getGroupById(id: 1) { groupId groupName description employees { employeeId } } }"
  }'

# Query: Get group incentive
curl -X POST http://localhost:5015/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getGroupIncentive(id: 1) { incentiveId groupId month year totalAmount status details { employeeId amount } } }"
  }'

# Query: Get incentives for group
curl -X POST http://localhost:5015/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getGroupIncentives(groupId: 1) { incentiveId month year totalAmount status } }"
  }'

# Query: Get pending incentives
curl -X POST http://localhost:5015/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getPendingIncentives { incentiveId groupId totalAmount status } }"
  }'

# Mutation: Create group
curl -X POST http://localhost:5015/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createGroup(input: { groupName: \"Team B\", description: \"Ops team\", createdBy: 1 }) }"
  }'

# Mutation: Create group incentive
curl -X POST http://localhost:5015/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createGroupIncentive(input: { groupId: 1, month: 4, year: 2025, totalAmount: 50000, createdBy: 1 }) }"
  }'

# Mutation: Approve incentive
curl -X POST http://localhost:5015/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { approveGroupIncentive(input: { incentiveId: 1, approvedBy: 1, remarks: \"OK\" }) }"
  }'

# Mutation: Reject incentive
curl -X POST http://localhost:5015/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { rejectGroupIncentive(input: { incentiveId: 1, rejectedBy: 1, reason: \"Budget\" }) }"
  }'
```

---

