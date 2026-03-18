-- ==========================================
-- SRFStipendModule
-- Database: SRFSPARSHDB
-- Module Purpose: SRF Stipend Stored Procedures and Functions
-- Created: March 09, 2026
-- ==========================================

USE SRFSPARSHDB;
GO

-- ==========================================
-- FUNCTION: fn_CalculateSRFStipend
-- Purpose: Calculate SRF stipend amount based on research category and rank
-- Returns: Decimal (19,0) - Stipend amount
-- Parameters:
--   @p_ResearchCategoryID: Research Category ID
--   @p_RankID: SRF Rank ID
-- ==========================================
IF OBJECT_ID('dbo.fn_CalculateSRFStipend', 'FN') IS NOT NULL 
    DROP FUNCTION dbo.fn_CalculateSRFStipend;
GO
CREATE FUNCTION dbo.fn_CalculateSRFStipend (
    @p_ResearchCategoryID BIGINT,
    @p_RankID BIGINT
)
RETURNS DECIMAL(19,2)
AS BEGIN
    DECLARE @Stipend DECIMAL(19,2) = 0;
    
    BEGIN TRY
        SELECT @Stipend = ISNULL(SRF_MONTHLY_STIPEND, 0) 
        FROM dbo.SRF_STIPEND_MASTER 
        WHERE RESEARCH_CATEGORY_ID = @p_ResearchCategoryID 
          AND SRF_RANK_ID = @p_RankID
          AND STATUS = 'A'
          AND EFFECTIVE_FROM <= GETDATE()
          AND (EFFECTIVE_TO IS NULL OR EFFECTIVE_TO >= GETDATE());
    END TRY 
    BEGIN CATCH 
        SET @Stipend = 0;
    END CATCH
    
    RETURN @Stipend;
END;
GO

-- ==========================================
-- PROCEDURE: usp_ProcessSRFMonthlyStipend
-- Purpose: Process monthly SRF stipend disbursement for a specific month
-- Parameters:
--   @p_MonthYear: Month and Year in format YYYY-MM
--   @p_ProcessedBy: Employee System ID of processor
--   @p_RowsProcessed: Output parameter - Number of records processed
-- ==========================================
IF OBJECT_ID('dbo.usp_ProcessSRFMonthlyStipend', 'P') IS NOT NULL 
    DROP PROCEDURE dbo.usp_ProcessSRFMonthlyStipend;
GO
CREATE PROCEDURE dbo.usp_ProcessSRFMonthlyStipend (
    @p_MonthYear VARCHAR(7),
    @p_ProcessedBy BIGINT,
    @p_RowsProcessed INT OUTPUT
)
AS BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @ProcessDate DATETIME2(3) = GETDATE();
        DECLARE @RowCount INT = 0;
        
        -- Update disbursement status from Draft to Processed
        UPDATE dbo.SRF_STIPEND_DISBURSEMENT 
        SET DISBURSEMENT_STATUS = 'P',
            UPDATED_BY = @p_ProcessedBy,
            UPDATED_ON = @ProcessDate 
        WHERE MONTH_YEAR = @p_MonthYear
          AND DISBURSEMENT_STATUS = 'D';
        
        SET @p_RowsProcessed = @@ROWCOUNT;
        
        COMMIT TRANSACTION;
        
        -- Log success
        PRINT CONCAT('Successfully processed ', @p_RowsProcessed, ' SRF stipend records for month: ', @p_MonthYear);
        
    END TRY 
    BEGIN CATCH 
        ROLLBACK TRANSACTION;
        
        -- Log error details
        DECLARE @ErrorMessage NVARCHAR(MAX) = ERROR_MESSAGE();
        DECLARE @ErrorNumber INT = ERROR_NUMBER();
        
        PRINT CONCAT('Error in usp_ProcessSRFMonthlyStipend: [', @ErrorNumber, '] ', @ErrorMessage);
        THROW;
    END CATCH
END;
GO

-- ==========================================
-- PROCEDURE: usp_CalculateAndDisburseSRFStipend
-- Purpose: Calculate and create disbursement records for eligible SRF members
-- Parameters:
--   @p_MonthYear: Month and Year in format YYYY-MM
--   @p_ProcessedBy: Employee System ID of processor
--   @p_RowsCreated: Output parameter - Number of records created
-- ==========================================
IF OBJECT_ID('dbo.usp_CalculateAndDisburseSRFStipend', 'P') IS NOT NULL 
    DROP PROCEDURE dbo.usp_CalculateAndDisburseSRFStipend;
GO
CREATE PROCEDURE dbo.usp_CalculateAndDisburseSRFStipend (
    @p_MonthYear VARCHAR(7),
    @p_ProcessedBy BIGINT,
    @p_RowsCreated INT OUTPUT
)
AS BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @ProcessDate DATETIME2(3) = GETDATE();
        DECLARE @RowCount INT = 0;
        
        -- Insert new disbursement records for the month
        INSERT INTO dbo.SRF_STIPEND_DISBURSEMENT (
            SRF_ID,
            STIPEND_ID,
            DISBURSEMENT_DATE,
            DISBURSEMENT_AMOUNT,
            DISBURSEMENT_STATUS,
            MONTH_YEAR,
            CREATED_BY,
            CREATED_ON
        )
        SELECT 
            1 AS SRF_ID, -- Placeholder - should be replaced with actual SRF reference
            sm.STIPEND_ID,
            GETDATE() AS DISBURSEMENT_DATE,
            sm.SRF_MONTHLY_STIPEND AS DISBURSEMENT_AMOUNT,
            'D' AS DISBURSEMENT_STATUS,
            @p_MonthYear,
            @p_ProcessedBy,
            @ProcessDate
        FROM dbo.SRF_STIPEND_MASTER sm
        WHERE sm.STATUS = 'A'
          AND sm.EFFECTIVE_FROM <= GETDATE()
          AND (sm.EFFECTIVE_TO IS NULL OR sm.EFFECTIVE_TO >= GETDATE());
        
        SET @p_RowsCreated = @@ROWCOUNT;
        
        COMMIT TRANSACTION;
        
        PRINT CONCAT('Successfully created ', @p_RowsCreated, ' SRF stipend disbursement records for month: ', @p_MonthYear);
        
    END TRY 
    BEGIN CATCH 
        ROLLBACK TRANSACTION;
        
        DECLARE @ErrorMessage NVARCHAR(MAX) = ERROR_MESSAGE();
        DECLARE @ErrorNumber INT = ERROR_NUMBER();
        
        PRINT CONCAT('Error in usp_CalculateAndDisburseSRFStipend: [', @ErrorNumber, '] ', @ErrorMessage);
        THROW;
    END CATCH
END;
GO

PRINT 'SRFStipendModule_Procedures created successfully.';
GO
