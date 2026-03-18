# InsuranceManagement Module

## Purpose
Comprehensive insurance enrollment, claims processing, and reimbursement management system.

## Functions

### fn_InsuranceGetEligibility
**Purpose**: Determine employee's insurance eligibility

**Parameters**:
- @p_EmpSysID: Employee system ID
- @p_CheckDate: Date to check eligibility

**Returns**: VARCHAR(50)
- 'ELIGIBLE': 6+ months service, active status
- 'PROBATION': 0-6 months service, active status
- 'INACTIVE': Inactive/left employee
- 'INELIGIBLE': Does not meet criteria

**Logic**:
- Requires 6 months service from joining date
- Only active employees eligible
- Queries HRDB.EMPLOYEE_MASTER

### fn_InsuranceCalculatePremium
**Purpose**: Calculate monthly insurance premium

**Parameters**:
- @p_InsurancePlanID: Insurance plan ID
- @p_CoverageType: 'EMPLOYEE' or 'FAMILY'
- @p_EmpSalary: Employee's basic salary

**Returns**: DECIMAL(19,0)

**Calculation**:
- Base = Salary × Premium Rate (%) / 100
- Family: Base × 1.5 (50% additional for dependents)
- Apply min/max limits from INSURANCE_PLAN_MASTER

### fn_InsuranceGetClaimReimbursement
**Purpose**: Calculate reimbursable amount for claims

**Parameters**:
- @p_ClaimAmount: Claim amount
- @p_ClaimType: 'IN_PATIENT', 'OUT_PATIENT', 'DENTAL', 'OPTICAL'
- @p_CopayPercentage: Employee copay % (default 20%)

**Returns**: DECIMAL(19,0)

**Coverage Limits**:
- DENTAL: 50% coverage
- OPTICAL: 75% coverage
- Other: 100% coverage

**Formula**: (ClaimAmount - Copay) × CoverageLimit%

## Stored Procedures

### usp_InsuranceEnrollPlan
**Purpose**: Enroll employee in insurance plan

**Parameters**:
- @p_EmpSysID, @p_InsurancePlanID, @p_CoverageType
- @p_EnrollmentDate, @p_EffectiveDate, @p_EnrolledBy
- @p_EnrollmentID (OUTPUT)

**Logic**:
1. Check employee eligibility
2. Fetch current salary from PAYDB
3. Calculate monthly premium
4. Verify no existing active enrollment
5. Insert enrollment record
6. Return enrollment ID

**Error Codes**: 50001-50002

### usp_InsuranceSubmitClaim
**Purpose**: Submit insurance claim for reimbursement

**Parameters**:
- @p_EmpSysID, @p_EnrollmentID, @p_ClaimType
- @p_ClaimAmount, @p_ServiceDate, @p_HospitalName
- @p_Remarks, @p_SubmittedBy
- @p_ClaimID (OUTPUT)

**Logic**:
1. Validate active enrollment exists
2. Validate claim amount > 0
3. Calculate reimbursable amount using function
4. Insert claim record with 'S' (Submitted) status
5. Return claim ID

**Error Codes**: 50003-50004

### usp_InsuranceApproveClaim
**Purpose**: Approve submitted claim

**Parameters**:
- @p_ClaimID, @p_ApprovedAmount
- @p_ApprovalRemarks, @p_ApprovedBy

**Logic**:
1. Retrieve claim and reimbursable amount
2. Validate approved amount ≤ reimbursable
3. Update claim to 'A' (Approved) status
4. Record approval details and date

**Error Codes**: 50005-50006

### usp_InsuranceProcessReimbursement
**Purpose**: Process approved claim for reimbursement

**Parameters**:
- @p_ClaimID, @p_ProcessedBy
- @p_ReimbursementID (OUTPUT)

**Logic**:
1. Fetch approved amount from claim
2. Validate claim is approved (status 'A')
3. Create reimbursement record (status 'P')
4. Update claim status to 'R' (Reimbursed)
5. Return reimbursement ID

**Error Codes**: 50007

## Triggers

### trg_InsuranceEnrollmentValidate
**Event**: INSTEAD OF INSERT on INSURANCE_ENROLLMENT

**Validations**:
- Insurance plan exists in INSURANCE_PLAN_MASTER
- Coverage type is valid (EMPLOYEE, FAMILY, DEPENDENT)

**Error Codes**: 50008-50009

### trg_InsuranceClaimAudit
**Event**: AFTER INSERT, UPDATE on INSURANCE_CLAIM

**Action**: Insert audit record with:
- Claim details
- Audit action (INSERT/UPDATE)
- Timestamp

**Target**: INSURANCE_CLAIM_AUDIT

### trg_InsuranceClaimValidateAmount
**Event**: INSTEAD OF INSERT on INSURANCE_CLAIM

**Validations**:
- Claim amount > 0
- Service date not in future

**Error Codes**: 50010-50011

## Claim Status Workflow

```
SUBMITTED (S)
    ↓
APPROVED (A)
    ↓
REIMBURSED (R)
```

## Error Codes Summary

| Code | Message | Cause |
|------|---------|-------|
| 50001 | Employee not eligible | Service < 6 months or inactive |
| 50002 | Already enrolled | Duplicate active enrollment |
| 50003 | Enrollment not found | No active enrollment for employee |
| 50004 | Invalid claim amount | Amount ≤ 0 |
| 50005 | Claim not found | CLAIM_ID doesn't exist |
| 50006 | Amount exceeds limit | Approved > reimbursable |
| 50007 | Claim not approved | Claim status ≠ 'A' |
| 50008 | Invalid plan | Plan doesn't exist |
| 50009 | Invalid coverage type | Type not EMPLOYEE/FAMILY/DEPENDENT |
| 50010 | Invalid claim amount | Amount ≤ 0 at insert |
| 50011 | Future service date | Service date > today |

## Cross-Database Dependencies

- **HRDB.EMPLOYEE_MASTER**: Employee status, DOJ
- **PAYDB.PAY_SALARY_MASTER**: Current salary for premium calculation
- **INSURANCE_PLAN_MASTER** (local): Plan rates and limits

## Notes

- All procedures wrapped in transactions for consistency
- Premium rates stored per plan for flexibility
- Audit trail maintained for claims
- Extensible coverage types via validation trigger
