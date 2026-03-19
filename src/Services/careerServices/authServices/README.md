# AUTH Module - Authentication & User Management

## Overview
The AUTH module handles user registration, login, password management, and SMTP configuration for the Careers system.

## Tables

### REGISTRATION_TABLE
Primary table for user authentication and basic user information.
- **LOGIN_ID**: Unique identifier (PK)
- **FIRST_NAME, MIDDLE_NAME, LAST_NAME**: User name components
- **PERSONAL_MAIID**: Personal email (unique)
- **OFFICEAL_MAILID**: Official email
- **PASSWORD**: User password (plain text - should be encrypted)
- **USER_TYPE**: CANDIDATE, MANAGER, ADMIN
- **UserDept**: Department identifier
- **INITIAL**: Name initial
- **ISVALID**: Active status
- **ENTERED_ON, CHANGED_ON**: Audit timestamps
- **ENTERED_BY, CHANGED_BY**: Audit user tracking

### SMTP_DETAILS
Email server configuration for system notifications.
- **ID**: Configuration ID (PK)
- **SMTP_IP**: SMTP server IP address
- **PORTNO**: SMTP port number
- **ISVALID**: Active status

## Stored Procedures

### SP_LOGIN
Authenticates user with email and password.
```sql
EXEC SP_LOGIN @PERSONAL_MAIID, @Password, @Status OUTPUT
```
**Returns**: Success/Error status

### SP_CHANGE_PASSWORD
Allows user to change their password.
```sql
EXEC SP_CHANGE_PASSWORD @PERSONAL_MAIID, @OLDPASSWORD, @NEWPASSWORD, @STATUS OUTPUT
```
**Returns**: SUCCESS or error message

### SP_REGISTER
Registers a new candidate with job application details.
```sql
EXEC SP_REGISTER @PASSWORD, @FIRST_NAME, @MIDDLE_NAME, @LAST_NAME, @INITIAL, 
    @JOB_APPLIED_FOR, @PERSONAL_MAIID, @OFFICEAL_MAILID, @FUNCTIONS, 
    @INDUSTRY_DOMAIN, @EXPERIENCE_IN_YEAR, @CURRENT_LOCATION, 
    @PREFERED_LOCATION, @MOBILE_NO, @ISVALID, @RESULT OUTPUT
```
**Returns**: Login ID on success, 0 on failure

### SP_GET_LOGINID
Retrieves complete login and user information.
```sql
EXEC SP_GET_LOGINID @PERSONAL_MAIID
```
**Returns**: User record with all registration details

### SP_GET_PASSWORD
Password reset functionality (requires date of birth verification).
```sql
EXEC SP_GET_PASSWORD @PERSONAL_MAIID, @DATE_OF_BIRTH, @STATUS OUTPUT
```
**Returns**: Password if DOB matches

### SP_GET_SMTP
Retrieves SMTP configuration for email sending.
```sql
EXEC SP_GET_SMTP
```
**Returns**: Active SMTP configuration

### SP_GET_REGISTRATION_DETAILS
Retrieves user registration and job application details.
```sql
EXEC SP_GET_REGISTRATION_DETAILS @LOGIN_ID
```
**Returns**: Registration and applied job information

## Dependencies
- Requires PROFILE module for personal details lookup
- Requires CAREER module for job application details (REGISTERED_JOBS)
- Requires MASTERS module for job/function/domain lookups

## Key Features
- User self-registration
- Email-based authentication
- Password management
- SMTP configuration for notifications

## Security Notes
⚠️ **Issues Identified**:
- Passwords stored in plain text (should use hashing)
- No password complexity validation
- No account lockout on failed attempts
- No rate limiting on login attempts
- Missing encryption for sensitive data

## Future Enhancements
1. Implement bcrypt or PBKDF2 for password hashing
2. Add multi-factor authentication (MFA)
3. Implement OAuth 2.0 integration
4. Add audit logging for all authentication events
5. Implement retry limits and account lockout
6. Add JTWtoken generation for API authentication

---

**Files**:
- `schema.sql` - Table definitions
- `procedures.sql` - Stored procedures

**Database**: [Careers]
