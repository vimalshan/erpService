-- =========================================================================
-- CONTRIBUTION MODULE - Stored Procedures and Functions
-- Database: PFDB
-- Module: Contribution Management
-- Description: Procedures for contribution processing and validation
-- Created: March 9, 2026
-- =========================================================================

USE PFDB;
GO

-- =========================================================================
-- FUNCTION: Calculate PF Contribution
-- Description: Calculates employee and employer PF contributions
-- =========================================================================
IF OBJECT_ID('dbo.fn_CalculatePFContribution', 'FN') IS NOT NULL
    DROP FUNCTION dbo.fn_CalculatePFContribution;
GO

CREATE FUNCTION dbo.fn_CalculatePFContribution (
    @p_BasicSalary DECIMAL(19,0),
    @p_EmployeeContributionRate DECIMAL(5,2) = 12.0,
    @p_EmployerContributionRate DECIMAL(5,2) = 12.0
)
RETURNS TABLE
AS
RETURN (
    SELECT 
        CAST(@p_BasicSalary * (@p_EmployeeContributionRate / 100) AS DECIMAL(19,0)) AS EmpContribution,
        CAST(@p_BasicSalary * (@p_EmployerContributionRate / 100) AS DECIMAL(19,0)) AS ErContribution,
        CAST(@p_BasicSalary * ((@p_EmployeeContributionRate + @p_EmployerContributionRate) / 100) AS DECIMAL(19,0)) AS TotalContribution
);
GO

-- =========================================================================
-- PROCEDURE: Process Monthly PF Contribution
-- Description: Processes monthly PF contributions for all active employees
-- Parameters:
--   @p_MonthYear: Format YYYY-MM
--   @p_ProcessedBy: Employee System ID who processed
--   @p_RowsProcessed: Count of rows processed (OUTPUT)
-- =========================================================================
IF OBJECT_ID('dbo.usp_ProcessMonthlyPFContribution', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_ProcessMonthlyPFContribution;
GO

CREATE PROCEDURE dbo.usp_ProcessMonthlyPFContribution
    @p_MonthYear VARCHAR(7),  -- Format: YYYY-MM
    @p_ProcessedBy BIGINT,
    @p_RowsProcessed INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @ProcessDate DATETIME2(3) = GETDATE();
        DECLARE @EmpSysID BIGINT;
        DECLARE @BasicSalary DECIMAL(19,0);
        DECLARE @EmpContribution DECIMAL(19,0);
        DECLARE @ErContribution DECIMAL(19,0);
        DECLARE @TotalContribution DECIMAL(19,0);
        
        -- Validate input
        IF @p_MonthYear IS NULL OR LEN(@p_MonthYear) <> 7
        BEGIN
            THROW 50001, 'Invalid month year format. Expected YYYY-MM format.', 1;
        END
        
        SET @p_RowsProcessed = 0;
        
        -- Log contribution processing start
        INSERT INTO dbo.CONTRIBUTION_PROCESS_LOG (
            LOG_TYPE,
            LOG_MESSAGE,
            PROCESS_DATE,
            USER_ID
        ) VALUES (
            'START',
            'Monthly contribution processing started for ' + @p_MonthYear,
            @ProcessDate,
            @p_ProcessedBy
        );
        
        -- For demonstration - would integrate with PAYDB and HRDB
        -- Process contribution batch
        INSERT INTO dbo.CONTRIBUTION_MAIN (
            CONTRIBUTION_BATCH_NO,
            CONTRIBUTION_TRUST_CODE,
            CONTRIBUTION_CATEGORY,
            CONTRIBUTION_PAYUNIT_CODE,
            CONTRIBUTION_PAY_MONTHSTART,
            CONTRIBUTION_PAY_MONTHEND,
            CONTRIBUTION_STATUS,
            CONTRIBUTION_REFNO,
            CONTRIBUTION_ENT_ON
        ) VALUES (
            ISNULL((SELECT MAX(CONTRIBUTION_BATCH_NO) FROM dbo.CONTRIBUTION_MAIN), 0) + 1,
            'DFL',  -- Default trust code
            'REG',  -- Regular contribution
            '001',  -- Default unit
            CAST(@p_MonthYear + '-01' AS DATETIME2(3)),
            EOMONTH(CAST(@p_MonthYear + '-01' AS DATE)),
            'P',    -- Pending
            ISNULL((SELECT MAX(CONTRIBUTION_REFNO) FROM dbo.CONTRIBUTION_MAIN), 0) + 1,
            @ProcessDate
        );
        
        SET @p_RowsProcessed = 1;
        
        -- Log processing completion
        INSERT INTO dbo.CONTRIBUTION_PROCESS_LOG (
            LOG_TYPE,
            LOG_MESSAGE,
            PROCESS_DATE,
            USER_ID
        ) VALUES (
            'END',
            'Monthly contribution processing completed. Batches: ' + CAST(@p_RowsProcessed AS VARCHAR),
            @ProcessDate,
            @p_ProcessedBy
        );
        
        COMMIT TRANSACTION;
        
        PRINT 'Monthly PF contribution processing completed. Rows processed: ' + CAST(@p_RowsProcessed AS VARCHAR);
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        
        INSERT INTO dbo.CONTRIBUTION_PROCESS_LOG (
            LOG_TYPE,
            LOG_MESSAGE,
            PROCESS_DATE,
            USER_ID
        ) VALUES (
            'ERROR',
            'Error in monthly contribution processing: ' + ERROR_MESSAGE(),
            GETDATE(),
            @p_ProcessedBy
        );
        
        THROW 50001, 'Error in PF contribution processing', 1;
    END CATCH
END;
GO

-- =========================================================================
-- PROCEDURE: Validate Contribution Details
-- Description: Validates contribution details before posting
-- Parameters:
--   @p_ContributionID: Contribution ID to validate
-- =========================================================================
IF OBJECT_ID('dbo.usp_ValidateContributionDetails', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_ValidateContributionDetails;
GO

CREATE PROCEDURE dbo.usp_ValidateContributionDetails
    @p_ContributionID DECIMAL(38)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        DECLARE @BasicAmount DECIMAL(38);
        DECLARE @EEAmount DECIMAL(38);
        DECLARE @ERAmount DECIMAL(38);
        DECLARE @TotalAmount DECIMAL(38);
        
        -- Get contribution details
        SELECT 
            @BasicAmount = CONTRIBUTION_BASIC_AMOUNT,
            @EEAmount = CONTRIBUTION_EE_AMOUNT,
            @ERAmount = CONTRIBUTION_ER_AMOUNT
        FROM dbo.CONTRIBUTION_DETAILS
        WHERE CONTRIBUTION_ID = @p_ContributionID;
        
        IF @BasicAmount IS NULL
        BEGIN
            THROW 50002, 'Contribution record not found', 1;
        END
        
        -- Validate EE and ER amounts
        IF @EEAmount < 0 OR @ERAmount < 0
        BEGIN
            THROW 50003, 'Contribution amounts cannot be negative', 1;
        END
        
        -- Validate total not exceeding basic
        SET @TotalAmount = ISNULL(@EEAmount, 0) + ISNULL(@ERAmount, 0);
        IF @TotalAmount > (@BasicAmount * 2)  -- Reasonable max threshold
        BEGIN
            THROW 50004, 'Total contribution amount exceeds reasonable threshold', 1;
        END
        
        -- Update status
        UPDATE dbo.CONTRIBUTION_DETAILS
        SET CONTRIBUTION_TYPE_CODE = 'V'  -- Validated
        WHERE CONTRIBUTION_ID = @p_ContributionID;
        
        SELECT 'Contribution validation successful' AS ValidationResult;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- =========================================================================
-- PROCEDURE: Post Contribution Batch
-- Description: Posts validated contribution batch to accounting
-- Parameters:
--   @p_BatchNo: Contribution Batch Number
--   @p_PostedBy: Employee System ID
-- =========================================================================
IF OBJECT_ID('dbo.usp_PostContributionBatch', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_PostContributionBatch;
GO

CREATE PROCEDURE dbo.usp_PostContributionBatch
    @p_BatchNo BIGINT,
    @p_PostedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @ProcessDate DATETIME2(3) = GETDATE();
        DECLARE @TotalEEAmount DECIMAL(19,0) = 0;
        DECLARE @TotalERAmount DECIMAL(19,0) = 0;
        
        -- Validate batch exists
        IF NOT EXISTS (SELECT 1 FROM dbo.CONTRIBUTION_MAIN WHERE CONTRIBUTION_BATCH_NO = @p_BatchNo)
        BEGIN
            THROW 50005, 'Contribution batch not found', 1;
        END
        
        -- Calculate totals
        SELECT 
            @TotalEEAmount = ISNULL(SUM(CONTRIBUTION_EE_AMOUNT), 0),
            @TotalERAmount = ISNULL(SUM(CONTRIBUTION_ER_AMOUNT), 0)
        FROM dbo.CONTRIBUTION_DETAILS
        WHERE CONTRIBUTION_BATCH_NO = @p_BatchNo;
        
        -- Update batch status to Posted
        UPDATE dbo.CONTRIBUTION_MAIN
        SET 
            CONTRIBUTION_STATUS = 'PO',  -- Posted
            CONTRIBUTION_ENT_ON = @ProcessDate
        WHERE CONTRIBUTION_BATCH_NO = @p_BatchNo;
        
        -- Log posting activity
        INSERT INTO dbo.CONTRIBUTION_PROCESS_LOG (
            LOG_TYPE,
            LOG_MESSAGE,
            PROCESS_DATE,
            USER_ID
        ) VALUES (
            'POST',
            'Batch ' + CAST(@p_BatchNo AS VARCHAR) + ' posted. EE: ' + 
            CAST(@TotalEEAmount AS VARCHAR) + ', ER: ' + CAST(@TotalERAmount AS VARCHAR),
            @ProcessDate,
            @p_PostedBy
        );
        
        COMMIT TRANSACTION;
        
        PRINT 'Contribution batch posted successfully.';
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- =========================================================================
-- PROCEDURE: Get Contribution Summary
-- Description: Retrieves contribution summary for a period
-- Parameters:
--   @p_StartDate: Period start date
--   @p_EndDate: Period end date
-- =========================================================================
IF OBJECT_ID('dbo.usp_GetContributionSummary', 'P') IS NOT NULL
    DROP PROCEDURE dbo.usp_GetContributionSummary;
GO

CREATE PROCEDURE dbo.usp_GetContributionSummary
    @p_StartDate DATETIME2(3),
    @p_EndDate DATETIME2(3)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        cm.CONTRIBUTION_BATCH_NO,
        cm.CONTRIBUTION_TRUST_CODE,
        cm.CONTRIBUTION_PAYUNIT_CODE,
        cm.CONTRIBUTION_STATUS,
        COUNT(DISTINCT cd.CONTRIBUTION_MEMBER_NO) AS MemberCount,
        SUM(cd.CONTRIBUTION_EE_AMOUNT) AS TotalEEContribution,
        SUM(cd.CONTRIBUTION_ER_AMOUNT) AS TotalERContribution,
        SUM(cd.CONTRIBUTION_EE_AMOUNT) + SUM(cd.CONTRIBUTION_ER_AMOUNT) AS TotalContribution
    FROM dbo.CONTRIBUTION_MAIN cm
    LEFT JOIN dbo.CONTRIBUTION_DETAILS cd ON cm.CONTRIBUTION_BATCH_NO = cd.CONTRIBUTION_BATCH_NO
    WHERE cm.CONTRIBUTION_PAY_MONTHSTART >= @p_StartDate
      AND cm.CONTRIBUTION_PAY_MONTHEND <= @p_EndDate
    GROUP BY 
        cm.CONTRIBUTION_BATCH_NO,
        cm.CONTRIBUTION_TRUST_CODE,
        cm.CONTRIBUTION_PAYUNIT_CODE,
        cm.CONTRIBUTION_STATUS
    ORDER BY 
        cm.CONTRIBUTION_BATCH_NO DESC;
END;
GO

PRINT 'Contribution Module Procedures created successfully!';
GO
