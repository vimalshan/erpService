# Auth Service Database

## Overview
The Auth (Authentication) microservice manages all user authentication, login management, and access control.

## Database: `DELL_RTU_AUTH`

## Tables

### LOGIN_MASTER
Stores user login credentials and authentication information.

**Key Fields:**
- `LOGIN_ID` (PK) - Unique user identifier
- `BRANCH` - Branch/location assignment
- `LOGIN_NAME` - Username
- `PASWORD` - Encrypted password
- `USER_TYPE` - Type of user (Admin, Engineer, Manager, etc.)
- `KIT_CODE` - Associated toolkit/equipment
- `IMEI_NO` - Mobile device IMEI number
- `FLAG` - Active/inactive status
- `MOBILE_NO` - Contact number

**Notes:**
- Password should be encrypted/hashed in production
- Consider adding password reset tokens, 2FA columns
- Add last_login and failed_attempts tracking

### LOGIN_TYPE_MASTER
Defines different types of login roles/permissions.

**Key Fields:**
- `LOGIN_TYPE` (PK) - Type identifier
- `LOGIN_TYPE_NAME` - Display name (e.g., "Administrator", "Field Engineer")
- `ISVALID` - Whether type is active

### LOGIN_ERROR
Tracks login failures for security monitoring.

**Key Fields:**
- `ID` (PK) - Log entry ID
- `LOGINID` - User attempting login
- `ERROR` - Error type/code
- `ENTERDATE` - Timestamp of failed attempt

**Uses:**
- Detect brute force attacks
- Monitor failed login patterns
- Security audit trails

## Common Queries

```sql
-- Authenticate user
SELECT * FROM LOGIN_MASTER 
WHERE LOGIN_ID = @LoginId AND PASWORD = @Password AND ISVALID = 1;

-- Get user by branch
SELECT * FROM LOGIN_MASTER 
WHERE BRANCH = @Branch AND FLAG = 'Y';

-- Recent failed logins
SELECT TOP 10 * FROM LOGIN_ERROR 
WHERE LOGINID = @LoginId 
ORDER BY ENTERDATE DESC;
```

## API Patterns

```
GET    /auth/verify/{loginId}          - Verify user exists
POST   /auth/login                      - Authenticate user
POST   /auth/logout                     - Logout user
GET    /auth/user/{loginId}             - Get user details
PUT    /auth/user/{loginId}             - Update user
POST   /auth/change-password            - Change password
```

## Security Considerations

1. **Password Storage**: Implement bcrypt/PBKDF2 hashing
2. **Session Management**: Add session/token tables
3. **Audit Logging**: Track all authentication events
4. **Rate Limiting**: Prevent brute force attacks
5. **Multi-Factor Auth**: Add 2FA table structure
6. **Encryption**: Encrypt sensitive fields

## Integration Points

- Required by: All other microservices
- Calls: None (independent service)
- Used by: API Gateway, Web/Mobile Apps

## Setup

Run: `Auth/schema/01_auth_tables.sql`

## Future Enhancements

- [ ] Add OAuth2/OpenID Connect integration tables
- [ ] Implement JWT token storage
- [ ] Add session tracking table
- [ ] Implement role-based access control (RBAC) tables
- [ ] Add audit log table
- [ ] Multi-tenancy support
