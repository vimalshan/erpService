# Problem Management Module (MOD_ProblemManagement)

## Module Overview
The Problem Management module manages organizational problems, solutions, approvals, and communications. It provides end-to-end problem tracking from identification through resolution, with approval workflows and audience management.

## Module Code: **PROBLEM**

## Scope
- Problem creation and tracking
- Solution proposal and evaluation
- Multi-level approvals
- Audience/scope management
- Problem categorization (Function, General)
- Solution implementation tracking
- Comment/discussion on solutions
- Attachment management

## Tables

### 1. PROBLEM_FUNCTION
**Purpose:** Function categories for problem classification

| Column | Data Type | Notes |
|--------|-----------|-------|
| FUNCID | BIGINT | Function ID (PK) |
| FUNCNAME | VARCHAR(200) | Function Name |

**Primary Key:** FUNCID

### 2. PROBLEM_IMPACT
**Purpose:** Impact levels for problem severity classification

| Column | Data Type | Notes |
|--------|-----------|-------|
| IMPACT_ID | BIGINT | Impact ID (PK) |
| IMPACT_DESC | VARCHAR(200) | Impact Description |

**Primary Key:** IMPACT_ID

### 3. PROBLEM_MAIN
**Purpose:** Main problem records and tracking information

| Column | Data Type | Notes |
|--------|-----------|-------|
| PR_ID | BIGINT | Problem ID (PK) |
| PR_OWNER | BIGINT | Problem Owner (Posted By) |
| PR_ENTEREDBY | BIGINT | Problem Entered By |
| PR_DESCRIPTION | VARCHAR(255) | Problem Description |
| PR_RESPEXPBY | DATETIME2(3) | Response Expected By |
| PR_CATEGORY | CHAR(1) | 01=Function, 02=General |
| PR_SPECIALIZATION | BIGINT | Specialization ID |
| PR_IMPACT | VARCHAR(255) | Impact Description |
| PR_EXPRESULT | VARCHAR(255) | Expected Result |
| PR_ENTEREDON | DATETIME2(3) | Problem Entered On |
| PR_STATUS | CHAR(1) | P=Posted, A=Accepted, R=Rejected |
| PR_APPID | BIGINT | Last Approval ID |
| PR_STATEMENT | VARCHAR(255) | Problem Statement |
| PR_TYPE | CHAR(1) | Problem Type |
| PR_ATTACH | VARCHAR(255) | Attachment Reference |
| PR_PRBFLAG | CHAR(1) | Problem Flag |
| PR_PRBDESCRIPTION | VARCHAR(255) | Additional Description |
| PR_POSTFLAG | CHAR(1) | Post Flag |
| PR_QUESTION | VARCHAR(255) | Question |
| PR_UNITID | BIGINT | Unit ID (FK) |
| PR_SITEID | BIGINT | Site ID (FK) |
| PR_SOURCEID | BIGINT | Source ID |
| PR_MODBY | BIGINT | Modified By |
| PR_MODON | DATETIME2(3) | Modified On |

**Primary Key:** PR_ID
**Indexes:** IX_PROBLEM_MAIN_STATUS, IX_PROBLEM_MAIN_OWNER, IX_PROBLEM_MAIN_CATEGORY

### 4. PROBLEM_ATTACHMENT
**Purpose:** File attachments for problem records

| Column | Data Type | Notes |
|--------|-----------|-------|
| PRAT_ID | BIGINT | Attachment ID (PK) |
| PRAT_PRID | BIGINT | Problem ID (FK) |
| PRAT_FILENAME | VARCHAR(2000) | File Name |
| PRAT_ENTEREDON | DATETIME2(3) | Entered On |

**Primary Key:** PRAT_ID
**Foreign Key:** PRAT_PRID -> PROBLEM_MAIN(PR_ID)

### 5. PROBLEM_SOLUTION
**Purpose:** Solutions proposed for problems

| Column | Data Type | Notes |
|--------|-----------|-------|
| SOL_ID | BIGINT | Solution ID (PK) |
| SOL_PRID | BIGINT | Problem ID (FK) |
| SOL_DESCRIPTION | VARCHAR(255) | Solution Description |
| SOL_IMPLEMENTATION | CHAR(1) | Y=Yes, N=No |
| SOL_ENTEREDBY | BIGINT | Entered By |
| SOL_ENTEREDON | DATETIME2(3) | Entered On |
| SOL_ATTACH | VARCHAR(255) | Attachment Reference |

**Primary Key:** SOL_ID
**Foreign Key:** SOL_PRID -> PROBLEM_MAIN(PR_ID)

### 6. PROBLEM_APP
**Purpose:** Approval records for problems

| Column | Data Type | Notes |
|--------|-----------|-------|
| PRAPP_ID | BIGINT | Approval ID (PK) |
| PRAPP_PRID | BIGINT | Problem ID (FK) |
| PRAPP_BY | BIGINT | Approved By |
| PRAPP_ON | DATETIME2(3) | Approved On |
| PRAPP_STATUS | CHAR(1) | Approval Status |
| PRAPP_REASON | VARCHAR(255) | Reason |
| PRAPP_AUDFLAG | CHAR(1) | 0=All/1=Selected |

**Primary Key:** PRAPP_ID
**Foreign Key:** PRAPP_PRID -> PROBLEM_MAIN(PR_ID)

### 7. PROBLEM_APPAUDIENCE
**Purpose:** Audience scope for problem approvals

| Column | Data Type | Notes |
|--------|-----------|-------|
| PRAUD_ID | BIGINT | Audience ID (PK) |
| PRAUD_PRID | BIGINT | Reporting Unit ID (FK) |
| PRAUD_UNITID | INT | Problem ID |

**Primary Key:** PRAUD_ID
**Foreign Key:** PRAUD_PRID -> PROBLEM_MAIN(PR_ID)

### 8. SOLUTION_APP
**Purpose:** Approval records for solutions

| Column | Data Type | Notes |
|--------|-----------|-------|
| SOLAPP_ID | BIGINT | Approval ID (PK) |
| SOLAPP_SOLID | BIGINT | Solution ID (FK) |
| SOLAPP_BY | BIGINT | Approved By |
| SOLAPP_ON | DATETIME2(3) | Approved On |
| SOLAPP_STATUS | CHAR(1) | Approval Status |
| SOLAPP_REASON | VARCHAR(255) | Reason |
| SOLAPP_AUDFLAG | CHAR(1) | Audience Flag |

**Primary Key:** SOLAPP_ID
**Foreign Key:** SOLAPP_SOLID -> PROBLEM_SOLUTION(SOL_ID)

### 9. SOLUTION_COMMENT
**Purpose:** Comments on solutions

| Column | Data Type | Notes |
|--------|-----------|-------|
| SOLCOMMENT_ID | BIGINT | Comment ID (PK) |
| SOLCOMMENT_SOLID | BIGINT | Solution ID (FK) |
| SOLCOMMENT_TEXT | VARCHAR(500) | Comment Text |
| SOLCOMMENT_BY | BIGINT | Commented By |
| SOLCOMMENT_ON | DATETIME2(3) | Commented On |

**Primary Key:** SOLCOMMENT_ID
**Foreign Key:** SOLCOMMENT_SOLID -> PROBLEM_SOLUTION(SOL_ID)

## Key Stored Procedures

### usp_PROBLEM_CreateProblem
- **Purpose:** Create a new problem record
- **Parameters:** Owner, Description, Category, Impact, ExpectedResult, UnitId, SiteId, EnteredBy
- **Returns:** ProblemId, ErrorMessage

### usp_PROBLEM_RecordSolution
- **Purpose:** Record a solution for a problem
- **Parameters:** ProblemId, Description, EnteredBy
- **Returns:** SolutionId, ErrorMessage

### usp_PROBLEM_ApproveProblem
- **Purpose:** Approve a problem for posting
- **Parameters:** ProblemId, ApprovedBy, Status, Reason, AudienceFlag
- **Returns:** ApprovalId, ErrorMessage

### usp_PROBLEM_GetProblemsByStatus
- **Purpose:** Get problems filtered by status
- **Parameters:** Status
- **Returns:** Problem list

### usp_PROBLEM_GetSolutionsByProblem
- **Purpose:** Get all solutions for a problem
- **Parameters:** ProblemId
- **Returns:** Solution list

## Workflow
1. **Problem Creation:** Employee identifies and posts problem
2. **Solution Proposal:** Multiple teams propose solutions
3. **Solution Approval:** Management reviews and approves solutions
4. **Implementation:** Approved solution is implemented
5. **Closure:** Problem marked as resolved

## Status Values
- **P (Posted):** Initial status - awaiting approval
- **A (Accepted/Approved):** Approved for posting/implementation
- **R (Rejected):** Rejected - not approved

## Approval Levels
- Problem approval (multi-level if needed)
- Solution approval (solution-specific)
- Audience control (All vs. Selected)

## Business Rules
1. Problem must have owner and description
2. Response expected date should be future-dated
3. Solutions can only be added to existing problems
4. Approvals create audit trail
5. Implementation flag tracks solution usage

## Data Retention
- Closed problems: Retain for 2 years minimum
- Active problems: Retain indefinitely
- Comments and approvals: Maintain audit trail

## Security Considerations
- Control problem visibility by department/unit
- Validate audience assignments
- Maintain detailed audit trail
- Implement workflow validation
- Notify relevant stakeholders

## Usage Example
```sql
-- Create a problem
EXEC usp_PROBLEM_CreateProblem
    @p_Owner = 1001,
    @p_Description = 'Slow API response times',
    @p_Category = '01',
    @p_Impact = 'Affects user experience',
    @p_ExpectedResult = 'API response < 200ms',
    @p_UnitId = 10,
    @p_SiteId = 1,
    @p_EnteredBy = 1001,
    @p_ProblemId = @ProblemId OUTPUT,
    @p_ErrorMessage = @ErrorMsg OUTPUT;

-- Record a solution
EXEC usp_PROBLEM_RecordSolution
    @p_ProblemId = @ProblemId,
    @p_Description = 'Implement database query optimization',
    @p_EnteredBy = 1002,
    @p_SolutionId = @SolutionId OUTPUT,
    @p_ErrorMessage = @ErrorMsg OUTPUT;

-- Approve the problem
EXEC usp_PROBLEM_ApproveProblem
    @p_ProblemId = @ProblemId,
    @p_ApprovedBy = 1005,
    @p_Status = 'A',
    @p_Reason = 'Approved for implementation',
    @p_AudienceFlag = '0',
    @p_ApprovalId = @ApprovalId OUTPUT,
    @p_ErrorMessage = @ErrorMsg OUTPUT;
```

## Related Modules
- MOD_MobileAppManagement
- HR/Employee Management
- Department Management

## Implementation Scripts
- **Tables Script:** MOD_ProblemManagement_Tables.sql
- **Procedures Script:** MOD_ProblemManagement_Procedures.sql

**Last Updated:** March 9, 2026
**Version:** 1.0
