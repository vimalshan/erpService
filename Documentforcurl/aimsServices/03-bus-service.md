# AIMS Services — API Documentation

---

## 3. Bus Service

**Port**: 5012 · **Auth**: JWT Bearer

### REST Endpoints

| Method | Endpoint                                      | Description                  | Auth    |
| ------ | --------------------------------------------- | ---------------------------- | ------- |
| `POST` | `/api/auth/login`                             | Get JWT token                | Anon    |
| `GET`  | `/api/buses`                                  | Get all buses                | Bearer  |
| `GET`  | `/api/buses/{id}`                             | Get bus by ID                | Bearer  |
| `POST` | `/api/buses`                                  | Register new bus             | Bearer  |
| `PUT`  | `/api/buses/{id}`                             | Update bus                   | Bearer  |
| `GET`  | `/api/buses/{busId}/routes`                   | Get routes for bus           | Bearer  |
| `POST` | `/api/buses/{busId}/routes`                   | Create route for bus         | Bearer  |
| `GET`  | `/api/buses/{busId}/deductions`               | Get deduction rates          | Bearer  |
| `POST` | `/api/buses/{busId}/deductions`               | Set deduction rate           | Bearer  |
| `GET`  | `/api/employeebus/employee/{empSysId}`        | Get employee assignments     | Bearer  |
| `POST` | `/api/employeebus`                            | Assign employee to bus       | Bearer  |
| `PUT`  | `/api/employeebus/{empBusId}/close`           | Close assignment             | Bearer  |
| `GET`  | `/api/arrivals/bus/{busId}`                   | Get arrivals by bus          | Bearer  |
| `GET`  | `/api/arrivals/date/{date}`                   | Get arrivals by date         | Bearer  |
| `POST` | `/api/arrivals`                               | Record arrival               | Bearer  |

### Minimal API (v2)

| Method | Endpoint                                      | Description              |
| ------ | --------------------------------------------- | ------------------------ |
| `GET`  | `/api/v2/buses/`                              | Get all buses (v2)       |
| `GET`  | `/api/v2/buses/{id}`                          | Get bus by ID (v2)       |
| `GET`  | `/api/v2/buses/reports/summary`               | Bus summary report       |
| `GET`  | `/api/v2/buses/reports/arrivals`              | Arrival report           |
| `GET`  | `/api/v2/buses/reports/employees`             | Employee-bus report      |

### cURL Examples

```bash
# Login
curl -X POST http://localhost:5012/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "Admin@1234"}'

# Get all buses
curl http://localhost:5012/api/buses \
  -H "Authorization: Bearer <TOKEN>"

# Get bus by ID
curl http://localhost:5012/api/buses/1 \
  -H "Authorization: Bearer <TOKEN>"

# Register new bus
curl -X POST http://localhost:5012/api/buses \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "registrationNumber": "MH-12-AB-1234",
    "description": "Route A Morning Bus",
    "capacity": 50,
    "registeredBy": 1
  }'

# Update bus
curl -X PUT http://localhost:5012/api/buses/1 \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"description": "Updated route", "capacity": 55, "modifiedBy": 1}'

# Create route
curl -X POST http://localhost:5012/api/buses/1/routes \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"name": "Route A", "description": "Main campus to downtown", "createdBy": 1}'

# Set deduction rate
curl -X POST http://localhost:5012/api/buses/1/deductions \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"amount": 500.00, "effectiveDate": "2025-04-01", "createdBy": 1}'

# Assign employee to bus
curl -X POST http://localhost:5012/api/employeebus \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"empSysId": 1001, "busId": 1, "routeId": 1, "assignedBy": 1}'

# Close assignment
curl -X PUT http://localhost:5012/api/employeebus/1/close \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"closingDate": "2025-12-31", "modifiedBy": 1}'

# Record bus arrival
curl -X POST http://localhost:5012/api/arrivals \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "busId": 1,
    "arrivalDate": "2025-04-01",
    "arrivalTime": "08:30",
    "status": "O",
    "remarks": "On time",
    "recordedBy": 1
  }'

# v2: Bus summary report (Dapper)
curl http://localhost:5012/api/v2/buses/reports/summary \
  -H "Authorization: Bearer <TOKEN>"

# v2: Arrival report
curl "http://localhost:5012/api/v2/buses/reports/arrivals?from=2025-04-01&to=2025-04-30" \
  -H "Authorization: Bearer <TOKEN>"
```

### GraphQL

**Endpoint**: `POST http://localhost:5012/graphql`

```bash
# Query: Get all buses
curl -X POST http://localhost:5012/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getBuses { busId registrationNumber description capacity } }"
  }'

# Query: Get bus by ID
curl -X POST http://localhost:5012/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getBusById(busId: 1) { busId registrationNumber description capacity } }"
  }'

# Query: Get routes for bus
curl -X POST http://localhost:5012/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getRoutesByBus(busId: 1) { routeId busId name description } }"
  }'

# Query: Get arrivals for bus
curl -X POST http://localhost:5012/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getArrivalsByBus(busId: 1) { arrivalId busId arrivalDate arrivalTime status } }"
  }'

# Query: Get arrivals by date
curl -X POST http://localhost:5012/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getArrivalsByDate(date: \"2025-04-01\") { arrivalId busId arrivalTime status } }"
  }'

# Query: Get employee assignments
curl -X POST http://localhost:5012/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getAssignmentsByEmployee(empSysId: 1001) { empBusId empSysId busId routeId } }"
  }'

# Mutation: Register bus
curl -X POST http://localhost:5012/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { registerBus(registrationNumber: \"MH-12-AB-5678\", description: \"Evening bus\", capacity: 40, registeredBy: 1) { busId registrationNumber } }"
  }'

# Mutation: Create route
curl -X POST http://localhost:5012/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { createRoute(busId: 1, name: \"Route B\", description: \"Alt route\", createdBy: 1) { routeId name } }"
  }'

# Mutation: Assign employee to bus
curl -X POST http://localhost:5012/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { assignEmployeeToBus(empSysId: 1001, busId: 1, routeId: 1, assignedBy: 1) { empBusId empSysId } }"
  }'

# Mutation: Record arrival
curl -X POST http://localhost:5012/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { recordArrival(busId: 1, arrivalDate: \"2025-04-01\", arrivalTime: \"08:30\", status: \"O\", remarks: \"On time\", recordedBy: 1) { arrivalId busId } }"
  }'
```

---

