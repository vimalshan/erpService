# CommunityModule Documentation

## Module Overview
The CommunityModule manages online communities, forums, and group memberships with role-based access control.

## Tables

### COMMUNITY_MAST
- **Purpose**: Community/forum definitions with privacy and ownership
- **Key Columns**: COMMUNITY_CODE, COMMUNITY_NAME, COMMUNITY_TYPE, PRIVACY_LEVEL, OWNER_ID

### COMMUNITY_MEMBERS
- **Purpose**: Community membership and role management
- **Relationship**: References COMMUNITY_MAST
- **Key Columns**: USER_SYSID, MEMBER_ROLE, MEMBER_STATUS

## Privacy Levels
- PUBLIC: Anyone can join and view
- PRIVATE: Invite-only, members only
- RESTRICTED: Approval required to join

## Member Roles
- ADMIN: Full control over community
- MODERATOR: Can manage content and members
- MEMBER: Regular member with posting rights
- GUEST: View-only access

## Member Status
- ACTIVE: Active member
- INACTIVE: Inactive but not removed
- SUSPENDED: Temporarily restricted
- REMOVED: No longer a member

## Deployment
```sql
:r "CommunityModule_Schema.sql"
```

## Setup Example
```sql
-- Create public community
INSERT INTO COMMUNITY_MAST (COMMUNITY_CODE, COMMUNITY_NAME, PRIVACY_LEVEL, OWNER_ID, CREATED_BY)
VALUES ('COM-001', 'Developers Community', 'PUBLIC', 1, 1);

-- Add member
INSERT INTO COMMUNITY_MEMBERS (COMMUNITY_ID, USER_SYSID, MEMBER_ROLE, MEMBER_STATUS, CREATED_BY)
SELECT COMMUNITY_ID, 100, 'MEMBER', 'ACTIVE', 1
FROM COMMUNITY_MAST WHERE COMMUNITY_CODE = 'COM-001';
```

---
**Created**: March 09, 2026
**Version**: 1.0
