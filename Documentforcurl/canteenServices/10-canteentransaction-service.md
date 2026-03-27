# Canteen Services API Documentation

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

