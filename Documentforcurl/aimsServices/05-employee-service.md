# AIMS Services — API Documentation

---

## 5. Employee Service

**Port**: 5014 · **Auth**: JWT Bearer

### REST Endpoints

| Method | Endpoint                                  | Description                  | Auth    |
| ------ | ----------------------------------------- | ---------------------------- | ------- |
| `POST` | `/api/auth/token`                         | Get JWT token                | Anon    |
| `GET`  | `/api/timeinfo/employee/{empSysId}`       | Get time-info by employee    | Bearer  |
| `GET`  | `/api/timeinfo/{id}`                      | Get time-info by ID          | Bearer  |
| `POST` | `/api/timeinfo`                           | Record time-info             | Bearer  |
| `GET`  | `/api/approver/employee/{empSysId}`       | Get approvers by employee    | Bearer  |
| `POST` | `/api/approver`                           | Assign approver              | Bearer  |
| `GET`  | `/api/calendar/employee/{empSysId}`       | Get calendar mappings        | Bearer  |
| `POST` | `/api/calendar`                           | Map employee to calendar     | Bearer  |

### cURL Examples

```bash
# Get JWT token
curl -X POST http://localhost:5014/api/auth/token \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "userId": 1, "role": "Admin"}'

# Get time-info records for employee
curl http://localhost:5014/api/timeinfo/employee/1001 \
  -H "Authorization: Bearer <TOKEN>"

# Get time-info by ID
curl http://localhost:5014/api/timeinfo/1 \
  -H "Authorization: Bearer <TOKEN>"

# Record time-info (attendance flag)
curl -X POST http://localhost:5014/api/timeinfo \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"empSysId": 1001, "attFlag": "P", "modifiedBy": 1}'

# Get approvers for employee
curl http://localhost:5014/api/approver/employee/1001 \
  -H "Authorization: Bearer <TOKEN>"

# Assign approver
curl -X POST http://localhost:5014/api/approver \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "empSysId": 1001,
    "approverSysId": 2001,
    "level": 1,
    "assignedBy": 1
  }'

# Get calendar mappings for employee
curl http://localhost:5014/api/calendar/employee/1001 \
  -H "Authorization: Bearer <TOKEN>"

# Map employee to calendar
curl -X POST http://localhost:5014/api/calendar \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"empSysId": 1001, "calendarId": 1, "mappedBy": 1}'
```

### GraphQL

**Endpoint**: `POST http://localhost:5014/graphql`

```bash
# Query: Get time-info records
curl -X POST http://localhost:5014/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getTimeInfos(empSysId: 1001) { timeInfoId empSysId empAttFlag lastModifiedBy lastModifiedOn } }"
  }'

# Query: Get approvers
curl -X POST http://localhost:5014/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getApprovers(empSysId: 1001) { approverId empSysId level approverSysId effDate } }"
  }'

# Query: Get calendars
curl -X POST http://localhost:5014/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getCalendars(empSysId: 1001) { empCalId empSysId calendarId swipeId effDate clsDate status } }"
  }'

# Mutation: Assign approver
curl -X POST http://localhost:5014/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { assignApprover(input: { empSysId: 1001, approverSysId: 2001, level: 1, assignedBy: 1 }) { approverId empSysId level } }"
  }'

# Mutation: Map calendar
curl -X POST http://localhost:5014/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { mapCalendar(input: { empSysId: 1001, calendarId: 1, mappedBy: 1 }) { empCalId empSysId calendarId } }"
  }'

# Mutation: Record time-info
curl -X POST http://localhost:5014/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { recordTimeInfo(input: { empSysId: 1001, attFlag: \"P\", modifiedBy: 1 }) { timeInfoId empSysId empAttFlag } }"
  }'
```

---

