# LoanManagement Module

## Module Overview
The LoanManagement module handles loan origination, disbursement schedules, interest rate management, and repayment tracking for various types of loans.

## Tables

### LOAN_MAIN
**Purpose:** Master loan records

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| LOAN_ID | DECIMAL(38) | NO | Primary Key |
| LOAN_KEY | VARCHAR(15) | NO | Unique loan identifier |
| LOAN_ORGID | DECIMAL(38) | NO | Organization ID |
| LOAN_ORGCURR | DECIMAL(38) | YES | Organization currency |
| LOAN_CURR | DECIMAL(38) | YES | Loan currency |
| LOAN_DATE | DATETIME2(3) | NO | Loan origination date |
| LOAN_TYPEID | DECIMAL(38) | NO | Loan type |
| LOAN_BANKID | DECIMAL(38) | NO | Lending bank ID |
| LOAN_CREATEDBY | DECIMAL(38) | NO | Created by user |
| LOAN_CREATEDON | DATETIME2(3) | NO | Created timestamp |
| LOAN_MODIFIEDBY | DECIMAL(38) | YES | Modified by user |
| LOAN_MODIFIEDON | DATETIME2(3) | YES | Modified timestamp |
| LOAN_AMOUNT | DECIMAL(38) | NO | Principal amount |
| LOAN_STATUS | CHAR(1) | YES | A=Active, C=Closed, D=Defaulted |

**Primary Key:** PK_LOAN_MAIN (LOAN_ID)

**Indexes:**
- IX_LOAN_MAIN_ORGID
- IX_LOAN_MAIN_DATE

**Sample Data:**
```sql
INSERT INTO LOAN_MAIN VALUES 
(1, 'L2026-001', 100, 1, 1, GETDATE(), 1, 10, 1, GETDATE(), 
 NULL, NULL, 5000000, 'A');  -- 50 Lakhs loan
```

---

### LOAN_DISBSCH
**Purpose:** Loan disbursement schedule

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| DISB_ID | BIGINT | NO | Primary Key |
| DISB_LOANID | BIGINT | YES | Loan ID (FK to LOAN_MAIN) |
| DISB_DATE | DATETIME2(3) | YES | Disbursement date |
| DISB_AMOUNT | DECIMAL(19,0) | YES | Disbursed amount |
| DISB_EXCRATE | DECIMAL(19,0) | YES | Exchange rate (if applicable) |
| DISB_EXCAMT | DECIMAL(19,0) | YES | Amount in home currency |
| DISB_MODIFIEDBY | BIGINT | YES | Modified by |
| DISB_MODIFIEDON | DATETIME2(3) | YES | Modified timestamp |

**Primary Key:** PK_LOAN_DISBSCH (DISB_ID)

**Foreign Keys:**
- FK_LOAN_DISBSCH_MAIN → LOAN_MAIN(LOAN_ID)

**Indexes:**
- IX_LOAN_DISBSCH_LOANID
- IX_LOAN_DISBSCH_DATE

**Sample Data:**
```sql
-- Disbursement in 3 tranches
INSERT INTO LOAN_DISBSCH VALUES 
(1, 1, '2026-01-15', 1500000, NULL, 1500000, 1, GETDATE()),
(2, 1, '2026-02-15', 1750000, NULL, 1750000, 1, GETDATE()),
(3, 1, '2026-03-15', 1750000, NULL, 1750000, 1, GETDATE());
```

---

### LOAN_INTEREST
**Purpose:** Interest rate configuration for loans

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| INT_ID | BIGINT | NO | Primary Key |
| INT_LOANID | BIGINT | YES | Loan ID (FK to LOAN_MAIN) |
| INT_RATETYPE | CHAR(2) | YES | FX=Fixed, FL=Floating |
| INT_PER | DECIMAL(19,0) | YES | Interest rate percentage |
| INT_FLOATTYPEID | BIGINT | YES | Floating rate type (LIBOR, MIBOR, etc.) |
| INT_EFFDATE | DATETIME2(3) | YES | Effective date |
| INT_CLSDATE | DATETIME2(3) | YES | Closure/change date |

**Primary Key:** PK_LOAN_INTEREST (INT_ID)

**Foreign Keys:**
- FK_LOAN_INTEREST_MAIN → LOAN_MAIN(LOAN_ID)

**Indexes:**
- IX_LOAN_INTEREST_LOANID

**Sample Data:**
```sql
-- Fixed rate of 8.5% p.a.
INSERT INTO LOAN_INTEREST VALUES 
(1, 1, 'FX', 850, NULL, GETDATE(), NULL);

-- Floating rate: MIBOR + 1%
INSERT INTO LOAN_INTEREST VALUES 
(2, 1, 'FL', 100, 1, '2026-04-15', NULL);
```

---

### LOAN_REPAYSCH
**Purpose:** Loan repayment schedule

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| REPAY_ID | BIGINT | NO | Primary Key |
| REPAY_LOANID | BIGINT | YES | Loan ID (FK to LOAN_MAIN) |
| REPAY_DATE | DATETIME2(3) | YES | Due repayment date |
| REPAY_AMT | DECIMAL(19,0) | YES | Repayment amount |
| REPAY_FLAG | CHAR(1) | YES | O=Original, A=Amended |
| REPAY_MODIFIEDON | DATETIME2(3) | YES | Last modified |
| REPAY_MODIFIEDBY | BIGINT | YES | Modified by |

**Primary Key:** PK_LOAN_REPAYSCH (REPAY_ID)

**Foreign Keys:**
- FK_LOAN_REPAYSCH_MAIN → LOAN_MAIN(LOAN_ID)

**Indexes:**
- IX_LOAN_REPAYSCH_LOANID
- IX_LOAN_REPAYSCH_DATE

**Sample Data:**
```sql
-- EMI of 100,000 for 50 months
INSERT INTO LOAN_REPAYSCH 
SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS REPAY_ID,
       1 AS REPAY_LOANID,
       DATEADD(MONTH, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)), '2026-01-31'),
       100000 AS REPAY_AMT,
       'O' AS REPAY_FLAG,
       GETDATE(),
       1
FROM master..spt_values  -- Helper table for generating rows
WHERE type = 'p' AND number BETWEEN 1 AND 50;
```

---

## Relationship Diagram

```
LOAN_MAIN (1)
    ├─ LOAN_DISBSCH (N)    ← Disbursement tranches
    ├─ LOAN_INTEREST (N)   ← Interest configurations
    └─ LOAN_REPAYSCH (N)   ← Repayment schedule
```

---

## Loan Processing Workflow

```
1. CREATE LOAN
   └─ LOAN_MAIN record with status 'A'

2. SCHEDULE DISBURSEMENTS
   ├─ DISB_DATE
   ├─ DISB_AMOUNT (can be partial)
   └─ Support for exchange rates

3. CONFIGURE INTEREST
   ├─ Fixed or Floating rate
   ├─ Rate type + percentage
   └─ Effective date tracking

4. SCHEDULE REPAYMENTS
   ├─ EMI schedule
   ├─ Support for amendments
   └─ Track modifications

5. MONITOR STATUS
   ├─ Original vs Amended schedules
   ├─ Disbursement tracking
   └─ Interest accrual
```

---

## Usage Examples

### Create New Loan
```sql
INSERT INTO LOAN_MAIN (LOAN_ID, LOAN_KEY, LOAN_ORGID, LOAN_ORGCURR, 
    LOAN_CURR, LOAN_DATE, LOAN_TYPEID, LOAN_BANKID, LOAN_CREATEDBY, 
    LOAN_CREATEDON, LOAN_AMOUNT, LOAN_STATUS)
VALUES (
    1, 'L2026-001-CORP', 100, 1, 1, GETDATE(), 1, 10, 1, GETDATE(),
    50000000, 'A'
);
```

### Schedule Disbursements
```sql
INSERT INTO LOAN_DISBSCH (DISB_ID, DISB_LOANID, DISB_DATE, DISB_AMOUNT, 
    DISB_MODIFIEDBY, DISB_MODIFIEDON)
VALUES
(1, 1, DATEADD(DAY, 15, GETDATE()), 10000000, 1, GETDATE()),
(2, 1, DATEADD(DAY, 45, GETDATE()), 20000000, 1, GETDATE()),
(3, 1, DATEADD(DAY, 75, GETDATE()), 20000000, 1, GETDATE());
```

### Set Interest Rate
```sql
INSERT INTO LOAN_INTEREST (INT_ID, INT_LOANID, INT_RATETYPE, INT_PER, INT_EFFDATE)
VALUES (1, 1, 'FX', 850, GETDATE());  -- 8.5% p.a.
```

### Create Repayment Schedule
```sql
INSERT INTO LOAN_REPAYSCH (REPAY_ID, REPAY_LOANID, REPAY_DATE, REPAY_AMT, REPAY_FLAG)
VALUES
(1, 1, '2026-02-28', 925000, 'O'),  -- EMI includes principal + interest
(2, 1, '2026-03-31', 925000, 'O'),
(3, 1, '2026-04-30', 925000, 'O'),
...
(54, 1, '2030-03-31', 925000, 'O');  -- 54 months
```

---

## Reporting Queries

### Loan Summary Report
```sql
SELECT 
    lm.LOAN_KEY,
    lm.LOAN_AMOUNT,
    ISNULL(SUM(ld.DISB_AMOUNT), 0) AS TotalDisbursed,
    lm.LOAN_AMOUNT - ISNULL(SUM(ld.DISB_AMOUNT), 0) AS PendingDisbursal,
    li.INT_PER AS InterestRate,
    lm.LOAN_STATUS
FROM LOAN_MAIN lm
LEFT JOIN LOAN_DISBSCH ld ON lm.LOAN_ID = ld.DISB_LOANID
LEFT JOIN LOAN_INTEREST li ON lm.LOAN_ID = li.INT_LOANID
GROUP BY lm.LOAN_ID, lm.LOAN_KEY, lm.LOAN_AMOUNT, lm.LOAN_STATUS, li.INT_PER;
```

### Upcoming Repayments
```sql
SELECT 
    lm.LOAN_KEY,
    lr.REPAY_DATE,
    lr.REPAY_AMT,
    DATEDIFF(DAY, GETDATE(), lr.REPAY_DATE) AS DaysUntilDue
FROM LOAN_REPAYSCH lr
JOIN LOAN_MAIN lm ON lr.REPAY_LOANID = lm.LOAN_ID
WHERE lr.REPAY_DATE BETWEEN GETDATE() AND DATEADD(MONTH, 3, GETDATE())
  AND lr.REPAY_FLAG = 'O'
ORDER BY lr.REPAY_DATE ASC;
```

### Interest Accrual Calculation
```sql
SELECT 
    lm.LOAN_KEY,
    lm.LOAN_AMOUNT,
    li.INT_PER,
    DATEDIFF(DAY, lm.LOAN_DATE, GETDATE()) AS DaysOutstanding,
    (lm.LOAN_AMOUNT * li.INT_PER / 36500 * 
     DATEDIFF(DAY, lm.LOAN_DATE, GETDATE())) AS InterestAccrued
FROM LOAN_MAIN lm
JOIN LOAN_INTEREST li ON lm.LOAN_ID = li.INT_LOANID
WHERE lm.LOAN_STATUS = 'A';
```

---

## Integration with Other Modules

### Currency Integration:
- LOAN_ORGCURR → DEAL_CURRMAST (Organization base currency)
- LOAN_CURR → DEAL_CURRMAST (Loan currency)
- Multi-currency loans supported

### Organization Integration:
- LOAN_ORGID → DEAL_ORGPARAMS (Organization reference)
- Budget tracking through organization parameters

### Deal Ticketing Integration:
- Loan instruments can be derivative deals (DEALTICKET_DET.DEAL_IRLOAN)
- Interest rate swaps use loan module data

---

## Key Design Features

### 1. **Flexible Disbursement**
- Supports partial disbursements
- Tracks exchange rates for currency conversion
- Multiple tranches possible

### 2. **Interest Rate Management**
- Fixed rate support
- Floating rate with multiple bases (LIBOR, MIBOR, etc.)
- Rate change tracking with effective dates

### 3. **Schedule Amendments**
- REPAY_FLAG tracks original vs amended schedules
- Complete modification history maintained
- Supports one-time or recurring amendments

### 4. **Multi-Currency Support**
- Loan in different currency than organization
- Exchange rates tracked per disbursement
- Currency conversion at account level

---

## Validation Rules

| Check | Rule | Error |
|-------|------|-------|
| Loan Amount | > 0 | "Invalid loan amount" |
| Disbursement Date | >= LOAN_DATE | "Invalid disbursement date" |
| Disbursement Amount | > 0 and <= remaining | "Invalid disbursement amount" |
| Interest Rate | >= 0 and <= 50 | "Invalid interest rate" |
| Repayment Date | After last disbursement | "Invalid repayment date" |
| Original/Amended | Flag must be 'O' or 'A' | "Invalid repayment flag" |

---

## Performance Tips

1. Index searches on LOAN_ORGID for organization-level reports
2. Archive closed loans (status = 'C') after 7 years
3. Materialized view for loan summaries
4. Batch interest calculations monthly
5. Use partitioning for large repayment schedules

---

## Best Practices

1. **Schedule All Items First:** Create full schedules before disbursement
2. **Lock Rates:** Use INT_CLSDATE to prevent accidental changes
3. **Audit Trail:** Track all modifications in REPAY_MODIFIEDON
4. **Backup:** Regular backups before bulk amendments
5. **Reconciliation:** Monthly reconciliation of disbursed vs scheduled

---

**Version:** 1.0
**Last Updated:** March 9, 2026
