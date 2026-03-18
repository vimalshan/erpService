# Mobile Expense Management Module (MOD_MobileExpenseManagement)

## Module Overview
The Mobile Expense Management module tracks employee expenses incurred during mobile operations (field visits, trips, projects). It manages expense records and associated file attachments (receipts, invoices, photos).

## Module Code: **EXP**

## Scope
- Mobile field expense tracking
- Trip/Project-based expense recording
- Expense categorization
- File attachment management (receipts, invoices, photos)
- Multi-currency support

## Tables

### 1. MOBEXP_DET
**Purpose:** Mobile expense details and transaction records

| Column | Data Type | Notes |
|--------|-----------|-------|
| MOBEXP_ID | DECIMAL(38) | Expense ID (PK) |
| MOBEXP_TPID | DECIMAL(38) | Trip/Project ID (FK) |
| MOBEXP_CATID | DECIMAL(38) | Expense Category ID (FK) |
| MOBEXP_DATE | DATETIME2(3) | Expense Date |
| MOBEXP_COMMENT | VARCHAR(500) | Expense Description/Comment |
| MOBEXP_AMOUNT | DECIMAL(19,2) | Expense Amount |
| MOBEXP_CURRID | DECIMAL(38) | Currency ID |
| MOBEXP_ENTEREDBY | DECIMAL(38) | Entered By (Employee ID) |
| MOBEXP_ENTEREDON | DATETIME2(3) | Entry Timestamp |

**Primary Key:** MOBEXP_ID
**Indexes:** IX_MOBEXP_TPID, IX_MOBEXP_CATID, IX_MOBEXP_DATE, IX_MOBEXP_ENTEREDBY

### 2. MOBEXP_FILE
**Purpose:** Mobile expense file attachments (photos, receipts, etc.)

| Column | Data Type | Notes |
|--------|-----------|-------|
| MOBEXPPHT_ID | DECIMAL(38) | File ID (PK) |
| MOBEXPPHT_EXPID | DECIMAL(38) | Expense ID (FK to MOBEXP_DET) |
| MOBEXPPHT_FILENAME | VARCHAR(500) | File Name |
| MOBEXPPHT_FILEDATA | NVARCHAR(MAX) | File Data (Base64 or path) |

**Primary Key:** MOBEXPPHT_ID
**Foreign Key:** MOBEXPPHT_EXPID -> MOBEXP_DET(MOBEXP_ID)
**Indexes:** IX_MOBEXP_FILE_EXPID

## Key Stored Procedures

### usp_EXP_RecordExpense
- **Purpose:** Record a new mobile expense
- **Parameters:** TripId, CategoryId, Comment, Amount, CurrencyId, EnteredBy
- **Returns:** ExpenseId, ErrorMessage

### usp_EXP_AttachExpenseFile
- **Purpose:** Attach a file to an expense record
- **Parameters:** ExpenseId, FileName, FileData
- **Returns:** FileId, ErrorMessage

### usp_EXP_GetExpensesByTrip
- **Purpose:** Retrieve all expenses for a trip
- **Parameters:** TripId
- **Returns:** Expense list

### usp_EXP_GetExpenseFiles
- **Purpose:** Retrieve files attached to an expense
- **Parameters:** ExpenseId
- **Returns:** File list

## Relationships
- **Trip/Project Master:** External reference (MOBEXP_TPID)
- **Expense Categories:** External reference (MOBEXP_CATID)
- **Currency Master:** External reference (MOBEXP_CURRID)
- **Employee Master:** External reference (MOBEXP_ENTEREDBY)

## Business Rules
1. Expenses must be recorded with a valid trip/project ID
2. Expense amount must be positive and non-zero
3. File attachments are mandatory for expenses above a threshold amount
4. Expense date cannot be future-dated
5. Category-based business rules for maximum expense amounts

## Approval Workflow
1. Employee records expense
2. Manager reviews and approves/rejects
3. Finance verifies against receipts
4. Payment processing

## Data Retention
- Approved expenses: Retain indefinitely
- Pending expenses: Archive after 90 days if expired
- File attachments: Retain based on company policy

## Security Considerations
- Validate file types and sizes before storage
- Encrypt sensitive file data
- Audit all expense modifications
- Implement expense amount limits per employee
- Regular file cleanup for orphaned records

## Usage Example
```sql
-- Record an expense
EXEC usp_EXP_RecordExpense
    @p_TripId = 5001,
    @p_CategoryId = 10,
    @p_Comment = 'Fuel for field visit to Site A',
    @p_Amount = 2500.00,
    @p_CurrencyId = 1,
    @p_EnteredBy = 1001,
    @p_ExpenseId = @ExpenseId OUTPUT,
    @p_ErrorMessage = @ErrorMsg OUTPUT;

-- Attach receipt file
EXEC usp_EXP_AttachExpenseFile
    @p_ExpenseId = @ExpenseId,
    @p_FileName = 'Fuel_receipt_20260309.pdf',
    @p_FileData = @FileBase64,
    @p_FileId = @FileId OUTPUT,
    @p_ErrorMessage = @ErrorMsg OUTPUT;
```

## Related Modules
- MOD_MobileAppManagement
- Trip/Project Management
- Finance Module

## Implementation Scripts
- **Tables Script:** MOD_MobileExpenseManagement_Tables.sql
- **Procedures Script:** MOD_MobileExpenseManagement_Procedures.sql

**Last Updated:** March 9, 2026
**Version:** 1.0
