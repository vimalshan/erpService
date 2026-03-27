# Admin Services — API Documentation

---

## 4. Scholarship Service

**Port**: 5166 · **Base**: `/api/scholarships`

### REST Endpoints

| Method   | Endpoint                                                 | Description              | Auth    |
| -------- | -------------------------------------------------------- | ------------------------ | ------- |
| `GET`    | `/api/scholarships?page=1&pageSize=20`                   | Get all (paged)          | Bearer  |
| `GET`    | `/api/scholarships/{id}`                                 | Get by ID                | Bearer  |
| `GET`    | `/api/scholarships/employee/{employeeId}?page=&pageSize=`| Get by employee          | Bearer  |
| `POST`   | `/api/scholarships`                                      | Submit application       | Bearer  |
| `PUT`    | `/api/scholarships/{id}/approve`                         | Approve                  | Admin   |
| `PUT`    | `/api/scholarships/{id}/stop`                            | Stop scholarship         | Admin   |
| `GET`    | `/api/scholarship-amounts`                               | Get amount configs       | Bearer  |
| `POST`   | `/api/auth/token`                                        | Get JWT token            | Anon    |

### cURL Examples

```bash
# Get auth token
curl -X POST http://localhost:5166/api/auth/token \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "Admin@1234"}'

# Get all scholarships (paged)
curl "http://localhost:5166/api/scholarships?page=1&pageSize=20" \
  -H "Authorization: Bearer <TOKEN>"

# Get scholarship by ID
curl http://localhost:5166/api/scholarships/1 \
  -H "Authorization: Bearer <TOKEN>"

# Get scholarships for employee
curl "http://localhost:5166/api/scholarships/employee/1001?page=1&pageSize=10" \
  -H "Authorization: Bearer <TOKEN>"

# Submit scholarship application
curl -X POST http://localhost:5166/api/scholarships \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "employeeId": 1001,
    "childName": "Raj Kumar",
    "courseName": "B.Tech Computer Science",
    "details": {
      "marksStatus": "A",
      "payStatus": "P"
    }
  }'

# Approve scholarship (Admin only)
curl -X PUT http://localhost:5166/api/scholarships/1/approve \
  -H "Authorization: Bearer <ADMIN_TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"approvedBy": 1, "remarks": "Approved per policy"}'

# Stop scholarship (Admin only)
curl -X PUT http://localhost:5166/api/scholarships/1/stop \
  -H "Authorization: Bearer <ADMIN_TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"reason": "Course discontinued", "stoppedBy": 1}'

# Get scholarship amount configurations
curl http://localhost:5166/api/scholarship-amounts \
  -H "Authorization: Bearer <TOKEN>"
```

### GraphQL

**Endpoint**: `POST http://localhost:5166/graphql`

```bash
# Query: Get scholarships
curl -X POST http://localhost:5166/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getScholarships(page: 1, pageSize: 20) { id childName courseName entryStatus liveStatus } }"
  }'

# Query: Get scholarships by employee
curl -X POST http://localhost:5166/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getScholarships(employeeId: 1001) { id childName courseName entryStatus } }"
  }'

# Query: Get scholarship amounts
curl -X POST http://localhost:5166/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getScholarshipAmounts { id amount eligibleFrom eligibleTo } }"
  }'

# Mutation: Create scholarship
curl -X POST http://localhost:5166/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createScholarship(input: { employeeId: 1001, childName: \"Raj\", courseName: \"B.Tech\" }) }"
  }'

# Mutation: Approve
curl -X POST http://localhost:5166/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { approveScholarship(scholarshipId: 1, approvedBy: 1, remarks: \"Approved\") }"
  }'
```

---

