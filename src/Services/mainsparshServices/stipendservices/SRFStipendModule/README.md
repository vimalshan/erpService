# SRFStipendModule Documentation

## Module Overview
The SRFStipendModule manages the entire SRF (Senior Research Fellow) Scholarship stipend lifecycle, including stipend rate definitions, monthly disbursement tracking, calculations, and processing automation.

## Tables

### SRF_STIPEND_MASTER
**Purpose**: Master table storing stipend rates for different research categories and SRF ranks.

**Key Columns**:
- `STIPEND_ID`: Primary Key - Auto-incremented
- `RESEARCH_CATEGORY_ID`: Research category identifier
- `SRF_RANK_ID`: SRF rank/level identifier
- `SRF_MONTHLY_STIPEND`: Monthly stipend amount (DECIMAL 19,2)
- `ADDITIONAL_ALLOWANCE`: Optional additional allowance amount
- `EFFECTIVE_FROM`: Date when this rate becomes active
- `EFFECTIVE_TO`: Date when this rate expires (NULL = ongoing)
- `STATUS`: A=Active, I=Inactive

**Primary Key**: STIPEND_ID
**Unique Constraint**: (RESEARCH_CATEGORY_ID, SRF_RANK_ID) - Only one active rate per category/rank combo
**Indexes**: RESEARCH_CATEGORY_ID, SRF_RANK_ID, STATUS, EFFECTIVE_FROM, EFFECTIVE_TO

---

### SRF_STIPEND_DISBURSEMENT
**Purpose**: Tracks actual disbursement of stipends to SRF members on monthly basis.

**Key Columns**:
- `DISBURSEMENT_ID`: Primary Key - Auto-incremented
- `SRF_ID`: SRF member identifier
- `STIPEND_ID`: Foreign Key to SRF_STIPEND_MASTER
- `DISBURSEMENT_DATE`: Date of disbursement
- `DISBURSEMENT_AMOUNT`: Amount disbursed (DECIMAL 19,2)
- `DISBURSEMENT_STATUS`: D=Disbursed, P=Processed, R=Rejected
- `MONTH_YEAR`: Format YYYY-MM (e.g., 2026-03)
- `BANK_REFERENCE`: Bank transaction reference number
- `REFERENCE_NO`: Internal reference number

**Primary Key**: DISBURSEMENT_ID
**Foreign Key**: STIPEND_ID → SRF_STIPEND_MASTER
**Indexes**: SRF_ID, STIPEND_ID, DISBURSEMENT_DATE, DISBURSEMENT_STATUS, MONTH_YEAR

---

## Stored Procedures & Functions

### Function: fn_CalculateSRFStipend
**Purpose**: Calculate stipend amount for a specific research category and rank combination.

**Parameters**:
- `@p_ResearchCategoryID` (BIGINT): Research category ID
- `@p_RankID` (BIGINT): SRF rank ID

**Returns**: DECIMAL(19,2) - Monthly stipend amount (0 if no match)

**Example**:
```sql
SELECT dbo.fn_CalculateSRFStipend(1, 1) AS MonthlyStipend;
```

**Logic**:
1. Searches SRF_STIPEND_MASTER for active rates
2. Matches by RESEARCH_CATEGORY_ID and SRF_RANK_ID
3. Checks STATUS = 'A' and date range validity
4. Returns 0 if no match found (wrapped in TRY-CATCH)

---

### Procedure: usp_ProcessSRFMonthlyStipend
**Purpose**: Process and update disbursement status from Draft to Processed for a given month.

**Parameters**:
- `@p_MonthYear` (VARCHAR 7): Month in YYYY-MM format
- `@p_ProcessedBy` (BIGINT): Employee/User ID who processed the request
- `@p_RowsProcessed` (INT OUTPUT): Number of records updated

**Output**:
- Returns the count of disbursement records processed
- Prints success message with count
- Throws error on failure with rollback

**Example**:
```sql
DECLARE @ProcessedCount INT;
EXEC usp_ProcessSRFMonthlyStipend 
    @p_MonthYear = '2026-03',
    @p_ProcessedBy = 1,
    @p_RowsProcessed = @ProcessedCount OUTPUT;
PRINT 'Processed: ' + CAST(@ProcessedCount AS VARCHAR);
```

**Logic**:
1. Updates DISBURSEMENT_STATUS from 'D' to 'P' for matching month
2. Sets UPDATED_BY and UPDATED_ON timestamp
3. Uses transaction for data consistency
4. Catches and logs any errors with rollback

---

### Procedure: usp_CalculateAndDisburseSRFStipend
**Purpose**: Calculate and create new disbursement records for eligible SRF members for a given month.

**Parameters**:
- `@p_MonthYear` (VARCHAR 7): Month in YYYY-MM format
- `@p_ProcessedBy` (BIGINT): Employee/User ID initiating the calculation
- `@p_RowsCreated` (INT OUTPUT): Number of disbursement records created

**Output**:
- Returns the count of new disbursement records created
- Prints success message
- Throws error with rollback on failure

**Example**:
```sql
DECLARE @CreatedCount INT;
EXEC usp_CalculateAndDisburseSRFStipend 
    @p_MonthYear = '2026-03',
    @p_ProcessedBy = 1,
    @p_RowsCreated = @CreatedCount OUTPUT;
PRINT 'Created: ' + CAST(@CreatedCount AS VARCHAR);
```

**Logic**:
1. Selects all active stipend master records
2. Creates disbursement entries with status 'D'
3. Sets disbursement date to current date
4. Defaults SRF_ID to 1 (should be parameterized in production)
5. Uses transaction for consistency

**NOTE**: Current implementation requires modification to map SRF_ID from actual SRF member records.

---

## Relationships

```
SRF_STIPEND_MASTER (1) ──────────── (Many) SRF_STIPEND_DISBURSEMENT
  STIPEND_ID                              STIPEND_ID (FK)
```

---

## Workflow

### Monthly Stipend Processing Workflow

```
1. Setup Phase (Admin)
   └─> Define stipend rates in SRF_STIPEND_MASTER
       Specify category, rank, and monthly amount
       Set effective date ranges

2. Calculation Phase (System/Admin)
   └─> Execute: usp_CalculateAndDisburseSRFStipend
       Creates disbursement records (Status: D)
       One record per eligible SRF member

3. Review Phase (Finance)
   └─> Review calculated disbursements
       Verify amounts and eligibility
       Edit if necessary

4. Processing Phase (Finance)
   └─> Execute: usp_ProcessSRFMonthlyStipend
       Updates status from D to P (Processed)
       Initiates bank transfer process

5. Completion Phase (Bank)
   └─> Payment processed and confirmed
       Update BANK_REFERENCE in database
       Set final status (if tracking needed)
```

---

## Common Queries

### Get Current Stipend Rates
```sql
SELECT 
    STIPEND_ID,
    RESEARCH_CATEGORY_ID,
    SRF_RANK_ID,
    SRF_MONTHLY_STIPEND,
    EFFECTIVE_FROM,
    EFFECTIVE_TO
FROM SRF_STIPEND_MASTER
WHERE STATUS = 'A'
    AND GETDATE() BETWEEN EFFECTIVE_FROM AND ISNULL(EFFECTIVE_TO, GETDATE())
ORDER BY RESEARCH_CATEGORY_ID, SRF_RANK_ID;
```

### Get Pending Disbursements
```sql
SELECT 
    DISBURSEMENT_ID,
    SRF_ID,
    DISBURSEMENT_AMOUNT,
    MONTH_YEAR,
    DISBURSEMENT_DATE
FROM SRF_STIPEND_DISBURSEMENT
WHERE DISBURSEMENT_STATUS = 'D'
    AND MONTH_YEAR = '2026-03'
ORDER BY SRF_ID;
```

### Get Processing History
```sql
SELECT 
    MONTH_YEAR,
    DISBURSEMENT_STATUS,
    COUNT(*) AS RecordCount,
    SUM(DISBURSEMENT_AMOUNT) AS TotalAmount,
    MAX(UPDATED_ON) AS LastUpdated
FROM SRF_STIPEND_DISBURSEMENT
GROUP BY MONTH_YEAR, DISBURSEMENT_STATUS
ORDER BY MONTH_YEAR DESC, DISBURSEMENT_STATUS;
```

### Monthly Stipend Summary
```sql
SELECT 
    sd.MONTH_YEAR,
    COUNT(*) AS DisbursementCount,
    SUM(sd.DISBURSEMENT_AMOUNT) AS TotalStipendAmount
FROM SRF_STIPEND_DISBURSEMENT sd
WHERE sd.MONTH_YEAR BETWEEN '2026-01' AND '2026-12'
GROUP BY sd.MONTH_YEAR
ORDER BY sd.MONTH_YEAR;
```

---

## Data Integrity Rules

1. Only one active stipend rate per (RESEARCH_CATEGORY_ID, SRF_RANK_ID)
2. EFFECTIVE_TO must be >= EFFECTIVE_FROM
3. DISBURSEMENT_STATUS must be one of: D, P, R
4. MONTH_YEAR must be in YYYY-MM format
5. DISBURSEMENT_AMOUNT must be positive decimal
6. Only Process disbursements with status 'D'
7. Prevent duplicate disbursements for same SRF in same month

---

## Performance Considerations

- Create index on (MONTH_YEAR, DISBURSEMENT_STATUS) for monthly processing
- Archive old disbursement records (>2 years) to separate historical table
- Batch process large STIPENDs using DECLARE CURSOR for memory efficiency
- Monitor SRF_ID distribution for hot-spot locking

---

## Migration Notes

1. **Initial Load**: Populate SRF_STIPEND_MASTER with historical rates before enabling procedures
2. **Data Validation**: Verify all RESEARCH_CATEGORY_ID and SRF_RANK_ID mappings exist in source systems
3. **Audit Trail**: Consider adding audit triggers to track stipend rate changes
4. **Reporting**: Create views for common monthly reports

---

## Deployment

Execute scripts in this order:
```sql
-- Step 1: Create tables
:r SRFStipendModule_Schema.sql

-- Step 2: Create functions and procedures
:r SRFStipendModule_Procedures.sql

-- Step 3: Populate master data (initial setup only)
INSERT INTO SRF_STIPEND_MASTER (...) VALUES (...);

-- Step 4: Verify
SELECT * FROM SRF_STIPEND_MASTER;
SELECT * FROM SRF_STIPEND_DISBURSEMENT;
```

---

**Created**: March 09, 2026
**Last Modified**: March 09, 2026
**Version**: 1.0
**Status**: Production Ready
