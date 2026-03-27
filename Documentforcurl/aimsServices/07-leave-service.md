# AIMS Services — API Documentation

---

## 7. Leave Service

**Port**: 5016 · **Auth**: JWT Bearer

### REST Endpoints

| Method   | Endpoint                                            | Description                    | Auth             |
| -------- | --------------------------------------------------- | ------------------------------ | ---------------- |
| `POST`   | `/api/leaves`                                       | Apply for leave                | Bearer           |
| `GET`    | `/api/leaves/{id}`                                  | Get leave by ID                | Bearer           |
| `GET`    | `/api/leaves/employee/{empId}`                      | Get leaves by employee         | Bearer           |
| `GET`    | `/api/leaves/pending`                               | Get pending leaves             | Admin,HR,Approver|
| `DELETE` | `/api/leaves/{id}?cancelledBy=`                     | Cancel leave                   | Bearer           |
| `GET`    | `/api/leaves/balance/{empId}/{leaveTypeId}`         | Get leave balance              | Bearer           |
| `GET`    | `/api/leaves/balance/{empId}/year/{year}`           | Get all balances for year      | Bearer           |
| `GET`    | `/api/leave-master`                                 | Get all leave types            | Bearer           |
| `GET`    | `/api/leave-master/{id}`                            | Get leave type by ID           | Bearer           |
| `POST`   | `/api/leave-master`                                 | Create leave type              | Admin,HR         |
| `PUT`    | `/api/leave-master/{id}`                            | Update leave type              | Admin,HR         |
| `POST`   | `/api/leave-approvals`                              | Process leave approval         | Admin,HR,Approver|
| `GET`    | `/api/leave-approvals/{leaveDetailId}/history`      | Get approval history           | Admin,HR,Approver|
| `POST`   | `/api/leave-credits`                                | Credit leave                   | Admin,HR         |
| `GET`    | `/api/compoff/employee/{empId}`                     | Get comp-off by employee       | Bearer           |
| `POST`   | `/api/compoff`                                      | Add comp-off                   | Admin,HR         |

### cURL Examples

```bash
# Apply for leave
curl -X POST http://localhost:5016/api/leaves \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "empId": 1001,
    "leaveTypeId": 1,
    "fromDate": "2025-04-10",
    "toDate": "2025-04-12",
    "leaveDays": 3,
    "reason": "Personal work",
    "appliedBy": 1001
  }'

# Get leave by ID
curl http://localhost:5016/api/leaves/1 \
  -H "Authorization: Bearer <TOKEN>"

# Get leaves for employee
curl http://localhost:5016/api/leaves/employee/1001 \
  -H "Authorization: Bearer <TOKEN>"

# Get pending approvals
curl http://localhost:5016/api/leaves/pending \
  -H "Authorization: Bearer <TOKEN>"

# Cancel leave
curl -X DELETE "http://localhost:5016/api/leaves/1?cancelledBy=1001" \
  -H "Authorization: Bearer <TOKEN>"

# Get leave balance
curl http://localhost:5016/api/leaves/balance/1001/1 \
  -H "Authorization: Bearer <TOKEN>"

# Get all balances for year
curl http://localhost:5016/api/leaves/balance/1001/year/2025 \
  -H "Authorization: Bearer <TOKEN>"

# Get all leave types
curl http://localhost:5016/api/leave-master \
  -H "Authorization: Bearer <TOKEN>"

# Create leave type
curl -X POST http://localhost:5016/api/leave-master \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "leaveName": "Casual Leave",
    "leaveCode": "CL",
    "maxDays": 12,
    "carryForward": true
  }'

# Process leave approval (approve)
curl -X POST http://localhost:5016/api/leave-approvals \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "leaveDetailId": 1,
    "isApproved": true,
    "remarks": "Approved",
    "processedBy": 2001
  }'

# Get approval history
curl http://localhost:5016/api/leave-approvals/1/history \
  -H "Authorization: Bearer <TOKEN>"

# Credit leave
curl -X POST http://localhost:5016/api/leave-credits \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"empId": 1001, "leaveTypeId": 1, "days": 5, "creditedBy": 1}'

# Get comp-off for employee
curl http://localhost:5016/api/compoff/employee/1001 \
  -H "Authorization: Bearer <TOKEN>"

# Add comp-off
curl -X POST http://localhost:5016/api/compoff \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"empId": 1001, "hours": 8, "addedBy": 1}'
```

---

