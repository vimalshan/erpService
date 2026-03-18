-- SPARSHDB Stored Procedures, Functions & Triggers
-- Scholarship Program Management System
-- Created: February 13, 2026

USE SPARSHDB;
GO

-- =====================================================
-- FUNCTIONS
-- =====================================================

IF OBJECT_ID('dbo.fn_GetStudentEligibility', 'FN') IS NOT NULL DROP FUNCTION dbo.fn_GetStudentEligibility;
GO
CREATE FUNCTION dbo.fn_GetStudentEligibility (@p_StudentID BIGINT, @p_SchemeID BIGINT)
RETURNS VARCHAR(50)
AS BEGIN
    DECLARE @Status VARCHAR(50) = 'INELIGIBLE';
    BEGIN TRY
        SELECT @Status = 'ELIGIBLE'
        WHERE EXISTS (
            SELECT 1 FROM dbo.SCHOLARSHIP_ELIGIBILITY_CRITERIA
            WHERE SCHOLARSHIP_ID = @p_SchemeID AND ELIGIBILITY_STATUS = 'A'
        );
    END TRY BEGIN CATCH SET @Status = 'ERROR'; END CATCH
    RETURN @Status;
END;
GO

IF OBJECT_ID('dbo.fn_CalculateScholarshipAmount', 'FN') IS NOT NULL DROP FUNCTION dbo.fn_CalculateScholarshipAmount;
GO
CREATE FUNCTION dbo.fn_CalculateScholarshipAmount (@p_SchemeID BIGINT, @p_StudentAnnualFees DECIMAL(19,0))
RETURNS DECIMAL(19,0)
AS BEGIN
    DECLARE @Amount DECIMAL(19,0) = 0, @CoveragePercentage DECIMAL(5,2);
    BEGIN TRY
        SELECT @CoveragePercentage = SCHOLARSHIP_COVERAGE_PERCENT FROM dbo.SCHOLARSHIP_MASTER WHERE SCHOLARSHIP_ID = @p_SchemeID;
        SET @Amount = CAST(@p_StudentAnnualFees * (ISNULL(@CoveragePercentage, 100) / 100) AS DECIMAL(19,0));
    END TRY BEGIN CATCH SET @Amount = 0; END CATCH
    RETURN @Amount;
END;
GO

-- =====================================================
-- STORED PROCEDURES
-- =====================================================

IF OBJECT_ID('dbo.usp_ApplyForScholarship', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_ApplyForScholarship;
GO
CREATE PROCEDURE dbo.usp_ApplyForScholarship
    @p_StudentID BIGINT, @p_ScholarshipID BIGINT, @p_ApplicationDate DATE, 
    @p_FamilyIncome DECIMAL(19,0), @p_ApplicantID BIGINT, @p_ApplicationID BIGINT OUTPUT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF dbo.fn_GetStudentEligibility(@p_StudentID, @p_ScholarshipID) <> 'ELIGIBLE'
            THROW 50001, 'Student not eligible for scholarship', 1;
        INSERT INTO dbo.SCHOLARSHIP_APPLICATION (EMP_STUDENT_ID, SCHOLARSHIP_ID, APPLICATION_DATE, 
            FAMILY_INCOME, APPLICATION_STATUS, CREATED_BY, CREATED_ON)
        VALUES (@p_StudentID, @p_ScholarshipID, @p_ApplicationDate, @p_FamilyIncome, 'S', @p_ApplicantID, GETDATE());
        SET @p_ApplicationID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY BEGIN CATCH ROLLBACK TRANSACTION; THROW; END CATCH
END;
GO

IF OBJECT_ID('dbo.usp_ApproveScholarship', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_ApproveScholarship;
GO
CREATE PROCEDURE dbo.usp_ApproveScholarship
    @p_ApplicationID BIGINT, @p_ApprovedBy BIGINT
AS BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        DECLARE @Amount DECIMAL(19,0), @StudentFees DECIMAL(19,0), @ScholarshipID BIGINT, @StudentID BIGINT;
        SELECT @ScholarshipID = SCHOLARSHIP_ID, @StudentID = EMP_STUDENT_ID FROM dbo.SCHOLARSHIP_APPLICATION WHERE APPLICATION_ID = @p_ApplicationID;
        SET @Amount = dbo.fn_CalculateScholarshipAmount(@ScholarshipID, ISNULL(@StudentFees, 50000));
        UPDATE dbo.SCHOLARSHIP_APPLICATION SET APPLICATION_STATUS = 'A', APPROVED_AMOUNT = @Amount, APPROVED_BY = @p_ApprovedBy, UPDATED_ON = GETDATE() WHERE APPLICATION_ID = @p_ApplicationID;
        INSERT INTO dbo.SCHOLARSHIP_DISBURSEMENT (APPLICATION_ID, STUDENT_ID, SCHOLARSHIP_ID, DISBURSEMENT_AMOUNT, DISBURSEMENT_STATUS, CREATED_BY, CREATED_ON)
        VALUES (@p_ApplicationID, @StudentID, @ScholarshipID, @Amount, 'P', @p_ApprovedBy, GETDATE());
        COMMIT TRANSACTION;
    END TRY BEGIN CATCH ROLLBACK TRANSACTION; THROW; END CATCH
END;
GO

PRINT 'SPARSHDB Procedures created successfully.';
GO
