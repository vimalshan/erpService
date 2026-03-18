# GroupManagementModule Documentation

## Module Overview
The GroupManagementModule manages user groups, roles, and menu-based access control for the system.

## Tables

### GROUP_MAST
- **Purpose**: User group and role definitions
- **Key Columns**: GROUP_CODE, GROUP_NAME, IS_ADMIN, GROUP_STATUS

### GROUP_MENUMAP
- **Purpose**: Menu and feature access control per group
- **Relationship**: References GROUP_MAST
- **Key Columns**: MENU_CODE, CAN_VIEW, CAN_CREATE, CAN_EDIT, CAN_DELETE, CAN_APPROVE

## Permission Flags
- CAN_VIEW: Y/N - Can view the menu/feature
- CAN_CREATE: Y/N - Can create new records
- CAN_EDIT: Y/N - Can modify existing records
- CAN_DELETE: Y/N - Can delete records
- CAN_APPROVE: Y/N - Can approve requests

## Deployment
```sql
:r "GroupManagementModule_Schema.sql"
```

## Setup Example
```sql
-- Create admin group
INSERT INTO GROUP_MAST (GROUP_CODE, GROUP_NAME, IS_ADMIN, CREATED_BY)
VALUES ('ADMIN', 'Administrator', 'Y', 1);

-- Grant full access
INSERT INTO GROUP_MENUMAP (GROUP_ID, MENU_CODE, MENU_NAME, CAN_VIEW, CAN_CREATE, CAN_EDIT, CAN_DELETE, CAN_APPROVE, CREATED_BY)
SELECT GROUP_ID, 'MENU-ALL', 'All Features', 'Y', 'Y', 'Y', 'Y', 'Y', 1
FROM GROUP_MAST WHERE GROUP_CODE = 'ADMIN';
```

---
**Created**: March 09, 2026
**Version**: 1.0
