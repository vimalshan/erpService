-- HEALTHDB Stored Procedures, Functions & Triggers
-- Health Insurance & Medical Claims Management System
-- Created: February 13, 2026

USE HEALTHDB;
GO

-- =====================================================
-- FUNCTIONS
-- =====================================================

-- Function: Get Employee Insurance Eligibility
IF OBJECT_ID('dbo.fn_GetInsuranceEligibility', 'FN') IS NOT NULL
    DROP FUNCTION dbo.fn_GetInsuranceEligibility;
GO

CREATE FUNCTION dbo.fn_GetInsuranceEligibility (
    @p_EmpSysID BIGINT,
    @p_CheckDate DATETIME2(3)
)
RETURNS VARCHAR(50)
AS
BEGIN
    DECLARE @Status VARCHAR(50) = 'INELIGIBLE';
    DECLARE @EmpStatus VARCHAR(1);
    DECLARE @ServiceMonths INT;
    DECLARE @EmpDOJ DATE;
    
    BEGIN TRY
        -- Get employee details from HRDB
        SELECT 
            @EmpStatus = EMP_STATUS,
            @EmpDOJ = EMP_DOJ
        FROM HRDB.dbo.EMPLOYEE_MASTER 
        WHERE EMP_SYS_ID = @p_EmpSysID;
        
        IF @EmpStatus = 'A'  -- Active
        BEGIN
            SET @ServiceMonths = DATEDIFF(MONTH, @EmpDOJ, @p_CheckDate);
            
            -- Eligible after probation or 6 months service
            IF @ServiceMonths >= 6
                SET @Status = 'ELIGIBLE';
            ELSE IF @ServiceMonths >= 0
                SET @Status = 'PROBATION';
        END
        ELSE IF @EmpStatus = 'L'  -- Left
            SET @Status = 'INACTIVE';
    END TRY
    BEGIN CATCH
        SET @Status = 'ERROR';
    END CATCH
    
    RETURN @Status;
END;
GO

-- Function: Calculate Insurance Premium
IF OBJECT_ID('dbo.fn_CalculateInsurancePremium', 'FN') IS NOT NULL
    DROP FUNCTION dbo.fn_CalculateInsurancePremium;
GO

CREATE FUNCTION dbo.fn_CalculateInsurancePremium (
    @p_InsurancePlanID BIGINT,
    @p_CoverageType VARCHAR(20),  -- 'EMPLOYEE', 'FAMILY'
    @p_EmpSalary DECIMAL(19,0)
)
RETURNS DECIMAL(19,0)
AS
BEGIN
    DECLARE @Premium DECIMAL(19,0) = 0;
    DECLARE @PremiumRate DECIMAL(5,2);
    DECLARE @MinPremium DECIMAL(19,0);
    DECLARE @MaxPremium DECIMAL(19,0);
    
    BEGIN TRY
        SELECT 
            @PremiumRate = ISNULL(INSURANCE_PREMIUM_RATE, 2.0),
            @MinPremium = ISNULL(INSURANCE_MIN_PREMIUM, 1000),
            @MaxPremium = ISNULL(INSURANCE_MAX_PREMIUM, 5000)
        FROM dbo.INSURANCE_PLAN_MASTER
        WHERE INSURANCE_PLAN_ID = @p_InsurancePlanID;
        
        -- Calculate premium as percentage of salary
        SET @Premium = CAST(@p_EmpSalary * (@PremiumRate / 100) AS DECIMAL(19,0));
        
        -- Apply family coverage multiplier (1.5x)
        IF @p_CoverageType = 'FAMILY'
            SET @Premium = CAST(@Premium * 1.5 AS DECIMAL(19,0));
        
        -- Apply limits
        IF @Premium < @MinPremium
            SET @Premium = @MinPremium;
        ELSE IF @Premium > @MaxPremium
            SET @Premium = @MaxPremium;
    END TRY
    BEGIN CATCH
        SET @Premium = 0;
    END CATCH
    
    RETURN @Premium;
END;
GO

-- Function: Get Claim Reimbursement Amount
IF OBJECT_ID('dbo.fn_GetClaimReimbursement', 'FN') IS NOT NULL
    DROP FUNCTION dbo.fn_GetClaimReimbursement;
GO

CREATE FUNCTION dbo.fn_GetClaimReimbursement (
    @p_ClaimAmount DECIMAL(19,0),
    @p_ClaimType VARCHAR(20),  -- 'IN_PATIENT', 'OUT_PATIENT', 'DENTAL', 'OPTICAL'
    @p_CopayPercentage DECIMAL(5,2) = 20.0
)
RETURNS DECIMAL(19,0)
AS
BEGIN
    DECLARE @ReimbursableAmount DECIMAL(19,0);
    DECLARE @CopayAmount DECIMAL(19,0);
    DECLARE @CoverageLimitPercentage DECIMAL(5,2) = 100.0;
    
    BEGIN TRY
        -- Apply coverage limits by claim type
        IF @p_ClaimType = 'DENTAL'
            SET @CoverageLimitPercentage = 50.0  -- 50% coverage for dental
        ELSE IF @p_ClaimType = 'OPTICAL'
            SET @CoverageLimitPercentage = 75.0  -- 75% coverage for optical
        ELSE
            SET @CoverageLimitPercentage = 100.0;  -- 100% for general
        
        -- Calculate copay (amount borne by employee)
        SET @CopayAmount = CAST(@p_ClaimAmount * (@p_CopayPercentage / 100) AS DECIMAL(19,0));
        
        -- Calculate reimbursable amount
        SET @ReimbursableAmount = CAST((@p_ClaimAmount - @CopayAmount) * (@CoverageLimitPercentage / 100) AS DECIMAL(19,0));
    END TRY
    BEGIN CATCH
        SET @ReimbursableAmount = 0;
    END CATCH
    
    RETURN @ReimbursableAmount;
END;
GO

-- =====================================================
-- STORED PROCEDURES
-- =====================================================

-- Procedure: Enroll Employee in Insurance Plan
IF OBJECT_ID('dbo.usp_EnrollInsurancePlan', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_EnrollInsurancePlan;
GO

CREATE PROCEDURE dbo.usp_EnrollInsurancePlan
    @p_EmpSysID BIGINT,
    @p_InsurancePlanID BIGINT,
    @p_CoverageType VARCHAR(20),  -- 'EMPLOYEE', 'FAMILY'
    @p_EnrollmentDate DATE,
    @p_EffectiveDate DATE,
    @p_EnrolledBy BIGINT,
    @p_EnrollmentID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @ProcessDate DATETIME2(3) = GETDATE();
        DECLARE @EligibilityStatus VARCHAR(50);
        DECLARE @BasicSalary DECIMAL(19,0);
        DECLARE @MonthlyPremium DECIMAL(19,0);
        
        -- Check eligibility
        SET @EligibilityStatus = dbo.fn_GetInsuranceEligibility(@p_EmpSysID, @ProcessDate);
        
        IF @EligibilityStatus NOT IN ('ELIGIBLE', 'PROBATION')
        BEGIN
            THROW 50001, 'Employee not eligible for insurance enrollment', 1;
        END
        
        -- Get employee salary
        SELECT TOP 1 @BasicSalary = PAY_BASIC
        FROM PAYDB.dbo.PAY_SALARY_MASTER
        WHERE EMP_SYS_ID = @p_EmpSysID
        ORDER BY PAY_PERIOD_ENDING DESC;
        
        IF @BasicSalary IS NULL
            SET @BasicSalary = 0;
        
        -- Calculate premium
        SET @MonthlyPremium = dbo.fn_CalculateInsurancePremium(@p_InsurancePlanID, @p_CoverageType, @BasicSalary);
        
        -- Check for existing enrollment
        IF EXISTS (
            SELECT 1 FROM dbo.INSURANCE_ENROLLMENT
            WHERE EMP_SYS_ID = @p_EmpSysID
              AND INSURANCE_PLAN_ID = @p_InsurancePlanID
              AND ENROLLMENT_STATUS = 'A'
        )
        BEGIN
            THROW 50002, 'Employee already enrolled in this insurance plan', 1;
        END
        
        -- Create enrollment record
        INSERT INTO dbo.INSURANCE_ENROLLMENT (
            EMP_SYS_ID,
            INSURANCE_PLAN_ID,
            COVERAGE_TYPE,
            ENROLLMENT_DATE,
            EFFECTIVE_DATE,
            MONTHLY_PREMIUM,
            ENROLLMENT_STATUS,
            CREATED_BY,
            CREATED_ON
        ) VALUES (
            @p_EmpSysID,
            @p_InsurancePlanID,
            @p_CoverageType,
            @p_EnrollmentDate,
            @p_EffectiveDate,
            @MonthlyPremium,
            'A',  -- Active
            @p_EnrolledBy,
            @ProcessDate
        );
        
        SET @p_EnrollmentID = SCOPE_IDENTITY();
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: Submit Insurance Claim
IF OBJECT_ID('dbo.usp_SubmitInsuranceClaim', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_SubmitInsuranceClaim;
GO

CREATE PROCEDURE dbo.usp_SubmitInsuranceClaim
    @p_EmpSysID BIGINT,
    @p_EnrollmentID BIGINT,
    @p_ClaimType VARCHAR(20),  -- 'IN_PATIENT', 'OUT_PATIENT', 'DENTAL', 'OPTICAL'
    @p_ClaimAmount DECIMAL(19,0),
    @p_ServiceDate DATE,
    @p_HospitalName VARCHAR(100),
    @p_Remarks VARCHAR(500),
    @p_SubmittedBy BIGINT,
    @p_ClaimID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @ProcessDate DATETIME2(3) = GETDATE();
        DECLARE @InsurancePlanID BIGINT;
        DECLARE @ReimbursableAmount DECIMAL(19,0);
        
        -- Validate enrollment
        SELECT @InsurancePlanID = INSURANCE_PLAN_ID
        FROM dbo.INSURANCE_ENROLLMENT
        WHERE ENROLLMENT_ID = @p_EnrollmentID
          AND EMP_SYS_ID = @p_EmpSysID
          AND ENROLLMENT_STATUS = 'A';
        
        IF @InsurancePlanID IS NULL
        BEGIN
            THROW 50003, 'Active insurance enrollment not found', 1;
        END
        
        -- Validate claim amount
        IF @p_ClaimAmount <= 0
        BEGIN
            THROW 50004, 'Claim amount must be greater than zero', 1;
        END
        
        -- Calculate reimbursable amount
        SET @ReimbursableAmount = dbo.fn_GetClaimReimbursement(@p_ClaimAmount, @p_ClaimType);
        
        -- Submit claim
        INSERT INTO dbo.INSURANCE_CLAIM (
            EMP_SYS_ID,
            ENROLLMENT_ID,
            INSURANCE_PLAN_ID,
            CLAIM_TYPE,
            CLAIM_AMOUNT,
            REIMBURSABLE_AMOUNT,
            SERVICE_DATE,
            HOSPITAL_NAME,
            CLAIM_REMARKS,
            CLAIM_STATUS,
            CREATED_BY,
            CREATED_ON
        ) VALUES (
            @p_EmpSysID,
            @p_EnrollmentID,
            @InsurancePlanID,
            @p_ClaimType,
            @p_ClaimAmount,
            @ReimbursableAmount,
            @p_ServiceDate,
            @p_HospitalName,
            @p_Remarks,
            'S',  -- Submitted
            @p_SubmittedBy,
            @ProcessDate
        );
        
        SET @p_ClaimID = SCOPE_IDENTITY();
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: Approve Insurance Claim
IF OBJECT_ID('dbo.usp_ApproveInsuranceClaim', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_ApproveInsuranceClaim;
GO

CREATE PROCEDURE dbo.usp_ApproveInsuranceClaim
    @p_ClaimID BIGINT,
    @p_ApprovedAmount DECIMAL(19,0),
    @p_ApprovalRemarks VARCHAR(500),
    @p_ApprovedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @ProcessDate DATETIME2(3) = GETDATE();
        DECLARE @ReimbursableAmount DECIMAL(19,0);
        DECLARE @ClaimAmount DECIMAL(19,0);
        
        -- Get claim details
        SELECT 
            @ReimbursableAmount = REIMBURSABLE_AMOUNT,
            @ClaimAmount = CLAIM_AMOUNT
        FROM dbo.INSURANCE_CLAIM
        WHERE CLAIM_ID = @p_ClaimID;
        
        IF @ReimbursableAmount IS NULL
        BEGIN
            THROW 50005, 'Claim record not found', 1;
        END
        
        -- Validate approved amount
        IF @p_ApprovedAmount > @ReimbursableAmount
        BEGIN
            THROW 50006, 'Approved amount cannot exceed reimbursable amount', 1;
        END
        
        -- Approve claim
        UPDATE dbo.INSURANCE_CLAIM
        SET 
            CLAIM_STATUS = 'A',  -- Approved
            APPROVED_AMOUNT = @p_ApprovedAmount,
            APPROVAL_REMARKS = @p_ApprovalRemarks,
            APPROVED_BY = @p_ApprovedBy,
            APPROVAL_DATE = @ProcessDate,
            UPDATED_BY = @p_ApprovedBy,
            UPDATED_ON = @ProcessDate
        WHERE CLAIM_ID = @p_ClaimID;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: Process Insurance Reimbursement
IF OBJECT_ID('dbo.usp_ProcessInsuranceReimbursement', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_ProcessInsuranceReimbursement;
GO

CREATE PROCEDURE dbo.usp_ProcessInsuranceReimbursement
    @p_ClaimID BIGINT,
    @p_ProcessedBy BIGINT,
    @p_ReimbursementID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @ProcessDate DATETIME2(3) = GETDATE();
        DECLARE @ApprovedAmount DECIMAL(19,0);
        DECLARE @EmpSysID BIGINT;
        
        -- Get approved amount
        SELECT 
            @ApprovedAmount = APPROVED_AMOUNT,
            @EmpSysID = EMP_SYS_ID
        FROM dbo.INSURANCE_CLAIM
        WHERE CLAIM_ID = @p_ClaimID
          AND CLAIM_STATUS = 'A';  -- Must be approved
        
        IF @ApprovedAmount IS NULL OR @ApprovedAmount = 0
        BEGIN
            THROW 50007, 'Claim not approved or no approved amount', 1;
        END
        
        -- Create reimbursement record
        INSERT INTO dbo.INSURANCE_REIMBURSEMENT (
            CLAIM_ID,
            EMP_SYS_ID,
            REIMBURSEMENT_AMOUNT,
            REIMBURSEMENT_DATE,
            REIMBURSEMENT_STATUS,
            CREATED_BY,
            CREATED_ON
        ) VALUES (
            @p_ClaimID,
            @EmpSysID,
            @ApprovedAmount,
            @ProcessDate,
            'P',  -- Posted
            @p_ProcessedBy,
            @ProcessDate
        );
        
        SET @p_ReimbursementID = SCOPE_IDENTITY();
        
        -- Update claim status to reimbursed
        UPDATE dbo.INSURANCE_CLAIM
        SET 
            CLAIM_STATUS = 'R',  -- Reimbursed
            REIMBURSEMENT_DATE = @ProcessDate,
            UPDATED_BY = @p_ProcessedBy,
            UPDATED_ON = @ProcessDate
        WHERE CLAIM_ID = @p_ClaimID;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- =====================================================
-- TRIGGERS
-- =====================================================

-- Trigger: Validate Insurance Plan
IF OBJECT_ID('dbo.trg_InsuranceEnrollment_ValidatePlan', 'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_InsuranceEnrollment_ValidatePlan;
GO

CREATE TRIGGER dbo.trg_InsuranceEnrollment_ValidatePlan
ON dbo.INSURANCE_ENROLLMENT
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Validate insurance plan exists
        IF EXISTS (
            SELECT 1 FROM inserted i
            WHERE NOT EXISTS (
                SELECT 1 FROM dbo.INSURANCE_PLAN_MASTER
                WHERE INSURANCE_PLAN_ID = i.INSURANCE_PLAN_ID
            )
        )
        BEGIN
            THROW 50008, 'Invalid insurance plan', 1;
        END
        
        -- Validate coverage type
        IF EXISTS (SELECT 1 FROM inserted WHERE COVERAGE_TYPE NOT IN ('EMPLOYEE', 'FAMILY', 'DEPENDENT'))
        BEGIN
            THROW 50009, 'Invalid coverage type', 1;
        END
        
        INSERT INTO dbo.INSURANCE_ENROLLMENT (
            EMP_SYS_ID,
            INSURANCE_PLAN_ID,
            COVERAGE_TYPE,
            ENROLLMENT_DATE,
            EFFECTIVE_DATE,
            MONTHLY_PREMIUM,
            ENROLLMENT_STATUS,
            CREATED_BY,
            CREATED_ON
        )
        SELECT 
            EMP_SYS_ID,
            INSURANCE_PLAN_ID,
            COVERAGE_TYPE,
            ENROLLMENT_DATE,
            EFFECTIVE_DATE,
            MONTHLY_PREMIUM,
            ENROLLMENT_STATUS,
            CREATED_BY,
            CREATED_ON
        FROM inserted;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Trigger: Audit Insurance Claim
IF OBJECT_ID('dbo.trg_InsuranceClaim_Audit', 'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_InsuranceClaim_Audit;
GO

CREATE TRIGGER dbo.trg_InsuranceClaim_Audit
ON dbo.INSURANCE_CLAIM
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO dbo.INSURANCE_CLAIM_AUDIT (
        CLAIM_ID,
        EMP_SYS_ID,
        CLAIM_AMOUNT,
        CLAIM_STATUS,
        AUDIT_ACTION,
        AUDIT_DATE
    )
    SELECT 
        CLAIM_ID,
        EMP_SYS_ID,
        CLAIM_AMOUNT,
        CLAIM_STATUS,
        CASE WHEN EXISTS (SELECT 1 FROM deleted WHERE CLAIM_ID = inserted.CLAIM_ID) THEN 'UPDATE' ELSE 'INSERT' END,
        GETDATE()
    FROM inserted;
END;
GO

-- Trigger: Validate Claim Amount
IF OBJECT_ID('dbo.trg_InsuranceClaim_ValidateAmount', 'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_InsuranceClaim_ValidateAmount;
GO

CREATE TRIGGER dbo.trg_InsuranceClaim_ValidateAmount
ON dbo.INSURANCE_CLAIM
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Validate claim amount
        IF EXISTS (SELECT 1 FROM inserted WHERE CLAIM_AMOUNT <= 0)
        BEGIN
            THROW 50010, 'Claim amount must be greater than zero', 1;
        END
        
        -- Validate service date is not in future
        IF EXISTS (SELECT 1 FROM inserted WHERE SERVICE_DATE > CAST(GETDATE() AS DATE))
        BEGIN
            THROW 50011, 'Service date cannot be in future', 1;
        END
        
        INSERT INTO dbo.INSURANCE_CLAIM (
            EMP_SYS_ID,
            ENROLLMENT_ID,
            INSURANCE_PLAN_ID,
            CLAIM_TYPE,
            CLAIM_AMOUNT,
            REIMBURSABLE_AMOUNT,
            SERVICE_DATE,
            HOSPITAL_NAME,
            CLAIM_REMARKS,
            CLAIM_STATUS,
            CREATED_BY,
            CREATED_ON
        )
        SELECT 
            EMP_SYS_ID,
            ENROLLMENT_ID,
            INSURANCE_PLAN_ID,
            CLAIM_TYPE,
            CLAIM_AMOUNT,
            REIMBURSABLE_AMOUNT,
            SERVICE_DATE,
            HOSPITAL_NAME,
            CLAIM_REMARKS,
            CLAIM_STATUS,
            CREATED_BY,
            CREATED_ON
        FROM inserted;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

PRINT 'HEALTHDB Procedures, Functions & Triggers created successfully.';
GO
