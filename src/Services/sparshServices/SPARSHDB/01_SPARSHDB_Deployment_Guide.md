# SPARSHDB - Modularized Database Structure

## Database Overview
**Database Name:** SPARSHDB (Scholarship and Mobile Application DB)
**Created:** March 9, 2026
**Version:** 1.0 (Modularized)

## Project Structure

```
SPARSHDB/
├── 00_SPARSHDB_Sequences_Setup.sql
├── 01_SPARSHDB_Deployment_Guide.md (this file)
├── MOD_MobileAppManagement/
│   ├── MOD_MobileAppManagement_Tables.sql
│   ├── MOD_MobileAppManagement_Procedures.sql
│   ├── MOD_MobileAppManagement_README.md
│   └── [Related files]
├── MOD_MobileExpenseManagement/
│   ├── MOD_MobileExpenseManagement_Tables.sql
│   ├── MOD_MobileExpenseManagement_Procedures.sql
│   ├── MOD_MobileExpenseManagement_README.md
│   └── [Related files]
├── MOD_EmployeePrideManagement/
│   ├── MOD_EmployeePrideManagement_Tables.sql
│   ├── MOD_EmployeePrideManagement_Procedures.sql
│   ├── MOD_EmployeePrideManagement_README.md
│   └── [Related files]
├── MOD_ProblemManagement/
│   ├── MOD_ProblemManagement_Tables.sql
│   ├── MOD_ProblemManagement_Procedures.sql
│   ├── MOD_ProblemManagement_README.md
│   └── [Related files]
└── MOD_ScholarshipManagement/
    ├── MOD_ScholarshipManagement_Tables.sql
    ├── MOD_ScholarshipManagement_Procedures.sql
    ├── MOD_ScholarshipManagement_README.md
    └── [Related files]
```

## Modules

### 1. Mobile App Management (MOD_MobileAppManagement)
**Module Code:** MAM
**Purpose:** Device registration and login tracking for mobile applications

**Tables:**
- MOB_APPDEVICE_DETAILS - Device registration
- MOB_LOGINDET - Login tracking
- MOBAPP_REGISTER - User registration

**Key Procedures:**
- usp_MOB_RegisterDevice
- usp_MOB_LogUserLogin
- usp_MOB_GetDevicesByEmployee

---

### 2. Mobile Expense Management (MOD_MobileExpenseManagement)
**Module Code:** EXP
**Purpose:** Track field expenses and attachments

**Tables:**
- MOBEXP_DET - Expense records
- MOBEXP_FILE - File attachments

**Key Procedures:**
- usp_EXP_RecordExpense
- usp_EXP_AttachExpenseFile
- usp_EXP_GetExpensesByTrip

---

### 3. Employee Pride Management (MOD_EmployeePrideManagement)
**Module Code:** PRIDE
**Purpose:** Capture employee achievements and celebrations

**Tables:**
- MOMENT_PRIDE - Pride moments

**Key Procedures:**
- usp_PRIDE_CreatePrideMoment
- usp_PRIDE_GetPrideMomentsByEmployee
- usp_PRIDE_UpdatePrideMoment

---

### 4. Problem Management (MOD_ProblemManagement)
**Module Code:** PROBLEM
**Purpose:** Track problems, solutions, and approvals

**Tables:**
- PROBLEM_MAIN - Main problems
- PROBLEM_SOLUTION - Proposed solutions
- PROBLEM_APP - Problem approvals
- PROBLEM_APPAUDIENCE - Approval audience
- PROBLEM_ATTACHMENT - File attachments
- PROBLEM_FUNCTION - Function categories
- PROBLEM_IMPACT - Impact levels
- SOLUTION_APP - Solution approvals
- SOLUTION_COMMENT - Solution comments

**Key Procedures:**
- usp_PROBLEM_CreateProblem
- usp_PROBLEM_RecordSolution
- usp_PROBLEM_ApproveProblem

---

### 5. Scholarship Management (MOD_ScholarshipManagement)
**Module Code:** SCHOLARSHIP
**Purpose:** Manage scholarship applications and disbursements

**Tables:**
- SCHOLARSHIP_MASTER - Scholarship schemes
- SCHOLARSHIP_ELIGIBILITY_CRITERIA - Eligibility rules
- SCHOLARSHIP_APPLICATION - Student applications
- SCHOLARSHIP_DISBURSEMENT - Disbursement records

**Key Functions:**
- fn_GetStudentEligibility
- fn_CalculateScholarshipAmount

**Key Procedures:**
- usp_SCHOLARSHIP_ApplyForScholarship
- usp_SCHOLARSHIP_ApproveScholarship
- usp_SCHOLARSHIP_ProcessDisbursement

---

## Deployment Instructions

### Step 1: Prerequisites
- SQL Server 2016 or later
- SPARSHDB database created
- Appropriate permissions to execute DDL and DML

### Step 2: Sequence Setup
Execute the sequences setup script first (MUST BE DONE BEFORE CREATING TABLES):
```sql
EXECUTE 00_SPARSHDB_Sequences_Setup.sql
```

### Step 3: Module Deployment (Execute in order)

#### Option A: Deploy All Modules
Each module should be deployed using this sequence:
1. Tables script
2. Procedures script

#### Option B: Deploy Specific Modules

**Mobile App Management:**
```sql
-- Execute in order:
1. MOD_MobileAppManagement_Tables.sql
2. MOD_MobileAppManagement_Procedures.sql
```

**Mobile Expense Management:**
```sql
-- Execute in order:
1. MOD_MobileExpenseManagement_Tables.sql
2. MOD_MobileExpenseManagement_Procedures.sql
```

**Employee Pride Management:**
```sql
-- Execute in order:
1. MOD_EmployeePrideManagement_Tables.sql
2. MOD_EmployeePrideManagement_Procedures.sql
```

**Problem Management:**
```sql
-- Execute in order:
1. MOD_ProblemManagement_Tables.sql
2. MOD_ProblemManagement_Procedures.sql
```

**Scholarship Management:**
```sql
-- Execute in order:
1. MOD_ScholarshipManagement_Tables.sql
2. MOD_ScholarshipManagement_Procedures.sql
```

### Step 4: Validation

After each module deployment, verify:
```sql
-- Check tables
SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'

-- Check procedures
SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'dbo' AND ROUTINE_TYPE = 'PROCEDURE'

-- Check functions
SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'dbo' AND ROUTINE_TYPE = 'FUNCTION'

-- Check sequences
SELECT * FROM sys.sequences
```

## Database Architecture

### Key Features
- **Modular Design:** Independent modules with minimal cross-dependencies
- **Standardized Naming:** Consistent prefix-based naming convention
- **Proper Indexing:** Primary and foreign key indexes on all tables
- **Audit Trail:** CreatedBy, CreatedOn, UpdatedBy, UpdatedOn fields
- **Data Integrity:** Foreign key constraints with appropriate cascade rules
- **Stored Procedures:** Comprehensive business logic encapsulation

### Naming Conventions
- **Modules:** MOD_[FunctionalArea]
- **Tables:** [MODULE_PREFIX]_[ENTITY_NAME] (e.g., MOB_APPDEVICE_DETAILS)
- **Procedures:** usp_[MODULE_PREFIX]_[ACTION]_[ENTITY]
- **Sequences:** seq_[ENTITY]_[ID]
- **Indexes:** IX_[TABLE]_[COLUMN]

### Data Relationships

#### Foreign Key Relationships
- MOBEXP_FILE → MOBEXP_DET (1:N)
- PROBLEM_ATTACHMENT → PROBLEM_MAIN (1:N)
- PROBLEM_SOLUTION → PROBLEM_MAIN (1:N)
- PROBLEM_APP → PROBLEM_MAIN (1:N)
- PROBLEM_APPAUDIENCE → PROBLEM_MAIN (1:N)
- SOLUTION_APP → PROBLEM_SOLUTION (1:N)
- SOLUTION_COMMENT → PROBLEM_SOLUTION (1:N)
- SCHOLARSHIP_ELIGIBILITY_CRITERIA → SCHOLARSHIP_MASTER (1:N)
- SCHOLARSHIP_APPLICATION → SCHOLARSHIP_MASTER (1:N)
- SCHOLARSHIP_DISBURSEMENT → SCHOLARSHIP_APPLICATION (1:N)
- SCHOLARSHIP_DISBURSEMENT → SCHOLARSHIP_MASTER (1:N)

## Maintenance

### Backup Strategy
- Full database backup weekly
- Transaction log backup every 4 hours
- Module-specific backups after major changes

### Index Maintenance
- Rebuild indexes monthly
- Update statistics weekly
- Monitor fragmentation

### Data Cleanup
- Archive closed problems (>2 years)
- Archive completed applications (>7 years per compliance)
- Delete orphaned file attachments

## Security Guidelines

### Access Control
- Role-based access to each module
- User permissions by function/unit
- Audit all modifications

### Data Protection
- Encrypt sensitive fields (IMEI, bank details, family income)
- Secure file storage with encryption
- Regular security audits

## Troubleshooting

### Common Issues

**Error: Sequence not found**
- Solution: Execute 00_SPARSHDB_Sequences_Setup.sql before creating tables

**Error: Foreign key constraint violation**
- Solution: Ensure parent records exist before inserting child records
- Check cascade delete rules

**Error: Table already exists**
- Solution: Scripts include DROP IF EXISTS - safe to rerun
- Verify no active transactions are using the objects

## Module Dependencies

```
MOB_MobileAppManagement (Independent)
↓
MOB_MobileExpenseManagement (Uses device info from MAM)
↓
PRIDE_EmployeePrideManagement (Independent)
↓
PROBLEM_ProblemManagement (Independent)
↓
SCHOLARSHIP_ScholarshipManagement (Independent)
```

## Rollback Procedure

If a module deployment fails:
1. Identify the failed module
2. Check error messages in SQL Server logs
3. Fix the issue in the script
4. Rerun the module deployment (DROP IF EXISTS handles cleanup)
5. Run validation queries

## Documentation

Each module includes:
- **README.md** - Comprehensive module documentation
- **Tables.sql** - DDL for all tables
- **Procedures.sql** - Stored procedures and functions
- Inline comments explaining business logic

## Contact & Support

- Database Administrator: [Contact Info]
- Module Owner: [Contact Info]
- Documentation Version: 1.0
- Last Updated: March 9, 2026

## Future Enhancements

### Planned Modules
- Payroll Management
- Attendance Management
- Leave Management
- Performance Management

### Enhancement List
- Add views for easier reporting
- Implement triggers for audit logging
- Create comprehensive stored procedures for data migration
- Add dashboard/BI integration points

---

**For detailed information about each module, refer to the respective README.md file in each module folder.**
