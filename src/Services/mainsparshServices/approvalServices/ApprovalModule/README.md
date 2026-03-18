# ApprovalModule Documentation

## Module Overview
The ApprovalModule manages the complete approval workflows and process definitions for the SRF Scholarship Database. It handles module master data (PER, DDP, LET modules) and maintains the mapping of approvers to approval processes.

## Tables

### APPR_MAST (Approval Master)
**Purpose**: Stores approval process definitions at different levels.

**Key Columns**:
- `APPR_ID`: Primary Key - Auto-incremented approval ID
- `APPR_CODE`: Unique code for the approval process
- `APPR_NAME`: Friendly name of the approval process
- `APPR_MODULE`: Type of module (PER, DDP, LET)
- `APPR_LEVEL`: Approval level in the workflow
- `APPR_STATUS`: Active/Inactive flag

**Primary Key**: APPR_ID
**Unique Constraint**: APPR_CODE
**Indexes**: APPR_MODULE

---

### APPROVER_EMP (Approver Employee)
**Purpose**: Maps employees to approval processes with effective date ranges.

**Key Columns**:
- `APPROVER_ID`: Primary Key
- `APPR_ID`: Foreign Key to APPR_MAST
- `EMP_SYSID`: Employee System ID
- `APPROVER_LEVEL`: Level at which this employee approves
- `EFFECTIVE_FROM`: Date from which assignment is valid
- `EFFECTIVE_TO`: Date until which assignment is valid (nullable for ongoing)
- `APPROVER_STATUS`: Active/Inactive flag

**Primary Key**: APPROVER_ID
**Foreign Key**: APPR_ID → APPR_MAST
**Unique Constraint**: None
**Indexes**: APPR_ID, EMP_SYSID

---

## Relationships

```
APPR_MAST (1) ──────────── (Many) APPROVER_EMP
  APPR_ID                      APPR_ID (FK)
```

---

## Common Queries

### Get Active Approvers for a Module
```sql
SELECT 
    am.APPR_CODE,
    am.APPR_NAME,
    ae.EMP_SYSID,
    ae.APPROVER_LEVEL
FROM APPR_MAST am
INNER JOIN APPROVER_EMP ae ON am.APPR_ID = ae.APPR_ID
WHERE am.APPR_MODULE = 'PER'
    AND am.APPR_STATUS = 'A'
    AND ae.APPROVER_STATUS = 'A'
    AND GETDATE() BETWEEN ae.EFFECTIVE_FROM AND ISNULL(ae.EFFECTIVE_TO, GETDATE());
```

### Add New Approval Process
```sql
INSERT INTO APPR_MAST (APPR_CODE, APPR_NAME, APPR_MODULE, APPR_LEVEL, CREATED_BY)
VALUES ('APR001', 'Travel Request Approval', 'PER', 1, 1);
```

### Assign Approver to Process
```sql
INSERT INTO APPROVER_EMP (APPR_ID, EMP_SYSID, APPROVER_LEVEL, EFFECTIVE_FROM, CREATED_BY)
VALUES (1, 1001, 1, GETDATE(), 1);
```

---

## Performance Considerations

- Create composite indexes on (APPR_ID, APPROVER_STATUS) for frequent queries
- Partition by APPR_MODULE if dataset grows large
- Regular archiving of historical approver assignments (beyond EFFECTIVE_TO)

---

## Data Integrity Rules

1. APPR_CODE must be unique across all approval modules
2. EFFECTIVE_TO date must be >= EFFECTIVE_FROM date
3. Only one active approver can exist per (APPR_ID, APPROVER_LEVEL) at any given time
4. Setting APPROVER_STATUS = 'I' should cascade to dependent approval requests

---

## Deployment

Execute the following script to deploy this module:
```sql
:r ApprovalModule_Schema.sql
```

---

**Created**: March 09, 2026
**Last Modified**: March 09, 2026
**Version**: 1.0
