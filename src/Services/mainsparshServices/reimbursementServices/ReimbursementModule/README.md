# ReimbursementModule Documentation

## Module Overview
The ReimbursementModule manages employee expense claims, reimbursement requests, and payment tracking.

## Tables

### REIM_TRAN
- **Purpose**: Reimbursement transaction records
- **Key Columns**: REIM_REF_NO, EMP_SYSID, REIM_TYPE, REIM_AMOUNT, REIM_STATUS

## Reimbursement Types
- TRAVEL: Travel-related expenses
- MEAL: Food and beverage expenses
- ACCOMMODATION: Hotel and lodging
- MISC: Miscellaneous expenses
- CONFERENCE: Conference and seminar fees

## Status Values
- DRAFT: Initial submission
- SUBMITTED: Awaiting approval
- APPROVED: Approved and ready for payment
- REJECTED: Returned for revision
- PAID: Payment completed

## Deployment
```sql
:r "ReimbursementModule_Schema.sql"
```

## Query Examples
```sql
-- Pending reimbursements
SELECT * FROM REIM_TRAN 
WHERE REIM_STATUS IN ('SUBMITTED', 'APPROVED')
ORDER BY REIM_DATE DESC;

-- Monthly summary by employee
SELECT 
    EMP_SYSID,
    REIM_TYPE,
    COUNT(*) AS Count,
    SUM(REIM_AMOUNT) AS Total
FROM REIM_TRAN
WHERE REIM_STATUS = 'PAID'
GROUP BY EMP_SYSID, REIM_TYPE;
```

---
**Created**: March 09, 2026
**Version**: 1.0
