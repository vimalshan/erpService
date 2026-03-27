# AIMS Services — API Documentation

---

## 2. Attendance Service

**Port**: 5011 · **Auth**: JWT Bearer

### REST Endpoints

| Method | Endpoint                                        | Description                       | Auth           |
| ------ | ----------------------------------------------- | --------------------------------- | -------------- |
| `POST` | `/api/auth/login`                               | Get JWT token                     | Anonymous      |
| `GET`  | `/api/auth/me`                                  | Get current user                  | Bearer         |
| `POST` | `/api/swipe`                                    | Record biometric swipe            | Bearer         |
| `GET`  | `/api/swipe/employee/{empSysId}`                | Get swipes for employee           | Bearer         |
| `GET`  | `/api/swipe/employee/{empSysId}/percentage`     | Get attendance percentage         | Bearer         |
| `POST` | `/api/overtime/{id}/approve`                    | Approve overtime                  | Admin,Hr       |
| `POST` | `/api/batches/process`                          | Process monthly attendance        | Admin,Hr       |
| `GET`  | `/api/batches/{id}`                             | Get batch by ID                   | Admin,Hr       |

### Minimal API (v2)

| Method | Endpoint                                            | Description                  |
| ------ | --------------------------------------------------- | ---------------------------- |
| `POST` | `/api/v2/attendance/swipe`                          | Record swipe (v2)            |
| `GET`  | `/api/v2/attendance/swipe/employee/{empSysId}`      | Get swipes by employee (v2)  |
| `GET`  | `/api/v2/attendance/percentage/{empSysId}`           | Get attendance % (v2)        |

### cURL Examples

```bash
# Login
curl -X POST http://localhost:5011/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "Admin@123!"}'

# Record swipe punch
curl -X POST http://localhost:5011/api/swipe \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "empSysId": 1001,
    "punchTime": "2025-04-01T09:00:00",
    "gateNo": "G1",
    "punchStatus": "I"
  }'

# Get swipes for employee (date range)
curl "http://localhost:5011/api/swipe/employee/1001?from=2025-04-01&to=2025-04-30" \
  -H "Authorization: Bearer <TOKEN>"

# Get attendance percentage
curl "http://localhost:5011/api/swipe/employee/1001/percentage?monthStart=2025-04-01&monthEnd=2025-04-30" \
  -H "Authorization: Bearer <TOKEN>"

# Approve overtime
curl -X POST "http://localhost:5011/api/overtime/1/approve?approvedBy=1" \
  -H "Authorization: Bearer <TOKEN>"

# Process monthly batch
curl -X POST http://localhost:5011/api/batches/process \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "monthStart": "2025-04-01",
    "monthEnd": "2025-04-30",
    "processedBy": 1
  }'

# Get batch by ID
curl http://localhost:5011/api/batches/1 \
  -H "Authorization: Bearer <TOKEN>"

# v2: Record swipe
curl -X POST http://localhost:5011/api/v2/attendance/swipe \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"empSysId": 1001, "punchTime": "2025-04-01T09:00:00", "gateNo": "G1", "punchStatus": "I"}'
```

### GraphQL

**Endpoint**: `POST http://localhost:5011/graphql`

```bash
# Query: Get swipe punches
curl -X POST http://localhost:5011/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getSwipePunches(empSysId: 1001, from: \"2025-04-01\", to: \"2025-04-30\") { swipeId empSysId punchTime gateNo punchStatus } }"
  }'

# Query: Get attendance batch
curl -X POST http://localhost:5011/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getBatch(batchId: 1) { batchId monthStart monthEnd processedBy } }"
  }'

# Query: Get attendance percentage
curl -X POST http://localhost:5011/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getAttendancePercentage(empSysId: 1001, monthStart: \"2025-04-01\", monthEnd: \"2025-04-30\") { empSysId percentage totalDays presentDays } }"
  }'

# Mutation: Record swipe punch
curl -X POST http://localhost:5011/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { recordSwipePunch(empSysId: 1001, punchTime: \"2025-04-01T09:00:00\", gateNo: \"G1\", punchStatus: \"I\") { swipeId empSysId punchTime } }"
  }'

# Mutation: Process monthly attendance
curl -X POST http://localhost:5011/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { processMonthlyAttendance(monthStart: \"2025-04-01\", monthEnd: \"2025-04-30\", processedBy: 1) { batchId monthStart monthEnd } }"
  }'
```

---

