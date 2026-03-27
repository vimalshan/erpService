# Canteen Services API Documentation

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

