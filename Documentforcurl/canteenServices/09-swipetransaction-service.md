# Canteen Services API Documentation

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

