# CurrencyManagement Module

## Module Overview
The CurrencyManagement module handles currency master data and exchange rate management across the CASHDB system.

## Tables

### DEAL_CURRMAST
**Purpose:** Currency master records

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| CURR_ID | BIGINT | NO | Primary Key - Currency ID |
| CURR_NAME | VARCHAR(255) | NO | Currency full name (e.g., US Dollar) |
| CURR_SYMBOL | VARCHAR(25) | NO | Currency symbol (e.g., $, €, £) |
| CURR_MODIFIEDBY | BIGINT | NO | User ID who modified |
| CURR_MODIFIEDON | DATETIME2(3) | NO | Timestamp of modification |

**Primary Key:** PK_DEAL_CURRMAST (CURR_ID)

**Sample Data:**
```sql
INSERT INTO DEAL_CURRMAST VALUES 
(1, 'US Dollar', '$', 1, GETDATE()),
(2, 'Euro', '€', 1, GETDATE()),
(3, 'British Pound', '£', 1, GETDATE()),
(4, 'Indian Rupee', '₹', 1, GETDATE());
```

---

### DEAL_CURRATES
**Purpose:** Exchange rates for currency pairs by financial year and month

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| CURRATE_ID | BIGINT | NO | Primary Key - Rate ID |
| CURRATE_FINYEAR | BIGINT | NO | Financial Year (e.g., 2026) |
| CURRATE_MONTH | BIGINT | NO | Month number (1-12) |
| CURRATE_FROMCUR | BIGINT | NO | From Currency ID (FK to DEAL_CURRMAST) |
| CURRATE_TOCUR | BIGINT | NO | To Currency ID (FK to DEAL_CURRMAST) |
| CURRATE_RATE | DECIMAL(19,0) | NO | Exchange rate value |
| CURRATE_MODIFIEDBY | DECIMAL(38) | NO | User ID who modified |
| CURRATE_MODIFIEDON | DATETIME2(3) | NO | Timestamp of modification |

**Primary Key:** PK_DEAL_CURRATES (CURRATE_ID)

**Indexes:**
- IX_DEAL_CURRATES_FINYEAR_MONTH - For financial year/month lookups
- IX_DEAL_CURRATES_FROMCUR_TOCUR - For currency pair lookups

**Sample Data:**
```sql
INSERT INTO DEAL_CURRATES VALUES 
(1, 2026, 1, 2, 1, 1175000, 1, GETDATE()),  -- EUR to USD in Jan 2026
(2, 2026, 2, 2, 1, 1180000, 1, GETDATE());  -- EUR to USD in Feb 2026
```

---

### DEAL_ORGCURRMAP
**Purpose:** Maps organizations to their operating currencies

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| ORG_ID | BIGINT | NO | Organization ID |
| ORG_CURRID | BIGINT | NO | Currency ID (FK to DEAL_CURRMAST) |
| ORG_MODIFIEDBY | DECIMAL(38) | NO | User ID who modified |
| ORG_MODIFIEDON | DATETIME2(3) | NO | Timestamp of modification |

**Primary Key:** None (Composite key recommended: ORG_ID, ORG_CURRID)

**Indexes:**
- IX_DEAL_ORGCURRMAP_ORG_ID - For organization lookups

**Foreign Keys:**
- FK_DEAL_ORGCURRMAP_CURRMAST → DEAL_CURRMAST(CURR_ID)

**Sample Data:**
```sql
INSERT INTO DEAL_ORGCURRMAP VALUES 
(100, 1, 1, GETDATE()),  -- Org 100 uses USD
(100, 2, 1, GETDATE()),  -- Org 100 also uses EUR
(101, 4, 1, GETDATE());  -- Org 101 uses INR
```

---

## Usage Scenarios

### Scenario 1: Get Current Exchange Rate
```sql
SELECT TOP 1 
    CURRATE_RATE,
    CURRATE_FINYEAR,
    CURRATE_MONTH
FROM DEAL_CURRATES
WHERE CURRATE_FROMCUR = 2  -- EUR
  AND CURRATE_TOCUR = 1    -- USD
  AND CURRATE_FINYEAR = 2026
  AND CURRATE_MONTH = 3
ORDER BY CURRATE_MONTH DESC;
```

### Scenario 2: Get Organization's Base Currency
```sql
SELECT 
    c.CURR_ID,
    c.CURR_NAME,
    c.CURR_SYMBOL
FROM DEAL_ORGCURRMAP m
JOIN DEAL_CURRMAST c ON m.ORG_CURRID = c.CURR_ID
WHERE m.ORG_ID = 100;
```

### Scenario 3: Convert Amount Between Currencies
```sql
DECLARE @Amount DECIMAL(19,0) = 1000;
DECLARE @FromCurr BIGINT = 2;  -- EUR
DECLARE @ToCurr BIGINT = 1;    -- USD

SELECT 
    @Amount AS OriginalAmount,
    cr.CURRATE_RATE AS ExchangeRate,
    (@Amount * cr.CURRATE_RATE / 1000000) AS ConvertedAmount
FROM DEAL_CURRATES cr
WHERE cr.CURRATE_FROMCUR = @FromCurr
  AND cr.CURRATE_TOCUR = @ToCurr
  AND cr.CURRATE_FINYEAR = 2026
  AND cr.CURRATE_MONTH = 3;
```

---

## Integration Points

### With DealTicketing Module:
- DEALTICKET_DET.DEAL_CURRENCY1 references currency for deals
- DEALTICKET_DET.DEAL_CURRENCY2 references counter currency for derivatives

### With LoanManagement Module:
- LOAN_MAIN.LOAN_ORGCURR references organization currency
- LOAN_MAIN.LOAN_CURR references loan currency

### Currency Conversion Requirements:
- All spot rates must be defined in DEAL_CURRATES
- Exchange rates should be updated monthly or when markets change significantly
- Organizations must be mapped to at least one base currency

---

## Data Maintenance

### Adding New Currency:
```sql
INSERT INTO DEAL_CURRMAST (CURR_ID, CURR_NAME, CURR_SYMBOL, CURR_MODIFIEDBY, CURR_MODIFIEDON)
VALUES (5, 'Japanese Yen', '¥', 1, GETDATE());
```

### Updating Exchange Rates:
```sql
UPDATE DEAL_CURRATES
SET CURRATE_RATE = 1185000,
    CURRATE_MODIFIEDBY = 1,
    CURRATE_MODIFIEDON = GETDATE()
WHERE CURRATE_FINYEAR = 2026
  AND CURRATE_MONTH = 3
  AND CURRATE_FROMCUR = 2
  AND CURRATE_TOCUR = 1;
```

### Adding Organization-Currency Mapping:
```sql
INSERT INTO DEAL_ORGCURRMAP (ORG_ID, ORG_CURRID, ORG_MODIFIEDBY, ORG_MODIFIEDON)
VALUES (102, 2, 1, GETDATE());
```

---

## Performance Considerations

1. **Index Usage:** DEAL_CURRATES queries leverage composite index (FINYEAR, MONTH)
2. **Archive Strategy:** Consider archiving old exchange rates (> 3 years) to archival storage
3. **Query Optimization:** Filter by FINYEAR and MONTH before FROMCUR/TOCUR for better index utilization
4. **Statistics:** Rebuild index statistics monthly: `UPDATE STATISTICS DEAL_CURRATES`

---

## Related Modules
- **DealTicketing:** Uses currencies for deal instruments
- **LoanManagement:** References currencies for multi-currency loans
- **OrganizationSetup:** Organizations mapped to currencies

---

**Version:** 1.0
**Last Updated:** March 9, 2026
