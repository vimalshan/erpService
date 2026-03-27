# AIMS Services — API Documentation

---

## 4. Calendar Service

**Port**: 5013 · **Auth**: JWT Bearer

### REST Endpoints

| Method | Endpoint                          | Description              | Auth          |
| ------ | --------------------------------- | ------------------------ | ------------- |
| `POST` | `/api/auth/token`                 | Get JWT token            | Anonymous     |
| `GET`  | `/api/calendars`                  | Get all calendars        | Bearer        |
| `GET`  | `/api/calendars/{id}`             | Get calendar by ID       | Bearer        |
| `POST` | `/api/calendars`                  | Create calendar          | Bearer        |
| `PUT`  | `/api/calendars/{id}`             | Update calendar          | Bearer        |
| `POST` | `/api/calendars/{id}/close`       | Close calendar           | Bearer        |
| `GET`  | `/api/holidays`                   | Get all holidays         | Bearer        |
| `GET`  | `/api/holidays/{id}`              | Get holiday by ID        | Bearer        |
| `GET`  | `/api/holidays/range`             | Get holidays in range    | Bearer        |
| `POST` | `/api/holidays`                   | Create holiday           | Bearer        |
| `PUT`  | `/api/holidays/{id}`              | Update holiday           | Bearer        |
| `GET`  | `/api/shifts`                     | Get all shifts           | Bearer        |
| `GET`  | `/api/shifts/{id}`                | Get shift by ID          | Bearer        |
| `POST` | `/api/shifts`                     | Create shift             | Bearer        |
| `PUT`  | `/api/shifts/{id}`                | Update shift             | Bearer        |
| `GET`  | `/api/patterns`                   | Get all patterns         | Bearer        |
| `GET`  | `/api/patterns/{id}`              | Get pattern by ID        | Bearer        |
| `POST` | `/api/patterns`                   | Create pattern           | Bearer        |
| `PUT`  | `/api/patterns/{id}`              | Update pattern           | Bearer        |

### Minimal API (Reports)

| Method | Endpoint                              | Description              |
| ------ | ------------------------------------- | ------------------------ |
| `GET`  | `/api/reports/shifts/summary`         | Shift summary report     |
| `GET`  | `/api/reports/holidays/upcoming`      | Upcoming holidays        |
| `GET`  | `/api/reports/calendars/summary`      | Calendar summary         |

### cURL Examples

```bash
# Get JWT token
curl -X POST http://localhost:5013/api/auth/token \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "admin123"}'

# Get all calendars
curl http://localhost:5013/api/calendars \
  -H "Authorization: Bearer <TOKEN>"

# Create calendar
curl -X POST http://localhost:5013/api/calendars \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "General Calendar 2025",
    "description": "Main office calendar",
    "startDate": "2025-01-01",
    "endDate": "2025-12-31",
    "createdBy": 1
  }'

# Close calendar
curl -X POST http://localhost:5013/api/calendars/1/close \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"id": 1, "closedBy": 1}'

# Get holidays in date range
curl "http://localhost:5013/api/holidays/range?from=2025-01-01&to=2025-12-31" \
  -H "Authorization: Bearer <TOKEN>"

# Create holiday
curl -X POST http://localhost:5013/api/holidays \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Republic Day",
    "date": "2025-01-26",
    "type": "National",
    "createdBy": 1
  }'

# Create shift
curl -X POST http://localhost:5013/api/shifts \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Morning Shift",
    "startTime": "09:00",
    "endTime": "17:00",
    "createdBy": 1
  }'

# Create pattern
curl -X POST http://localhost:5013/api/patterns \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "5-Day Week",
    "description": "Mon-Fri pattern",
    "createdBy": 1
  }'

# Get upcoming holidays (next 60 days)
curl "http://localhost:5013/api/reports/holidays/upcoming?days=60" \
  -H "Authorization: Bearer <TOKEN>"

# Get shift summary
curl http://localhost:5013/api/reports/shifts/summary \
  -H "Authorization: Bearer <TOKEN>"
```

### GraphQL

**Endpoint**: `POST http://localhost:5013/graphql`

```bash
# Query: Get all calendars
curl -X POST http://localhost:5013/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getCalendars { id name description startDate endDate } }"
  }'

# Query: Get calendar by ID
curl -X POST http://localhost:5013/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getCalendar(id: 1) { id name description startDate endDate } }"
  }'

# Query: Get all holidays
curl -X POST http://localhost:5013/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getHolidays { id name date type } }"
  }'

# Query: Get all shifts
curl -X POST http://localhost:5013/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getShifts { id name startTime endTime } }"
  }'

# Query: Get all patterns
curl -X POST http://localhost:5013/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getPatterns { id name description } }"
  }'

# Mutation: Create calendar
curl -X POST http://localhost:5013/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createCalendar(input: { name: \"Calendar 2025\", startDate: \"2025-01-01\", endDate: \"2025-12-31\", createdBy: 1 }) { id name } }"
  }'

# Mutation: Create holiday
curl -X POST http://localhost:5013/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createHoliday(input: { name: \"Independence Day\", date: \"2025-08-15\", type: \"National\", createdBy: 1 }) { id name date } }"
  }'

# Mutation: Create shift
curl -X POST http://localhost:5013/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createShift(input: { name: \"Night Shift\", startTime: \"22:00\", endTime: \"06:00\", createdBy: 1 }) { id name } }"
  }'

# Mutation: Create pattern
curl -X POST http://localhost:5013/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createPattern(input: { name: \"6-Day Week\", description: \"Mon-Sat\", createdBy: 1 }) { id name } }"
  }'
```

---

