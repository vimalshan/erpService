# AccidentManagement Module

## Purpose
Manages accident/injury incidents at workplace with detailed reporting and tracking.

## Database Setup
**Connection String**: `Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name="SQL Server Management Studio";Command Timeout=0`

**Database**: HEALTHDB

## Tables

### ACCIDENT_SEVERITY (Reference Master)
- **Primary Key**: SEVERITY_ID (BIGINT IDENTITY)
- **Purpose**: Master list of accident severity levels
- **Fields**: SEVERITY_ID, SEVERITY_CODE, SEVERITY_NAME, DESCRIPTION, Audit columns
- **Status**: ✅ Active

### ACCIDENT_STATUS (Reference Master)
- **Primary Key**: STATUS_ID (BIGINT IDENTITY)
- **Purpose**: Master list of accident status types
- **Fields**: STATUS_ID, STATUS_CODE, STATUS_NAME, DESCRIPTION, Audit columns
- **Status**: ✅ Active

### CATEGORY_INJURY (Reference Master)
- **Primary Key**: CAT_ID (BIGINT IDENTITY)
- **GUID**: CAT_GUID (UNIQUEIDENTIFIER)
- **Purpose**: Master list of injury categories
- **Fields**: CAT_ID, CAT_GUID, CAT_NAME, DESCRIPTION, CreatedDate, UpdatedDate, CreatedBy, UpdatedBy, IsDeleted
- **Indexes**: CAT_GUID
- **Status**: ✅ Active

### NATURE_INJURY (Reference Master)
- **Primary Key**: NATURE_ID (BIGINT IDENTITY)
- **GUID**: NATURE_GUID (UNIQUEIDENTIFIER)
- **Purpose**: Types/nature of injuries
- **Fields**: NATURE_ID, NATURE_GUID, NATURE_NAME, DESCRIPTION, Audit columns
- **Indexes**: NATURE_GUID
- **Status**: ✅ Active

### ACC_CONTRCT_LST (Contractor Master)
- **Primary Key**: ACL_ID (BIGINT IDENTITY)
- **GUID**: ACL_GUID (UNIQUEIDENTIFIER)
- **Purpose**: Contractor list for accident tracking
- **Fields**: ACL_ID, ACL_GUID, ACL_CONT_NAM, ACL_CONT_ID, ACL_STATUS, Audit columns
- **Check Constraint**: ACL_STATUS IN ('A' = Active, 'I' = Inactive)
- **Status**: ✅ Active

### ACC_PERS_INJ (Injured Person)
- **Primary Key**: API_ID (BIGINT IDENTITY)
- **GUID**: API_GUID (UNIQUEIDENTIFIER)
- **Purpose**: Track persons injured in accidents
- **Fields**: API_ID, API_GUID, API_SRL_NUM, API_PERS_NAM, API_EMP_STATUS, Audit columns
- **Check Constraint**: API_EMP_STATUS IN ('S' = Staff, 'C' = Contractor)
- **Status**: ✅ Active

### DAILY_ACC_FIR (Accident Report - Main Entity)
- **Primary Key**: DAF_ID (BIGINT IDENTITY)
- **GUID**: DAF_GUID (UNIQUEIDENTIFIER)
- **Unique**: DAF_ACC_NUM (Accident Number)
- **Purpose**: Daily Accident First Information Report
- **Fields**: 
  - Employee info (DAF_EMP_NUM, DAF_EMP_NAM, DAF_WRK_NAM)
  - Accident details (DAF_ACC_DAT, DAF_ACC_LOC, DAF_ACC_NUM)
  - Contractor info (DAF_CONT_ID, DAF_CONT_NAM)
  - Injury details (DAF_NATURE_INJ, DAF_BODY_PART, DAF_CAT_INJ, DAF_NAT_INJ)
  - Treatment info (DAF_MEDCENTRE_NAM, DAF_TRT_GIVEN, DAF_MEDCENTRE_DAT)
  - Tracking (DAF_COM_COD, DAF_ENT_USR, DAF_ENT_NUM, DAF_ENT_DATE)
  - Prevention (DAF_PRV_MES, DAF_CAU_INC)
  - Severity & Status (DAF_SEVERITY_ID, DAF_STATUS_ID)
  - Audit columns
- **Relationships**: 
  - FK → CATEGORY_INJURY (DAF_CAT_INJ)
  - FK → NATURE_INJURY (DAF_NAT_INJ)
  - FK → ACCIDENT_SEVERITY (DAF_SEVERITY_ID)
  - FK → ACCIDENT_STATUS (DAF_STATUS_ID)
- **Indexes**: DAF_COM_COD, DAF_ACC_DAT, DAF_EMP_NUM, DAF_GUID
- **Status**: ✅ Active

### AUDIT_LOG (Audit Trail)
- **Primary Key**: AUDIT_ID (BIGINT IDENTITY)
- **Purpose**: Tracks all changes to critical tables
- **Fields**: AUDIT_ID, TABLE_NAME, RECORD_ID, OPERATION (INSERT/UPDATE/DELETE), OLD_VALUES, NEW_VALUES, CHANGED_BY, CHANGED_DATE, IP_ADDRESS
- **Indexes**: TABLE_NAME, CHANGED_DATE
- **Status**: ✅ Active

## Key Workflows

### 1. Report Accident (Write)
- Create DAILY_ACC_FIR record with full incident details
- Link to injury category and nature (foreign keys enforced)
- Identify contractor if involved
- Set initial severity and status
- Audit trail automatically created

### 2. Track Injury (Read/Update)
- Person injured details (ACC_PERS_INJ)
- Body part affected
- Nature of injury (linked to NATURE_INJURY)
- Treatment location and procedures
- Status transitions tracked in AUDIT_LOG

### 3. Query Accidents
- By company code (DAF_COM_COD)
- By accident date range (DAF_ACC_DAT)
- By employee (DAF_EMP_NUM)
- By severity levels
- By status

## Schema Enhancements (v2.0)

### Added Features:
✅ **GUID Support**: All master and transactional tables include UNIQUEIDENTIFIER for microservices compatibility
✅ **Audit Columns**: CreatedBy, CreatedDate, UpdatedBy, UpdatedDate on all tables
✅ **Soft Delete**: IsDeleted flag on all tables for logical deletion
✅ **Reference Masters**: ACCIDENT_SEVERITY and ACCIDENT_STATUS for better data integrity
✅ **Foreign Keys**: Enforced relationships between DAILY_ACC_FIR and reference tables
✅ **Check Constraints**: Validates ACL_STATUS (A/I) and API_EMP_STATUS (S/C)
✅ **Identity Columns**: All ID fields now auto-increment (IDENTITY)
✅ **Helpful Indexes**: GUID, Date, Date+Table combinations for query optimization
✅ **Audit Log Table**: Tracks inserts, updates, deletes across all critical tables

### TODO Items:
1. **Populate ACCIDENT_SEVERITY** with severity levels:
   - 1: Critical (Fatality/Hospitalization)
   - 2: High (Severe injury requiring treatment)
   - 3: Medium (Medical attention required)
   - 4: Low (First aid only)

2. **Populate ACCIDENT_STATUS** with status types:
   - 1: New (Newly reported)
   - 2: InProgress (Under investigation)
   - 3: Resolved (Investigation complete)
   - 4: Closed (Case closed)

3. **Populate CATEGORY_INJURY** with injury categories:
   - Chemical Burn, Electrical Burn, Fracture, Cut/Laceration, Crush Injury, Eye Injury, etc.

4. **Populate NATURE_INJURY** with injury types:
   - Deep, Superficial, Severe, Penetrating, Blunt Force, etc.

5. **Create audit triggers** to automatically populate AUDIT_LOG table

6. **Create stored procedures**:
   - sp_GetAccidentsByDate
   - sp_GetAccidentsByEmployee
   - sp_GetAccidentsByStatus
   - sp_CreateAccidentReport
   - sp_UpdateAccidentStatus

## Dependencies
- None (standalone module)
- Assumes HEALTHDB database exists

## Notes
- DAF_ACC_NUM must be unique for each accident
- Supports both employees (S) and contractors (C)
- Medical centre information required for reporting compliance
- All date fields use DATETIME2(3) for millisecond precision
- All changes are tracked in AUDIT_LOG table
- New records default to Status_ID=1 (New) and Severity_ID=1 (by default)

## Version History
- **v1.0** (2026-03-09): Initial schema created
  - Basic tables: CATEGORY_INJURY, NATURE_INJURY, ACC_CONTRCT_LST, ACC_PERS_INJ, DAILY_ACC_FIR
  - Primary indexes on company code, date, employee number
  
- **v2.0** (2026-03-13): Schema enhancements
  - Added GUID columns for microservices
  - Added audit columns to all tables
  - Added soft delete functionality
  - Added ACCIDENT_SEVERITY and ACCIDENT_STATUS reference tables
  - Added foreign key constraints
  - Added AUDIT_LOG table for compliance
  - Added check constraints for status validation
  - Converted DECIMAL(38) to BIGINT for consistency
  - Added comprehensive indexes for query optimization

