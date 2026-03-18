# OrganizationSetup Module

## Module Overview
The OrganizationSetup module manages organizational structure, role-based access control, organization parameters, and trade limits.

## Tables

### DEAL_ROLE
**Purpose:** Role master records for access control

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| ROLE_ID | BIGINT | NO | Primary Key |
| ROLE_NAME | VARCHAR(50) | NO | Role name (e.g., "Approver", "Dealer") |
| ROLE_LEVEL | BIGINT | NO | Hierarchy level (1=highest, 10=lowest) |
| ROLE_MODIFIEDBY | DECIMAL(38) | NO | Modified by user ID |
| ROLE_MODIFIEDON | DATETIME2(3) | NO | Modified timestamp |

**Primary Key:** PK_DEAL_ROLE (ROLE_ID)

**Indexes:**
- IX_DEAL_ROLE_NAME

**Sample Data:**
```sql
INSERT INTO DEAL_ROLE VALUES 
(1, 'Treasury Manager', 1, 1, GETDATE()),
(2, 'Dealer', 2, 1, GETDATE()),
(3, 'Approver', 2, 1, GETDATE()),
(4, 'Accountant', 3, 1, GETDATE()),
(5, 'Data Entry', 4, 1, GETDATE());
```

---

### DEAL_USERMAP
**Purpose:** Maps users to roles within organizations

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| ROLE_MAPID | BIGINT | NO | Primary Key |
| ROLE_ID | BIGINT | NO | Role ID (FK to DEAL_ROLE) |
| ROLE_EMPSYSID | BIGINT | NO | Employee system ID |
| ROLE_ORGID | BIGINT | NO | Organization ID |
| ROLE_BUSINESS | BIGINT | YES | Business unit (sub-organization) |

**Primary Key:** PK_DEAL_USERMAP (ROLE_MAPID)

**Foreign Keys:**
- FK_DEAL_USERMAP_ROLE → DEAL_ROLE(ROLE_ID)

**Indexes:**
- IX_DEAL_USERMAP_EMPID
- IX_DEAL_USERMAP_ORGID

**Sample Data:**
```sql
INSERT INTO DEAL_USERMAP VALUES 
(1, 1, 1001, 100, NULL),        -- User 1001 is Treasury Manager in Org 100
(2, 2, 1002, 100, 10),          -- User 1002 is Dealer in Business Unit 10
(3, 3, 1003, 100, 10),          -- User 1003 is Approver in Business Unit 10
(4, 4, 1004, 100, NULL);        -- User 1004 is Accountant in Org 100
```

---

### DEAL_ORGPARAMS
**Purpose:** Organization-specific configuration parameters

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| ORG_PARAMID | BIGINT | NO | Primary Key |
| ORG_PARAMTYPE | CHAR(6) | NO | Parameter type code |
| ORG_PARAMVALUE | BIGINT | NO | Parameter value |
| ORG_ID | BIGINT | NO | Organization ID |
| ORG_MODIFIEDBY | DECIMAL(38) | NO | Modified by user |
| ORG_MODIFIEDON | DATETIME2(3) | NO | Modified timestamp |

**Primary Key:** PK_DEAL_ORGPARAMS (ORG_PARAMID)

**Indexes:**
- IX_DEAL_ORGPARAMS_ORGID
- IX_DEAL_ORGPARAMS_PARAMTYPE

**Parameter Types:**
| Type | Description | Example Value |
|------|-------------|---------------|
| MAXDEAL | Maximum single deal limit | 10000000 |
| MAXEXP | Maximum exposure limit | 50000000 |
| MINAPP | Minimum approval amount | 100000 |
| REPFRQ | Reporting frequency (days) | 7 |
| FISYEAR | Financial year start month | 4 (April) |
| BASECUR | Base currency ID | 1 (USD) |

**Sample Data:**
```sql
INSERT INTO DEAL_ORGPARAMS VALUES 
(1, 'MAXDEAL', 10000000, 100, 1, GETDATE()),  -- 10 Crores max per deal
(2, 'MAXEXP', 50000000, 100, 1, GETDATE()),   -- 50 Crores exposure
(3, 'MINAPP', 1000000, 100, 1, GETDATE()),    -- 1 Crore minimum approval
(4, 'REPFRQ', 7, 100, 1, GETDATE()),          -- Weekly reporting
(5, 'FISYEAR', 4, 100, 1, GETDATE()),         -- FY starts April
(6, 'BASECUR', 1, 100, 1, GETDATE());         -- USD base currency
```

---

### DEAL_PPLIMIT
**Purpose:** PP (Provisional Prepayment) limit management per organization

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| PP_LIMITID | BIGINT | NO | Primary Key |
| PP_ORGID | BIGINT | NO | Organization ID |
| PP_TRANTYPE | CHAR(1) | NO | I=Import, E=Export |
| PP_BASCURR | BIGINT | NO | Base currency |
| PP_LIMITAMT | DECIMAL(19,0) | YES | Total PP limit amount |
| PP_FINYEAR | INT | NO | Financial year |
| PP_LIMITACT | DECIMAL(19,0) | YES | Limit utilized/actual |
| PP_CERTIFICATEUPLOAD | VARCHAR(500) | YES | Certificate file path |
| PP_MODIFIEDBY | DECIMAL(38) | YES | Modified by |
| PP_MODIFIEDON | DATETIME2(3) | YES | Modified timestamp |

**Primary Key:** PK_DEAL_PPLIMIT (PP_LIMITID)

**Indexes:**
- IX_DEAL_PPLIMIT_ORGID
- IX_DEAL_PPLIMIT_FINYEAR

**Sample Data:**
```sql
INSERT INTO DEAL_PPLIMIT VALUES 
(1, 100, 'I', 1, 100000000, 2026, 25000000, NULL, 1, GETDATE()),
(2, 100, 'E', 1, 150000000, 2026, 75000000, NULL, 1, GETDATE());
```

---

## Relationship Diagram

```
DEAL_ROLE
    ↓
DEAL_USERMAP
    ├─ Links to employees
    ├─ Links to organizations
    └─ Links to business units

DEAL_ORGPARAMS
    ├─ Configuration parameters per org
    └─ Used by DealTicketing, LoanManagement

DEAL_PPLIMIT
    ├─ PP limits per org
    ├─ Tracks import/export separately
    └─ Financial year specific
```

---

## Role-Based Access Control

### Role Hierarchy

```
Level 1: Treasury Manager (System Admin)
  ├─ Level 2: Dealer (Execute trades)
  ├─ Level 2: Approver (Approve transactions)
  ├─ Level 3: Accountant (Recording & reconciliation)
  └─ Level 4: Data Entry (Input support staff)
```

### Permission Model
- Higher level roles can perform lower level activities
- Organization and business unit filtering for visibility
- Audit trail tracks all user actions

---

## Configuration Management

### Setting Organization Parameters

```sql
-- Step 1: Find organization ID
SELECT ROLE_ORGID FROM DEAL_USERMAP 
WHERE ROLE_EMPSYSID = 1001;  -- Employee system ID

-- Step 2: Insert/Update parameters
INSERT INTO DEAL_ORGPARAMS 
(ORG_PARAMID, ORG_PARAMTYPE, ORG_PARAMVALUE, ORG_ID, ORG_MODIFIEDBY, ORG_MODIFIEDON)
VALUES (NEW_ID, 'MAXDEAL', 20000000, 100, 1, GETDATE());

-- Step 3: Verify setting
SELECT ORG_PARAMTYPE, ORG_PARAMVALUE 
FROM DEAL_ORGPARAMS 
WHERE ORG_ID = 100;
```

### Assigning User Roles

```sql
-- Assign employee to role
INSERT INTO DEAL_USERMAP 
(ROLE_MAPID, ROLE_ID, ROLE_EMPSYSID, ROLE_ORGID, ROLE_BUSINESS)
VALUES (NEW_ID, 2, 1002, 100, 10);  -- User 1002 as Dealer in Business Unit 10

-- Multi-organization assignment
INSERT INTO DEAL_USERMAP 
(ROLE_MAPID, ROLE_ID, ROLE_EMPSYSID, ROLE_ORGID, ROLE_BUSINESS)
VALUES (NEW_ID, 3, 1003, 101, NULL);  -- Same user can be Approver in Org 101
```

---

## Usage Examples

### Get User's Primary Organization
```sql
SELECT TOP 1
    um.ROLE_MAPID,
    um.ROLE_EMPSYSID,
    dr.ROLE_NAME,
    um.ROLE_ORGID,
    um.ROLE_BUSINESS
FROM DEAL_USERMAP um
JOIN DEAL_ROLE dr ON um.ROLE_ID = dr.ROLE_ID
WHERE um.ROLE_EMPSYSID = 1002
ORDER BY dr.ROLE_LEVEL ASC;
```

### Check Authorization for Transaction
```sql
DECLARE @EmpID BIGINT = 1002;
DECLARE @OrgID BIGINT = 100;
DECLARE @TxnAmount DECIMAL(19,0) = 5000000;

-- Get authorization limit
SELECT 
    um.ROLE_MAPID,
    dr.ROLE_NAME,
    dop.ORG_PARAMVALUE AS AuthLimit
FROM DEAL_USERMAP um
JOIN DEAL_ROLE dr ON um.ROLE_ID = dr.ROLE_ID
JOIN DEAL_ORGPARAMS dop ON um.ROLE_ORGID = dop.ORG_ID
WHERE um.ROLE_EMPSYSID = @EmpID
  AND um.ROLE_ORGID = @OrgID
  AND dop.ORG_PARAMTYPE = 'MINAPP';
```

### PP Limit Utilization Report
```sql
SELECT 
    dl.PP_ORGID,
    CASE WHEN dl.PP_TRANTYPE = 'I' THEN 'Import' ELSE 'Export' END AS Type,
    dl.PP_LIMITAMT AS TotalLimit,
    dl.PP_LIMITACT AS Utilized,
    (dl.PP_LIMITAMT - dl.PP_LIMITACT) AS Available,
    CAST(100.0 * dl.PP_LIMITACT / dl.PP_LIMITAMT AS DECIMAL(5,2)) AS UtilizationPct,
    dl.PP_FINYEAR
FROM DEAL_PPLIMIT dl
WHERE dl.PP_FINYEAR = YEAR(GETDATE());
```

---

## Parameter Configuration Guide

### MAXDEAL - Maximum Single Deal Limit
- Maximum principal amount per deal
- Typically 5-20% of total exposure
- Example: 10 Crores

### MAXEXP - Maximum Exposure Limit
- Total outstanding exposure across all deals
- Based on organization's risk appetite
- Example: 50 Crores

### MINAPP - Minimum Approval Amount
- Threshold for automatic vs manual approval
- Below this: auto-approved
- Above this: requires approver signature
- Example: 1 Crore

### REPFRQ - Reporting Frequency
- Days between mandatory reports
- Typically 7 (weekly) or 30 (monthly)
- Example: 7 days

### FISYEAR - Financial Year Start Month
- Month when FY begins (1-12)
- 1 = January, 4 = April, etc.
- Used for period-based reporting

### BASECUR - Base Currency ID
- Organization's reporting currency
- FK to DEAL_CURRMAST(CURR_ID)
- Multi-currency deals converted to this

---

## PP Limit Management

### Import PP Limits
```sql
SELECT 
    dl.PP_ORGID,
    c.CURR_NAME,
    dl.PP_LIMITAMT,
    dl.PP_LIMITACT,
    (dl.PP_LIMITAMT - dl.PP_LIMITACT) AS Available
FROM DEAL_PPLIMIT dl
JOIN DEAL_CURRMAST c ON dl.PP_BASCURR = c.CURR_ID
WHERE dl.PP_TRANTYPE = 'I'
  AND dl.PP_FINYEAR = 2026
ORDER BY dl.PP_ORGID;
```

### Update PP Utilization
```sql
UPDATE DEAL_PPLIMIT
SET PP_LIMITACT = PP_LIMITACT + 5000000,
    PP_MODIFIEDBY = 1,
    PP_MODIFIEDON = GETDATE()
WHERE PP_LIMITID = 1;
```

---

## Reporting Queries

### Organization Hierarchy Report
```sql
SELECT 
    um.ROLE_ORGID,
    dr.ROLE_NAME,
    dr.ROLE_LEVEL,
    COUNT(DISTINCT um.ROLE_EMPSYSID) AS UserCount
FROM DEAL_USERMAP um
JOIN DEAL_ROLE dr ON um.ROLE_ID = dr.ROLE_ID
GROUP BY um.ROLE_ORGID, dr.ROLE_NAME, dr.ROLE_LEVEL
ORDER BY um.ROLE_ORGID, dr.ROLE_LEVEL;
```

### User Access Summary
```sql
SELECT 
    um.ROLE_EMPSYSID,
    STRING_AGG(dr.ROLE_NAME, ', ') AS Roles,
    COUNT(DISTINCT um.ROLE_ORGID) AS OrganizationCount
FROM DEAL_USERMAP um
JOIN DEAL_ROLE dr ON um.ROLE_ID = dr.ROLE_ID
GROUP BY um.ROLE_EMPSYSID
ORDER BY um.ROLE_EMPSYSID;
```

### Configuration Compliance Check
```sql
SELECT 
    dop.ORG_ID,
    dop.ORG_PARAMTYPE,
    dop.ORG_PARAMVALUE,
    CASE 
        WHEN dop.ORG_PARAMTYPE = 'MAXDEAL' AND dop.ORG_PARAMVALUE <= 0 THEN 'Invalid'
        WHEN dop.ORG_PARAMTYPE = 'LISYEAR' AND dop.ORG_PARAMVALUE NOT BETWEEN 1 AND 12 THEN 'Invalid'
        ELSE 'Valid'
    END AS Status
FROM DEAL_ORGPARAMS dop
ORDER BY dop.ORG_ID;
```

---

## Integration with Other Modules

### DealTicketing Integration:
- DEAL_BUSINESSID references ROLE_BUSINESS for approval routing
- DEAL_MODIFIEDBY tracks user from DEAL_USERMAP
- Deal limits enforced from DEAL_ORGPARAMS(MAXDEAL)

### LoanManagement Integration:
- LOAN_ORGID references organization
- LOAN_CREATEDBY from DEAL_USERMAP
- Organization currency from DEAL_ORGPARAMS(BASECUR)

### CashManagement Integration:
- Cash unit authorization by DEAL_USERMAP role
- Approval limits from DEAL_ORGPARAMS(MINAPP)
- User audit trails

### CurrencyManagement Integration:
- DEAL_ORGCURRMAP links to DEAL_ORGPARAMS configuration
- Multi-currency support per organization

---

## Best Practices

1. **Document Role Hierarchy:** Maintain clear role definitions
2. **Regular Access Review:** Quarterly review of user assignments
3. **Segregation of Duties:** Separate dealer and approver roles
4. **Parameter Audit:** Periodic review of organization parameters
5. **PP Limit Monitoring:** Weekly utilization checks
6. **Archive Old Maps:** Archive revoked role assignments
7. **Change Tracking:** Log all parameter modifications

---

## Security Considerations

1. **Role Lock:** Prevent unauthorized role changes
2. **Audit Trail:** All modifications tracked
3. **Multi-level Approval:** Major config changes require approval
4. **Parameter Encryption:** Sensitive limits in secured storage
5. **Access Logs:** Maintain comprehensive audit logs
6. **Role Segregation:** Clear separation of duties

---

**Version:** 1.0
**Last Updated:** March 9, 2026
