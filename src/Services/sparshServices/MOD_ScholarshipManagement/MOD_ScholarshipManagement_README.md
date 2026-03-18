# Scholarship Management Module (MOD_ScholarshipManagement)

## Module Overview
The Scholarship Management module manages scholarship schemes, student applications, eligibility verification, approvals, and fund disbursements. It provides complete lifecycle management for employee/student scholarship programs.

## Module Code: **SCHOLARSHIP**

## Scope
- Scholarship scheme definition and management
- Eligibility criteria management
- Student application tracking
- Application approval workflow
- Scholarship amount calculation
- Fund disbursement management
- Multi-currency support
- Audit trail and reporting

## Tables

### 1. SCHOLARSHIP_MASTER
**Purpose:** Master data for scholarship schemes

| Column | Data Type | Notes |
|--------|-----------|-------|
| SCHOLARSHIP_ID | BIGINT | Scholarship ID (PK) |
| SCHOLARSHIP_NAME | VARCHAR(255) | Scholarship Name |
| SCHOLARSHIP_CODE | VARCHAR(50) | Scholarship Code (Unique) |
| SCHOLARSHIP_DESCRIPTION | NVARCHAR(MAX) | Description |
| SCHOLARSHIP_COVERAGE_PERCENT | DECIMAL(5,2) | Coverage % (e.g., 50.00) |
| SCHOLARSHIP_STATUS | CHAR(1) | A=Active, I=Inactive |
| SCHOLARSHIP_CREATEDBY | BIGINT | Created By |
| SCHOLARSHIP_CREATEDON | DATETIME2(3) | Created On |
| SCHOLARSHIP_MODIFIEDBY | BIGINT | Modified By |
| SCHOLARSHIP_MODIFIEDON | DATETIME2(3) | Modified On |

**Primary Key:** SCHOLARSHIP_ID
**Unique Key:** SCHOLARSHIP_CODE
**Indexes:** IX_SCHOLARSHIP_MASTER_STATUS, IX_SCHOLARSHIP_MASTER_CODE

### 2. SCHOLARSHIP_ELIGIBILITY_CRITERIA
**Purpose:** Eligibility criteria for scholarships

| Column | Data Type | Notes |
|--------|-----------|-------|
| ELIGIBILITY_ID | BIGINT | Eligibility Criteria ID (PK) |
| SCHOLARSHIP_ID | BIGINT | Scholarship ID (FK) |
| ELIGIBILITY_CRITERIA | VARCHAR(500) | Criteria Description |
| ELIGIBILITY_STATUS | CHAR(1) | A=Active, I=Inactive |
| ELIGIBILITY_CREATEDBY | BIGINT | Created By |
| ELIGIBILITY_CREATEDON | DATETIME2(3) | Created On |

**Primary Key:** ELIGIBILITY_ID
**Foreign Key:** SCHOLARSHIP_ID -> SCHOLARSHIP_MASTER
**Indexes:** IX_ELIGIBILITY_SCHID, IX_ELIGIBILITY_STATUS

### 3. SCHOLARSHIP_APPLICATION
**Purpose:** Student scholarship applications

| Column | Data Type | Notes |
|--------|-----------|-------|
| APPLICATION_ID | BIGINT | Application ID (PK) |
| EMP_STUDENT_ID | BIGINT | Student/Employee ID |
| SCHOLARSHIP_ID | BIGINT | Scholarship ID (FK) |
| APPLICATION_DATE | DATE | Application Date |
| FAMILY_INCOME | DECIMAL(19,0) | Family Annual Income |
| APPLICATION_STATUS | CHAR(1) | S=Submitted, A=Approved, R=Rejected, C=Closed |
| APPROVED_AMOUNT | DECIMAL(19,0) | Approved Amount |
| APPROVED_BY | BIGINT | Approved By |
| APPROVAL_DATE | DATETIME2(3) | Approval Date |
| REMARKS | VARCHAR(500) | Remarks |
| CREATED_BY | BIGINT | Created By |
| CREATED_ON | DATETIME2(3) | Created On |
| UPDATED_BY | BIGINT | Updated By |
| UPDATED_ON | DATETIME2(3) | Updated On |

**Primary Key:** APPLICATION_ID
**Foreign Key:** SCHOLARSHIP_ID -> SCHOLARSHIP_MASTER
**Indexes:** IX_APPLICATION_STUDENTID, IX_APPLICATION_SCHID, IX_APPLICATION_STATUS, IX_APPLICATION_DATE

### 4. SCHOLARSHIP_DISBURSEMENT
**Purpose:** Scholarship disbursement transactions

| Column | Data Type | Notes |
|--------|-----------|-------|
| DISBURSEMENT_ID | BIGINT | Disbursement ID (PK) |
| APPLICATION_ID | BIGINT | Application ID (FK) |
| STUDENT_ID | BIGINT | Student/Employee ID |
| SCHOLARSHIP_ID | BIGINT | Scholarship ID (FK) |
| DISBURSEMENT_AMOUNT | DECIMAL(19,0) | Amount Disbursed |
| DISBURSEMENT_DATE | DATETIME2(3) | Disbursement Date |
| DISBURSEMENT_STATUS | CHAR(1) | P=Pending, D=Disbursed, C=Cancelled |
| REFERENCE_NUMBER | VARCHAR(100) | Payment Reference Number |
| BANK_DETAILS | VARCHAR(500) | Bank Account Details |
| CREATED_BY | BIGINT | Created By |
| CREATED_ON | DATETIME2(3) | Created On |
| UPDATED_BY | BIGINT | Updated By |
| UPDATED_ON | DATETIME2(3) | Updated On |

**Primary Key:** DISBURSEMENT_ID
**Foreign Keys:** APPLICATION_ID -> SCHOLARSHIP_APPLICATION, SCHOLARSHIP_ID -> SCHOLARSHIP_MASTER
**Indexes:** IX_DISBURSEMENT_APPID, IX_DISBURSEMENT_STUDENTID, IX_DISBURSEMENT_STATUS, IX_DISBURSEMENT_DATE

## Key Functions

### fn_GetStudentEligibility
- **Purpose:** Check if a student is eligible for a scholarship
- **Parameters:** StudentID, SchemeID
- **Returns:** VARCHAR(50) - ELIGIBLE, INELIGIBLE, or ERROR

### fn_CalculateScholarshipAmount
- **Purpose:** Calculate scholarship amount based on coverage percentage
- **Parameters:** SchemeID, StudentAnnualFees
- **Returns:** DECIMAL(19,0) - Calculated amount

## Key Stored Procedures

### usp_SCHOLARSHIP_ApplyForScholarship
- **Purpose:** Submit a scholarship application for a student
- **Parameters:** StudentID, ScholarshipID, ApplicationDate, FamilyIncome, ApplicantID
- **Returns:** ApplicationID, ErrorMessage
- **Validation:** Checks student eligibility before insertion

### usp_SCHOLARSHIP_ApproveScholarship
- **Purpose:** Approve a scholarship application and create disbursement
- **Parameters:** ApplicationID, ApprovedBy, ApprovedAmount (optional)
- **Returns:** ErrorMessage
- **Actions:** Updates application, calculates approved amount, creates disbursement record

### usp_SCHOLARSHIP_ProcessDisbursement
- **Purpose:** Process disbursement of approved scholarship
- **Parameters:** DisbursementID, ProcessedBy, ReferenceNumber
- **Returns:** ErrorMessage
- **Validation:** Verifies disbursement is pending before processing

### usp_SCHOLARSHIP_GetApplicationsByStatus
- **Purpose:** Retrieve applications filtered by status
- **Parameters:** Status
- **Returns:** Application list with scholarship details

### usp_SCHOLARSHIP_GetStudentApplications
- **Purpose:** Retrieve all applications for a specific student
- **Parameters:** StudentID
- **Returns:** Application and scholarship details

## Workflow

### Application Process
1. **Student Application:** Submit application for a scholarship
2. **Eligibility Check:** System validates eligibility criteria
3. **Amount Calculation:** Calculate approved amount based on coverage
4. **Approval:** Manager/Admin approves application
5. **Disbursement:** Create disbursement record for approved amount
6. **Payment:** Process payment to student
7. **Closure:** Mark application as processed/closed

### Status Codes
- **S (Submitted):** Application submitted, pending review
- **A (Approved):** Approved and ready for disbursement
- **R (Rejected):** Application rejected with remarks
- **C (Closed):** Processed/Closed

### Disbursement Status
- **P (Pending):** Awaiting payment processing
- **D (Disbursed):** Payment completed
- **C (Cancelled):** Disbursement cancelled

## Business Rules
1. Eligibility criteria must be active for consideration
2. Coverage percentage determines disbursement amount
3. Family income used for priority/selection criteria
4. One application per student per scholarship per period
5. Application date cannot be future-dated
6. Approved amount cannot exceed calculated amount (unless override)
7. Disbursement can only be created for approved applications
8. Bank details required for disbursement processing

## Data Retention
- Active applications: Retain indefinitely
- Closed applications: Retain for 7 years (audit/compliance)
- Disbursement records: Retain permanently

## Security Considerations
- Encrypt bank account details
- Implement role-based access (approver, processor, viewer)
- Maintain complete audit trail
- Validate all financial transactions
- Regular reconciliation with finance
- Secure payment gateway integration

## Integration Points
- Employee/Student Master data
- Finance/Accounts system
- HR Information System
- Payment processing system
- Reporting/BI systems

## Usage Example
```sql
-- Apply for a scholarship
EXEC usp_SCHOLARSHIP_ApplyForScholarship
    @p_StudentID = 1001,
    @p_ScholarshipID = 100,
    @p_ApplicationDate = '2026-03-09',
    @p_FamilyIncome = 500000,
    @p_ApplicantID = 1001,
    @p_ApplicationID = @ApplicationID OUTPUT,
    @p_ErrorMessage = @ErrorMsg OUTPUT;

-- Approve the scholarship
EXEC usp_SCHOLARSHIP_ApproveScholarship
    @p_ApplicationID = @ApplicationID,
    @p_ApprovedBy = 1005,
    @p_ApprovedAmount = NULL, -- Use calculated amount
    @p_ErrorMessage = @ErrorMsg OUTPUT;

-- Process disbursement
EXEC usp_SCHOLARSHIP_ProcessDisbursement
    @p_DisbursementID = 500,
    @p_ProcessedBy = 1006,
    @p_ReferenceNumber = 'TRF20260309001',
    @p_ErrorMessage = @ErrorMsg OUTPUT;

-- Get applications by status
EXEC usp_SCHOLARSHIP_GetApplicationsByStatus
    @p_Status = 'A';

-- Get student's applications
EXEC usp_SCHOLARSHIP_GetStudentApplications
    @p_StudentID = 1001;
```

## Related Modules
- MOD_MobileAppManagement
- Employee/HR Management
- Finance Module

## Implementation Scripts
- **Tables Script:** MOD_ScholarshipManagement_Tables.sql
- **Procedures Script:** MOD_ScholarshipManagement_Procedures.sql

## Future Enhancements
- Installment-based disbursement
- Scholarship review and renewal cycles
- Performance tracking (GPA-based eligibility)
- Appeal process for rejected applications
- Integration with payroll for fund transfer

**Last Updated:** March 9, 2026
**Version:** 1.0
