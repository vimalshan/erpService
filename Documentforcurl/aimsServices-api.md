# AIMS Services — API Documentation

> **Gateway Port**: 5020 · **Auth**: JWT Bearer · **GraphQL**: HotChocolate

---

## Table of Contents

- [API Gateway (Port 5020)](#api-gateway)
- [1. Access Service (Port 5010)](#1-access-service)
- [2. Attendance Service (Port 5011)](#2-attendance-service)
- [3. Bus Service (Port 5012)](#3-bus-service)
- [4. Calendar Service (Port 5013)](#4-calendar-service)
- [5. Employee Service (Port 5014)](#5-employee-service)
- [6. Group Incentive Service (Port 5015)](#6-group-incentive-service)
- [7. Leave Service (Port 5016)](#7-leave-service)
- [8. Reference Service (Port 5017)](#8-reference-service)
- [9. Visitor Service (Port 5018)](#9-visitor-service)
- [10. AIMS Transaction Service (Port 5019)](#10-aims-transaction-service)

---

## API Gateway

**Port**: 5020 (primary entry point)

### Gateway Endpoints

| Method | Endpoint                                          | Description                     |
| ------ | ------------------------------------------------- | ------------------------------- |
| `GET`  | `/api/gateway/services`                           | List all downstream services    |
| `GET`  | `/api/gateway/services/{name}/health`             | Health of a specific service    |
| `GET`  | `/api/gateway/services/health`                    | Health of all services          |
| `GET`  | `/api/gateway/proxy/{serviceName}/{path}`         | Proxy GET to downstream         |
| `POST` | `/api/gateway/proxy/{serviceName}/{path}`         | Proxy POST to downstream        |
| `POST` | `/api/graphqlproxy/{serviceName}`                 | Forward GraphQL to downstream   |

```bash
# List all registered services
curl http://localhost:5020/api/gateway/services

# Check health of access service
curl http://localhost:5020/api/gateway/services/access/health

# Check health of all services
curl http://localhost:5020/api/gateway/services/health

# Proxy GET through gateway (e.g. get all buses)
curl http://localhost:5020/api/gateway/proxy/bus/buses \
  -H "Authorization: Bearer <TOKEN>"

# Forward GraphQL query to attendance service
curl -X POST http://localhost:5020/api/graphqlproxy/attendance \
  -H "Content-Type: application/json" \
  -d '{"query": "{ getSwipePunches(empSysId: 1001) { empSysId punchTime } }"}'
```

### Ocelot Routing Table

| Service          | Gateway Route                                | Downstream Port | Rate Limit |
| ---------------- | -------------------------------------------- | --------------- | ---------- |
| Access           | `/api/access/{everything}`                   | 5010            | 100/min    |
| Attendance       | `/api/attendance/{everything}`               | 5011            | 100/min    |
| Bus              | `/api/bus/{everything}`                       | 5012            | 100/min    |
| Calendar         | `/api/calendar/{everything}`                 | 5013            | 100/min    |
| Employee         | `/api/employee/{everything}`                 | 5014            | 100/min    |
| Group Incentive  | `/api/groupincentive/{everything}`           | 5015            | 100/min    |
| Leave            | `/api/leave/{everything}`                    | 5016            | 100/min    |
| Reference        | `/api/reference/{everything}`                | 5017            | 100/min    |
| Visitor          | `/api/visitor/{everything}`                  | 5018            | 100/min    |
| AIMS Transaction | `/api/aimstransaction/{everything}`          | 5019            | 100/min    |

Each service also has a GraphQL route: `/api/{service}/graphql` → downstream `/graphql`

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

## 8. Reference Service

**Port**: 5017 · **Auth**: JWT Bearer

### REST Endpoints

| Method   | Endpoint                                | Description                | Auth    |
| -------- | --------------------------------------- | -------------------------- | ------- |
| `GET`    | `/api/lovtypes?pageNumber=&pageSize=`   | Get LOV types (paged)      | Bearer  |
| `GET`    | `/api/lovtypes/{id}`                    | Get LOV type by ID         | Bearer  |
| `POST`   | `/api/lovtypes`                         | Create LOV type            | Bearer  |
| `PUT`    | `/api/lovtypes/{id}`                    | Update LOV type            | Bearer  |
| `DELETE` | `/api/lovtypes/{id}?modifiedBy=`        | Deactivate LOV type        | Bearer  |
| `GET`    | `/api/lovvalues/by-type/{typeId}`       | Get LOV values by type     | Bearer  |
| `GET`    | `/api/lovvalues/{id}`                   | Get LOV value by ID        | Bearer  |
| `POST`   | `/api/lovvalues`                        | Create LOV value           | Bearer  |
| `PUT`    | `/api/lovvalues/{id}`                   | Update LOV value           | Bearer  |
| `DELETE` | `/api/lovvalues/{id}?modifiedBy=`       | Deactivate LOV value       | Bearer  |

### cURL Examples

```bash
# Get LOV types (paged)
curl "http://localhost:5017/api/lovtypes?pageNumber=1&pageSize=10" \
  -H "Authorization: Bearer <TOKEN>"

# Get LOV type by ID
curl http://localhost:5017/api/lovtypes/1 \
  -H "Authorization: Bearer <TOKEN>"

# Create LOV type
curl -X POST http://localhost:5017/api/lovtypes \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "typeName": "Department",
    "description": "Department categories",
    "createdBy": 1
  }'

# Update LOV type
curl -X PUT http://localhost:5017/api/lovtypes/1 \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"id": 1, "typeName": "Department (Updated)", "modifiedBy": 1}'

# Deactivate LOV type
curl -X DELETE "http://localhost:5017/api/lovtypes/1?modifiedBy=1" \
  -H "Authorization: Bearer <TOKEN>"

# Get LOV values by type
curl http://localhost:5017/api/lovvalues/by-type/1 \
  -H "Authorization: Bearer <TOKEN>"

# Get LOV value by ID
curl http://localhost:5017/api/lovvalues/1 \
  -H "Authorization: Bearer <TOKEN>"

# Create LOV value
curl -X POST http://localhost:5017/api/lovvalues \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "typeId": 1,
    "valueName": "HR Department",
    "description": "Human Resources",
    "createdBy": 1
  }'
```

---

## 9. Visitor Service

**Port**: 5018 · **Auth**: JWT Bearer

### REST Endpoints

| Method | Endpoint                                   | Description                  | Auth    |
| ------ | ------------------------------------------ | ---------------------------- | ------- |
| `GET`  | `/api/visitors/active`                     | Get active (checked-in)      | Bearer  |
| `GET`  | `/api/visitors/{id}`                       | Get visitor by ID            | Bearer  |
| `POST` | `/api/visitors`                            | Register (check-in) visitor  | Bearer  |
| `POST` | `/api/visitors/{id}/checkout`              | Check out visitor            | Bearer  |
| `POST` | `/api/visitors/{id}/items`                 | Add item to visitor          | Bearer  |
| `GET`  | `/api/approvals/pending?approverId=`       | Get pending approvals        | Bearer  |
| `POST` | `/api/approvals/{id}/process`              | Process approval             | Bearer  |

### Minimal API (v2)

| Method | Endpoint                                     | Description               |
| ------ | -------------------------------------------- | ------------------------- |
| `GET`  | `/api/v2/visitors/active`                    | Get active visitors (v2)  |
| `GET`  | `/api/v2/visitors/{id}`                      | Get visitor by ID (v2)    |
| `POST` | `/api/v2/visitors/`                          | Register visitor (v2)     |
| `POST` | `/api/v2/visitors/{id}/checkout`             | Checkout visitor (v2)     |
| `POST` | `/api/v2/visitors/{id}/items`                | Add item (v2)             |
| `GET`  | `/api/v2/approvals/pending?approverId=`      | Get pending (v2)          |
| `POST` | `/api/v2/approvals/{id}/process`             | Process approval (v2)     |

### cURL Examples

```bash
# Get active visitors
curl http://localhost:5018/api/visitors/active \
  -H "Authorization: Bearer <TOKEN>"

# Get visitor by ID
curl http://localhost:5018/api/visitors/1 \
  -H "Authorization: Bearer <TOKEN>"

# Register visitor (check-in)
curl -X POST http://localhost:5018/api/visitors \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "visitorName": "John Doe",
    "idType": "A",
    "idNumber": "ABC123",
    "phoneNumber": "9876543210",
    "email": "john@example.com",
    "company": "Acme Corp",
    "purpose": "Meeting",
    "whomToVisit": 1001,
    "enteredBy": 1
  }'

# Check out visitor
curl -X POST "http://localhost:5018/api/visitors/1/checkout?checkedOutBy=1" \
  -H "Authorization: Bearer <TOKEN>"

# Add item to visitor
curl -X POST http://localhost:5018/api/visitors/1/items \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"visitorId": 1, "itemName": "Laptop", "serialNumber": "SN123", "addedBy": 1}'

# Get pending approvals
curl "http://localhost:5018/api/approvals/pending?approverId=1001" \
  -H "Authorization: Bearer <TOKEN>"

# Approve visitor request
curl -X POST http://localhost:5018/api/approvals/1/process \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"requestId": 1, "isApproved": true, "remarks": "Approved", "processedBy": 1001}'
```

### GraphQL

**Endpoint**: `POST http://localhost:5018/graphql`

```bash
# Query: Get visitor by ID
curl -X POST http://localhost:5018/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getVisitorById(id: 1) { visitorId visitorName idType idNumber phoneNumber email company purpose checkInTime checkOutTime } }"
  }'

# Query: Get active visitors
curl -X POST http://localhost:5018/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getActiveVisitors { visitorId visitorName company purpose checkInTime } }"
  }'

# Query: Get pending approvals
curl -X POST http://localhost:5018/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ getPendingApprovals(approverId: 1001) { requestId visitorName purpose status } }"
  }'

# Mutation: Register visitor
curl -X POST http://localhost:5018/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { registerVisitor(input: { visitorName: \"Jane Doe\", idType: \"P\", idNumber: \"XYZ456\", phoneNumber: \"9876543210\", email: \"jane@test.com\", company: \"Tech Co\", purpose: \"Interview\", whomToVisit: 1001, enteredBy: 1 }) { visitorId visitorName checkInTime } }"
  }'

# Mutation: Checkout visitor
curl -X POST http://localhost:5018/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { checkoutVisitor(visitorId: 1, checkedOutBy: 1) }"
  }'

# Mutation: Process approval
curl -X POST http://localhost:5018/graphql \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "mutation { processApproval(requestId: 1, isApproved: true, remarks: \"OK\", processedBy: 1001) { requestId status } }"
  }'
```

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

## Quick Reference — Port Summary

| Service             | Port | Auth Endpoint                    | GraphQL Endpoint          |
| ------------------- | ---- | -------------------------------- | ------------------------- |
| API Gateway         | 5020 | —                                | `/api/graphqlproxy/{svc}` |
| Access              | 5010 | `POST /api/auth/login`           | `/graphql`                |
| Attendance          | 5011 | `POST /api/auth/login`           | `/graphql`                |
| Bus                 | 5012 | `POST /api/auth/login`           | `/graphql`                |
| Calendar            | 5013 | `POST /api/auth/token`           | `/graphql`                |
| Employee            | 5014 | `POST /api/auth/token`           | `/graphql`                |
| Group Incentive     | 5015 | `POST /api/auth/login`           | `/graphql`                |
| Leave               | 5016 | (via gateway)                    | `/graphql`                |
| Reference           | 5017 | (via gateway)                    | `/graphql`                |
| Visitor             | 5018 | (via gateway)                    | `/graphql`                |
| AIMS Transaction    | 5019 | (via gateway)                    | `/graphql`                |
