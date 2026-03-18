# EmailNotification Module

## Module Overview
The EmailNotification module manages email configurations, notification types, and access control for email distribution lists.

## Tables

### EMAIL_TYPEMAST
**Purpose:** Email type master for alert categorization

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| EMAIL_TYPEID | BIGINT | NO | Primary Key |
| EMAIL_NAME | VARCHAR(500) | NO | Email alert name |
| EMAIL_TYPE | CHAR(1) | NO | D=Daily alert, E=Event-based alert |
| EMAIL_PRCNAME | VARCHAR(100) | NO | Procedure name to generate email |
| EMAIL_MODIFIEDBY | DECIMAL(19,0) | NO | Modified by user ID |
| EMAIL_MODIFIEDON | DATETIME2(3) | NO | Modified timestamp |

**Primary Key:** PK_EMAIL_TYPEMAST (EMAIL_TYPEID)

**Indexes:**
- IX_EMAIL_TYPEMAST_TYPE

**Email Types:**
| Type | Frequency | Use Case | Example |
|------|-----------|----------|---------|
| D | Daily | Scheduled reports | Daily P&L, Daily limits utilization |
| E | Event-based | Transactional | Trade confirmation, Exception alert |

**Sample Data:**
```sql
INSERT INTO EMAIL_TYPEMAST VALUES 
(1, 'Daily Treasury Report', 'D', 'usp_GenerateTreasuryReport', 1, GETDATE()),
(2, 'Daily Exposure Report', 'D', 'usp_GenerateExposureReport', 1, GETDATE()),
(3, 'Trade Confirmation', 'E', 'usp_SendTradeConfirmation', 1, GETDATE()),
(4, 'Limit Breach Alert', 'E', 'usp_AlertLimitBreach', 1, GETDATE()),
(5, 'Cheque Bounce Notification', 'E', 'usp_AlertChequeBounce', 1, GETDATE()),
(6, 'Bank Reconciliation Summary', 'D', 'usp_ReconSummary', 1, GETDATE());
```

---

### MAIL_ACCESS
**Purpose:** Email access control and distribution list management

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| MAIL_ACCESSID | BIGINT | NO | Primary Key |
| MAIL_TYPEID | BIGINT | NO | Email type (FK to EMAIL_TYPEMAST) |
| MAIL_ORGID | BIGINT | YES | Organization ID (0=All) |
| MAIL_BUSINESSID | BIGINT | YES | Business unit (0=All or NULL) |
| MAIL_EMPSYSID | BIGINT | YES | Employee system ID |
| MAIL_EMAILID | VARCHAR(200) | NO | Email address |
| MAIL_MODIFIEDBY | DECIMAL(19,0) | NO | Modified by user |
| MAIL_MODIFIEDON | DATETIME2(3) | NO | Modified timestamp |
| MAIL_NAME | VARCHAR(100) | YES | Non-employee name |

**Primary Key:** PK_MAIL_ACCESS (MAIL_ACCESSID)

**Foreign Keys:**
- FK_MAIL_ACCESS_TYPEMAST → EMAIL_TYPEMAST(EMAIL_TYPEID)

**Indexes:**
- IX_MAIL_ACCESS_TYPEID
- IX_MAIL_ACCESS_EMAILID
- IX_MAIL_ACCESS_ORGID
- IX_MAIL_ACCESS_EMPSYSID

**Flexibility:**
```
MAIL_EMPSYSID  | MAIL_EMAILID | MAIL_ORGID | Result
NULL           | external@... | NULL       | External distribution list
1001           | emp@corp.com | NULL       | Specific employee (primary org)
1002           | emp2@corp.com| 100        | Employee @ specific org
NULL           | vendor@... | NULL       | Vendor email (generic)
```

**Sample Data:**
```sql
-- Daily Treasury Report recipients
INSERT INTO MAIL_ACCESS VALUES
(1, 1, NULL, NULL, 1001, 'treasurer@bank.com', 1, GETDATE(), NULL),
(2, 1, NULL, NULL, 1002, 'cfo@bank.com', 1, GETDATE(), NULL),
(3, 1, NULL, NULL, NULL, 'external.advisor@firm.com', 1, GETDATE(), 'External Advisor'),

-- Trade Confirmation (per business unit)
(4, 3, 100, 10, 1003, 'dealer@bank.com', 1, GETDATE(), NULL),
(5, 3, 100, 10, NULL, 'trade.ops@bank.com', 1, GETDATE(), NULL),

-- Limit Breach Alert (All organizations)
(6, 4, 0, NULL, 1001, 'treasurer@bank.com', 1, GETDATE(), NULL),
(7, 4, 0, NULL, 1004, 'risk.mgmt@bank.com', 1, GETDATE(), NULL);
```

---

## Email Type Configuration

### Daily Alert Configuration

**Daily Treasury Report:**
```sql
INSERT INTO EMAIL_TYPEMAST VALUES 
(1, 'Daily Treasury Report - Complete', 'D', 'usp_GenerateTreasuryReport', 1, GETDATE());

-- Recipients across all organizations
INSERT INTO MAIL_ACCESS VALUES
(101, 1, 0, NULL, 1001, 'treasurer@bank.com', 1, GETDATE(), NULL),  -- All orgs
(102, 1, 0, NULL, 1002, 'cfo@bank.com', 1, GETDATE(), NULL);
```

**Daily Exposure Report:**
```sql
INSERT INTO EMAIL_TYPEMAST VALUES 
(2, 'Daily Exposure Summary', 'D', 'usp_GenerateExposureReport', 1, GETDATE());

-- Recipients per organization
INSERT INTO MAIL_ACCESS VALUES
(201, 2, 100, NULL, 1001, 'risk.100@bank.com', 1, GETDATE(), NULL),
(202, 2, 101, NULL, 1005, 'risk.101@bank.com', 1, GETDATE(), NULL);
```

### Event-Based Alert Configuration

**Trade Confirmation:**
```sql
INSERT INTO EMAIL_TYPEMAST VALUES 
(3, 'Trade Confirmation', 'E', 'usp_SendTradeConfirmation', 1, GETDATE());

-- Recipients per business unit
INSERT INTO MAIL_ACCESS VALUES
(301, 3, 100, 10, 1003, 'dealer.bu10@bank.com', 1, GETDATE(), NULL),
(302, 3, 100, 10, NULL, 'ops.bu10@bank.com', 1, GETDATE(), NULL),
(303, 3, 100, NULL, 1001, 'supervisor.100@bank.com', 1, GETDATE(), NULL);
```

**Limit Breach Alert:**
```sql
INSERT INTO EMAIL_TYPEMAST VALUES 
(4, 'Limit Breach Alert', 'E', 'usp_AlertLimitBreach', 1, GETDATE());

-- System-wide critical recipients
INSERT INTO MAIL_ACCESS VALUES
(401, 4, 0, NULL, 1001, 'cro@bank.com', 1, GETDATE(), NULL),
(402, 4, 0, NULL, 1004, 'risk.mgmt@bank.com', 1, GETDATE(), NULL);
```

---

## Usage Examples

### Get Email Recipients for Alert Type
```sql
SELECT 
    et.EMAIL_TYPEID,
    et.EMAIL_NAME,
    et.EMAIL_TYPE,
    ma.MAIL_EMAILID,
    CASE 
        WHEN ma.MAIL_EMPSYSID IS NOT NULL THEN 'Employee'
        ELSE 'External'
    END AS RecipientType,
    ma.MAIL_ORGID,
    ma.MAIL_BUSINESSID
FROM EMAIL_TYPEMAST et
JOIN MAIL_ACCESS ma ON et.EMAIL_TYPEID = ma.MAIL_TYPEID
WHERE et.EMAIL_TYPEID = 1  -- Daily Treasury Report
  AND (ma.MAIL_ORGID = 0 OR ma.MAIL_ORGID IS NULL OR ma.MAIL_ORGID = 100)
ORDER BY ma.MAIL_EMAILID;
```

### Configure New Email Alert Type
```sql
-- Step 1: Create email type
INSERT INTO EMAIL_TYPEMAST 
(EMAIL_TYPEID, EMAIL_NAME, EMAIL_TYPE, EMAIL_PRCNAME, EMAIL_MODIFIEDBY, EMAIL_MODIFIEDON)
VALUES (NEW_ID, 'Monthly Reconciliation Report', 'D', 'usp_GenerateReconReport', 1, GETDATE());

-- Step 2: Add recipients
INSERT INTO MAIL_ACCESS 
(MAIL_ACCESSID, MAIL_TYPEID, MAIL_ORGID, MAIL_BUSINESSID, MAIL_EMPSYSID, 
 MAIL_EMAILID, MAIL_MODIFIEDBY, MAIL_MODIFIEDON, MAIL_NAME)
VALUES 
(NEW_ID, NEW_EMAIL_TYPEID, 100, NULL, 1001, 'accountant@bank.com', 1, GETDATE(), NULL),
(NEW_ID+1, NEW_EMAIL_TYPEID, 100, NULL, NULL, 'external.auditor@audit.com', 1, GETDATE(), 'External Auditor');
```

### Add External Recipient
```sql
INSERT INTO MAIL_ACCESS 
(MAIL_ACCESSID, MAIL_TYPEID, MAIL_ORGID, MAIL_BUSINESSID, MAIL_EMPSYSID, 
 MAIL_EMAILID, MAIL_MODIFIEDBY, MAIL_MODIFIEDON, MAIL_NAME)
VALUES (NEW_ID, 1, NULL, NULL, NULL, 'cfo@mainco.com', 1, GETDATE(), 'Main Company CFO');
```

### Manage Organization-Level Distribution
```sql
INSERT INTO MAIL_ACCESS 
(MAIL_ACCESSID, MAIL_TYPEID, MAIL_ORGID, MAIL_BUSINESSID, MAIL_EMPSYSID, 
 MAIL_EMAILID, MAIL_MODIFIEDBY, MAIL_MODIFIEDON)
VALUES 
(NEW_ID, 2, 101, NULL, 1005, 'risk.101@bank.com', 1, GETDATE());  -- New org identifier
```

---

## Reporting Queries

### Email Distribution List Report
```sql
SELECT 
    et.EMAIL_TYPEID,
    et.EMAIL_NAME,
    CASE WHEN et.EMAIL_TYPE = 'D' THEN 'Daily' ELSE 'Event' END AS AlertType,
    ma.MAIL_EMAILID,
    CASE 
        WHEN ma.MAIL_ORGID = 0 THEN 'All Organizations'
        ELSE 'Org ' + CAST(ma.MAIL_ORGID AS VARCHAR(10))
    END AS Scope,
    CASE 
        WHEN ma.MAIL_BUSINESSID IS NULL THEN 'Full Organization'
        ELSE 'Business Unit ' + CAST(ma.MAIL_BUSINESSID AS VARCHAR(10))
    END AS Unit
FROM EMAIL_TYPEMAST et
JOIN MAIL_ACCESS ma ON et.EMAIL_TYPEID = ma.MAIL_TYPEID
ORDER BY et.EMAIL_TYPEID, ma.MAIL_EMAILID;
```

### External Recipients Report
```sql
SELECT 
    et.EMAIL_NAME,
    ma.MAIL_EMAILID,
    ma.MAIL_NAME AS ExternalName,
    ma.MAIL_MODIFIEDON AS LastUpdated
FROM EMAIL_TYPEMAST et
JOIN MAIL_ACCESS ma ON et.EMAIL_TYPEID = ma.MAIL_TYPEID
WHERE ma.MAIL_EMPSYSID IS NULL
ORDER BY et.EMAIL_NAME, ma.MAIL_EMAILID;
```

### Email Recipients per Organization
```sql
SELECT 
    CASE WHEN ma.MAIL_ORGID = 0 THEN 'All' ELSE CAST(ma.MAIL_ORGID AS VARCHAR) END AS Organization,
    COUNT(DISTINCT ma.MAIL_EMAILID) AS TotalRecipients,
    COUNT(DISTINCT ma.MAIL_TYPEID) AS AlertTypes
FROM MAIL_ACCESS ma
GROUP BY ma.MAIL_ORGID
ORDER BY 
    CASE WHEN ma.MAIL_ORGID = 0 THEN 0 ELSE 1 END,
    ma.MAIL_ORGID;
```

---

## Integration with Other Modules

### DealTicketing Integration:
- Trade Confirmation alerts (Event-based)
- Deal rejection notifications
- Settlement completion alerts

### LoanManagement Integration:
- Disbursement verification emails
- Repayment reminders (can be scheduled)
- Interest accrual notifications

### CashManagement Integration:
- Cheque bounce alerts
- Bank reconciliation summaries
- Cash shortage notifications
- Transaction approval alerts

### OrganizationSetup Integration:
- Business unit filtering for selective distribution
- Organization-level configuration alerts
- PP limit breach notifications

---

## Email Processing Flow

```
1. EVENT GENERATION
   └─ Transaction recorded in database

2. EMAIL TYPE LOOKUP
   └─ Query EMAIL_TYPEMAST for trigger procedure

3. RECIPIENT LIST BUILD
   └─ Query MAIL_ACCESS based on:
      ├─ Organization filter
      ├─ Business unit filter
      └─ Employee/External flag

4. EMAIL COMPOSITION
   └─ Execute EMAIL_PRCNAME procedure
      ├─ Generate email body
      ├─ Format attachments
      └─ Prepare headers

5. DISTRIBUTION
   └─ Send via SMTP to all recipients
      ├─ Log delivery status
      ├─ Retry on failure
      └─ Archive sent emails
```

---

## Configuration Best Practices

1. **Clear Naming:** Use descriptive email type names
2. **Procedure Mapping:** Ensure EMAIL_PRCNAME procedure exists
3. **Recipient Validation:** Validate email format before insertion
4. **Org Layering:** Support both org-level and business-unit-level distribution
5. **External Management:** Track external recipients with MAIL_NAME
6. **Change Tracking:** Log all distribution list modifications
7. **Testing:** Test new email types before production deployment

---

## Security Considerations

1. **Email Validation:** Verify email format and validity
2. **Sensitive Data:** Don't include unencrypted sensitive info in subjects
3. **Encryption:** Use TLS for SMTP transmission
4. **Access Control:** Restrict distribution list modifications to admins
5. **Audit Trail:** Log all email sends for compliance
6. **PII Protection:** Mask personal identifying information if needed
7. **Retention:** Archive emails per company policy

---

## Performance Tips

1. **Batch Processing:** Generate daily emails in batch
2. **Async Distribution:** Use queued service for email sends
3. **Archive Old Records:** Archive historical MAIL_ACCESS changes
4. **Index Usage:** Use MAIL_TYPEID index for lookups
5. **Pagination:** For large distribution lists, process in batches
6. **Caching:** Cache email type procedures in memory

---

## Troubleshooting

| Issue | Solution |
|-------|----------|
| Emails not sent | Check EMAIL_PRCNAME procedure exists and is executable |
| Wrong recipients | Verify MAIL_ORGID and MAIL_BUSINESSID filters |
| Duplicate emails | Check for duplicate MAIL_ACCESSID entries |
| External emails failed | Validate email format in MAIL_EMAILID |
| Missing employee emails | Ensure MAIL_EMPSYSID exists in DEAL_USERMAP |

---

**Version:** 1.0
**Last Updated:** March 9, 2026
