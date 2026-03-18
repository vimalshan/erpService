-- ==========================================
-- Module: ENERGY MANAGEMENT
-- Database: TASKDB
-- Purpose: Energy/Utility Management Procedures & Functions
-- ==========================================

USE [TASKDB];
GO

-- ==========================================
-- STORED PROCEDURES
-- ==========================================

-- ------------------------------------------------------------------
-- Procedure: usp_ENERGY_InsertReading
-- Purpose:  Record energy/utility consumption reading
-- Parameters:
--   @p_UnitCode - Unit code
--   @p_ProcessID - Process ID
--   @p_ReadingValue - Meter reading value
--   @p_TargetValue - Target value for comparison
--   @p_Remarks - Additional remarks
--   @p_ModifiedBy - User making the entry
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_ENERGY_InsertReading
(
    @p_UnitCode CHAR(3),
    @p_ProcessID INT,
    @p_ReadingValue BIGINT,
    @p_TargetValue BIGINT = NULL,
    @p_Remarks VARCHAR(100) = NULL,
    @p_ModifiedBy INT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @ActualUsage BIGINT;
        DECLARE @PreviousReading BIGINT;
        
        -- Get previous reading for usage calculation
        SELECT TOP 1 @PreviousReading = EB_READING 
        FROM EC_READING 
        WHERE EB_UNIT_CODE = @p_UnitCode AND EB_PROCESS_ID = @p_ProcessID
        ORDER BY EB_DATE DESC;
        
        -- Calculate actual usage
        SET @ActualUsage = @p_ReadingValue - ISNULL(@PreviousReading, 0);
        
        -- Insert new reading record
        INSERT INTO EC_READING
        (
            EB_UNIT_CODE, EB_PROCESS_ID, EB_DATE, EB_TARGET, EB_READING,
            EB_ACTUAL_USAGE, EB_REMARKS, LAST_MODIFIED_BY, LAST_MODIFIED_ON
        )
        VALUES
        (
            @p_UnitCode, @p_ProcessID, GETDATE(), @p_TargetValue, @p_ReadingValue,
            @ActualUsage, @p_Remarks, @p_ModifiedBy, GETDATE()
        );
        
        COMMIT TRANSACTION;
        PRINT 'Energy reading recorded for Unit: ' + @p_UnitCode + ', Process: ' + CAST(@p_ProcessID AS VARCHAR);
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Reading insertion failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ------------------------------------------------------------------
-- Procedure: usp_ENERGY_UpdateProcessAccess
-- Purpose:  Update employee access to energy processes
-- Parameters:
--   @p_ProcessID - Process ID
--   @p_EmployeeSysID - Employee system ID
--   @p_StartDate - Access start date
--   @p_CloseDate - Access close date (NULL if active)
--   @p_ModifiedBy - User making change
-- ------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.usp_ENERGY_UpdateProcessAccess
(
    @p_ProcessID INT,
    @p_EmployeeSysID INT,
    @p_StartDate DATETIME2(3),
    @p_CloseDate DATETIME2(3) = NULL,
    @p_ModifiedBy INT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF EXISTS(SELECT 1 FROM EC_PROCESS_ACCESS WHERE PA_PROCESS_ID = @p_ProcessID AND PA_EMP_SYSID = @p_EmployeeSysID)
        BEGIN
            UPDATE EC_PROCESS_ACCESS
            SET PA_CLOSE_DATE = @p_CloseDate,
                PA_LAST_MODIFIEDBY = @p_ModifiedBy,
                PA_LAST_MODIFIEDON = CONVERT(VARCHAR(30), GETDATE(), 121)
            WHERE PA_PROCESS_ID = @p_ProcessID AND PA_EMP_SYSID = @p_EmployeeSysID;
        END
        ELSE
        BEGIN
            INSERT INTO EC_PROCESS_ACCESS
            (
                PA_PROCESS_ID, PA_EMP_SYSID, PA_START_DATE, PA_CLOSE_DATE,
                PA_LAST_MODIFIEDBY, PA_LAST_MODIFIEDON
            )
            VALUES
            (
                @p_ProcessID, @p_EmployeeSysID, @p_StartDate, @p_CloseDate,
                @p_ModifiedBy, CONVERT(VARCHAR(30), GETDATE(), 121)
            );
        END
        
        COMMIT TRANSACTION;
        PRINT 'Process access updated for Process: ' + CAST(@p_ProcessID AS VARCHAR) + ', Employee: ' + CAST(@p_EmployeeSysID AS VARCHAR);
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RAISERROR('Access update failed: %s', 16, 1, ERROR_MESSAGE());
    END CATCH
END;
GO

-- ==========================================
-- END OF SCRIPT - ENERGY MODULE PROCEDURES
-- ==========================================
