# ProxyModule Documentation

## Module Overview
The ProxyModule manages proxy/delegate access rights, allowing one user to act on behalf of another.

## Tables

### PROXY_RIGHTS
- **Purpose**: Proxy access assignments with date range management
- **Key Columns**: PROXY_USER_ID, DELEGATED_USER_ID, PROXY_TYPE, PROXY_START_DATE, PROXY_END_DATE

## Proxy Types
- APPROVAL: Can approve documents/requests on behalf of user
- SUBMISSION: Can submit documents on behalf of user
- FULL: Complete delegation of authority
- READONLY: View-only access on behalf of user

## Scope Levels
- ALL: Full organizational access
- DEPARTMENT: Department-specific access
- LOCATION: Location-based access
- SPECIFIC: Specific process access

## Deployment
```sql
:r "ProxyModule_Schema.sql"
```

## Setup Example
```sql
-- Grant approval proxy for 30 days
INSERT INTO PROXY_RIGHTS (PROXY_USER_ID, DELEGATED_USER_ID, PROXY_TYPE, PROXY_START_DATE, PROXY_END_DATE, SCOPE, CREATED_BY)
VALUES (100, 101, 'APPROVAL', GETDATE(), DATEADD(DAY, 30, GETDATE()), 'DEPARTMENT', 1);
```

---
**Created**: March 09, 2026
**Version**: 1.0
