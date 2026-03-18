# TimesheetModule Documentation

## Module Overview
The TimesheetModule manages employee work hour tracking, daily timesheet entries, and approval workflows.

## Tables

### TSE_TIMESHEET
- **Purpose**: Employee timesheet entries with project and task tracking
- **Key Columns**: EMP_SYSID, WORK_DATE, TOTAL_HOURS, PROJECT_ID, TASK_ID, RECORDED_DATE

## Status Values
### Timesheet Status
- DRAFT: Initial entry
- SUBMITTED: Ready for approval
- APPROVED: Approved and finalized
- REJECTED: Returned for correction

### Approval Status
- PENDING: Awaiting approval
- APPROVED: Approved
- REJECTED: Rejected with reason

## Deployment
```sql
:r "TimesheetModule_Schema.sql"
```

## Features
- Daily work hour logging
- Project and task association
- Work description tracking
- Recorded date for audit trail
- Multi-level approval workflow

## Query Examples
```sql
-- Get pending timesheets for approval
SELECT * FROM TSE_TIMESHEET 
WHERE APPROVAL_STATUS = 'PENDING'
  AND RECORDED_DATE >= DATEADD(WEEK, -1, GETDATE())
ORDER BY EMP_SYSID, WORK_DATE DESC;

-- Monthly hours per employee
SELECT 
    EMP_SYSID,
    YEAR(WORK_DATE) AS Year,
    MONTH(WORK_DATE) AS Month,
    SUM(TOTAL_HOURS) AS TotalHours
FROM TSE_TIMESHEET
WHERE APPROVAL_STATUS = 'APPROVED'
GROUP BY EMP_SYSID, YEAR(WORK_DATE), MONTH(WORK_DATE)
ORDER BY EMP_SYSID, Year, Month;
```

---
**Created**: March 09, 2026
**Version**: 1.0
