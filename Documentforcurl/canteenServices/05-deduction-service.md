# Canteen Services API Documentation

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

