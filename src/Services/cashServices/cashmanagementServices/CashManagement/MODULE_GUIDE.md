# CashManagement Module

## Module Overview
The CashManagement module handles cash transactions, bank reconciliation, cheque management, and provides comprehensive functions for balance calculations and reconciliation.

## Architecture

```
┌─────────────────────────────────────────────────┐
│          CashManagement Module                  │
├─────────────────────────────────────────────────┤
│ Functions:                                      │
│  • fn_GetCashInHand()                          │
│  • fn_GetBankBalance()                         │
│  • fn_GetUnclearedChequesTotal()               │
├─────────────────────────────────────────────────┤
│ Procedures:                                     │
│  • usp_RecordCashReceipt()                     │
│  • usp_RecordCashDisbursement()                │
│  • usp_PerformBankReconciliation()             │
│  • usp_IssueCheque()                           │
│  • usp_MarkChequeBounced()                     │
├─────────────────────────────────────────────────┤
│ Triggers:                                       │
│  • trg_CashTransaction_Validate                │
│  • trg_BankTransaction_Validate                │
│  • trg_ChequeRegister_Audit                    │
├─────────────────────────────────────────────────┤
│ Tables:                                         │
│  • CASH_UNIT / CASH_TRANSACTION                │
│  • BANK_ACCOUNT / BANK_TRANSACTION             │
│  • CHEQUE_REGISTER / CHEQUE_REGISTER_AUDIT     │
│  • BANK_RECONCILIATION                         │
└─────────────────────────────────────────────────┘
```

---

## Core Tables

### CASH_UNIT
**Purpose:** Master record for cash handling units (Petty cash, Tills, Cash boxes)

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| CASH_UNIT_ID | BIGINT | NO | Primary Key |
| CASH_UNIT_NAME | VARCHAR(100) | NO | Unit name (e.g., "Petty Cash A") |
| CASH_UNIT_CODE | VARCHAR(20) | NO | Unique code |
| CASH_UNIT_LOCATION | VARCHAR(100) | YES | Physical location |
| CASH_UNIT_INCHARGE | BIGINT | YES | Employee responsible |
| CASH_UNIT_OPENINGBAL | DECIMAL(19,0) | YES | Opening balance |
| CASH_UNIT_STATUS | CHAR(1) | NO | A=Active, I=Inactive |
| CREATED_BY | BIGINT | NO | Created by user |
| CREATED_ON | DATETIME2(3) | NO | Created timestamp |
| UPDATED_BY | BIGINT | YES | Last updated by |
| UPDATED_ON | DATETIME2(3) | YES | Last updated timestamp |

**Sample Data:**
```sql
INSERT INTO CASH_UNIT VALUES 
(1, 'Petty Cash - Office A', 'PC-OFA', 'Floor 2, Accounting', 1001, 50000, 'A', 1, GETDATE(), NULL, NULL),
(2, 'Sales Till - Counter 1', 'STL-C1', 'Ground Floor, Sales', 1002, 100000, 'A', 1, GETDATE(), NULL, NULL);
```

---

### CASH_TRANSACTION
**Purpose:** All cash receipts and disbursements

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| CASH_TXN_ID | BIGINT | NO | Primary Key (Identity) |
| CASH_UNIT_ID | BIGINT | NO | Cash unit (FK) |
| CASH_TXN_TYPE | CHAR(1) | NO | R=Receipt, D=Disbursement |
| CASH_TXN_AMOUNT | DECIMAL(19,0) | NO | Transaction amount (>0) |
| CASH_TXN_SOURCE | VARCHAR(100) | YES | Source or purpose |
| CASH_TXN_PAYEE_ID | BIGINT | YES | Payee employee ID |
| CASH_TXN_REF_NO | VARCHAR(50) | YES | Reference number |
| CASH_TXN_DATE | DATETIME2(3) | NO | Transaction date |
| CASH_TXN_REMARKS | VARCHAR(500) | YES | Additional notes |
| CASH_TXN_STATUS | CHAR(1) | NO | P=Posted, H=Hold |
| AUTHORIZED_BY | BIGINT | YES | Authorization ID |
| CREATED_BY | BIGINT | NO | User ID |
| CREATED_ON | DATETIME2(3) | NO | Timestamp |

**Validation Trigger:** `trg_CashTransaction_Validate`
- Validates CASH_TXN_TYPE ∈ ('R', 'D')
- Validates CASH_TXN_AMOUNT > 0

**Indexes:**
- IX_CASH_TRANSACTION_UNIT_DATE
- IX_CASH_TRANSACTION_TYPE

---

### BANK_ACCOUNT
**Purpose:** Bank account master records

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| BANK_ACCOUNT_ID | BIGINT | NO | Primary Key |
| BANK_NAME | VARCHAR(100) | NO | Bank name |
| BANK_ACCOUNT_NO | VARCHAR(20) | NO | Account number |
| BANK_BRANCH | VARCHAR(100) | YES | Branch name |
| BANK_ACCOUNT_TYPE | VARCHAR(20) | YES | Savings/Current/etc |
| BANK_ACCOUNT_STATUS | CHAR(1) | NO | A=Active, I=Inactive |
| CREATED_BY | BIGINT | NO | Created by |
| CREATED_ON | DATETIME2(3) | NO | Created on |
| UPDATED_BY | BIGINT | YES | Updated by |
| UPDATED_ON | DATETIME2(3) | YES | Updated on |

---

### BANK_TRANSACTION
**Purpose:** Bank deposits and withdrawals

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| BANK_TXN_ID | BIGINT | NO | Primary Key (Identity) |
| BANK_ACCOUNT_ID | BIGINT | NO | Bank account (FK) |
| BANK_TXN_TYPE | CHAR(1) | NO | D=Deposit, W=Withdrawal |
| BANK_TXN_AMOUNT | DECIMAL(19,0) | NO | Amount (>0) |
| BANK_TXN_DATE | DATETIME2(3) | NO | Transaction date |
| BANK_TXN_REFERENCE | VARCHAR(50) | YES | Reference |
| BANK_TXN_REMARKS | VARCHAR(500) | YES | Notes |
| BANK_TXN_STATUS | CHAR(1) | NO | P=Posted, H=Hold |
| CREATED_BY | BIGINT | NO | Created by |
| CREATED_ON | DATETIME2(3) | NO | Created on |

**Validation Trigger:** `trg_BankTransaction_Validate`
- Validates BANK_TXN_TYPE ∈ ('D', 'W')
- Validates BANK_TXN_AMOUNT > 0

---

### CHEQUE_REGISTER
**Purpose:** Cheque issuance and clearing tracking

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| CHEQUE_ID | BIGINT | NO | Primary Key (Identity) |
| BANK_ACCOUNT_ID | BIGINT | NO | Bank account (FK) |
| CHEQUE_NUMBER | VARCHAR(20) | NO | Cheque number |
| PAYEE_NAME | VARCHAR(100) | NO | Payee name |
| CHEQUE_AMOUNT | DECIMAL(19,0) | NO | Cheque amount |
| CHEQUE_ISSUE_DATE | DATE | NO | Date issued |
| CHEQUE_DATE | DATE | NO | Cheque date |
| CHEQUE_REFERENCE | VARCHAR(100) | YES | Reference |
| CHEQUE_STATUS | CHAR(1) | NO | I=Issued, C=Cleared, B=Bounced, X=Cancelled |
| CHEQUE_BOUNCE_REASON | VARCHAR(200) | YES | Bounce reason |
| CREATED_BY | BIGINT | NO | Created by |
| CREATED_ON | DATETIME2(3) | NO | Created on |
| UPDATED_BY | BIGINT | YES | Updated by |
| UPDATED_ON | DATETIME2(3) | YES | Updated on |

**Audit Trigger:** `trg_ChequeRegister_Audit`
- Logs all INSERT and UPDATE operations

**Unique Constraint:** (BANK_ACCOUNT_ID, CHEQUE_NUMBER)

**Indexes:**
- IX_CHEQUE_REGISTER_ACCOUNT
- IX_CHEQUE_REGISTER_STATUS

---

### BANK_RECONCILIATION
**Purpose:** Bank statement reconciliation records

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| RECON_ID | BIGINT | NO | Primary Key (Identity) |
| BANK_ACCOUNT_ID | BIGINT | NO | Bank account (FK) |
| BANK_STATEMENT_BALANCE | DECIMAL(19,0) | NO | Balance per statement |
| LEDGER_BALANCE | DECIMAL(19,0) | NO | Balance per ledger |
| UNCLEARED_CHEQUES | DECIMAL(19,0) | YES | Total uncleared cheques |
| DIFFERENCE_AMOUNT | DECIMAL(19,0) | YES | Difference (Statement - Ledger) |
| RECONCILIATION_STATUS | VARCHAR(10) | YES | R=Reconciled, D=Difference |
| RECONCILIATION_DATE | DATE | NO | Reconciliation date |
| CREATED_BY | BIGINT | NO | Performed by |
| CREATED_ON | DATETIME2(3) | NO | Timestamp |

---

### CHEQUE_REGISTER_AUDIT
**Purpose:** Audit trail for cheque status changes

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| AUDIT_ID | BIGINT | NO | Primary Key (Identity) |
| CHEQUE_ID | BIGINT | NO | Reference to cheque |
| BANK_ACCOUNT_ID | BIGINT | NO | Bank account |
| CHEQUE_NUMBER | VARCHAR(20) | NO | Cheque number |
| PREVIOUS_STATUS | VARCHAR(10) | YES | Before status |
| NEW_STATUS | VARCHAR(10) | NO | After status |
| AUDIT_ACTION | VARCHAR(10) | NO | INSERT / UPDATE |
| AUDIT_DATE | DATETIME2(3) | NO | When changed |

---

## Functions

### fn_GetCashInHand
**Purpose:** Calculate total cash in hand as of a specific date

```sql
DECLARE @CashInHand DECIMAL(19,0);
SET @CashInHand = dbo.fn_GetCashInHand(
    @p_CashUnitID = 1,
    @p_AsOfDate = GETDATE()
);
SELECT @CashInHand AS CashInHand;
```

**Logic:**
- SUM(CASH_TXN_AMOUNT) where CASH_TXN_TYPE = 'R' (Receipts)
- MINUS SUM(CASH_TXN_AMOUNT) where CASH_TXN_TYPE = 'D' (Disbursements)
- Only includes Posted transactions (CASH_TXN_STATUS = 'P')
- Only transactions up to @p_AsOfDate

**Return:** DECIMAL(19,0) - Cash balance (0 if error)

---

### fn_GetBankBalance
**Purpose:** Calculate ledger balance for a bank account as of a date

```sql
DECLARE @BankBalance DECIMAL(19,0);
SET @BankBalance = dbo.fn_GetBankBalance(
    @p_BankAccountID = 1,
    @p_AsOfDate = GETDATE()
);
SELECT @BankBalance AS BankBalance;
```

**Logic:**
- SUM(BANK_TXN_AMOUNT) where BANK_TXN_TYPE = 'D' (Deposits)
- MINUS SUM(BANK_TXN_AMOUNT) where BANK_TXN_TYPE = 'W' (Withdrawals)
- Only Posted transactions (BANK_TXN_STATUS = 'P')
- Only transactions up to @p_AsOfDate

**Return:** DECIMAL(19,0) - Bank balance

---

### fn_GetUnclearedChequesTotal
**Purpose:** Calculate total of uncleared cheques

```sql
DECLARE @UnclearedAmount DECIMAL(19,0);
SET @UnclearedAmount = dbo.fn_GetUnclearedChequesTotal(
    @p_BankAccountID = 1,
    @p_AsOfDate = GETDATE()
);
SELECT @UnclearedAmount AS UnclearedAmount;
```

**Logic:**
- SUM(CHEQUE_AMOUNT) where:
  - CHEQUE_ISSUE_DATE ≤ @p_AsOfDate
  - CHEQUE_STATUS IN ('I', 'B') - Issued or Bounced

**Return:** DECIMAL(19,0) - Total uncleared cheques

---

## Stored Procedures

### usp_RecordCashReceipt
**Purpose:** Record cash receipt with validation

```sql
DECLARE @TxnID BIGINT;
EXEC usp_RecordCashReceipt
    @p_CashUnitID = 1,
    @p_ReceiptAmount = 5000,
    @p_ReceiptSource = 'SALES',
    @p_ReferenceNumber = 'INV-001',
    @p_Remarks = 'Payment from Customer ABC',
    @p_RecordedBy = 1001,
    @p_TransactionID = @TxnID OUTPUT;

SELECT @TxnID AS TransactionID;
```

**Validations:**
- Amount > 0 (Error 50001: "Receipt amount must be greater than zero")
- Cash unit must exist
- Validates through trigger

**Returns:** @p_TransactionID (newly created CASH_TXN_ID)

---

### usp_RecordCashDisbursement
**Purpose:** Record cash disbursement with funds verification

```sql
DECLARE @TxnID BIGINT;
EXEC usp_RecordCashDisbursement
    @p_CashUnitID = 1,
    @p_DisbursementAmount = 2000,
    @p_DisbursementType = 'ADVANCE',
    @p_PayeeID = 1002,
    @p_ReferenceNumber = 'ADV-001',
    @p_Remarks = 'Employee advance',
    @p_AuthorizedBy = 1001,
    @p_RecordedBy = 1003,
    @p_TransactionID = @TxnID OUTPUT;
```

**Validations:**
- Amount > 0 (Error 50003)
- Sufficient cash in hand (Error 50004: "Insufficient cash in hand")
- Calls fn_GetCashInHand() for verification
- Validates through trigger

**Returns:** @p_TransactionID

---

### usp_PerformBankReconciliation
**Purpose:** Reconcile bank statement with ledger

```sql
DECLARE @ReconID BIGINT;
EXEC usp_PerformBankReconciliation
    @p_BankAccountID = 1,
    @p_BankStatementBalance = 150000,
    @p_ReconciliationDate = '2026-03-09',
    @p_ReconciliationBy = 1001,
    @p_ReconciliationID = @ReconID OUTPUT;
```

**Calculation:**
```
Ledger Balance = fn_GetBankBalance(account, date)
Uncleared Cheques = fn_GetUnclearedChequesTotal(account, date)
Computed Balance = Ledger Balance - Uncleared Cheques + Deposits in Transit
Difference = Bank Statement Balance - Computed Balance

Status = 'R' (Reconciled) if Difference = 0
         'D' (Difference Found) if Difference ≠ 0
```

**Returns:** @p_ReconciliationID

---

### usp_IssueCheque
**Purpose:** Issue cheque with duplicate prevention

```sql
DECLARE @ChequeID BIGINT;
EXEC usp_IssueCheque
    @p_BankAccountID = 1,
    @p_ChequeNumber = 'CHQ001234',
    @p_PayeeName = 'Vendor ABC Ltd',
    @p_ChequeAmount = 25000,
    @p_ChequeDate = '2026-03-15',
    @p_Reference = 'INV-123',
    @p_IssuedBy = 1001,
    @p_ChequeID = @ChequeID OUTPUT;
```

**Validations:**
- Cheque number must be unique per account (Error 50005)
- Cannot reuse cancelled/cleared cheques
- Status set to 'I' (Issued)

**Returns:** @p_ChequeID

---

### usp_MarkChequeBounced
**Purpose:** Mark cheque as bounced with reason

```sql
EXEC usp_MarkChequeBounced
    @p_ChequeID = 123,
    @p_BouncedReason = 'Insufficient Funds',
    @p_ProcessedBy = 1001;
```

**Changes:**
- CHEQUE_STATUS = 'B' (Bounced)
- CHEQUE_BOUNCE_REASON = provided reason
- Updates UPDATED_BY and UPDATED_ON
- Triggers audit record creation

---

## Triggers

### trg_CashTransaction_Validate
**Type:** INSTEAD OF INSERT

**Validations:**
- CASH_TXN_TYPE must be 'R' or 'D' (Error 50007)
- CASH_TXN_AMOUNT must be > 0 (Error 50008)

**Action:** Inserts validated record or throws error

---

### trg_BankTransaction_Validate
**Type:** INSTEAD OF INSERT

**Validations:**
- BANK_TXN_TYPE must be 'D' or 'W' (Error 50009)
- BANK_TXN_AMOUNT must be > 0 (Error 50010)

---

### trg_ChequeRegister_Audit
**Type:** AFTER INSERT, UPDATE

**Action:** Inserts audit record to CHEQUE_REGISTER_AUDIT
- Logs previous status for updates
- Logs action type (INSERT/UPDATE)
- Maintains complete history

---

## Error Codes

| Code | Message | Procedure |
|------|---------|-----------|
| 50001 | Receipt amount must be greater than zero | usp_RecordCashReceipt |
| 50002 | Error recording cash receipt | usp_RecordCashReceipt |
| 50003 | Disbursement amount must be greater than zero | usp_RecordCashDisbursement |
| 50004 | Insufficient cash in hand | usp_RecordCashDisbursement |
| 50005 | Cheque number already exists | usp_IssueCheque |
| 50006 | Error updating cheque status | usp_MarkChequeBounced |
| 50007 | Invalid cash transaction type | trg_CashTransaction_Validate |
| 50008 | Transaction amount must be greater than zero | trg_CashTransaction_Validate |
| 50009 | Invalid bank transaction type | trg_BankTransaction_Validate |
| 50010 | Transaction amount must be greater than zero | trg_BankTransaction_Validate |

---

## Usage Workflows

### Daily Petty Cash Receipt
```sql
-- Step 1: Record receipt
DECLARE @TxnID BIGINT;
EXEC usp_RecordCashReceipt
    @p_CashUnitID = 1,
    @p_ReceiptAmount = 10000,
    @p_ReceiptSource = 'EXPENSE_REIMBURSEMENT',
    @p_ReferenceNumber = 'REF-2026-001',
    @p_Remarks = 'Reimbursement for office supplies',
    @p_RecordedBy = 1001,
    @p_TransactionID = @TxnID OUTPUT;

-- Step 2: Verify balance
SELECT dbo.fn_GetCashInHand(1, GETDATE()) AS CurrentBalance;
```

### Bank Reconciliation Process
```sql
-- Step 1: Issue cheques in the period
DECLARE @ChequeID BIGINT;
EXEC usp_IssueCheque
    @p_BankAccountID = 1,
    @p_ChequeNumber = 'CHQ001001',
    @p_PayeeName = 'Supplier XYZ',
    @p_ChequeAmount = 50000,
    @p_ChequeDate = '2026-03-10',
    @p_Reference = 'PO-123',
    @p_IssuedBy = 1001,
    @p_ChequeID = @ChequeID OUTPUT;

-- Step 2: Record bank deposits/withdrawals
-- (INSERT into BANK_TRANSACTION through application)

-- Step 3: Perform reconciliation
DECLARE @ReconID BIGINT;
EXEC usp_PerformBankReconciliation
    @p_BankAccountID = 1,
    @p_BankStatementBalance = 500000,
    @p_ReconciliationDate = '2026-03-09',
    @p_ReconciliationBy = 1001,
    @p_ReconciliationID = @ReconID OUTPUT;

-- Step 4: Check reconciliation status
SELECT * FROM BANK_RECONCILIATION WHERE RECON_ID = @ReconID;
```

### Cheque Bounce Handling
```sql
-- Step 1: Mark cheque as bounced
EXEC usp_MarkChequeBounced
    @p_ChequeID = 1,
    @p_BouncedReason = 'Account Closed',
    @p_ProcessedBy = 1001;

-- Step 2: Review audit trail
SELECT * FROM CHEQUE_REGISTER_AUDIT WHERE CHEQUE_ID = 1;

-- Step 3: Reissue cheque
DECLARE @NewChequeID BIGINT;
EXEC usp_IssueCheque
    @p_BankAccountID = 1,
    @p_ChequeNumber = 'CHQ001002',
    @p_PayeeName = 'Supplier XYZ',
    @p_ChequeAmount = 50000,
    @p_ChequeDate = '2026-03-12',
    @p_Reference = 'PO-123-REISSUE',
    @p_IssuedBy = 1001,
    @p_ChequeID = @NewChequeID OUTPUT;
```

---

## Reporting Queries

### Daily Cash Position
```sql
SELECT 
    cu.CASH_UNIT_NAME,
    dbo.fn_GetCashInHand(cu.CASH_UNIT_ID, GETDATE()) AS CashInHand,
    COUNT(ct.CASH_TXN_ID) AS TransactionCount,
    MAX(ct.CASH_TXN_DATE) AS LastTransaction
FROM CASH_UNIT cu
LEFT JOIN CASH_TRANSACTION ct ON cu.CASH_UNIT_ID = ct.CASH_UNIT_ID
WHERE cu.CASH_UNIT_STATUS = 'A'
GROUP BY cu.CASH_UNIT_ID, cu.CASH_UNIT_NAME;
```

### Uncleared Cheques Report
```sql
SELECT 
    ba.BANK_NAME,
    ba.BANK_ACCOUNT_NO,
    cr.CHEQUE_NUMBER,
    cr.PAYEE_NAME,
    cr.CHEQUE_AMOUNT,
    cr.CHEQUE_DATE,
    DATEDIFF(DAY, cr.CHEQUE_ISSUE_DATE, GETDATE()) AS DaysOld
FROM CHEQUE_REGISTER cr
JOIN BANK_ACCOUNT ba ON cr.BANK_ACCOUNT_ID = ba.BANK_ACCOUNT_ID
WHERE cr.CHEQUE_STATUS IN ('I', 'B')  -- Issued or Bounced
ORDER BY cr.CHEQUE_DATE ASC;
```

### Monthly Reconciliation Summary
```sql
SELECT 
    MONTH(RECONCILIATION_DATE) AS Month,
    COUNT(*) AS TotalReconciliations,
    SUM(CASE WHEN RECONCILIATION_STATUS = 'R' THEN 1 ELSE 0 END) AS Reconciled,
    SUM(CASE WHEN RECONCILIATION_STATUS = 'D' THEN 1 ELSE 0 END) AS Differences
FROM BANK_RECONCILIATION
WHERE YEAR(RECONCILIATION_DATE) = 2026
GROUP BY MONTH(RECONCILIATION_DATE);
```

---

## Best Practices

1. **Always Post Transactions:** Ensure CASH_TXN_STATUS/BANK_TXN_STATUS = 'P' for reconciliation
2. **Verify Before Disburse:** Always check cash balance before recording disbursements
3. **Regular Reconciliation:** Reconcile bank accounts daily/weekly as per policy
4. **Audit Trail:** Review CHEQUE_REGISTER_AUDIT for compliance
5. **Error Handling:** Catch error codes 50001-50010 for detailed validation messages
6. **Cheque Duplicate:** System prevents reissuance of same cheque number per account
7. **Amount Validation:** All transactions validated to be > 0

---

**Version:** 1.0
**Last Updated:** March 9, 2026
