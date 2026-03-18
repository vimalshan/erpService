-- ============================================================================
-- Module: Scholarship Management - Stored Procedures and Functions
-- Purpose: Manage scholarship applications, approvals, and disbursements
-- Created: March 9, 2026
-- Version: 1.0
-- ============================================================================

USE [SPARSHDB];
GO

-- ============================================================================
-- FUNCTIONS
-- ============================================================================

-- ============================================================================
-- Function: fn_GetStudentEligibility
-- Description: Check if a student is eligible for a scholarship
-- ============================================================================
IF OBJECT_ID('dbo.fn_GetStudentEligibility', 'FN') IS NOT NULL 
    DROP FUNCTION dbo.fn_GetStudentEligibility;
GO

CREATE FUNCTION dbo.fn_GetStudentEligibility(@p_StudentID BIGINT, @p_SchemeID BIGINT)
RETURNS VARCHAR(50)
AS BEGIN
    DECLARE @Status VARCHAR(50) = 'INELIGIBLE';
    BEGIN TRY
        SELECT @Status = 'ELIGIBLE'
        FROM dbo.SCHOLARSHIP_ELIGIBILITY_CRITERIA
        WHERE SCHOLARSHIP_ID = @p_SchemeID AND ELIGIBILITY_STATUS = 'A'
        LIMIT 1;
    END TRY 
    BEGIN CATCH 
        SET @Status = 'ERROR'; 
    END CATCH
    RETURN @Status;
END;
GO

-- ============================================================================
-- Function: fn_CalculateScholarshipAmount
-- Description: Calculate the scholarship amount based on coverage percentage
-- ============================================================================
IF OBJECT_ID('dbo.fn_CalculateScholarshipAmount', 'FN') IS NOT NULL 
    DROP FUNCTION dbo.fn_CalculateScholarshipAmount;
GO

CREATE FUNCTION dbo.fn_CalculateScholarshipAmount(@p_SchemeID BIGINT, @p_StudentAnnualFees DECIMAL(19,0))
RETURNS DECIMAL(19,0)
AS BEGIN
    DECLARE @Amount DECIMAL(19,0) = 0, @CoveragePercentage DECIMAL(5,2);
    BEGIN TRY
        SELECT @CoveragePercentage = SCHOLARSHIP_COVERAGE_PERCENT 
        FROM dbo.SCHOLARSHIP_MASTER 
        WHERE SCHOLARSHIP_ID = @p_SchemeID;
        
        SET @Amount = CAST(@p_StudentAnnualFees * (ISNULL(@CoveragePercentage, 100) / 100.0) AS DECIMAL(19,0));
    END TRY 
    BEGIN CATCH 
        SET @Amount = 0; 
    END CATCH
    RETURN @Amount;
END;
GO

-- ============================================================================
-- STORED PROCEDURES
-- ============================================================================

-- ============================================================================
-- Procedure: usp_SCHOLARSHIP_ApplyForScholarship
-- Description: Submit a scholarship application for a student
-- ============================================================================
IF OBJECT_ID('dbo.usp_SCHOLARSHIP_ApplyForScholarship', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_SCHOLARSHIP_ApplyForScholarship;
GO

CREATE PROCEDURE dbo.usp_SCHOLARSHIP_ApplyForScholarship
    @p_StudentID BIGINT,
    @p_ScholarshipID BIGINT,
    @p_ApplicationDate DATE,
    @p_FamilyIncome DECIMAL(19,0),
    @p_ApplicantID BIGINT,
    @p_ApplicationID BIGINT OUTPUT,
    @p_ErrorMessage NVARCHAR(MAX) OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Check student eligibility
        IF dbo.fn_GetStudentEligibility(@p_StudentID, @p_ScholarshipID) <> 'ELIGIBLE'
        BEGIN
            SET @p_ErrorMessage = 'Student not eligible for this scholarship';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Insert application
        INSERT INTO dbo.SCHOLARSHIP_APPLICATION (APPLICATION_ID, EMP_STUDENT_ID, SCHOLARSHIP_ID, 
            APPLICATION_DATE, FAMILY_INCOME, APPLICATION_STATUS, CREATED_BY, CREATED_ON)
        VALUES (NEXT VALUE FOR dbo.seq_SCHOLARSHIP_APPLICATION_Id, @p_StudentID, @p_ScholarshipID, 
            @p_ApplicationDate, @p_FamilyIncome, 'S', @p_ApplicantID, GETDATE());
        
        SET @p_ApplicationID = SCOPE_IDENTITY();
        SET @p_ErrorMessage = 'Application submitted successfully';
        
        COMMIT TRANSACTION;
    END TRY 
    BEGIN CATCH 
        ROLLBACK TRANSACTION; 
        SET @p_ErrorMessage = 'Error: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

-- ============================================================================
-- Procedure: usp_SCHOLARSHIP_ApproveScholarship
-- Description: Approve a scholarship application and create disbursement
-- ============================================================================
IF OBJECT_ID('dbo.usp_SCHOLARSHIP_ApproveScholarship', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_SCHOLARSHIP_ApproveScholarship;
GO

CREATE PROCEDURE dbo.usp_SCHOLARSHIP_ApproveScholarship
    @p_ApplicationID BIGINT,
    @p_ApprovedBy BIGINT,
    @p_ApprovedAmount DECIMAL(19,0) = NULL,
    @p_ErrorMessage NVARCHAR(MAX) OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @ScholarshipID BIGINT, @StudentID BIGINT, @CalculatedAmount DECIMAL(19,0),
                @FinalAmount DECIMAL(19,0), @DefaultFees DECIMAL(19,0) = 50000;
        
        -- Get application details
        SELECT @ScholarshipID = SCHOLARSHIP_ID, @StudentID = EMP_STUDENT_ID
        FROM dbo.SCHOLARSHIP_APPLICATION 
        WHERE APPLICATION_ID = @p_ApplicationID;
        
        IF @ScholarshipID IS NULL
        BEGIN
            SET @p_ErrorMessage = 'Application not found';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Calculate amount
        SET @CalculatedAmount = dbo.fn_CalculateScholarshipAmount(@ScholarshipID, @DefaultFees);
        SET @FinalAmount = ISNULL(@p_ApprovedAmount, @CalculatedAmount);
        
        -- Update application
        UPDATE dbo.SCHOLARSHIP_APPLICATION
        SET APPLICATION_STATUS = 'A',
            APPROVED_AMOUNT = @FinalAmount,
            APPROVED_BY = @p_ApprovedBy,
            APPROVAL_DATE = GETDATE(),
            UPDATED_BY = @p_ApprovedBy,
            UPDATED_ON = GETDATE()
        WHERE APPLICATION_ID = @p_ApplicationID;
        
        -- Create disbursement record
        INSERT INTO dbo.SCHOLARSHIP_DISBURSEMENT (DISBURSEMENT_ID, APPLICATION_ID, STUDENT_ID, 
            SCHOLARSHIP_ID, DISBURSEMENT_AMOUNT, DISBURSEMENT_STATUS, CREATED_BY, CREATED_ON)
        VALUES (NEXT VALUE FOR dbo.seq_SCHOLARSHIP_DISBURSEMENT_Id, @p_ApplicationID, 
            @StudentID, @ScholarshipID, @FinalAmount, 'P', @p_ApprovedBy, GETDATE());
        
        SET @p_ErrorMessage = 'Scholarship approved successfully';
        
        COMMIT TRANSACTION;
    END TRY 
    BEGIN CATCH 
        ROLLBACK TRANSACTION; 
        SET @p_ErrorMessage = 'Error: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

-- ============================================================================
-- Procedure: usp_SCHOLARSHIP_ProcessDisbursement
-- Description: Process disbursement of approved scholarship
-- ============================================================================
IF OBJECT_ID('dbo.usp_SCHOLARSHIP_ProcessDisbursement', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_SCHOLARSHIP_ProcessDisbursement;
GO

CREATE PROCEDURE dbo.usp_SCHOLARSHIP_ProcessDisbursement
    @p_DisbursementID BIGINT,
    @p_ProcessedBy BIGINT,
    @p_ReferenceNumber VARCHAR(100),
    @p_ErrorMessage NVARCHAR(MAX) OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Verify disbursement exists and is pending
        IF NOT EXISTS (SELECT 1 FROM dbo.SCHOLARSHIP_DISBURSEMENT 
                      WHERE DISBURSEMENT_ID = @p_DisbursementID AND DISBURSEMENT_STATUS = 'P')
        BEGIN
            SET @p_ErrorMessage = 'Disbursement not found or already processed';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Update disbursement record
        UPDATE dbo.SCHOLARSHIP_DISBURSEMENT
        SET DISBURSEMENT_STATUS = 'D',
            DISBURSEMENT_DATE = GETDATE(),
            REFERENCE_NUMBER = @p_ReferenceNumber,
            UPDATED_BY = @p_ProcessedBy,
            UPDATED_ON = GETDATE()
        WHERE DISBURSEMENT_ID = @p_DisbursementID;
        
        SET @p_ErrorMessage = 'Disbursement processed successfully';
        
        COMMIT TRANSACTION;
    END TRY 
    BEGIN CATCH 
        ROLLBACK TRANSACTION; 
        SET @p_ErrorMessage = 'Error: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

-- ============================================================================
-- Procedure: usp_SCHOLARSHIP_GetApplicationsByStatus
-- Description: Retrieve applications filtered by status
-- ============================================================================
IF OBJECT_ID('dbo.usp_SCHOLARSHIP_GetApplicationsByStatus', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_SCHOLARSHIP_GetApplicationsByStatus;
GO

CREATE PROCEDURE dbo.usp_SCHOLARSHIP_GetApplicationsByStatus
    @p_Status CHAR(1)
AS BEGIN
    SET NOCOUNT ON;
    SELECT 
        a.APPLICATION_ID,
        a.EMP_STUDENT_ID,
        a.SCHOLARSHIP_ID,
        s.SCHOLARSHIP_NAME,
        a.APPLICATION_DATE,
        a.FAMILY_INCOME,
        a.APPLICATION_STATUS,
        a.APPROVED_AMOUNT,
        a.APPROVED_BY,
        a.APPROVAL_DATE
    FROM dbo.SCHOLARSHIP_APPLICATION a
    INNER JOIN dbo.SCHOLARSHIP_MASTER s ON a.SCHOLARSHIP_ID = s.SCHOLARSHIP_ID
    WHERE a.APPLICATION_STATUS = @p_Status
    ORDER BY a.APPLICATION_DATE DESC;
END;
GO

-- ============================================================================
-- Procedure: usp_SCHOLARSHIP_GetStudentApplications
-- Description: Retrieve all applications for a specific student
-- ============================================================================
IF OBJECT_ID('dbo.usp_SCHOLARSHIP_GetStudentApplications', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_SCHOLARSHIP_GetStudentApplications;
GO

CREATE PROCEDURE dbo.usp_SCHOLARSHIP_GetStudentApplications
    @p_StudentID BIGINT
AS BEGIN
    SET NOCOUNT ON;
    SELECT 
        a.APPLICATION_ID,
        a.SCHOLARSHIP_ID,
        s.SCHOLARSHIP_NAME,
        a.APPLICATION_DATE,
        a.APPLICATION_STATUS,
        a.APPROVED_AMOUNT,
        a.APPROVAL_DATE
    FROM dbo.SCHOLARSHIP_APPLICATION a
    INNER JOIN dbo.SCHOLARSHIP_MASTER s ON a.SCHOLARSHIP_ID = s.SCHOLARSHIP_ID
    WHERE a.EMP_STUDENT_ID = @p_StudentID
    ORDER BY a.APPLICATION_DATE DESC;
END;
GO

PRINT 'Scholarship Management Procedures created successfully.';
GO
