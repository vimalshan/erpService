# UserManagementModule Documentation

## Module Overview
The UserManagementModule manages user profiles, security policies, contact information, and audit trails for user profile changes.

## Tables

### USER_POLICY
- **Purpose**: User-specific policies and settings
- **Key Columns**: USER_SYSID, POLICY_TYPE, SESSION_TIMEOUT_MINS, MAX_LOGIN_ATTEMPTS

### USER_PROFILEHIST
- **Purpose**: Audit trail for user profile modifications
- **Relationship**: References USER_POLICY
- **Key Columns**: PROFILE_FIELD, OLD_VALUE, NEW_VALUE, CHANGED_BY, CHANGED_ON

### WEBSITE_CON_MAILID
- **Purpose**: Contact and communication details
- **Key Columns**: PRIMARY_EMAIL, PHONE, MOBILE, NEWSLETTER_OPT_IN

## Policy Types
SECURITY, NOTIFICATION, PREFERENCES, ACCESS_CONTROL, etc.

## Deployment
```sql
:r "UserManagementModule_Schema.sql"
```

## Features
- Multi-channel contact support
- Multi-email capability
- Newsletter opt-in tracking
- Complete change audit history

---
**Created**: March 09, 2026
**Version**: 1.0
