# AIMS Services — API Documentation

---

## 10. AIMS Transaction Service

**Port**: 5019 · **Auth**: JWT Bearer

### REST Endpoints

| Method | Endpoint                                                     | Description               | Auth    |
| ------ | ------------------------------------------------------------ | ------------------------- | ------- |
| `GET`  | `/api/swipes/employee/{employeeSysId}?fromDate=&toDate=`    | Get swipes by employee    | Bearer  |
| `POST` | `/api/swipes`                                                | Record swipe              | Bearer  |
| `GET`  | `/api/leaves/employee/{employeeSysId}`                       | Get leaves by employee    | Bearer  |
| `GET`  | `/api/leaves/balance/{employeeSysId}/{leaveId}`              | Get leave balance         | Bearer  |
| `POST` | `/api/leaves`                                                | Apply for leave           | Bearer  |
| `POST` | `/api/leaves/{id}/approve`                                   | Approve/reject leave      | Bearer  |
| `GET`  | `/api/compoffs/employee/{employeeSysId}`                     | Get comp-offs             | Bearer  |
| `POST` | `/api/compoffs`                                              | Request comp-off          | Bearer  |
| `GET`  | `/api/attendance/summary/{employeeSysId}?monthStart=&End=`  | Get attendance summary    | Bearer  |
| `POST` | `/api/attendance/batch`                                      | Process attendance batch  | Bearer  |

### Minimal API (v2)

| Method | Endpoint                                                        | Description                |
| ------ | --------------------------------------------------------------- | -------------------------- |
| `GET`  | `/api/v2/swipes/employee/{employeeSysId}?fromDate=&toDate=`    | Get swipes (v2)            |
| `POST` | `/api/v2/swipes/`                                               | Record swipe (v2)          |
| `GET`  | `/api/v2/leaves/employee/{employeeSysId}`                       | Get leaves (v2)            |
| `GET`  | `/api/v2/leaves/balance/{employeeSysId}/{leaveId}`              | Get balance (v2)           |
| `POST` | `/api/v2/leaves/`                                               | Apply leave (v2)           |
| `POST` | `/api/v2/leaves/{id}/approve`                                   | Approve leave (v2)         |
| `GET`  | `/api/v2/compoffs/employee/{employeeSysId}`                     | Get comp-offs (v2)         |
| `POST` | `/api/v2/compoffs/`                                             | Request comp-off (v2)      |
| `GET`  | `/api/v2/attendance/summary/{employeeSysId}`                    | Attendance summary (v2)    |
| `POST` | `/api/v2/attendance/batch`                                      | Process batch (v2)         |

### cURL Examples

```bash
# Get swipes by employee
curl "http://localhost:5019/api/swipes/employee/1001?fromDate=2025-04-01&toDate=2025-04-30" \
  -H "Authorization: Bearer <TOKEN>"

# Record swipe
curl -X POST http://localhost:5019/api/swipes \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "employeeSysId": 1001,
    "gateNo": 1,
    "punchTime": "2025-04-01T09:00:00",
    "punchStatus": "I",
    "machineNo": 1,
    "referenceNo": "REF001",
    "updatedBy": 1
  }'

# Get leaves for employee
curl http://localhost:5019/api/leaves/employee/1001 \
  -H "Authorization: Bearer <TOKEN>"

# Get leave balance
curl http://localhost:5019/api/leaves/balance/1001/1 \
  -H "Authorization: Bearer <TOKEN>"

# Apply for leave
curl -X POST http://localhost:5019/api/leaves \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "employeeSysId": 1001,
    "leaveId": 1,
    "fromDate": "2025-04-10",
    "toDate": "2025-04-12",
    "leaveDays": 3,
    "reason": "Personal",
    "appliedBy": 1001
  }'

# Approve leave
curl -X POST http://localhost:5019/api/leaves/1/approve \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "leaveDetailId": 1,
    "isApproved": true,
    "remarks": "Approved",
    "processedBy": 2001
  }'

# Get comp-offs
curl http://localhost:5019/api/compoffs/employee/1001 \
  -H "Authorization: Bearer <TOKEN>"

# Request comp-off
curl -X POST http://localhost:5019/api/compoffs \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"employeeSysId": 1001, "hoursRequested": 8, "requestedBy": 1001}'

# Get attendance summary
curl "http://localhost:5019/api/attendance/summary/1001?monthStart=2025-04-01&monthEnd=2025-04-30" \
  -H "Authorization: Bearer <TOKEN>"

# Process attendance batch
curl -X POST http://localhost:5019/api/attendance/batch \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"monthStart": "2025-04-01", "monthEnd": "2025-04-30", "createdBy": 1}'
```

### GraphQL

**Endpoint**: `POST http://localhost:5019/graphql`

```bash
# Query: Get swipes by employee
curl -X POST http://localhost:5019/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getSwipesByEmployee(employeeSysId: 1001, fromDate: \"2025-04-01\", toDate: \"2025-04-30\") { swipeId employeeSysId gateNo punchTime punchStatus } }"
  }'

# Query: Get attendance summary
curl -X POST http://localhost:5019/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getAttendanceSummary(employeeSysId: 1001, monthStart: \"2025-04-01\", monthEnd: \"2025-04-30\") { employeeSysId totalDays presentDays absentDays lateDays } }"
  }'

# Query: Get leaves by employee
curl -X POST http://localhost:5019/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getLeavesByEmployee(employeeSysId: 1001) { leaveDetailId employeeSysId leaveId fromDate toDate leaveDays status } }"
  }'

# Query: Get leave balance
curl -X POST http://localhost:5019/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getLeaveBalance(employeeSysId: 1001, leaveId: 1) { employeeSysId leaveId totalEntitled consumed balance } }"
  }'

# Query: Get comp-offs
curl -X POST http://localhost:5019/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getCompOffsByEmployee(employeeSysId: 1001) { compOffId employeeSysId hoursRequested status } }"
  }'

# Mutation: Record swipe
curl -X POST http://localhost:5019/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { recordSwipe(input: { employeeSysId: 1001, gateNo: 1, punchTime: \"2025-04-01T09:00:00\", punchStatus: \"I\", machineNo: 1, referenceNo: \"REF001\", updatedBy: 1 }) { swipeId employeeSysId punchTime } }"
  }'

# Mutation: Apply for leave
curl -X POST http://localhost:5019/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { applyLeave(input: { employeeSysId: 1001, leaveId: 1, fromDate: \"2025-04-10\", toDate: \"2025-04-12\", leaveDays: 3, reason: \"Personal\", appliedBy: 1001 }) { leaveDetailId status } }"
  }'

# Mutation: Approve leave
curl -X POST http://localhost:5019/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { approveLeave(leaveDetailId: 1, isApproved: true, remarks: \"Approved\", processedBy: 2001) }"
  }'

# Mutation: Process attendance batch
curl -X POST http://localhost:5019/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { processAttendanceBatch(monthStart: \"2025-04-01\", monthEnd: \"2025-04-30\", createdBy: 1) { batchId monthStart monthEnd } }"
  }'

# Mutation: Request comp-off
curl -X POST http://localhost:5019/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { requestCompOff(employeeSysId: 1001, hoursRequested: 8, requestedBy: 1001) { compOffId status } }"
  }'
```

---

