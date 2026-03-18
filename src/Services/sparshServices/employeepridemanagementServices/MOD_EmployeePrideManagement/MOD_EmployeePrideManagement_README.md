# Employee Pride Management Module (MOD_EmployeePrideManagement)

## Module Overview
The Employee Pride Management module captures and celebrates employee achievements, special moments, and organizational celebrations. It provides a platform to recognize employee contributions and foster a positive workplace culture.

## Module Code: **PRIDE**

## Scope
- Capture employee achievements and pride moments
- Employee recognition and celebration
- Photo/image gallery management
- Organizational milestone documentation
- Employee engagement and morale tracking

## Tables

### 1. MOMENT_PRIDE
**Purpose:** Stores employee pride moments and achievement records

| Column | Data Type | Notes |
|--------|-----------|-------|
| MOMENTPRIDE_ID | DECIMAL(38) | Pride Moment ID (PK) |
| MOMENTPRIDE_TITLE | VARCHAR(50) | Pride Moment Title |
| MOMENTPRIDE_BODY | NVARCHAR(MAX) | Complete Description/Body |
| MOMENTPRIDE_EMPSYSID | DECIMAL(38) | Employee System ID |
| MOMENTPRIDE_FOOTER | VARCHAR(500) | Footer/Additional Info |
| MOMENTPRIDE_LOCATION | VARCHAR(100) | Location of Pride Moment |
| MOMENTPRIDE_IMAGE | VARCHAR(200) | Image/Photo Path or URL |
| MOMENTPRIDE_MODIFIEDBY | BIGINT | Modified By (Employee ID) |
| MOMENTPRIDE_MODIFIEDON | DATETIME2(3) | Modified Timestamp |

**Primary Key:** MOMENTPRIDE_ID
**Indexes:** IX_MOMENT_PRIDE_EMPSYSID, IX_MOMENT_PRIDE_MODIFIEDON

## Key Stored Procedures

### usp_PRIDE_CreatePrideMoment
- **Purpose:** Create a new employee pride moment record
- **Parameters:** Title, Body, EmployeeSysId, Footer, Location, ImagePath, ModifiedBy
- **Returns:** PrideMomentId, ErrorMessage

### usp_PRIDE_GetPrideMomentsByEmployee
- **Purpose:** Retrieve all pride moments for an employee
- **Parameters:** EmployeeSysId
- **Returns:** Pride moments list

### usp_PRIDE_GetAllPrideMoments
- **Purpose:** Retrieve all pride moments (paginated)
- **Parameters:** PageNumber, PageSize
- **Returns:** Paginated pride moments list

### usp_PRIDE_UpdatePrideMoment
- **Purpose:** Update an existing pride moment
- **Parameters:** PrideMomentId, Title, Body, Footer, Location, ImagePath, ModifiedBy
- **Returns:** ErrorMessage

## Content Categories
- Performance Achievements
- Project Completions
- Award/Recognition
- Team Celebrations
- Client Appreciations
- Internal Promotions
- Training Completions
- Safety Milestones

## Business Rules
1. Title must be 50 characters or less
2. Image path must point to valid resource
3. Creator and modifier must be valid employees
4. Location is mandatory for physical events
5. Timestamps are auto-generated for creation

## Workflow
1. Employee or manager creates pride moment
2. Content is added to gallery
3. organization-wide visibility (with permissions)
4. Comments and reactions (future feature)
5. Archive old moments (2+ years)

## Data Retention
- Active records: Retain indefinitely
- Archived records: Retain for 5 years
- Images: Store with redundancy

## Security Considerations
- Control visibility based on department/role
- Validate image formats and sizes
- Prevent unauthorized modifications
- Maintain audit trail for all changes
- Backup image files regularly

## Integration Points
- Employee Master (for employee details)
- Department Master (for organizational context)
- Photo/Document storage system
- Internal communication system

## Usage Example
```sql
-- Create a new pride moment
EXEC usp_PRIDE_CreatePrideMoment
    @p_Title = 'Q1 Sales Target Achievement',
    @p_Body = 'Our sales team exceeded Q1 targets by 25%...',
    @p_EmployeeSysId = 1001,
    @p_Footer = 'Celebrating Team Excellence',
    @p_Location = 'Head Office, Conference Room A',
    @p_ImagePath = '/images/pride/q1_achievement_2026.jpg',
    @p_ModifiedBy = 1002,
    @p_PrideMomentId = @PrideMomentId OUTPUT,
    @p_ErrorMessage = @ErrorMsg OUTPUT;

-- Get all pride moments for an employee
EXEC usp_PRIDE_GetPrideMomentsByEmployee
    @p_EmployeeSysId = 1001;

-- Get paginated list of all pride moments
EXEC usp_PRIDE_GetAllPrideMoments
    @p_PageNumber = 1,
    @p_PageSize = 10;
```

## Related Modules
- MOD_MobileAppManagement
- MOD_EmployeeManagement
- MOD_DepartmentManagement

## Implementation Scripts
- **Tables Script:** MOD_EmployeePrideManagement_Tables.sql
- **Procedures Script:** MOD_EmployeePrideManagement_Procedures.sql

## Future Enhancements
- Add rating/like functionality
- Comment system for pride moments
- Social sharing capabilities
- Email notifications for related employees
- Dashboard for trending pride moments

**Last Updated:** March 9, 2026
**Version:** 1.0
