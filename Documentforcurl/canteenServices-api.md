# Canteen Services API Documentation

> **API Gateway:** `http://localhost:5188`  
> **Gateway Swagger:** `http://localhost:5188/swagger`  
> **Gateway Health:** `http://localhost:5188/health`

---

## Table of Contents

1. [Overview](#overview)
2. [Service Map](#service-map)
3. [API Gateway](#api-gateway)
4. [CanteenUnit Service (5190)](#canteenunit-service-5190)
5. [CardManagement Service (5191)](#cardmanagement-service-5191)
6. [Deduction Service (5192)](#deduction-service-5192)
7. [Eligibility Service (5193)](#eligibility-service-5193)
8. [ItemMaster Service (5194)](#itemmaster-service-5194)
9. [ReferenceData Service (5195)](#referencedata-service-5195)
10. [SwipeTransaction Service (5196)](#swipetransaction-service-5196)
11. [CanteenTransaction Service (5197)](#canteentransaction-service-5197)
12. [Ocelot Gateway Routing](#ocelot-gateway-routing)
13. [Docker Compose Setup](#docker-compose-setup)

---

## Overview

Canteen Services is a microservices-based module managing canteen operations including units, cards, meals, deductions, eligibility, items/pricing, swipe transactions, and daily transactions.

**Tech Stack:**
- ASP.NET Core (.NET 8+) with CQRS (MediatR), Repository pattern
- HotChocolate GraphQL (queries, mutations)
- Ocelot API Gateway with rate limiting, circuit breaker, JWT auth, CacheManager
- Entity Framework Core + Dapper (read side)
- SQL Server 2022, RabbitMQ, Azurite
- API Versioning (v1 controllers, v2 Minimal APIs)

---

## Service Map

| Service | Port | Gateway Route | Database |
|---|---|---|---|
| API Gateway | 5188 | — | — |
| CanteenUnit | 5190 | `/api/canteen-unit/` | CanteenUnitDb |
| CardManagement | 5191 | `/api/card-management/` | CardManagementDb |
| Deduction | 5192 | `/api/deduction/` | DeductionDb |
| Eligibility | 5193 | `/api/eligibility/` | EligibilityDb |
| ItemMaster | 5194 | `/api/itemmaster/` | ItemMasterDb |
| ReferenceData | 5195 | `/api/referencedata/` | ReferenceDataDb |
| SwipeTransaction | 5196 | `/api/swipe-transaction/` | SwipeTransactionDb |
| CanteenTransaction | 5197 | `/api/canteen-transaction/` | CanteenTransactionDb |

---

## API Gateway

**Port:** 5188

### Gateway Auth – Get Token

```bash
curl -X POST http://localhost:5188/api/auth/token \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}'
```

**Response:**
```json
{ "token": "eyJhbGciOiJIUzI1NiIs..." }
```

### Gateway – List All Services

```bash
curl http://localhost:5188/api/gateway/services
```

### Gateway – Check Service Health

```bash
curl http://localhost:5188/api/gateway/services/canteenunit/health \
  -H "Authorization: Bearer <token>"
```

### Gateway Health Endpoints

| Endpoint | Description |
|---|---|
| `/health` | Full health check (all downstream services) |
| `/health/ready` | Readiness check (downstream + RabbitMQ) |
| `/health/live` | Liveness probe (always healthy) |

---

## CanteenUnit Service (5190)

**Direct:** `http://localhost:5190`  
**Via Gateway:** `http://localhost:5188/api/canteen-unit/`  
**GraphQL:** `http://localhost:5190/graphql`

### Auth – Get Token (per-service)

Each canteen sub-service has its own local auth endpoint for testing:

```bash
# (Pattern repeats for services that have AuthController)
```

### REST Endpoints

#### CanteenUnits Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/CanteenUnits` | Bearer | Get all canteen units |
| GET | `/api/CanteenUnits/{comCode}` | Bearer | Get unit by company code |
| POST | `/api/CanteenUnits` | Bearer | Create canteen unit |
| PUT | `/api/CanteenUnits/{comCode}` | Bearer | Update canteen unit |
| DELETE | `/api/CanteenUnits/{comCode}` | Admin role | Delete canteen unit |

**cURL Examples:**

```bash
# Get all canteen units
curl http://localhost:5190/api/CanteenUnits \
  -H "Authorization: Bearer <token>"

# Get by company code
curl http://localhost:5190/api/CanteenUnits/1001 \
  -H "Authorization: Bearer <token>"

# Create canteen unit
curl -X POST http://localhost:5190/api/CanteenUnits \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "unComCod": 1001,
    "unitName": "Main Canteen",
    "location": "Building A",
    "capacity": 200
  }'

# Update
curl -X PUT http://localhost:5190/api/CanteenUnits/1001 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "comCode": 1001,
    "unitName": "Main Canteen Updated",
    "location": "Building A",
    "capacity": 250
  }'

# Delete
curl -X DELETE http://localhost:5190/api/CanteenUnits/1001 \
  -H "Authorization: Bearer <token>"
```

#### CanteenMasters Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/CanteenMasters` | Bearer | Get all canteen masters |
| GET | `/api/CanteenMasters/{comCode}` | Bearer | Get by company code |
| POST | `/api/CanteenMasters` | Bearer | Create canteen master |
| PATCH | `/api/CanteenMasters/{comCode}/live?flag=Y` | Bearer | Set live flag |
| DELETE | `/api/CanteenMasters/{comCode}` | Admin role | Delete canteen master |

**cURL Examples:**

```bash
# Get all
curl http://localhost:5190/api/CanteenMasters \
  -H "Authorization: Bearer <token>"

# Set live flag
curl -X PATCH "http://localhost:5190/api/CanteenMasters/1001/live?flag=Y" \
  -H "Authorization: Bearer <token>"
```

### Minimal API (v2)

| Method | Route | Description |
|---|---|---|
| GET | `/api/v2/canteen-units/` | Get all canteen units |
| GET | `/api/v2/canteen-units/{comCode}` | Get by company code |
| POST | `/api/v2/canteen-units/` | Create canteen unit |
| GET | `/api/v2/canteen-units/search?name=Main` | Search units (Dapper) |
| GET | `/api/v2/canteen-units/with-access-count` | Units with access count |

```bash
# v2 Get all
curl http://localhost:5190/api/v2/canteen-units/ \
  -H "Authorization: Bearer <token>"

# v2 Search by name
curl "http://localhost:5190/api/v2/canteen-units/search?name=Main" \
  -H "Authorization: Bearer <token>"
```

### GraphQL

```bash
# Queries
curl -X POST http://localhost:5190/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "{ canteenUnits { unComCod unitName location capacity } }"}'

curl -X POST http://localhost:5190/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "{ canteenUnit(comCode: 1001) { unComCod unitName location capacity } }"}'

curl -X POST http://localhost:5190/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "{ canteenMasters { cnComCod canteenName liveFlag } }"}'

# Mutations
curl -X POST http://localhost:5190/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { createCanteenUnit(input: { unComCod: 1002, unitName: \"East Wing\", location: \"Building B\", capacity: 100 }) { unComCod unitName } }"}'

curl -X POST http://localhost:5190/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { deleteCanteenUnit(comCode: 1002) }"}'
```

---

## CardManagement Service (5191)

**Direct:** `http://localhost:5191`  
**Via Gateway:** `http://localhost:5188/api/card-management/`  
**GraphQL:** `http://localhost:5191/graphql`

### Auth – Get Token

```bash
curl -X POST http://localhost:5191/api/auth/token \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin"}'
```

### REST Endpoints

#### GuestCards Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/GuestCards` | Bearer | Get all guest cards (paged) |
| GET | `/api/GuestCards/{canteenUnit}` | Bearer | Get guest card by canteen unit |
| POST | `/api/GuestCards` | Bearer | Create guest card |
| PUT | `/api/GuestCards/{canteenUnit}` | Bearer | Update guest card |
| PATCH | `/api/GuestCards/{canteenUnit}/close` | Bearer | Close guest card |

**cURL Examples:**

```bash
# Get all guest cards (paged)
curl "http://localhost:5191/api/GuestCards?pageNumber=1&pageSize=20&canteenUnit=1001" \
  -H "Authorization: Bearer <token>"

# Get by canteen unit
curl http://localhost:5191/api/GuestCards/1001 \
  -H "Authorization: Bearer <token>"

# Create guest card
curl -X POST http://localhost:5191/api/GuestCards \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "canteenUnit": 1001,
    "cardNumber": "GC-2025-001",
    "holderName": "John Doe",
    "validFrom": "2025-01-01",
    "validTo": "2025-12-31"
  }'

# Close guest card
curl -X PATCH http://localhost:5191/api/GuestCards/1001/close \
  -H "Authorization: Bearer <token>"
```

#### CardMaps Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/CardMaps/{canteenUnit}?activeOnly=true` | Bearer | Get card maps for canteen unit |

```bash
curl "http://localhost:5191/api/CardMaps/1001?activeOnly=true" \
  -H "Authorization: Bearer <token>"
```

#### Settlements Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/api/Settlements` | Bearer | Create settlement record |

```bash
curl -X POST http://localhost:5191/api/Settlements \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "canteenUnit": 1001,
    "settlementDate": "2025-01-31",
    "totalAmount": 5000.00,
    "settledByUserId": 1
  }'
```

### Minimal API

| Method | Route | Description |
|---|---|---|
| GET | `/api/minimal/cards/` | List guest cards (paged) |
| GET | `/api/minimal/cards/{canteenUnit}` | Get guest card by canteen unit |

```bash
curl "http://localhost:5191/api/minimal/cards/?pageNumber=1&pageSize=10" \
  -H "Authorization: Bearer <token>"
```

### GraphQL

```graphql
# Queries
query {
  guestCards(pageNumber: 1, pageSize: 10, canteenUnit: 1001) {
    canteenUnit cardNumber holderName validFrom validTo
  }
}

query {
  guestCardById(canteenUnit: 1001) {
    canteenUnit cardNumber holderName validFrom validTo
  }
}
```

```bash
curl -X POST http://localhost:5191/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "{ guestCards(pageNumber: 1, pageSize: 10) { canteenUnit cardNumber holderName } }"}'

# Mutations
curl -X POST http://localhost:5191/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { createGuestCard(command: { canteenUnit: 1001, cardNumber: \"GC-001\", holderName: \"Jane\", validFrom: \"2025-01-01\", validTo: \"2025-12-31\" }) { canteenUnit cardNumber } }"}'

curl -X POST http://localhost:5191/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { closeGuestCard(canteenUnit: 1001) }"}'

curl -X POST http://localhost:5191/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { settleCard(command: { canteenUnit: 1001, settlementDate: \"2025-01-31\", totalAmount: 5000.00 }) { canteenUnit totalAmount } }"}'
```

---

## Deduction Service (5192)

**Direct:** `http://localhost:5192`  
**Via Gateway:** `http://localhost:5188/api/deduction/`  
**GraphQL:** `http://localhost:5192/graphql`

### REST Endpoints

#### Deduction Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/Deduction/{systemId}` | Bearer | Get deduction by ID |
| GET | `/api/Deduction/employee/{employeeNumber}` | Bearer | Get deductions by employee |
| GET | `/api/Deduction/employee/{employeeNumber}/history` | Bearer | Get deduction history |
| GET | `/api/Deduction/amount?empSysId=&itemCode=&dateTaken=` | Bearer | Get deduction amount |
| POST | `/api/Deduction` | Bearer | Create ad-hoc deduction |
| DELETE | `/api/Deduction/{systemId}?cancelledByUserId=` | Bearer | Cancel deduction |
| POST | `/api/Deduction/process-monthly` | PayrollAdmin | Process monthly deductions |

**cURL Examples:**

```bash
# Get deductions by employee
curl http://localhost:5192/api/Deduction/employee/10001 \
  -H "Authorization: Bearer <token>"

# Get deduction history
curl http://localhost:5192/api/Deduction/employee/10001/history \
  -H "Authorization: Bearer <token>"

# Get deduction amount (from pricing function)
curl "http://localhost:5192/api/Deduction/amount?empSysId=10001&itemCode=1&dateTaken=2025-01-15" \
  -H "Authorization: Bearer <token>"

# Create ad-hoc deduction
curl -X POST http://localhost:5192/api/Deduction \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "systemId": 0,
    "canteenUnit": 1001,
    "payAmount": 150.00,
    "earningDeductionCode": "CANTEEN",
    "employeeNumber": 10001,
    "enteredByUserId": 1,
    "companyCode": 100,
    "gradeType": "A"
  }'

# Cancel deduction
curl -X DELETE "http://localhost:5192/api/Deduction/12345?cancelledByUserId=1" \
  -H "Authorization: Bearer <token>"

# Process monthly deductions (PayrollAdmin only)
curl -X POST http://localhost:5192/api/Deduction/process-monthly \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "monthYear": "2025-01",
    "processedByUserId": 1
  }'
```

### Minimal API (v2)

| Method | Route | Description |
|---|---|---|
| GET | `/api/v2/deductions/employee/{employeeNumber}` | Get deductions by employee |
| POST | `/api/v2/deductions/` | Create ad-hoc deduction |

```bash
curl http://localhost:5192/api/v2/deductions/employee/10001 \
  -H "Authorization: Bearer <token>"
```

### GraphQL

```graphql
# Queries
query { deductionById(systemId: 12345) { systemId canteenUnit payAmount employeeNumber } }
query { deductionsByEmployee(employeeNumber: 10001) { systemId payAmount earningDeductionCode } }
query { deductionHistory(employeeNumber: 10001) { systemId payAmount effectiveDate } }
query { deductionAmount(empSysId: 10001, itemCode: 1, dateTaken: "2025-01-15") { amount } }
```

```bash
curl -X POST http://localhost:5192/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "{ deductionsByEmployee(employeeNumber: 10001) { systemId canteenUnit payAmount employeeNumber } }"}'

# Mutations
curl -X POST http://localhost:5192/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { createDeduction(input: { systemId: 0, canteenUnit: 1001, payAmount: 150.00, earningDeductionCode: \"CANTEEN\", employeeNumber: 10001, enteredByUserId: 1, companyCode: 100, gradeType: \"A\" }) { systemId payAmount } }"}'

curl -X POST http://localhost:5192/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { cancelDeduction(systemId: 12345, cancelledByUserId: 1) }"}'

curl -X POST http://localhost:5192/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { processMonthlyDeduction(monthYear: \"2025-01\", processedByUserId: 1) { processedCount totalAmount } }"}'
```

---

## Eligibility Service (5193)

**Direct:** `http://localhost:5193`  
**Via Gateway:** `http://localhost:5188/api/eligibility/`  
**GraphQL:** `http://localhost:5193/graphql`

### Auth – Get Token

```bash
curl -X POST http://localhost:5193/api/auth/token \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"Admin@123"}'
```

### REST Endpoints

#### EligibilityMaster Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/EligibilityMaster?canteenUnit=` | Bearer | Get all eligibility records |
| GET | `/api/EligibilityMaster/{canteenUnit}/{shiftCode}/{itemCode}` | Bearer | Get by composite key |
| GET | `/api/EligibilityMaster/check?canteenUnit=&shiftCode=&itemCode=&requestedQty=` | Bearer | Check meal eligibility |
| GET | `/api/EligibilityMaster/{canteenUnit}/{shiftCode}/{itemCode}/history` | Bearer | Get audit history |
| POST | `/api/EligibilityMaster` | Bearer | Create eligibility record |
| PUT | `/api/EligibilityMaster/{canteenUnit}/{shiftCode}/{itemCode}` | Bearer | Update record |
| DELETE | `/api/EligibilityMaster/{canteenUnit}/{shiftCode}/{itemCode}` | Bearer | Delete record |

**cURL Examples:**

```bash
# Get all eligibility records for a canteen unit
curl "http://localhost:5193/api/EligibilityMaster?canteenUnit=1001" \
  -H "Authorization: Bearer <token>"

# Get by composite key
curl http://localhost:5193/api/EligibilityMaster/1001/MORNING/1 \
  -H "Authorization: Bearer <token>"

# Check meal eligibility
curl "http://localhost:5193/api/EligibilityMaster/check?canteenUnit=1001&shiftCode=MORNING&itemCode=1&requestedQty=1" \
  -H "Authorization: Bearer <token>"

# Get audit history
curl http://localhost:5193/api/EligibilityMaster/1001/MORNING/1/history \
  -H "Authorization: Bearer <token>"

# Create eligibility record
curl -X POST http://localhost:5193/api/EligibilityMaster \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "canteenUnit": 1001,
    "shiftCode": "MORNING",
    "itemCode": 1,
    "maxQuantity": 2,
    "isActive": true
  }'
```

#### DaywiseEligibility Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/DaywiseEligibility/{serialNumber}` | Bearer | Get by serial number |
| GET | `/api/DaywiseEligibility/employee/{companyCode}/{employeeSysId}` | Bearer | Get by employee |
| GET | `/api/DaywiseEligibility/date/{companyCode}?date=` | Bearer | Get by date |
| POST | `/api/DaywiseEligibility` | Bearer | Create record |
| DELETE | `/api/DaywiseEligibility/{serialNumber}` | Bearer | Delete record |

```bash
# Get daywise eligibility by employee
curl http://localhost:5193/api/DaywiseEligibility/employee/100/10001 \
  -H "Authorization: Bearer <token>"

# Get by date
curl "http://localhost:5193/api/DaywiseEligibility/date/100?date=2025-01-15" \
  -H "Authorization: Bearer <token>"
```

### Minimal API (v2)

| Method | Route | Description |
|---|---|---|
| GET | `/eligibility/v2/` | Get all eligibility records |
| GET | `/eligibility/v2/{canteenUnit}/{shiftCode}/{itemCode}` | Get by composite key |
| POST | `/eligibility/v2/` | Create eligibility record |
| PUT | `/eligibility/v2/{canteenUnit}/{shiftCode}/{itemCode}` | Update record |
| DELETE | `/eligibility/v2/{canteenUnit}/{shiftCode}/{itemCode}` | Delete record |

```bash
curl "http://localhost:5193/eligibility/v2/?canteenUnit=1001" \
  -H "Authorization: Bearer <token>"
```

### GraphQL

```graphql
# Queries
query { eligibilityMasters(canteenUnit: 1001) { canteenUnit shiftCode itemCode maxQuantity isActive } }
query { eligibilityMaster(canteenUnit: 1001, shiftCode: "MORNING", itemCode: 1) { canteenUnit shiftCode itemCode } }
query { checkEligibility(canteenUnit: 1001, shiftCode: "MORNING", itemCode: 1, requestedQty: 1) { isEligible reason } }
query { eligibilityHistory(canteenUnit: 1001, shiftCode: "MORNING", itemCode: 1) { changeDate changeType } }
```

```bash
curl -X POST http://localhost:5193/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "{ eligibilityMasters(canteenUnit: 1001) { canteenUnit shiftCode itemCode maxQuantity isActive } }"}'

# Mutations
curl -X POST http://localhost:5193/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { createEligibility(input: { canteenUnit: 1001, shiftCode: \"MORNING\", itemCode: 1, maxQuantity: 2, isActive: true }) { canteenUnit shiftCode itemCode } }"}'

curl -X POST http://localhost:5193/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { deleteEligibility(canteenUnit: 1001, shiftCode: \"MORNING\", itemCode: 1) }"}'
```

---

## ItemMaster Service (5194)

**Direct:** `http://localhost:5194`  
**Via Gateway:** `http://localhost:5188/api/itemmaster/`  
**GraphQL:** `http://localhost:5194/graphql`

### Auth – Get Token

```bash
curl -X POST http://localhost:5194/api/auth/token \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin"}'
```

### REST Endpoints

#### CanteenItemMaster Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/CanteenItemMaster/{canteenUnitCode}` | Bearer | Get all items for a unit |
| GET | `/api/CanteenItemMaster/{canteenUnitCode}/{itemCode}` | Bearer | Get specific item |
| POST | `/api/CanteenItemMaster` | Bearer | Create item |
| PUT | `/api/CanteenItemMaster/{canteenUnitCode}/{itemCode}` | Bearer | Update item |
| DELETE | `/api/CanteenItemMaster/{canteenUnitCode}/{itemCode}` | Bearer | Delete item |

**cURL Examples:**

```bash
# Get all items for a canteen unit
curl http://localhost:5194/api/CanteenItemMaster/1001 \
  -H "Authorization: Bearer <token>"

# Get specific item
curl http://localhost:5194/api/CanteenItemMaster/1001/5 \
  -H "Authorization: Bearer <token>"

# Create item
curl -X POST http://localhost:5194/api/CanteenItemMaster \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "canteenUnitCode": 1001,
    "itemCode": 10,
    "itemName": "Lunch Thali",
    "itemCategory": "LUNCH",
    "isActive": true
  }'

# Update item
curl -X PUT http://localhost:5194/api/CanteenItemMaster/1001/10 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "canteenUnitCode": 1001,
    "itemCode": 10,
    "itemName": "Lunch Thali Special",
    "itemCategory": "LUNCH",
    "isActive": true
  }'

# Delete item
curl -X DELETE http://localhost:5194/api/CanteenItemMaster/1001/10 \
  -H "Authorization: Bearer <token>"
```

#### CanteenItemPrice Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/CanteenItemPrice/{canteenUnitCode}/{itemCode}/active` | Bearer | Get active price |
| GET | `/api/CanteenItemPrice/{canteenUnitCode}/{itemCode}/history` | Bearer | Get price history |
| POST | `/api/CanteenItemPrice` | Bearer | Create price record |
| PATCH | `/api/CanteenItemPrice/{canteenUnitCode}/{itemCode}/close` | Bearer | Close active price |

```bash
# Get active price
curl http://localhost:5194/api/CanteenItemPrice/1001/5/active \
  -H "Authorization: Bearer <token>"

# Get price history
curl http://localhost:5194/api/CanteenItemPrice/1001/5/history \
  -H "Authorization: Bearer <token>"

# Create price record
curl -X POST http://localhost:5194/api/CanteenItemPrice \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "canteenUnitCode": 1001,
    "itemCode": 5,
    "price": 75.00,
    "effectiveFrom": "2025-01-01"
  }'

# Close price
curl -X PATCH http://localhost:5194/api/CanteenItemPrice/1001/5/close \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '"2025-06-30"'
```

#### CanteenGradeItemPrice Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/CanteenGradeItemPrice` | Bearer | Get all grade item prices |
| GET | `/api/CanteenGradeItemPrice/{canteenUnitCode}` | Bearer | Get by canteen unit |
| POST | `/api/CanteenGradeItemPrice` | Bearer | Create grade item price |
| PUT | `/api/CanteenGradeItemPrice/{canteenUnitCode}` | Bearer | Update grade item price |

```bash
# Get all grade item prices
curl http://localhost:5194/api/CanteenGradeItemPrice \
  -H "Authorization: Bearer <token>"

# Get by canteen unit
curl http://localhost:5194/api/CanteenGradeItemPrice/1001 \
  -H "Authorization: Bearer <token>"
```

### Minimal API (v2)

| Method | Route | Description |
|---|---|---|
| GET | `/api/v2/canteen-items/{canteenUnitCode}` | Get all items for a unit |
| GET | `/api/v2/canteen-items/{canteenUnitCode}/{itemCode}` | Get specific item |
| POST | `/api/v2/canteen-items/` | Create item |
| DELETE | `/api/v2/canteen-items/{canteenUnitCode}/{itemCode}` | Delete item |

```bash
curl http://localhost:5194/api/v2/canteen-items/1001 \
  -H "Authorization: Bearer <token>"
```

### GraphQL

```graphql
# Queries
query { canteenItems(canteenUnitCode: 1001) { canteenUnitCode itemCode itemName itemCategory isActive } }
query { canteenItem(canteenUnitCode: 1001, itemCode: 5) { canteenUnitCode itemCode itemName } }
query { activePrice(canteenUnitCode: 1001, itemCode: 5) { canteenUnitCode itemCode price effectiveFrom } }
query { gradeItemPrices { canteenUnitCode gradeType price } }
```

```bash
curl -X POST http://localhost:5194/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "{ canteenItems(canteenUnitCode: 1001) { canteenUnitCode itemCode itemName itemCategory isActive } }"}'

# Mutations
curl -X POST http://localhost:5194/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { createCanteenItem(input: { canteenUnitCode: 1001, itemCode: 10, itemName: \"Lunch Thali\", itemCategory: \"LUNCH\" }) { canteenUnitCode itemCode itemName } }"}'

curl -X POST http://localhost:5194/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { deleteCanteenItem(canteenUnitCode: 1001, itemCode: 10) }"}'
```

---

## ReferenceData Service (5195)

**Direct:** `http://localhost:5195`  
**Via Gateway:** `http://localhost:5188/api/referencedata/`  
**GraphQL:** `http://localhost:5195/graphql`

### Auth – Get Token

```bash
curl -X POST http://localhost:5195/api/auth/token \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin"}'
```

### REST Endpoints

#### LovMaster Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/LovMaster` | Bearer | Get all LOV records |
| GET | `/api/LovMaster/{lovId}` | Bearer | Get LOV by ID |
| POST | `/api/LovMaster` | Bearer | Create LOV |
| PUT | `/api/LovMaster/{lovId}` | Bearer | Update LOV |
| DELETE | `/api/LovMaster/{lovId}` | Bearer | Delete LOV |

```bash
# Get all LOV records
curl http://localhost:5195/api/LovMaster \
  -H "Authorization: Bearer <token>"

# Get by ID
curl http://localhost:5195/api/LovMaster/MEAL_TYPE \
  -H "Authorization: Bearer <token>"

# Create LOV
curl -X POST http://localhost:5195/api/LovMaster \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"lovId": "MEAL_TYPE", "lovType": "CANTEEN", "lovName": "Meal Type"}'

# Delete LOV
curl -X DELETE http://localhost:5195/api/LovMaster/MEAL_TYPE \
  -H "Authorization: Bearer <token>"
```

#### LovTypeMaster Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/LovTypeMaster` | Bearer | Get all LOV types |
| GET | `/api/LovTypeMaster/{lovTypeCode}` | Bearer | Get type by code |
| POST | `/api/LovTypeMaster` | Bearer | Create LOV type |
| PUT | `/api/LovTypeMaster/{lovTypeCode}` | Bearer | Update LOV type |
| DELETE | `/api/LovTypeMaster/{lovTypeCode}` | Bearer | Delete LOV type |

```bash
curl http://localhost:5195/api/LovTypeMaster \
  -H "Authorization: Bearer <token>"
```

#### PathToSqlServer Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| GET | `/api/PathToSqlServer` | Bearer | Get all paths |
| POST | `/api/PathToSqlServer` | Bearer | Create path |
| PUT | `/api/PathToSqlServer/{id}` | Bearer | Update path |
| DELETE | `/api/PathToSqlServer/{id}` | Bearer | Delete path |

```bash
curl http://localhost:5195/api/PathToSqlServer \
  -H "Authorization: Bearer <token>"
```

### Minimal API (v2)

| Method | Route | Description |
|---|---|---|
| GET | `/api/v2/lov-masters` | Get all LOV masters |
| GET | `/api/v2/lov-masters/{lovId}` | Get LOV by ID |
| POST | `/api/v2/lov-masters` | Create LOV |
| PUT | `/api/v2/lov-masters/{lovId}` | Update LOV |
| DELETE | `/api/v2/lov-masters/{lovId}` | Delete LOV |
| GET | `/api/v2/lov-type-masters` | Get all LOV types |
| GET | `/api/v2/lov-type-masters/{lovTypeCode}` | Get type by code |
| POST | `/api/v2/lov-type-masters` | Create LOV type |
| PUT | `/api/v2/lov-type-masters/{lovTypeCode}` | Update LOV type |
| DELETE | `/api/v2/lov-type-masters/{lovTypeCode}` | Delete LOV type |
| GET | `/api/v2/path-to-sql-servers` | Get all SQL server paths |

```bash
curl http://localhost:5195/api/v2/lov-masters \
  -H "Authorization: Bearer <token>"

curl http://localhost:5195/api/v2/lov-type-masters \
  -H "Authorization: Bearer <token>"
```

### GraphQL

```graphql
# Queries
query { lovMasters { lovId lovType lovName } }
query { lovMasterById(lovId: "MEAL_TYPE") { lovId lovType lovName } }
query { lovTypeMasters { lovTypeCode lovTypeName } }
query { lovTypeMasterByCode(lovTypeCode: "CANTEEN") { lovTypeCode lovTypeName } }
query { pathToSqlServers { companyCode serverName databaseName } }
```

```bash
curl -X POST http://localhost:5195/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "{ lovMasters { lovId lovType lovName } }"}'

# Mutations
curl -X POST http://localhost:5195/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { createLovMaster(lovId: \"MEAL_TYPE\", lovType: \"CANTEEN\", lovName: \"Meal Type\") { lovId lovType lovName } }"}'

curl -X POST http://localhost:5195/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { deleteLovMaster(lovId: \"MEAL_TYPE\") }"}'

curl -X POST http://localhost:5195/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { createPathToSqlServer(companyCode: \"100\", serverName: \"db-server\", databaseName: \"CanteenDB\", userId: \"sa\", dbPassword: \"pass\") { companyCode serverName databaseName } }"}'
```

---

## SwipeTransaction Service (5196)

**Direct:** `http://localhost:5196`  
**Via Gateway:** `http://localhost:5188/api/swipe-transaction/`  
**GraphQL:** `http://localhost:5196/graphql`

### Auth – Get Token

```bash
curl -X POST http://localhost:5196/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"P@ssw0rd!"}'
```

### REST Endpoints

#### SwipeTransactions Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/api/SwipeTransactions` | Bearer | Record swipe upload |
| GET | `/api/SwipeTransactions/{employeeNumber}?from=&to=` | Bearer | Get swipes by employee & date range |
| GET | `/api/SwipeTransactions/pending` | Bearer | Get pending swipes |

**cURL Examples:**

```bash
# Record swipe upload
curl -X POST http://localhost:5196/api/SwipeTransactions \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "companyCode": 100,
    "employeeNumber": "10001",
    "swipeTime": "2025-01-15T12:30:00",
    "itemCode": 5,
    "itemQuantity": 1,
    "canteenNumber": 1001,
    "gateNumber": 1
  }'

# Get swipes by employee
curl "http://localhost:5196/api/SwipeTransactions/10001?from=2025-01-01&to=2025-01-31" \
  -H "Authorization: Bearer <token>"

# Get pending swipes
curl http://localhost:5196/api/SwipeTransactions/pending \
  -H "Authorization: Bearer <token>"
```

#### CanteenPunch Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/api/CanteenPunch` | Bearer | Record punch (check-in/check-out) |
| GET | `/api/CanteenPunch/{empSysId}/today` | Bearer | Get today's punch |
| GET | `/api/CanteenPunch/{empSysId}?from=&to=` | Bearer | Get punches by date range |

```bash
# Record punch
curl -X POST http://localhost:5196/api/CanteenPunch \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "companyCode": 100,
    "employeeSysId": 10001,
    "canteenUnit": 1001,
    "punchType": "IN"
  }'

# Get today's punch
curl http://localhost:5196/api/CanteenPunch/10001/today \
  -H "Authorization: Bearer <token>"

# Get punches by date range
curl "http://localhost:5196/api/CanteenPunch/10001?from=2025-01-01&to=2025-01-31" \
  -H "Authorization: Bearer <token>"
```

### Minimal API (v2)

| Method | Route | Description |
|---|---|---|
| POST | `/api/v2/swipes/` | Record swipe upload |
| GET | `/api/v2/swipes/{employeeNumber}/range?from=&to=` | Get swipes by employee & range |
| GET | `/api/v2/swipes/pending` | Get pending swipes |
| POST | `/api/v2/punches/` | Record canteen punch |

```bash
curl -X POST http://localhost:5196/api/v2/swipes/ \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "companyCode": 100,
    "employeeNumber": "10001",
    "swipeTime": "2025-01-15T12:30:00",
    "itemCode": 5,
    "itemQuantity": 1,
    "canteenNumber": 1001,
    "gateNumber": 1
  }'

curl http://localhost:5196/api/v2/swipes/pending \
  -H "Authorization: Bearer <token>"
```

### GraphQL

```graphql
# Queries
query {
  swipesByEmployee(employeeNumber: "10001", from: "2025-01-01", to: "2025-01-31") {
    companyCode employeeNumber swipeTime itemCode itemQuantity canteenNumber updateStatus
  }
}
query { batchSummary(batchNumber: 100) { batchNumber totalSwipes totalQuantity } }
query { dailyAvailed(empSysId: 10001, date: "2025-01-15") { itemCode itemQuantity } }
query { todayPunch(empSysId: 10001) { serialNumber companyCode employeeSysId canteenUnit punchDate timeIn timeOut workHours } }
```

```bash
curl -X POST http://localhost:5196/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "{ swipesByEmployee(employeeNumber: \"10001\", from: \"2025-01-01\", to: \"2025-01-31\") { companyCode employeeNumber swipeTime itemCode itemQuantity } }"}'

# Mutations
curl -X POST http://localhost:5196/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { recordSwipeUpload(input: { companyCode: 100, employeeNumber: \"10001\", swipeTime: \"2025-01-15T12:30:00\", itemCode: 5, itemQuantity: 1, canteenNumber: 1001, gateNumber: 1 }) { employeeNumber swipeTime } }"}'

curl -X POST http://localhost:5196/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { recordPunch(input: { companyCode: 100, employeeSysId: 10001, canteenUnit: 1001, punchType: \"IN\" }) { serialNumber punchDate timeIn } }"}'
```

---

## CanteenTransaction Service (5197)

**Direct:** `http://localhost:5197`  
**Via Gateway:** `http://localhost:5188/api/canteen-transaction/`  
**GraphQL:** `http://localhost:5197/graphql`

### Auth – Get Token

```bash
curl -X POST http://localhost:5197/api/auth/token \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin"}'
```

### REST Endpoints

#### CanteenTransaction Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/api/CanteenTransaction` | Bearer | Record meal transaction |
| GET | `/api/CanteenTransaction/{serialNumber}` | Bearer | Get by serial number |
| GET | `/api/CanteenTransaction/employee/{employeeSysId}?fromDate=&toDate=` | Bearer | By employee & range |
| GET | `/api/CanteenTransaction/company/{companyCode}?swipeDate=` | Bearer | By company & date |
| DELETE | `/api/CanteenTransaction/{serialNumber}` | Bearer | Cancel transaction |

**cURL Examples:**

```bash
# Record meal transaction
curl -X POST http://localhost:5197/api/CanteenTransaction \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "companyCode": 100,
    "employeeSysId": 10001,
    "canteenUnit": 1001,
    "itemCode": 5,
    "quantity": 1,
    "swipeDate": "2025-01-15"
  }'

# Get by serial number
curl http://localhost:5197/api/CanteenTransaction/12345 \
  -H "Authorization: Bearer <token>"

# Get by employee + date range
curl "http://localhost:5197/api/CanteenTransaction/employee/10001?fromDate=2025-01-01&toDate=2025-01-31" \
  -H "Authorization: Bearer <token>"

# Get by company + date
curl "http://localhost:5197/api/CanteenTransaction/company/100?swipeDate=2025-01-15" \
  -H "Authorization: Bearer <token>"

# Cancel transaction
curl -X DELETE http://localhost:5197/api/CanteenTransaction/12345 \
  -H "Authorization: Bearer <token>"
```

#### DailyAvailed Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/api/DailyAvailed` | Bearer | Process daily availed |
| GET | `/api/DailyAvailed/{serialNumber}` | Bearer | Get by serial number |
| GET | `/api/DailyAvailed/employee/{employeeSysId}?fromDate=&toDate=` | Bearer | By employee |
| GET | `/api/DailyAvailed/company/{companyCode}?swipeDate=` | Bearer | By company & date |

```bash
# Process daily availed
curl -X POST http://localhost:5197/api/DailyAvailed \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "companyCode": 100,
    "employeeSysId": 10001,
    "canteenUnit": 1001,
    "itemCode": 5,
    "quantity": 1,
    "swipeDate": "2025-01-15"
  }'

# Get by employee
curl "http://localhost:5197/api/DailyAvailed/employee/10001?fromDate=2025-01-01&toDate=2025-01-31" \
  -H "Authorization: Bearer <token>"
```

#### MisBatch Controller

| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/api/MisBatch` | Bearer | Submit MIS batch |
| GET | `/api/MisBatch/{serialNumber}` | Bearer | Get by serial number |
| GET | `/api/MisBatch/batch/{batchNumber}` | Bearer | Get by batch number |
| GET | `/api/MisBatch/pending` | Bearer | Get pending batches |
| PATCH | `/api/MisBatch/{serialNumber}/process` | Bearer | Mark as processed |
| PATCH | `/api/MisBatch/{serialNumber}/fail` | Bearer | Mark as failed |

```bash
# Submit MIS batch
curl -X POST http://localhost:5197/api/MisBatch \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "batchNumber": 100,
    "companyCode": 100,
    "canteenUnit": 1001,
    "batchDate": "2025-01-15"
  }'

# Get pending batches
curl http://localhost:5197/api/MisBatch/pending \
  -H "Authorization: Bearer <token>"

# Mark as processed
curl -X PATCH http://localhost:5197/api/MisBatch/12345/process \
  -H "Authorization: Bearer <token>"

# Mark as failed
curl -X PATCH http://localhost:5197/api/MisBatch/12345/fail \
  -H "Authorization: Bearer <token>"
```

### Minimal API (v2)

| Method | Route | Description |
|---|---|---|
| POST | `/api/v2/canteen-transactions/` | Record transaction |
| GET | `/api/v2/canteen-transactions/{serialNumber}` | Get by serial number |
| GET | `/api/v2/canteen-transactions/employee/{employeeSysId}?fromDate=&toDate=` | By employee |
| DELETE | `/api/v2/canteen-transactions/{serialNumber}` | Cancel transaction |
| POST | `/api/v2/daily-availed/` | Process daily availed |
| GET | `/api/v2/daily-availed/employee/{employeeSysId}?fromDate=&toDate=` | By employee |
| POST | `/api/v2/mis-batch/` | Submit MIS batch |
| GET | `/api/v2/mis-batch/pending` | Get pending batches |

```bash
curl -X POST http://localhost:5197/api/v2/canteen-transactions/ \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "companyCode": 100,
    "employeeSysId": 10001,
    "canteenUnit": 1001,
    "itemCode": 5,
    "quantity": 1,
    "swipeDate": "2025-01-15"
  }'

curl http://localhost:5197/api/v2/mis-batch/pending \
  -H "Authorization: Bearer <token>"
```

### GraphQL

```graphql
# Queries
query { transactionsByEmployee(employeeSysId: 10001, fromDate: "2025-01-01", toDate: "2025-01-31") { serialNumber companyCode employeeSysId canteenUnit itemCode quantity } }
query { transaction(serialNumber: 12345) { serialNumber companyCode employeeSysId } }
query { dailyAvailedByEmployee(employeeSysId: 10001, fromDate: "2025-01-01", toDate: "2025-01-31") { serialNumber itemCode quantity } }
query { pendingBatches { serialNumber batchNumber companyCode canteenUnit } }
query { dailySummary(companyCode: 100, swipeDate: "2025-01-15") { totalTransactions totalQuantity } }
```

```bash
curl -X POST http://localhost:5197/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "{ transactionsByEmployee(employeeSysId: 10001, fromDate: \"2025-01-01\", toDate: \"2025-01-31\") { serialNumber companyCode employeeSysId canteenUnit itemCode quantity } }"}'

curl -X POST http://localhost:5197/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "{ pendingBatches { serialNumber batchNumber companyCode canteenUnit } }"}'

# Mutations
curl -X POST http://localhost:5197/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { recordTransaction(input: { companyCode: 100, employeeSysId: 10001, canteenUnit: 1001, itemCode: 5, quantity: 1, swipeDate: \"2025-01-15\" }) { serialNumber companyCode } }"}'

curl -X POST http://localhost:5197/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { cancelTransaction(serialNumber: 12345) }"}'

curl -X POST http://localhost:5197/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{"query": "mutation { submitBatch(input: { batchNumber: 100, companyCode: 100, canteenUnit: 1001, batchDate: \"2025-01-15\" }) { serialNumber batchNumber } }"}'
```

---

## Ocelot Gateway Routing

All routes go through `http://localhost:5188`. JWT Bearer auth required for most routes. Rate limit: 100 req/min. Circuit breaker: 3 failures → 30s break.

| Gateway Route | Downstream | Port |
|---|---|---|
| `/api/canteen-unit/{everything}` | `/api/{everything}` | 5190 |
| `/api/canteen-unit/health` | `/health` | 5190 |
| `/api/canteen-unit/graphql` | `/graphql` | 5190 |
| `/api/canteen-unit/swagger/{everything}` | `/swagger/{everything}` | 5190 |
| `/api/card-management/{everything}` | `/api/{everything}` | 5191 |
| `/api/card-management/health` | `/health` | 5191 |
| `/api/card-management/graphql` | `/graphql` | 5191 |
| `/api/card-management/swagger/{everything}` | `/swagger/{everything}` | 5191 |
| `/api/deduction/{everything}` | `/api/{everything}` | 5192 |
| `/api/deduction/health` | `/health` | 5192 |
| `/api/deduction/graphql` | `/graphql` | 5192 |
| `/api/deduction/swagger/{everything}` | `/swagger/{everything}` | 5192 |
| `/api/eligibility/{everything}` | `/api/{everything}` | 5193 |
| `/api/eligibility/health` | `/health` | 5193 |
| `/api/eligibility/graphql` | `/graphql` | 5193 |
| `/api/eligibility/swagger/{everything}` | `/swagger/{everything}` | 5193 |
| `/api/itemmaster/{everything}` | `/api/{everything}` | 5194 |
| `/api/itemmaster/health` | `/health` | 5194 |
| `/api/itemmaster/graphql` | `/graphql` | 5194 |
| `/api/itemmaster/swagger/{everything}` | `/swagger/{everything}` | 5194 |
| `/api/referencedata/{everything}` | `/api/{everything}` | 5195 |
| `/api/referencedata/health` | `/health` | 5195 |
| `/api/referencedata/graphql` | `/graphql` | 5195 |
| `/api/referencedata/swagger/{everything}` | `/swagger/{everything}` | 5195 |
| `/api/swipe-transaction/{everything}` | `/api/{everything}` | 5196 |
| `/api/swipe-transaction/health` | `/health` | 5196 |
| `/api/swipe-transaction/graphql` | `/graphql` | 5196 |
| `/api/swipe-transaction/swagger/{everything}` | `/swagger/{everything}` | 5196 |
| `/api/canteen-transaction/{everything}` | `/api/{everything}` | 5197 |
| `/api/canteen-transaction/health` | `/health` | 5197 |
| `/api/canteen-transaction/graphql` | `/graphql` | 5197 |
| `/api/canteen-transaction/swagger/{everything}` | `/swagger/{everything}` | 5197 |

### Via Gateway cURL Examples

```bash
# Get a gateway token first
TOKEN=$(curl -s -X POST http://localhost:5188/api/auth/token \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}' | jq -r '.token')

# CanteenUnit via gateway
curl http://localhost:5188/api/canteen-unit/CanteenUnits \
  -H "Authorization: Bearer $TOKEN"

# CardManagement via gateway
curl "http://localhost:5188/api/card-management/GuestCards?pageNumber=1&pageSize=10" \
  -H "Authorization: Bearer $TOKEN"

# Deduction via gateway
curl http://localhost:5188/api/deduction/Deduction/employee/10001 \
  -H "Authorization: Bearer $TOKEN"

# ItemMaster via gateway
curl http://localhost:5188/api/itemmaster/CanteenItemMaster/1001 \
  -H "Authorization: Bearer $TOKEN"

# SwipeTransaction via gateway
curl http://localhost:5188/api/swipe-transaction/SwipeTransactions/pending \
  -H "Authorization: Bearer $TOKEN"

# CanteenTransaction via gateway
curl http://localhost:5188/api/canteen-transaction/CanteenTransaction/12345 \
  -H "Authorization: Bearer $TOKEN"

# GraphQL via gateway
curl -X POST http://localhost:5188/api/canteen-unit/graphql \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{"query": "{ canteenUnits { unComCod unitName } }"}'

# Health via gateway
curl http://localhost:5188/api/canteen-unit/health
```

---

## Docker Compose Setup

```bash
cd src/Services/canteenServices
docker-compose -f docker-compose.shared.yml -f docker-compose.yml up -d
```

### Infrastructure Services

| Service | Port | Description |
|---|---|---|
| SQL Server 2022 | 1434 | Shared database (7 databases) |
| RabbitMQ | 5673 / 15673 | Message broker / management UI |
| Azurite | 10010-10012 | Azure Storage emulator |

### Application Services

| Service | Port |
|---|---|
| API Gateway | 5188 |
| CanteenUnit | 5190 |
| CardManagement | 5191 |
| Deduction | 5192 |
| Eligibility | 5193 |
| ItemMaster | 5194 |
| ReferenceData | 5195 |
| SwipeTransaction | 5196 |
| CanteenTransaction | 5197 |
