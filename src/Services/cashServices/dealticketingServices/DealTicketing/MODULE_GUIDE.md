# DealTicketing Module

## Module Overview
The DealTicketing module manages financial instrument deals, derivatives, and their processing lifecycle from booking to settlement.

## Tables Overview

```
DEALTICKET_BATCH (Master)
    ↓
DEALTICKET_DET (Details)
    ├→ DEALTICKET_LOANSCH (Loan Schedules)
    ├→ DEALTICKET_SET (Settlements)
    │   └→ DEATICKETSET_ATTACHMENT (Settlement Docs)
    └→ DEATICKET_ATTACHMENT (Deal Docs)
```

---

## Core Tables

### DEAL_BANKMASTER
**Purpose:** Bank master records for counter-parties

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| BANK_ID | BIGINT | NO | Primary Key |
| BANK_NAME | VARCHAR(50) | NO | Bank name |
| BANK_EFFDATE | DATETIME2(3) | NO | Effective date |
| BANK_CLSDATE | DATETIME2(3) | YES | Closure date |
| BANK_MODIFIEDBY | DECIMAL(38) | NO | Modified by user |
| BANK_MODIFIEDON | DATETIME2(3) | NO | Modified timestamp |

**Primary Key:** PK_DEAL_BANKMASTER (BANK_ID)

---

### DEAL_CATEGORYMASTER
**Purpose:** Deal type categories (FX, Derivatives, Swaps, etc.)

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| CATEGORY_ID | BIGINT | NO | Primary Key |
| CATEGORY_NAME | VARCHAR(50) | NO | Category name |
| CATEGORY_TYPE | CHAR(1) | NO | F=FX, D=Derivatives, S=Swaps, etc. |
| CATEGORY_MODIFIEDON | DATETIME2(3) | NO | Modified timestamp |
| CATEGORY_MODIFIEDBY | DECIMAL(38) | NO | Modified by user |

**Primary Key:** PK_DEAL_CATEGORYMASTER (CATEGORY_ID)

---

### DEAL_LOVMASTER
**Purpose:** List of Values for code lookups

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| LOV_ID | BIGINT | NO | Primary Key |
| LOV_TYPE | VARCHAR(10) | NO | Type (001=Deriv, 002=Nature, 003=Category, 004=Options, 005=FloatingBase) |
| LOV_NAME | VARCHAR(150) | NO | Value name |

**Primary Key:** PK_DEAL_LOVMASTER (LOV_ID)

---

### DEALTICKET_BATCH
**Purpose:** Batch header for deal processing

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| DEAL_BATCHID | BIGINT | NO | Primary Key |
| DEAL_DATE | DATETIME2(3) | NO | Deal date |
| DEAL_DERTYPE | BIGINT | NO | Derivative type (FK to DEAL_LOVMASTER) |
| DEAL_SCREENSHOT | VARCHAR(50) | YES | Reuters screenshot reference |
| DEAL_BOOKEDBY | BIGINT | YES | User who booked deal |
| DEAL_BANKTRADER | VARCHAR(50) | YES | Bank trader name |
| DEAL_BANKID | BIGINT | YES | Bank (FK to DEAL_BANKMASTER) |
| DEAL_OPTIONTYPE | BIGINT | YES | Option type |
| DEAL_BUSINESSID | DECIMAL(38) | NO | Business unit ID |
| DEAL_REJSTATUS | CHAR(1) | YES | Y/N - Rejection status |
| DEAL_REJREASON | VARCHAR(50) | YES | Reason for rejection |
| DEAL_ERRREMARKS | VARCHAR(50) | YES | Error remarks |
| DEAL_MODIFIEDBY | DECIMAL(38) | NO | Modified by |
| DEAL_MODIFIEDON | DATETIME2(3) | NO | Modified on |
| DEAL_UNITID | DECIMAL(38) | YES | Unit ID |

**Primary Key:** PK_DEALTICKET_BATCH (DEAL_BATCHID)

**Foreign Keys:**
- FK_DEALTICKET_BATCH_BANKMASTER

**Indexes:**
- IX_DEALTICKET_BATCH_DATE
- IX_DEALTICKET_BATCH_BANKID

---

### DEALTICKET_DET
**Purpose:** Individual deal transaction records

**Key Columns:**
| Column | Type | Purpose |
|--------|------|---------|
| DEAL_ID | BIGINT | Primary Key |
| DEAL_BATCHID | BIGINT | Link to batch (FK) |
| DEAL_TRANTYPE | CHAR(1) | B=Buy, S=Sell, P=Put, C=Call |
| DEAL_POSITION | CHAR(2) | BC/BP/SP/SC (for options) |
| DEAL_AMOUNT | DECIMAL(19,0) | Deal amount |
| DEAL_SPOTRATE | DECIMAL(19,0) | Spot rate |
| DEAL_FORPOINTS | DECIMAL(19,0) | Forward points |
| DEAL_BOOKRATE | DECIMAL(19,0) | Booked rate |
| DEAL_MATDATE | DATETIME2(3) | Maturity date |
| DEAL_APPSTATUS | CHAR(1) | Y/N/R/P - Approval status |
| DEAL_APPREMARKS | VARCHAR(200) | Approval remarks |
| DEAL_SETSTATUS | CHAR(1) | Live/Closed settlement status |

**Primary Key:** PK_DEALTICKET_DET (DEAL_ID)

**Foreign Keys:**
- FK_DEALTICKET_DET_BATCH
- FK_DEALTICKET_DET_BANKMASTER

---

### DEALTICKET_LOANSCH
**Purpose:** Loan disbursement/repayment schedules for loan instruments

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| DEAL_SCHID | BIGINT | NO | Primary Key |
| DEAL_ID | BIGINT | NO | Deal ID (FK to DEALTICKET_DET) |
| DEAL_SCHDATE | DATETIME2(3) | NO | Schedule date |
| DEAL_SCHAMT | BIGINT | NO | Schedule amount |

**Primary Key:** PK_DEALTICKET_LOANSCH (DEAL_SCHID)

**Foreign Keys:**
- FK_DEALTICKET_LOANSCH_DET

---

### DEALTICKET_SET
**Purpose:** Deal settlement records with gain/loss calculations

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| SET_ID | BIGINT | NO | Primary Key |
| SET_DEALID | BIGINT | NO | Deal ID (FK to DEALTICKET_DET) |
| SET_SPOTRATE | DECIMAL(19,0) | YES | Spot rate at settlement |
| SET_DATE | DATETIME2(3) | YES | Settlement date |
| SET_GAINLOSSAMT | DECIMAL(19,0) | NO | Gain/Loss amount |
| SET_TYPE | CHAR(3) | YES | U=Utilized, C=Cancelled, R=Rollover |
| SET_EXCHANGERATE | DECIMAL(19,0) | YES | Final exchange rate |
| SET_ACTGAINLOSSAMT | DECIMAL(19,0) | YES | Actual gain/loss |

**Primary Key:** PK_DEALTICKET_SET (SET_ID)

**Foreign Keys:**
- FK_DEALTICKET_SET_DET

---

### Attachment Tables

#### DEATICKET_ATTACHMENT
Stores deal documentation

#### DEATICKETSET_ATTACHMENT
Stores settlement documentation

---

## Deal Processing Workflow

```
1. BATCH CREATION
   ├─ Create DEALTICKET_BATCH record
   ├─ Set DEAL_DATE, DEAL_DERTYPE, DEAL_BANKID
   └─ Initial DEAL_MODIFIEDBY, DEAL_MODIFIEDON

2. DEAL ENTRY
   ├─ Create DEALTICKET_DET records
   ├─ Set transaction type, amount, rates
   ├─ Attach documents to DEATICKET_ATTACHMENT
   └─ Set DEAL_APPSTATUS = 'P' (Pending)

3. VALIDATION & APPROVAL
   ├─ Verify deal parameters
   ├─ Check PP limits if applicable
   ├─ Update DEAL_APPSTATUS = 'Y' (Confirmed) or 'R' (Rejected)
   └─ Set DEAL_APPREMARKS if rejected

4. SETTLEMENT PROCESSING
   ├─ Create DEALTICKET_SET record
   ├─ Calculate gain/loss
   ├─ Attach settlement documents
   └─ Update DEAL_SETSTATUS

5. COMPLETION
   └─ Mark as 'L' (Live) or 'C' (Closed)
```

---

## Usage Examples

### Create Deal Batch
```sql
INSERT INTO DEALTICKET_BATCH (
    DEAL_BATCHID, DEAL_DATE, DEAL_DERTYPE, DEAL_BANKID,
    DEAL_BOOKEDBY, DEAL_BUSINESSID, DEAL_MODIFIEDBY, DEAL_MODIFIEDON
) VALUES (
    1, GETDATE(), 1, 100,  -- FX derivatives with Bank 100
    1001, 50, 1, GETDATE()
);
```

### Create Deal Detail
```sql
INSERT INTO DEALTICKET_DET (
    DEAL_ID, DEAL_NO, DEAL_VERSIONID, DEAL_BATCHID,
    DEAL_TRANTYPE, DEAL_AMOUNT, DEAL_CURRENCY1, DEAL_CURRENCY2,
    DEAL_SPOTRATE, DEAL_BOOKRATE, DEAL_MATDATE, DEAL_APPSTATUS,
    DEAL_MODIFIEDBY, DEAL_MODIFIEDON
) VALUES (
    101, 1, 1, 1,
    'B', 1000000, 2, 1,  -- Buy EUR 1,000,000 for USD
    '1175000', '1180000', DATEADD(DAY, 30, GETDATE()), 'P',
    1, GETDATE()
);
```

### Record Settlement
```sql
INSERT INTO DEALTICKET_SET (
    SET_ID, SET_DEALID, SET_DATE, SET_SPOTRATE,
    SET_EXCHANGERATE, SET_GAINLOSSAMT, SET_TYPE, SET_MODIFIEDBY, SET_MODIFIEDON
) VALUES (
    1, 101, GETDATE(), '1190000',  -- EUR appreciated to 1.19
    '1190000', 5000, 'U', 1, GETDATE()  -- 1000 EUR gain
);
```

---

## Reporting Queries

### Daily Deal Summary
```sql
SELECT 
    db.DEAL_DATE,
    bm.BANK_NAME,
    COUNT(*) AS DealCount,
    SUM(dd.DEAL_AMOUNT) AS TotalAmount,
    COUNT(CASE WHEN dd.DEAL_APPSTATUS = 'Y' THEN 1 END) AS ConfirmedDeals
FROM DEALTICKET_BATCH db
JOIN DEAL_BANKMASTER bm ON db.DEAL_BANKID = bm.BANK_ID
JOIN DEALTICKET_DET dd ON db.DEAL_BATCHID = dd.DEAL_BATCHID
WHERE db.DEAL_DATE >= DATEADD(DAY, -7, CAST(GETDATE() AS DATE))
GROUP BY db.DEAL_DATE, bm.BANK_NAME
ORDER BY db.DEAL_DATE DESC;
```

### Pending Deal Approvals
```sql
SELECT 
    dd.DEAL_ID,
    db.DEAL_DATE,
    dd.DEAL_AMOUNT,
    dd.DEAL_TRANTYPE,
    dd.DEAL_APPSTATUS,
    DATEDIFF(DAY, db.DEAL_DATE, GETDATE()) AS DaysOld
FROM DEALTICKET_DET dd
JOIN DEALTICKET_BATCH db ON dd.DEAL_BATCHID = db.DEAL_BATCHID
WHERE dd.DEAL_APPSTATUS = 'P'  -- Pending
ORDER BY db.DEAL_DATE ASC;
```

### Settlement & P&L Report
```sql
SELECT 
    dd.DEAL_ID,
    dd.DEAL_AMOUNT,
    ds.SET_DATE,
    ds.SET_GAINLOSSAMT,
    ds.SET_ACTGAINLOSSAMT,
    CASE 
        WHEN ds.SET_GAINLOSSAMT > 0 THEN 'Profit'
        WHEN ds.SET_GAINLOSSAMT < 0 THEN 'Loss'
        ELSE 'Break-even'
    END AS ResultType
FROM DEALTICKET_DET dd
JOIN DEALTICKET_SET ds ON dd.DEAL_ID = ds.SET_DEALID
WHERE ds.SET_DATE >= DATEADD(MONTH, -3, GETDATE())
ORDER BY ds.SET_DATE DESC;
```

---

## Integration with Other Modules

### Currency Integration:
- DEAL_CURRENCY1, DEAL_CURRENCY2 → DEAL_CURRMAST(CURR_ID)
- DEAL_SPOTRATE, DEAL_BOOKRATE use rates from DEAL_CURRATES

### Organization Integration:
- DEAL_BUSINESSID → Part of OrganizationSetup
- DEAL_PPLMITOUT → DEAL_PPLIMIT tracking

### Loan Integration:
- Loan instruments use DEALTICKET_LOANSCH for schedules
- Additional fields for interest rate swaps (DEAL_IRTYPE, DEAL_TOPAY, DEAL_TOREC)

---

## Error Scenarios & Handling

| Scenario | Check | Error Message |
|----------|-------|---------------|
| Duplicate Deal | DEAL_NO, DEAL_VERSIONID, DEAL_BATCHID | "Duplicate Deal Found" |
| Invalid Currency | DEAL_CURRENCY1, DEAL_CURRENCY2 | "Invalid Currency Code" |
| Past Maturity Date | DEAL_MATDATE > GETDATE() | "Invalid Maturity Date" |
| Negative Amount | DEAL_AMOUNT > 0 | "Invalid Deal Amount" |
| Approval Without Status | DEAL_APPSTATUS IN('Y','N','R','P') | "Invalid Approval Status" |

---

## Performance Tips

1. Always filter by DEAL_DATE or DEAL_BATCHID when searching deals
2. Use pagination for large result sets
3. Archive settlements older than 2 years
4. Index searches on DEAL_APPSTATUS for pending deal reports
5. Consider materialized views for daily P&L reports

---

**Version:** 1.0
**Last Updated:** March 9, 2026
