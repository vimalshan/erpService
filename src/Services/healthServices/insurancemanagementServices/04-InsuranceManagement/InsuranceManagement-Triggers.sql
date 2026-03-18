-- ==========================================
-- Module: InsuranceManagement
-- Purpose: Insurance Triggers & Audit
-- Generated: 2026-03-09
-- ==========================================

USE HEALTHDB;
GO

-- =====================================================
-- TRIGGERS
-- =====================================================

-- Trigger: Validate Insurance Plan
IF OBJECT_ID('dbo.trg_InsuranceEnrollmentValidate', 'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_InsuranceEnrollmentValidate;
GO

CREATE TRIGGER dbo.trg_InsuranceEnrollmentValidate
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
IF OBJECT_ID('dbo.trg_InsuranceClaimAudit', 'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_InsuranceClaimAudit;
GO

CREATE TRIGGER dbo.trg_InsuranceClaimAudit
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
IF OBJECT_ID('dbo.trg_InsuranceClaimValidateAmount', 'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_InsuranceClaimValidateAmount;
GO

CREATE TRIGGER dbo.trg_InsuranceClaimValidateAmount
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

PRINT 'InsuranceManagement: Triggers created successfully.';
GO
