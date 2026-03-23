# Scholarship Service

> **Port:** 5166 | **Swagger:** http://localhost:5166/swagger | **GraphQL:** http://localhost:5166/graphql

---

## REST Endpoints

### Get All Scholarships (Paginated)
```bash
curl -X GET "http://localhost:5166/api/Scholarships?page=1&pageSize=20"
```
**Response:**
```json
{
  "items": [
    {
      "id": 1,
      "employeeSysId": 5001,
      "gradeId": 2,
      "dependentId": 1,
      "childName": "Ravi Kumar",
      "lastSchool": "DPS",
      "lastYearOfSchool": 2025,
      "lastExam": "10th",
      "cgpaFlag": "P",
      "marksPercentage": 85.5,
      "marksGpa": 0,
      "courseName": "B.Tech",
      "courseJoinYear": 2025,
      "courseJoinMonth": 7,
      "courseDuration": 4,
      "paymentMode": "NEFT",
      "entryStatus": "Approved",
      "source": "Online",
      "disbursementAmount": 50000,
      "disbursementFrequency": "Annual",
      "liveStatus": "Active",
      "createdOn": "2025-08-01T10:00:00",
      "createdBy": 5001
    }
  ],
  "totalCount": 1,
  "page": 1,
  "pageSize": 20
}
```

### Get Scholarship by ID
```bash
curl -X GET http://localhost:5166/api/Scholarships/1
```

### Get Scholarships by Employee
```bash
curl -X GET "http://localhost:5166/api/Scholarships/employee/5001?page=1&pageSize=10"
```

### Submit Scholarship Application
```bash
curl -X POST http://localhost:5166/api/Scholarships \
  -H "Content-Type: application/json" \
  -d '{
    "employeeSysId": 5002,
    "gradeId": 3,
    "dependentId": 2,
    "childName": "Priya Sharma",
    "lastSchool": "KV",
    "lastYearOfSchool": 2025,
    "lastExam": "12th",
    "cgpaFlag": "P",
    "marksPercentage": 92.0,
    "marksGpa": 0,
    "courseName": "MBBS",
    "courseJoinYear": 2025,
    "courseJoinMonth": 9,
    "courseDuration": 5,
    "paymentMode": "NEFT",
    "childAccountNumber": "1234567890",
    "childBankIfsc": "SBIN0001234",
    "childBankMicr": "110002345",
    "source": "Online",
    "disbursementAmount": 75000,
    "disbursementFrequency": "Annual",
    "isOffline": "N"
  }'
```
**Response:**
```json
{
  "id": 2,
  "employeeSysId": 5002,
  "childName": "Priya Sharma",
  "entryStatus": "Pending",
  "liveStatus": "Active",
  "createdOn": "2026-03-23T10:00:00",
  "createdBy": 5002
}
```

### Approve Scholarship
```bash
curl -X PUT http://localhost:5166/api/Scholarships/2/approve \
  -H "Content-Type: application/json" \
  -d '{
    "remarks": "Approved by committee",
    "approvedBy": 9001
  }'
```

### Stop Scholarship
```bash
curl -X PUT http://localhost:5166/api/Scholarships/2/stop \
  -H "Content-Type: application/json" \
  -d '{
    "remarks": "Course completed",
    "stoppedBy": 9001
  }'
```

### Get Scholarship Amount Configurations
```bash
curl -X GET http://localhost:5166/api/ScholarshipAmounts
```
**Response:**
```json
[
  {
    "id": 1,
    "orgId": 1,
    "gradeCategory": "Officer",
    "eligibleExam": "12th",
    "applicableAllGrade": "Y",
    "gradeId": 0,
    "fromYear": 2020,
    "closeYear": null,
    "eligibleAmount": 75000,
    "eligibleYear": 5,
    "cutoffMarks": 60,
    "createdOn": "2020-01-01T00:00:00",
    "createdBy": 1
  }
]
```

---

## GraphQL

### Query: Get Scholarships
```bash
curl -X POST http://localhost:5166/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ scholarships { id employeeSysId childName courseName entryStatus liveStatus disbursementAmount } }"
  }'
```
**Response:**
```json
{
  "data": {
    "scholarships": [
      {
        "id": 1,
        "employeeSysId": 5001,
        "childName": "Ravi Kumar",
        "courseName": "B.Tech",
        "entryStatus": "Approved",
        "liveStatus": "Active",
        "disbursementAmount": 50000
      }
    ]
  }
}
```

### Mutation: Create Scholarship
```bash
curl -X POST http://localhost:5166/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createScholarship(input: { employeeSysId: 5003, gradeId: 1, dependentId: 1, childName: \"Amit Singh\", lastSchool: \"DAV\", lastYearOfSchool: 2025, lastExam: \"10th\", cgpaFlag: \"P\", marksPercentage: 88.0, courseName: \"B.Sc\", courseJoinYear: 2025, courseJoinMonth: 7, courseDuration: 3, source: \"Online\", disbursementAmount: 50000, disbursementFrequency: \"Annual\", isOffline: \"N\" }) { id childName entryStatus } }"
  }'
```
**Response:**
```json
{
  "data": {
    "createScholarship": {
      "id": 3,
      "childName": "Amit Singh",
      "entryStatus": "Pending"
    }
  }
}
```

### Mutation: Approve Scholarship
```bash
curl -X POST http://localhost:5166/graphql \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { approveScholarship(id: 3, remarks: \"Verified and approved\") { id entryStatus } }"
  }'
```
