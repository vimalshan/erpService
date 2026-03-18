# Mobile App Management Module (MOD_MobileAppManagement)

## Module Overview
The Mobile App Management module handles device registration, authentication, and login tracking for mobile applications. It provides functionality for managing mobile device details and monitoring user login activities.

## Module Code: **MAM**

## Scope
- Mobile device registration and management
- Device authentication and activation
- Login tracking and session management
- Multi-device support per employee

## Tables

### 1. MOB_APPDEVICE_DETAILS
**Purpose:** Stores mobile device details for registered applications

| Column | Data Type | Notes |
|--------|-----------|-------|
| MD_EMPSYSID | DECIMAL(38) | Employee System ID (PK) |
| MD_DEVICEID | VARCHAR(200) | Device Identifier (PK) |
| MD_ACTIVE | CHAR(1) | Y/N - Active status |
| MD_DEVICETYPE | CHAR(1) | A=Android, I=iOS |
| MD_IMEINO | VARCHAR(200) | Device IMEI Number |
| MD_CREATEDON | DATETIME2(3) | Creation Timestamp |
| MD_UPDATEDBY | DECIMAL(38) | Updated By Employee ID |
| MD_UPDATEDON | DATETIME2(3) | Last Updated Timestamp |

**Primary Key:** (MD_EMPSYSID, MD_DEVICEID)
**Indexes:** IX_MOB_APPDEVICE_ACTIVE, IX_MOB_APPDEVICE_DEVICE

### 2. MOB_LOGINDET
**Purpose:** Tracks mobile application login history

| Column | Data Type | Notes |
|--------|-----------|-------|
| LD_LOGINID | DECIMAL(38) | Login ID (PK) |
| LD_USERSYSID | DECIMAL(38) | User System ID |
| LD_DEVICEID | VARCHAR(200) | Device ID |
| LD_LOGON | DATETIME2(3) | Login DateTime |
| LD_GUID | VARCHAR(255) | Unique Sequential GUID |
| LD_IMEINO | VARCHAR(200) | Device IMEI Number |
| LD_DEVICETYPE | CHAR(1) | A=Android, I=iOS |

**Primary Key:** LD_LOGINID
**Indexes:** IX_MOB_LOGIN_USERID, IX_MOB_LOGIN_DEVICE, IX_MOB_LOGIN_LOGON

### 3. MOBAPP_REGISTER
**Purpose:** Mobile application user registration management

| Column | Data Type | Notes |
|--------|-----------|-------|
| REGISTER_ID | BIGINT | Registration ID (PK) |
| REGISTER_EMPSYSID | BIGINT | Employee System ID |
| REGISTER_USERID | VARCHAR(255) | User ID |
| REGISTER_USERSYSID | BIGINT | User System ID |
| REGISTER_USERTYPE | CHAR(1) | User Type |
| REGISTER_PINNO | BIGINT | Registration PIN |
| REGISTER_PINGENERATEDON | DATETIME2(3) | PIN Generated Timestamp |
| REGISTER_UPDATEDON | DATETIME2(3) | Last Updated Timestamp |
| REGISTER_STATUS | CHAR(1) | P=Pending, R=Registered, C=Closed |
| REGISTER_MOBILENO | VARCHAR(255) | Mobile Number |
| REGISTER_IMEINO | VARCHAR(255) | Device IMEI Number |
| REGISTER_GUID | CHAR(1) | GUID |
| REGISTER_DEVICEID | VARCHAR(255) | Device ID |
| REGISTER_DTYPE | CHAR(1) | A=Android, I=iOS |

**Primary Key:** REGISTER_ID
**Indexes:** IX_MOBAPP_REG_STATUS, IX_MOBAPP_REG_USERID

## Key Stored Procedures

### usp_MOB_RegisterDevice
- **Purpose:** Register or update a mobile device for an employee
- **Parameters:** EmpSysId, DeviceId, DeviceType, ImeiNo, UpdatedBy
- **Returns:** ErrorMessage

### usp_MOB_LogUserLogin
- **Purpose:** Log user login event
- **Parameters:** UserSysId, DeviceId, ImeiNo, DeviceType
- **Returns:** LoginId, ErrorMessage

### usp_MOB_GetDevicesByEmployee
- **Purpose:** Retrieve all devices registered by an employee
- **Parameters:** EmpSysId
- **Returns:** Device list

## Dependencies
- Employee management system (for EmpSysId)
- User management system (for UserId, UserSysId)

## Data Retention
- Login records: Archive after 12 months
- Device registrations: Retain active records indefinitely

## Security Considerations
- Encrypt device IMEI numbers
- Validate device authenticity
- Log all registration/deregistration activities
- Implement rate limiting on login attempts

## Usage Example
```sql
-- Register a new device
EXEC usp_MOB_RegisterDevice 
    @p_EmpSysId = 1001,
    @p_DeviceId = 'DEVICE_001',
    @p_DeviceType = 'A',
    @p_ImeiNo = '123456789012345',
    @p_UpdatedBy = 1001,
    @p_ErrorMessage = @ErrorMsg OUTPUT;

-- Log a login
EXEC usp_MOB_LogUserLogin 
    @p_UserSysId = 1001,
    @p_DeviceId = 'DEVICE_001',
    @p_ImeiNo = '123456789012345',
    @p_DeviceType = 'A',
    @p_LoginId = @LoginId OUTPUT,
    @p_ErrorMessage = @ErrorMsg OUTPUT;
```

## Related Modules
- MOD_MobileExpenseManagement
- MOD_EmployeePrideManagement

## Implementation Scripts
- **Tables Script:** MOD_MobileAppManagement_Tables.sql
- **Procedures Script:** MOD_MobileAppManagement_Procedures.sql

**Last Updated:** March 9, 2026
**Version:** 1.0
